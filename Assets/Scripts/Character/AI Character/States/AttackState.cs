using SKD.Character.AI_Character.Actions;
using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character.States
{
    [CreateAssetMenu(menuName = "AI/States/Attack")]
    public class AttackState : AIState
    {
        [Header("Current Attack")]
        [HideInInspector] public AICharacterAttackAction _currentAttack;
        [HideInInspector] public bool _willPerformCombo;

        [Header("State Flags")]
        protected bool _hasPerformAttack;
        protected bool _hasPerformCombo;

        [Header("Pivot After Attack")]
        [SerializeField] protected bool _pivotAfterAttack;

        public override AIState Tick(AICharacterManager aICharacter)
        {
            if (aICharacter._characterCombatManager._currentTarget == null)
                return SwitchState(aICharacter, aICharacter._idle);

            if (aICharacter._characterCombatManager._currentTarget._isDead.Value)
                return SwitchState(aICharacter, aICharacter._idle);

            aICharacter._aICharacterCombatManager.RotateTowardsAgent(aICharacter);

            //  ROTATE TOWARDS THE TARGET WHILST ATTACKING
            aICharacter._aICharacterCombatManager.RotateTowardsTargetWhilstAttacking(aICharacter);
            // Set Movement Values to 0
            aICharacter._characterAnimationManager.UpdateAnimatorMovementParameters(0, 0, false);

            // Perform a combo
            if (_willPerformCombo && !_hasPerformAttack)
            {
                if (_currentAttack._comboAction != null)
                {
                    // If we can combo 
                    /*   _hasPerformAttack = true;
                       _currentAttack._comboAction.AttempToPerformAction(aICharacter);*/
                }
            }

            if (aICharacter._isPerformingAction)
                return this;

            if (!_hasPerformAttack)
            {
                // If we are still recovering from an action, wait before performing another
                if (aICharacter._aICharacterCombatManager._actionRecoveryTime > 0)
                    return this;

                PerformAttack(aICharacter);

                // Return to the top, so if we have a combo we proceed that when we are able
                return this;
            }
            if (_pivotAfterAttack)
                aICharacter._aICharacterCombatManager.PivotTowardsTarget(aICharacter);

            return SwitchState(aICharacter, aICharacter._combatStance);
        }
        protected void PerformAttack(AICharacterManager aICharacter)
        {
            _hasPerformAttack = true;
            _currentAttack.AttempToPerformAction(aICharacter);
            aICharacter._aICharacterCombatManager._actionRecoveryTime = _currentAttack._actionRecoveryTime;
        }
        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            _hasPerformAttack = false;
            _hasPerformCombo = false;
        }
    }
}