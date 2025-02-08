using SKD.Character;
using SKD.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.UI.PlayerUI
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem _currentRightHandWeapon;
        public WeaponItem _currentLeftHandWeapon;
        [FormerlySerializedAs("_currentTwohandWeapon")] public WeaponItem _currentTwoHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] _weaponInRigthHandSlots = new WeaponItem[3];
        public int _rightHandWeaponIndex;
        public WeaponItem[] _weaponInLefthHandSlot = new WeaponItem[3];
        public int _leftHandWeaponIndex;
        
        [Header("Armor")]
        public HeadEquipmentItem _headEquipment;
        public BodyEquipmentItem _bodyEquipment;
        public LegEquipmentItem _legEquipment;
        public HandEquipmentItem _handEquipment;
    }
}