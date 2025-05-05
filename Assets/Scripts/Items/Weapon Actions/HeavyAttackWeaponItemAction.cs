using SKD.Character.Player;
using System.Collections;
using SKD.Items.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        // Main = main hand
        [SerializeField] string _heavy_Attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] string _heavy_Attack_02 = "Main_Heavy_Attack_02";
        [SerializeField] string _heavy_Jumping_Attack_01 = "Main_Heavy_Jump_Attack_01";

        // Two Hand
        [SerializeField] string _th_Heavy_Attack_01 = "TH_Heavy_Attack_01";
        [SerializeField] string _th_Heavy_Attack_02 = "TH_Heavy_Attack_02 ";
        [SerializeField] string _th_Heavy_Jumping_Attack_01 = "TH_Heavy_Jump_Attack_01";
        public override void AttemptToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            // Check for stops
            if (playerPerformingAction._playerNetworkManager._currentStamina.Value <= 0)
                return;
            
            // If we are in the air perform jumping/aerial Attack
            if (!playerPerformingAction._characterLocomotionManager._isGrounded)
            {
                PerformJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }
            if (playerPerformingAction._playerNetworkManager._isJumping.Value)
                return;

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
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, _th_Heavy_Attack_02, true);
                else
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, _th_Heavy_Attack_01, true);
            }
            // Otherwise, if we are not already attacking just perform a regular attack
            else if (!playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, _th_Heavy_Attack_01, true);
            }
        }
        private void PerformJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                PerformTwoHandJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
            else
                PerformMainHandJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);

        }
        private void PerformMainHandJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._isPerformingAction)
                return;

            playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpAttack01, _heavy_Jumping_Attack_01, true);
        }
        private void PerformTwoHandJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._isPerformingAction)
                return;

            playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpAttack01, _th_Heavy_Jumping_Attack_01, true);
        }

    }
}