using SKD.Items.Quick_Item_Slot;
using UnityEngine;
namespace Items.Flasks
{
    [CreateAssetMenu(menuName = "Items/Consumable/Flask")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Empty Flask")]
        [SerializeField] GameObject _emptyFlaskItem;
    }
}