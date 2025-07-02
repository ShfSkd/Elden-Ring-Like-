using SKD.Character.Player;
using SKD.Items.Weapons;
using UnityEngine;
namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Aim Action")]
    public class AimAction : WeaponItemAction
    {
        public override void AttemptToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction._playerLocomotionManager._isGrounded)
                return;

            if (playerPerformingAction._playerNetworkManager._isJumping.Value)
                return;

            if (playerPerformingAction._playerLocomotionManager._isRolling)
                return;

            if (playerPerformingAction._playerNetworkManager._isLockOn.Value)
                return;

            // If we are using an item, return
            if (playerPerformingAction._playerCombatManager._isUsingItem)
                return;

            if (playerPerformingAction.IsOwner)
            {
                // Two handed the weapon (bow) before we aim
                if (!playerPerformingAction._playerNetworkManager._isTwoHandingWeapon.Value)
                {
                    if (playerPerformingAction._playerNetworkManager._isUsingRightHand.Value)
                    {
                        playerPerformingAction._playerNetworkManager._isTwoHandingRightWepoen.Value = true;
                    }
                    else if (playerPerformingAction._playerNetworkManager._isUsingLeftHand.Value)
                    {
                        playerPerformingAction._playerNetworkManager._isTwoHandingLeftWeapon.Value = true;
                    }
                }
                playerPerformingAction._playerNetworkManager._isAiming.Value = true;
            }
        }
    }
}