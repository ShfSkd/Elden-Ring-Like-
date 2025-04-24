using SKD.Items;
using System.Collections;
using SKD.Colliders;
using SKD.World_Manager;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager _character;

        [Header("last Attack Animation Perform")]
        public string _lastAttackAnimationPerformed;

        [Header("Previous Poise Damage Taken")]
        public float _previousPoiseDamageTaken;

        [Header("Attack Character")]
        public CharacterManager _currentTarget;

        [Header("Attack Type")]
        public AttackType _currentAttackType;

        [Header("Lock On Transform")]
        public Transform _lockOnTransform;

        [Header("Attack Flags")]
        public bool _canPerformRollingAttack;
        public bool _canPerformBackstopAttack;
        public bool _canBlock = true;
        public bool _canBackstabbed = true;
        [Header("Critical Attack")]
        private Transform _riposteReciverTransform;
        private Transform _backstabReciverTransform;
        [SerializeField] float _criticalAttackDistanceCheck = 0.7f;
        public int _pendingCriticalDamage;

        protected virtual void Awake()
        {
            _character = GetComponent<CharacterManager>();
        }
        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (_character.IsOwner)
            {
                if (newTarget != null)
                {
                    _currentTarget = newTarget;
                    _character._characterNetworkManager._currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    _currentTarget = null;
                }
            }
        }

        // Used to attempt backstep/riposte
        public virtual void AttemptCriticalAttack()
        {
            // We cannot perform a critical strike if we are performing another action
            if (_character._isPerformingAction)
                return;

            // We cannot perform a critical strike if we are out of stamina
            if (_character._characterNetworkManager._currentStamina.Value <= 0)
                return;

            // Aim the raycast in front of us and check for any potential targets to critically attack  
            RaycastHit[] hits = Physics.RaycastAll(_character._characterCombatManager._lockOnTransform.position,
                _character.transform.TransformDirection(Vector3.forward), _criticalAttackDistanceCheck, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < hits.Length; i++)
            {
                // Check for each of the hits 1 by 1, giving them their own variable 
                RaycastHit hit = hits[i];

                CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();


                if (targetCharacter != null)
                {
                    // If the character is the one attempting the critical strike, go to the next hit in the array of total hits 
                    if (targetCharacter == _character)
                        continue;

                    // If we cannot damage the character that is targeted continue to check the next hot in the array of hits
                    if (!WorldUtilityManager.Instance.CanIDamageThisTarget(_character._characterGroup, targetCharacter._characterGroup))
                        continue;
                    // This gets us our position and angle in respect to our current critical damage target
                    Vector3 directionFromCharacterToTarget = _character.transform.position - targetCharacter.transform.position;
                    float targetViewableAngle = Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                    if (targetCharacter._characterNetworkManager._isRipostable.Value)
                    {
                        if (targetViewableAngle is >= -60f and <= 60f)
                        {
                            AttemptRiposte(hit);
                            return;
                        }
                    }

                    // Backstab
                    if (targetCharacter._characterCombatManager._canBackstabbed)
                    {
                        if (targetViewableAngle is <= 180f and >= 145)
                        {
                            AttemptBackstab(hit);
                            return;
                        }
                        if (targetViewableAngle is >= -180f and <= -145)
                        {

                            AttemptBackstab(hit);
                            return;
                        }
                    }
                }


            }
        }
        public virtual void AttemptBackstab(RaycastHit hit)
        {

        }
        public virtual void AttemptRiposte(RaycastHit hit)
        {


        }

        public virtual void ApplyCriticalDamage()
        {
            _character._characterEffectsManager.PlayCriticalBloodSplatterVFX(_character._characterCombatManager._lockOnTransform.position);
            _character._characterSoundFXManager.PlayCriticalStrikeSFX();

            if (_character.IsOwner)
                _character._characterNetworkManager._currentHealth.Value -= _pendingCriticalDamage;
        }
        public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter, Vector3 ripostePosition)
        {
            float timer = 0;

            while(timer < 0.2f)
            {
                timer += Time.deltaTime;

                if (_riposteReciverTransform == null)
                {
                    GameObject riposteTransformObject = new GameObject("Riposte Transform");
                    riposteTransformObject.transform.parent = transform;
                    riposteTransformObject.transform.position = Vector3.zero;
                    _riposteReciverTransform = riposteTransformObject.transform;
                }
                _riposteReciverTransform.localPosition = ripostePosition;
                enemyCharacter.transform.position = _riposteReciverTransform.position;
                transform.rotation = Quaternion.LookRotation(-enemyCharacter.transform.forward);
                yield return null;
            }
        }
        public IEnumerator ForceMoveEnemyCharacterToBackstabPosition(CharacterManager enemyCharacter, Vector3 backstabPosition)
        {
            float timer = 0;

            while(timer < 0.2f)
            {
                timer += Time.deltaTime;

                if (_riposteReciverTransform == null)
                {
                    GameObject backstabTransformObject = new GameObject("Backstab Transform");
                    backstabTransformObject.transform.parent = transform;
                    backstabTransformObject.transform.position = Vector3.zero;
                    _backstabReciverTransform = backstabTransformObject.transform;
                }
                _backstabReciverTransform.localPosition = backstabPosition;
                enemyCharacter.transform.position = _backstabReciverTransform.position;
                transform.rotation = Quaternion.LookRotation(enemyCharacter.transform.forward);
                yield return null;
            }
        }
        public void EnableIsInvulnerable()
        {
            if (_character.IsOwner)
                _character._characterNetworkManager._isInvulnerable.Value = true;
        }
        public void DisableIsInvulnerable()
        {
            if (_character.IsOwner)
                _character._characterNetworkManager._isInvulnerable.Value = false;

        }
        public void EnableIsParrying()
        {
            if(_character.IsOwner)
                _character._characterNetworkManager._isParrying.Value = true;
        }
        public void DisableIsParrying()
        {
            if(_character.IsOwner)
                _character._characterNetworkManager._isParrying.Value = false;
        }
        public void EnableIsRipostable()
        {
            if (_character.IsOwner)
                _character._characterNetworkManager._isRipostable.Value = true;
        }
        public void EnableCanDoRollingAttack()
        {
            _canPerformRollingAttack = true;
        }

        public void DisableCanDoRollingAttack()
        {
            _canPerformRollingAttack = false;
        }

        public void EnableCanDoBackstepAttack()
        {
            _canPerformBackstopAttack = true;
        }

        public void DisableCanDoBackstepAttack()
        {
            _canPerformBackstopAttack = false;
        }

        protected virtual void CloseAllDamageColliders()
        {
            
        }
        public void DestroyALlCurrentActionFX()
        {
            _character._characterNetworkManager.DestroyALlCurrentActionFXServerRpc();
        }

    }
}