using System;
using SKD.Character.Player;
using SKD.Items.Equipment;
using SKD.Items.Weapons;
using UnityEngine;
using UnityEngine.Serialization;
namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Fire Projectile Action")]
    public class FireProjectileAction : WeaponItemAction
    {
        [SerializeField] ProjectileSlot _projectileSlot;
        public override void AttemptToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;
            
            // If we are using an item, return
            if (playerPerformingAction._playerCombatManager._isUsingItem)
                return;

            if (playerPerformingAction._playerNetworkManager._currentStamina.Value <= 0)
                return;
            RangedProjectileItem projectileItem = null;
            // 1. Define which projectile we are using (main projectile slot,or secondary slot
            switch (_projectileSlot)
            {
                case ProjectileSlot.Main:
                    projectileItem = playerPerformingAction._playerInventoryManager._mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = playerPerformingAction._playerInventoryManager._secondaryProjectile;
                    break;
                default:
                    break;
            }
            // 2. If the projectile is null, return
            if (projectileItem == null)
                return;

            if (!playerPerformingAction.IsOwner)
                return;

            // 3. if the player is not 2 handing the weapon, make them two hand it now (Weapon must be 2 handed to fire projectile)
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


            // 4. If the player does not have an arrow notched, do so now
            if (!playerPerformingAction._playerNetworkManager._hasArrowNotched.Value)
            {
                playerPerformingAction._playerNetworkManager._hasArrowNotched.Value = true;
                
                bool canIDrawAProjectile = CanIFireThisProjectile(playerPerformingAction, projectileItem);

                if (!canIDrawAProjectile)
                    return;

                if (projectileItem._currentAmmoAmount <= 0)
                {
                    playerPerformingAction._playerAnimationManager.PlayTargetActionAnimation("Out_of_Ammo_01", true);
                    return;
                }
                playerPerformingAction._playerCombatManager._currentProjectileBeingUsed = _projectileSlot;
                playerPerformingAction._playerAnimationManager.PlayTargetActionAnimation("Bow_Draw_01",true);
                playerPerformingAction._playerNetworkManager.NotifyServerOfDrawnProjectileServerRpc(projectileItem._itemID);
            }
        }

        private bool CanIFireThisProjectile(PlayerManager playerPerformingAction, RangedProjectileItem projectileItem)
        {
            // Check for cross bow, great bow, and compare ammo to give result 

            return true;
        }

    }
}