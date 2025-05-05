using SKD.Character.Player;
using SKD.Items.Weapons;
using UnityEngine;

namespace SKD.Items.Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test.Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int _actionID;
        public virtual void AttemptToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction._playerNetworkManager._currentWeaponBeingUsed.Value = weaponPerformingAction._itemID;
            }
            Debug.Log("The Action Has Fired");

            // Notify the server we have performed the action, so we perform it from their perspective also 
            //_player._playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction._actionID, weaponPerformingAction._itemID);
        }
    }
}