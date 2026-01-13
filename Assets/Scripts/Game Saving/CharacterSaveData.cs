using System.Collections.Generic;
using SKD.Items.Quick_Item_Slot;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.GameSaving
{
    [System.Serializable]
    // Since we want to reference this data for every save file, this script in not MonoBehavoiur and instead Serializable
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int _sceneIndex = 1;

        [Header("Character Name")]
        public string _characterName = "Character";
        
        [Header("Body Type")]
        public bool _isMale = true;
        public int _hairStyleID;
        public float _hairColorRedID;
        public float _hairColorGreenID;
        public float _hairColorBlueID;

        [Header("Time Played")]
        public float _secondsPlayed;


        // Question: Why not to use vector3?
        // Answer: we can only use save data from "basic" variables types(float,Int,string etc..)
        [Header("World Coordinates")]
        public float _xPosition;
        public float _yPosition;
        public float _zPosition;

        [Header("Resources")]
        public int _currentHealth;
        public float _currentStamina;
        public int _currentFocusPoints;

        [Header("Stats")]
        public int _vitality;
        public int _endurance;
        public int _mind;

        [Header("Sites Of Grace")]
        public SerializableDictionary<int, bool> _siteOfGrace; // The int is in the site if grace ID, the bool is the "Activated" statues

        [Header("Bosses")]
        public SerializableDictionary<int, bool> _bossesAwakened; // the int is the boss ID , The bool is the awaken statues 
        public SerializableDictionary<int, bool> _bossesDefeated; // the int is the boss ID , The bool is the defeated statues

        [Header("World Items")]
        public SerializableDictionary<int, bool> _worldItemLooted; // The int is the Item ID , The bool is the looted statues
        
        [Header("Equipment")]
        public int _headEquipment;
        public int _bodyEquipment;
        public int _legEquipment;
        public int _handEquipment;

        public int _rightWeaponIndex;
        public SerializableWeapon _rightWeapon01;
        public SerializableWeapon _rightWeapon02;
        public SerializableWeapon _rightWeapon03;

        public int _leftWeaponIndex;
        public SerializableWeapon _leftWeapon01;
        public SerializableWeapon _leftWeapon02;
        public SerializableWeapon _leftWeapon03;

        public int _quickSlotIndex;
        public SerializableQuickSlotIcon _quickSlotItem01;
        public SerializableQuickSlotIcon _quickSlotItem02;
        public SerializableQuickSlotIcon _quickSlotItem03;
        
        public SerializableRangedProjectile _mainProjectile;
        public SerializableRangedProjectile _secondaryProjectile;
        
        public int _currentHelathFlaskRemaining = 3;
        public int _currentPocusPointFlaskRemaining = 1;
        
        [Header("Inventory")]
        public List<SerializableWeapon> _weaponInInventory;
        public List<SerializableRangedProjectile> _projectileInInventory;
        public List<SerializableQuickSlotIcon> _quickSlotItemInInventory;
        public List<int> _headEquipmentInInventory;
        public List<int> _bodyEquipmentInInventory;
        public List<int> _legEquipmentInInventory;
        public List<int> _handsEquipmentInInventory;
        
        public int _currentSpell;
        public CharacterSaveData()
        {
            _siteOfGrace = new SerializableDictionary<int, bool>();
            _bossesAwakened = new SerializableDictionary<int, bool>();
            _bossesDefeated = new SerializableDictionary<int, bool>();
            _worldItemLooted = new SerializableDictionary<int, bool>();

            _weaponInInventory = new List<SerializableWeapon>();
            _projectileInInventory = new List<SerializableRangedProjectile>();
            _quickSlotItemInInventory = new List<SerializableQuickSlotIcon>();
            _headEquipmentInInventory = new List<int>();
            _bodyEquipmentInInventory = new List<int>();
            _legEquipmentInInventory = new List<int>();
            _handsEquipmentInInventory = new List<int>();
        }
    }
}
