using SKD.Character.Player;
using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;

namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee  Action")]
    public class OffHandMeleeAction : WeaponItemAction
    {
        public override void AttampToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttampToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction._playerCombatManager._canBlock)
                return;

            // Check for attack statues 
            if (playerPerformingAction._playerNetworkManager._isAttacking.Value)
            {
                // Disable Blocking 
                if (playerPerformingAction.IsOwner)
                    playerPerformingAction._playerNetworkManager._isBlocking.Value = false;

                return;
            }
            if (playerPerformingAction._playerNetworkManager._isBlocking.Value)
                return;

            if (playerPerformingAction.IsOwner)
                playerPerformingAction._playerNetworkManager._isBlocking.Value = true; 
        }
    }
}