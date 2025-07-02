using SKD.Items.Weapons;
using SKD.Character.Player;
using UnityEngine;
namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Incantation Action")]
    public class CastIncantationAction : WeaponItemAction
    {
        public override void AttemptToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformedAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;
            
            // If we are using an item, return
            if (playerPerformingAction._playerCombatManager._isUsingItem)
                return;

            // Check for stops
            if (playerPerformingAction._playerNetworkManager._currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction._characterLocomotionManager._isGrounded)
                return;

            if (playerPerformingAction._playerInventoryManager._currentSpell == null)
                return;
            
            if (playerPerformingAction._playerInventoryManager._currentSpell._spellClass != SpellClass.Incantation)
                return;

            if (playerPerformingAction.IsOwner)
                playerPerformingAction._playerNetworkManager._isAttacking.Value = true;

            CastIncantation(playerPerformingAction, weaponPerformingAction);
        }
        private void CastIncantation(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction._playerInventoryManager._currentSpell.AttemptToCastSpell(playerPerformingAction);
        }
    }
}