using UnityEngine;

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

        [Header("Stats")]
        public int _vitality;
        public int _endurance;

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
        public int _rightWeapon01;
        public int _rightWeapon02;
        public int _rightWeapon03;

        public int _leftWeaponIndex;
        public int _leftWeapon01;
        public int _leftWeapon02;
        public int _leftWeapon03;
        
        
        public CharacterSaveData()
        {
            _siteOfGrace = new SerializableDictionary<int, bool>();
            _bossesAwakened = new SerializableDictionary<int, bool>();
            _bossesDefeated = new SerializableDictionary<int, bool>();
            _worldItemLooted = new SerializableDictionary<int, bool>();
        }
    }
}
