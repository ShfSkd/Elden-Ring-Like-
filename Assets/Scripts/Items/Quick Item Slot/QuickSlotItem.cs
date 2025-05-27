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

        public virtual void AttemptToUseItem(PlayerManager player)
        {
            if(!CanIUseThisItem(player))
                return;

            player._playerAnimationManager.PlayTargetActionAnimation(_useItemAnimation, true);
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }
    }
}