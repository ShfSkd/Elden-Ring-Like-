using SKD.Character.Player;
using SKD.Items.Quick_Item_Slot;
using UnityEngine;
namespace Items.Flasks
{
    [CreateAssetMenu(menuName = "Items/Consumable/Flask")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Empty Flask")]
        [SerializeField] GameObject _emptyFlaskItem;


        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            player._playerEffectsManager._activeQuickSlotItemFX = Instantiate(_itemModel, player._playerEquipmentManager._rightHandWeaponSlot.transform);

            player._playerAnimationManager.PlayTargetActionAnimation(_useItemAnimation, true, false, true, true, false);
        }
    }
}