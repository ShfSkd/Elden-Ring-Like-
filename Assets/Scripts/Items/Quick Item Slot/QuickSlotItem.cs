using SKD.Character.Player;
using UnityEngine;
namespace SKD.Items.Quick_Item_Slot
{
    public class QuickSlotItem : Item
    {
        [Header("Item Model")]
        [SerializeField] protected GameObject _itemModel;
        
        [Header("Animations")]
        [SerializeField] protected string _useItemAnimation;

        // Not all quick slot item are consumables
        [Header("Consumable")]
        public bool _isConsumable = true;
        public int _itemAmount;

        public virtual void AttemptToUseItem(PlayerManager player)
        {
            if(!CanIUseThisItem(player))
                return;

            player._playerAnimationManager.PlayTargetActionAnimation(_useItemAnimation, true);
        }
        public virtual void SuccessfullyUseItem(PlayerManager player)
        {
           
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }
        public virtual int GetCurrentAmount(PlayerManager player)
        {
            return 0;
        }
    }
}