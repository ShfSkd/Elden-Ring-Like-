using SKD.Character;
using SKD.Items;
using System.Collections;
using UnityEngine;

namespace SKD.UI.PlayerUI
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem _currentRightHandWeapon;
        public WeaponItem _currentLeftHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] _weaponInRigthHandSlots = new WeaponItem[3];
        public int _rightHandWeaponIndex;
        public WeaponItem[] _weaponInLefthHandSlot = new WeaponItem[3];
        public int _leftHandWeaponIndex;
    }
}