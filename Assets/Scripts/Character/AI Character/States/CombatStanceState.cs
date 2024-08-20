using SKD.Character.AI_Character.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SKD.Character.AI_Character.States
{
    [CreateAssetMenu(menuName = "AI/States/Combat Stance")]
    public class CombatStanceState : AIState
    {
        // 1.Select an attack for the target state, depending in distance and angle of target in relation to character
        // 2.Procces any combat logic here whilst waiting to attack(blocking strafing dodging etc)
        // 3.If target moves out of combat range switch to pursue target
        // 4. if the target is no longer present, switch to idle state

        [Header("Attacks")]
        public List<AICharacterAttackAction> _aICharacterAttacks = new List<AICharacterAttackAction>(); // A list of possible attacks this character can do 
        protected List<AICharacterAttackAction> _potenialAttacks; // A list that is created during the state, (based on angle, distance etc)
        private AICharacterAttackAction _chosenAttack;
        private AICharacterAttackAction _previousAttack;
        protected bool _hasAttack;

        [Header("Combo")]
        [SerializeField] protected bool _canPerformCombo;
        [SerializeField] protected int _chanceToPerformCombo = 25;
        protected bool _hasRolledForComboChance; // If we have already rolled for the chance during this state 

        [Header("Engagement Distance")]
        [SerializeField] protected float _maximunEngamnetDitance = 5f; // The distance we have to be away from the target before we enter the pursue target state

        public override AIState Tick(AICharacterManager aICharacter)
        {
            if (aICharacter._isPerformingAction)
                return this;

            if (!aICharacter._navMeshAgent.enabled)
                aICharacter._navMeshAgent.enabled = true;

            // If you want the AI character to face and turn toward its target when its outside its FOV include this 
            if (aICharacter._aICharacterNetworkManager._isMoving.Value)
            {
                if (aICharacter._aICharcterCombatManager._viewableAngle < -30 || aICharacter._aICharcterCombatManager._viewableAngle > 30)
                    aICharacter._aICharcterCombatManager.PivotTowardsTarget(aICharacter);
            }
            //  Rotate to face our target
            aICharacter._aICharcterCombatManager.RotateTowardsAgent(aICharacter);

            // if our target is no longer present, switch back to idle
            if (aICharacter._aICharcterCombatManager._currentTarget == null)
                return SwitchState(aICharacter, aICharacter._idle);

            // If we don't have an attack, get one 
            if (!_hasAttack)
            {
                GetNewAttack(aICharacter);
            }
            else
            {
                // Check recovery timer
                // Pass attack to attack state
                aICharacter._attack._currentAttack = _chosenAttack;
                // Switch state 
                return SwitchState(aICharacter, aICharacter._attack);
                // Roll for combo chance 
            }

            // If we are outside of the combat engagement distance, switch to pursue target state
            if (aICharacter._aICharcterCombatManager._distanceFromTarget > _maximunEngamnetDitance)
                return SwitchState(aICharacter, aICharacter._pursueTarget);

            NavMeshPath path = new NavMeshPath();
            aICharacter._navMeshAgent.CalculatePath(aICharacter._aICharcterCombatManager._currentTarget.transform.position, path);
            aICharacter._navMeshAgent.SetPath(path);

            return this;

        }
        protected virtual void GetNewAttack(AICharacterManager aICharacter)
        {
            _potenialAttacks = new List<AICharacterAttackAction>();

            // 1. Sort through all possible attacks
            // 2. Remove attacks that can be used in this situation (based on angle and distance)
            foreach (var potenialAttack in _aICharacterAttacks)
            {
                // The target is too close to perform this attack, check the next 
                if (potenialAttack._minimumAttackDistance > aICharacter._aICharcterCombatManager._distanceFromTarget)
                    continue;

                // The target is too Far to perform this attack, check the next
                if (potenialAttack._maximumAttackDistance < aICharacter._aICharcterCombatManager._distanceFromTarget)
                    continue;

                // If the target is outside minimum field of view for this attack, check the next 
                if (potenialAttack._minimumAttackAngle > aICharacter._aICharcterCombatManager._viewableAngle)
                    continue;

                // If the target is outside maximum field of view for this attack, check the next 
                if (potenialAttack._maximumAttackAngle < aICharacter._aICharcterCombatManager._viewableAngle)
                    continue;

                // 3. place renaming attacks into a list 
                _potenialAttacks.Add(potenialAttack);

            }
            if (_potenialAttacks.Count <= 0)
                return;

            var totoalweight = 0;

            foreach (var attack in _potenialAttacks)
            {
                totoalweight += attack._attackWeight;
            }

            var randomWeightValue = Random.Range(1, totoalweight + 1);
            var proccesedWeight = 0;

            foreach (var attack in _potenialAttacks)
            {
                proccesedWeight += attack._attackWeight;

                if (randomWeightValue <= proccesedWeight)
                {
                    // This is our attack 
                    _chosenAttack = attack;
                    _previousAttack = _chosenAttack;
                    _hasAttack = true;
                    return;
                }
            }

            // 4. Pick one of the remaining attacks randomly, based on weight 
            // 5. Select this attack and pass it to the attack state
        }
        protected virtual bool RollForoutcomeChance(int outcomeChance)
        {
            bool outcomeWillbePerforme = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
                outcomeWillbePerforme = true;

            return outcomeWillbePerforme;
        }
        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            _hasAttack = false;
            _hasRolledForComboChance = false;
        }

    }
}