using System;
using SKD.World_Manager;
using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AICharterCombatManager : CharacterCombatManager
    {
        protected AICharacterManager _aICharacter;

        [Header("Action Recovery")]
        public float _actionRecoveryTime;

        [Header("Pivot")]
        public bool _enablePivot = true;

        [Header("Target Information")]
        public float _distanceFromTarget;
        public float _viewableAngle;
        public Vector3 _targetDirection;

        [Header("Detection")]
        [SerializeField] float _detectionRaduis = 15f;
        public float _minimumFieldOfView = -35f;
        public float _maximumFieldOfView = 35f;

        [Header("Attack Rotation Speed")]
        public float _attackRotationSpeed = 25f;

        [Header("Stance Setting")]
        public float _maxStance = 150f;
        public float _currentStance;
        [SerializeField] float _stanceRegeneratedPersecond;
        [SerializeField] bool _ignoreStanceBreak;

        [Header("Stance Timer")]
        [SerializeField] float _stanceRegenerationTimer;
        private float _stanceTickTimer;
        [SerializeField] float _defultTimeUntilStanceRegenerationBegain = 15f;

        protected override void Awake()
        {
            base.Awake();
            _aICharacter = GetComponent<AICharacterManager>();
            _lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }
        private void FixedUpdate()
        {
            HandleStanceBreak();
        }
        private void HandleStanceBreak()
        {
            if (!_aICharacter.IsOwner)
                return;

            if (_aICharacter._isDead.Value)
                return;

            if (_stanceRegenerationTimer > 0)
            {
                _stanceRegenerationTimer -= Time.deltaTime;
            }
            else
            {
                _stanceRegenerationTimer = 0;

                if (_currentStance < _maxStance)
                {
                    // Begin adding stance each tick
                    _stanceTickTimer += Time.deltaTime;

                    if (_stanceTickTimer >= 1)
                    {
                        _stanceTickTimer = 0;
                        _currentStance += _stanceRegeneratedPersecond;
                    }
                }
                else
                {
                    _currentStance = _maxStance;
                }
            }
            if (_currentStance <= 0)
            {
                // This would feel less impactful in gameplay 
                DamageIntensity previousDamageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(_previousPoiseDamageTaken);

                if (previousDamageIntensity == DamageIntensity.Colossal)
                {
                    _currentStance = 1;
                    return;
                }

                _currentStance = _maxStance;

                if (_ignoreStanceBreak)
                    return;

                _aICharacter._characterAnimationManager.PlayTargetActionAnimationInstantly("Stance_Break_01",true);
            }
        }
        public void DamageStance(int stanceDamage)
        {
            // When stance is damaged, the timer is reset, meaning constant attack give no chance at recovering stance that is lost 
            _stanceRegenerationTimer = _defultTimeUntilStanceRegenerationBegain;

            _currentStance -= stanceDamage;
        }
        public void FindATargetViaLineOfSight(AICharacterManager aICharacter)
        {
            if (_currentTarget != null)
                return;

            Collider[] colliders = Physics.OverlapSphere(aICharacter.transform.position, _detectionRaduis, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
                if (targetCharacter == null)
                    continue;

                if (targetCharacter == aICharacter)
                    continue;

                if (targetCharacter._isDead.Value)
                    continue;

                // Can i even this character, is so, make them my target 
                if (WorldUtilityManager.Instance.CanIDamageThisTarget(aICharacter._characterGroup, targetCharacter._characterGroup))
                {
                    // If a potential target is found, it has to be in front of us 
                    Vector3 targetDirection = targetCharacter.transform.position - aICharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetDirection, aICharacter.transform.forward);

                    if (angleOfPotentialTarget > _minimumFieldOfView && angleOfPotentialTarget < _maximumFieldOfView)
                    {
                        //Lastly,we check for enviro blocks
                        if (Physics.Linecast(aICharacter._characterCombatManager._lockOnTransform.position, targetCharacter._characterCombatManager._lockOnTransform.position, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aICharacter._characterCombatManager._lockOnTransform.position, targetCharacter._characterCombatManager._lockOnTransform.position);
                            Debug.Log("Blocked");
                        }
                        else
                        {
                            targetDirection = targetCharacter.transform.position - transform.position;
                            _viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetDirection);
                            aICharacter._characterCombatManager.SetTarget(targetCharacter);

                            if (_enablePivot)
                                PivotTowardsTarget(aICharacter);
                        }
                    }
                }
            }
        }
        public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            // Play A pivot animation depending on viewable angle of target
            if (aiCharacter._isPerformingAction)
                return;

            if (_viewableAngle >= 20 && _viewableAngle <= 60)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Right Turn 45", true);

            else if (_viewableAngle <= -20 && _viewableAngle >= -60)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Left Turn 45", true);

            else if (_viewableAngle >= 61 && _viewableAngle <= 110)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Right Turn 90", true);

            else if (_viewableAngle <= -61 && _viewableAngle >= -110)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Left Turn 90", true);

        }
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter._aICharacterNetworkManager._isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter._navMeshAgent.transform.rotation;
            }
        }
        public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
        {
            if (_currentTarget == null)
                return;

            if (!aiCharacter._characterLocomotionManager._canRotate)
                return;

            if (!aiCharacter._isPerformingAction)
                return;

            //  Rotate towards the target at specified rotation speed during specified frames 
            Vector3 targetDirection = _currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
                targetDirection = aiCharacter.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, _attackRotationSpeed * Time.deltaTime);
        }
        public void HandleActionRecovery(AICharacterManager aiCharacter)
        {
            if (_actionRecoveryTime > 0)
            {
                if (!aiCharacter._isPerformingAction)
                    _actionRecoveryTime -= Time.deltaTime;
            }
        }
    }
}