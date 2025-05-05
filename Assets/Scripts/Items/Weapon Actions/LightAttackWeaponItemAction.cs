using SKD.Character.Player;
using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;

namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        // Main = main hand
        [SerializeField] string _light_Attack_01 = "Main_Light_Attack_01";
        [SerializeField] string _light_Attack_02 = "Main_Light_Attack_02";

        [Header("Running Attacks")]
        [SerializeField] string _run_Attack_01 = "Main_Run_Attack_01";

        [Header("Rolling Attacks")]
        [SerializeField] string _roll_Attack_01 = "Main_Roll_Attack_01";

        [Header("Backstep Attacks")]
        [SerializeField] string _backstep_Attack_01 = "Main_Backstep_Attack_01";

        // Two hands
        [SerializeField] string _th_light_Attack_01 = "TH_Light_Attack_01";
        [SerializeField] string _th_light_Attack_02 = "TH_Light_Attack_02";

        [Header("Running Attacks")]
        [SerializeField] string _th_run_Attack_01 = "TH_Run_Attack_01";

        [Header("Rolling Attacks")]
        [SerializeField] string _th_roll_Attack_01 = "TH_Roll_Attack_01";

        [Header("Backstep Attacks")]
        [SerializeField] string _th_backstep_Attack_01 = "TH_Backstep_Attack_01";

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

            //  IF WE ARE SPRINTING, PERFORM A RUNNING ATTACK
            if (playerPerformingAction._characterNetworkManager._isSprinting.Value)
            {
                PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            //  IF WE ARE ROLLING, PERFORM A ROLLING ATTACK
            if (playerPerformingAction._characterCombatManager._canPerformRollingAttack)
            {
                PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            //  IF WE ARE BACKSTEPPING, PERFORM A BACKSTEP ATTACK
            if (playerPerformingAction._characterCombatManager._canPerformBackstopAttack)
            {
                PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }
            playerPerformingAction._characterCombatManager.AttemptCriticalAttack() ;
            
            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                PerformTwoHandLightAttack(playerPerformingAction, weaponPerformingAction);
            else
                PerformMainLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        private void PerformMainLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // If we are attacking correctly,and we can combo, perform the combo attack
            if (playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon && playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon = false;

                // Perform an attack based on the previous attack we just played
                if (playerPerformingAction._characterCombatManager._lastAttackAnimationPerformed == _light_Attack_01)
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LigthAttack02, _light_Attack_02, true);
                else
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LigthAttack01, _light_Attack_01, true);
            }
            // Otherwise, if we are not already attacking just perform a regular attack
            else if (!playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LigthAttack01, _light_Attack_01, true);
            }
        }
        private void PerformTwoHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // If we are attacking correctly,and we can combo, perform the combo attack
            if (playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon && playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerCombatManager._canComboWithMainHandWeapon = false;

                // Perform an attack based on the previous attack we just played
                if (playerPerformingAction._characterCombatManager._lastAttackAnimationPerformed == _th_light_Attack_01)
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LigthAttack02, _th_light_Attack_02, true);
                else
                    playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LigthAttack01, _th_light_Attack_01, true);
            }
            // Otherwise, if we are not already attacking just perform a regular attack
            else if (!playerPerformingAction._isPerformingAction)
            {
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LigthAttack01, _th_light_Attack_01, true);
            }
        }
        private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, _th_run_Attack_01, true);
            else
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, _run_Attack_01, true);
        }

        private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction._playerCombatManager._canPerformRollingAttack = false;

            if (playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, _th_roll_Attack_01, true);
            else
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, _roll_Attack_01, true);
        }

        private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction._playerCombatManager._canPerformBackstopAttack = false;

            if (playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, _th_backstep_Attack_01, true);
            else
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, _backstep_Attack_01, true);
            
        }

    }
}