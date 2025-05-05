using SKD.Character.Player;
using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;

namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        // Main = main hand
        [SerializeField] string _heavy_Attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] string _heavy_Attack_02 = "Main_Heavy_Attack_02";
        
        // Two Hand
        [SerializeField] string _th_heavy_Attack_01 = "TH_Heavy_Attack_01";
        [SerializeField] string _th_heavy_Attack_02 = "TH_Heavy_Attack_02";
        public override void AttemptToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            // Check for stops
            if (playerPerformingAction._playerNetworkManager._currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction._characterLocomotionManager._isGrounded)
                return;

            if (playerPerformingAction.IsOwner)
                playerPerformingAction._playerNetworkManager._isAttacking.Value = true;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                PerformTwoHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
            else
                PerformMainHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
        private void PerformMainHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // If we are attacking correctly,and we can combo, perform the combo attack
            if (playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon && playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon = false;

                // Perform an attack based on the previous attack we just played
                if (playerPerformingAction._characterCombatManager._lastAttackAnimationPerformed == _heavy_Attack_01)
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, _heavy_Attack_02, true);
                else
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, _heavy_Attack_01, true);
            }
            // Otherwise, if we are not already attacking just perform a regular attack
            else if (!playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, _heavy_Attack_01, true);
            }
        }
        private void PerformTwoHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // If we are attacking correctly,and we can combo, perform the combo attack
            if (playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon && playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon = false;

                // Perform an attack based on the previous attack we just played
                if (playerPerformingAction._characterCombatManager._lastAttackAnimationPerformed == _heavy_Attack_01)
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, _th_heavy_Attack_02, true);
                else
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, _th_heavy_Attack_01, true);
            }
            // Otherwise, if we are not already attacking just perform a regular attack
            else if (!playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, _th_heavy_Attack_01, true);
            }
        }

    }
}