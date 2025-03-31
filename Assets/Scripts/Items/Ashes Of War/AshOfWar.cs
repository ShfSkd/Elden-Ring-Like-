using SKD.Character.Player;
using UnityEngine;
namespace SKD.Items.AshesOfWar
{
    public class AshOfWar : Item
    {
        [Header("Ashes Of War Information")]
        public WeaponClass[] _usableWeaponClasses;
        [Header("Costs")]
        public int _focusPointCost = 20;
        public int _staminaCost = 20;

        // This function Attempting to perform the ash of war 
        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            Debug.Log("Performing action");
        }
        // A helper function used to determine if we can at this moment use this ash of war
        public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            return false;
        }
        protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
        {
            playerPerformingAction._playerNetworkManager._currentStamina.Value -= _staminaCost;
        }
        protected virtual void DeductFoucusPointCost(PlayerManager playerPerformingAction)
        {
            
        }
    }
}