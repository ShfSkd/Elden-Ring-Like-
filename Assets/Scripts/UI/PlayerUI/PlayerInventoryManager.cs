using SKD.Character;
using System.Collections.Generic;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.Spells.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.UI.PlayerUI
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem _currentRightHandWeapon;
        public WeaponItem _currentLeftHandWeapon;
        public WeaponItem _currentTwoHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] _weaponInRigthHandSlots = new WeaponItem[2];
        public int _rightHandWeaponIndex;
        public WeaponItem[] _weaponInLefthHandSlots = new WeaponItem[2];
        public int _leftHandWeaponIndex;
        public SpellItem _currentSpell;
        public QuickSlotItem[] _quickSlotItemInQuickSlots = new QuickSlotItem[3];
        public int _quickSlotItemIndex;
        public QuickSlotItem _currentQuickSlotItem;

        [Header("Armor")]
        public HeadEquipmentItem _headEquipment;
        public BodyEquipmentItem _bodyEquipment;
        public LegEquipmentItem _legEquipment;
        public HandEquipmentItem _handEquipment;

        [Header("Projectiles")]
        public RangedProjectileItem _mainProjectile;
        [FormerlySerializedAs("_secondaryrojectile")]
        [FormerlySerializedAs("_seconderyrojectile")]
        [FormerlySerializedAs("_Seconderyrojectile")]
        public RangedProjectileItem _secondaryProjectile;

        [Header("Inventory")]
        public List<Item> _itemInTheInventory;

        public void AddItemsToInventory(Item item)
        {
            _itemInTheInventory.Add(item);
        }
        public void RemoveItemFromInventory(Item item)
        {
            _itemInTheInventory.Remove(item);

            for (int i = _itemInTheInventory.Count - 1; i > -1; i--)
            {
                if (_itemInTheInventory[i] == null)
                {
                    _itemInTheInventory.RemoveAt(i);
                }
            }
        }
    }
}