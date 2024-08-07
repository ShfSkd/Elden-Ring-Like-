using SKD.Character.Player;
using System.Collections;
using UnityEngine;

namespace SKD.Items.WeaponItems
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test.Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int _actionID;
        public virtual void AttampToPerformedAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction._playerNetworkManager._currentWeaponBeingUsed.Value = weaponPerformingAction._itemID;
            }
            Debug.Log("The Action Has Fired");
        }
    }
}