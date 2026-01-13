using SKD.Character.Player;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.MenuScreen;
using UnityEngine;
namespace SKD.Character
{
    [System.Serializable]
    public class CharacterClass
    {
        [Header("Class Information")]
        public string _className;

        [Header("Class Stats")]
        public int _vitality = 10;
        public int _endurance = 10;
        public int _mind = 10;
        public int _strength = 10;
        public int _dexterity = 10;
        public int _intelligence = 10;
        public int _faith = 10;
        // arcane/luck/whatever other stat you want
        [Header("Class Weapons")]
        public WeaponItem[] _mainHandWeapons = new WeaponItem[3];
        public WeaponItem[] _offHandWeapons = new WeaponItem[3];

        [Header("Class Armor")]
        public HeadEquipmentItem _headEquipment;
        public BodyEquipmentItem _bodyEquipment;
        public LegEquipmentItem _legEquipment;
        public HandEquipmentItem _handEquipment;

        [Header("Quick Slot Item")]
        public QuickSlotItem[] _quickSlotItems = new QuickSlotItem[3];

        public void SetClass(PlayerManager player)
        {
            TitleScreenManager.Instance.SetCharacterClass(player,_vitality,_endurance,_mind,_strength,_dexterity,_intelligence,_faith,
                _mainHandWeapons,_offHandWeapons,
                _headEquipment,_bodyEquipment,_legEquipment,_handEquipment,_quickSlotItems);
        }
    }
}