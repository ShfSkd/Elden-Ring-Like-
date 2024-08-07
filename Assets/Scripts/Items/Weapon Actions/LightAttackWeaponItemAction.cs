using SKD.Character.Player;
using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;

namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string _light_Attack_01 = "Main_Light_Attack_01";// Main = main hand
        public override void AttampToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttampToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if(!playerPerformingAction.IsOwner)
                return;

            // Check for stops
            if (playerPerformingAction._playerNetworkManager._currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction._isGrounded)
                return;

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction._playerNetworkManager._isUsingRightHand.Value)
            {
                playerPerformingAction._playerAnimationManager.PlayTargetAttackActioAnimation(AttackType.LigthAttack01,_light_Attack_01, true);
            }
            if (playerPerformingAction._playerNetworkManager._isUsingLeftHand.Value)
            {

            }
        }

    }
}