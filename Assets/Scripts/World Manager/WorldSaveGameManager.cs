using SKD.Character.Player;
using SKD.GameSaving;
using SKD.MenuScreen;
using System.Collections;
using Items.Flasks;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.UI.PlayerUI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SKD.WorldManager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;

        public PlayerManager _playerManager;

        [Header("Save/Load")]
        [SerializeField] bool _saveGame;
        [SerializeField] bool _loadGame;

        [Header("World Scene Index")]
        [SerializeField] int _worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter _saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot _currentCharacterSlotBeingUsed;
        public CharacterSaveData _currentCharacterData;
        private string _saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData _characterSlot01;
        public CharacterSaveData _characterSlot02;
        public CharacterSaveData _characterSlot03;
        public CharacterSaveData _characterSlot04;
        public CharacterSaveData _characterSlot05;
        public CharacterSaveData _characterSlot06;
        public CharacterSaveData _characterSlot07;
        public CharacterSaveData _characterSlot08;
        public CharacterSaveData _characterSlot09;
        public CharacterSaveData _characterSlot10;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            LoadAllCharacterProfiles();
        }
        private void Update()
        {
            if (_saveGame)
            {
                _saveGame = false;
                SaveGame();
            }
            if (_loadGame)
            {
                _loadGame = false;
                LoadGame();
            }

        }

        public bool HasFreeCharacterSlot()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;
            // Check to see if we can create a new save file (check for other existing files first
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            // Check to see if we can create a new save file (check for other existing files first
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())

                return true;

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;
            
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
                return true;

            // If they are no free slots, notify the player
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();

            return false;
        }
        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "characterSlot_10";
                    break;
                default:
                    break;
            }
            return fileName;
        }
        public void AttemptToCreateNewGame()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;
            // Check to see if we can create a new save file (check for other existing files first
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            // Check to see if we can create a new save file (check for other existing files first
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            if (!_saveFileDataWriter.ChechTooSeeFileExists())
            {
                // If this profile slot is spot been taken , make a new one using this slot
                _currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                _currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }

            // If they are no free slots, notify the player
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();


            // Create a new file, with a file name depending on which slot we are using 
            // _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_currentCharacterSlotBeingUsed);

        }
        private void NewGame()
        {
            // Saves the newly created character stats, and items (when creation screen is added)
            _playerManager._playerNetworkManager._vitality.Value = 15;
            _playerManager._playerNetworkManager._endurance.Value = 10;
            _playerManager._playerNetworkManager._mind.Value = 10;

            SaveGame();
            LoadWorldScene(_worldSceneIndex);
        }
        public void LoadGame()
        {
            //  Load a previous file, with a file name depending on which slot we are using 
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_currentCharacterSlotBeingUsed);

            _saveFileDataWriter = new SaveFileDataWriter();

            // Generally works on multiple machine types (Application.persistentDataPath)
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter._saveFileName = _saveFileName;
            _currentCharacterData = _saveFileDataWriter.LoadSaveFile();

            LoadWorldScene(_worldSceneIndex);
        }
        public void SaveGame()
        {
            // save the current file under a file name depending on which slot we are using 
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_currentCharacterSlotBeingUsed);

            _saveFileDataWriter = new SaveFileDataWriter();
            // Generally works on multiple machine types (Application.persistentDataPath)
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter._saveFileName = _saveFileName;

            // Pass the players Info, from game, to their save File
            _playerManager.SaveGameDataToCurrentCharacterData(ref _currentCharacterData);
            // Write that info into a JSON file , saved to this machine
            _saveFileDataWriter.CreateNewCharacterSaveFile(_currentCharacterData);
        }
        public void DeleteGame(CharacterSlot characterSlot)
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;

            // Choose file based on name
            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            _saveFileDataWriter.DeleteSaveFile();
        }
        // Load all character profiles on device when starting a game 
        private void LoadAllCharacterProfiles()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            _characterSlot01 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            _characterSlot02 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            _characterSlot03 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            _characterSlot04 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            _characterSlot05 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            _characterSlot06 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            _characterSlot07 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            _characterSlot08 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            _characterSlot09 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter._saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            _characterSlot10 = _saveFileDataWriter.LoadSaveFile();
        }
        public void LoadWorldScene(int buildIndex)
        {
            PlayerUIManager.Instance._playerUILoadingScreenManager.ActivateLoadingScreen();
            string worldScene = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            NetworkManager.Singleton.SceneManager.LoadScene(worldScene, LoadSceneMode.Single);

            _playerManager.LoadGameDataFromCurrentCharacterData(ref _currentCharacterData);
        }
        // If you want to use a multi scene setup, there is no current scene index om a new character
        /*private IEnumerator LoadWorldSceneNewGame()
        {

        }*/
        public int GetWorldIndex()
        {
            return _worldSceneIndex;
        }

        public SerializableWeapon GetSerializableWeaponFromWeaponItem(WeaponItem weapon)
        {
            SerializableWeapon serializableWeapon = new SerializableWeapon();

            serializableWeapon._itemID = weapon._itemID;

            // Get ash of war ID if one is present (There is always be one by default)
            if (weapon._ashesOfWarAction != null)
            {
                serializableWeapon._ashOfWarID = weapon._ashesOfWarAction._itemID;
            }
            else
            {
                // We use an invalid ID if there is no ash of war, so the value will bw null if it tries to search for one using ID
                serializableWeapon._ashOfWarID = -1;
            }
            return serializableWeapon;
        }
        public SerializableRangedProjectile GetSerializableRangedProjectileFromRangedProjectileItem(RangedProjectileItem projectile)
        {
            SerializableRangedProjectile serializableProjectile = new SerializableRangedProjectile();

            if (projectile != null)
            {

                serializableProjectile._itemID = projectile._itemID;
                serializableProjectile._itemAmount = projectile._currentAmmoAmount;

            }
            else
            {
                serializableProjectile._itemID = -1;
            }

            return serializableProjectile;
        }

        public SerializableQuickSlotIcon GetSerializableQuickSlotIconFromQuickSLotIcon(QuickSlotItem quickSlot)
        {
            SerializableQuickSlotIcon serializableQuickSlotIcon = new SerializableQuickSlotIcon();

            if (quickSlot != null)
            {
                serializableQuickSlotIcon._itemID = quickSlot._itemID;
                serializableQuickSlotIcon._itemAmount = quickSlot._itemAmount;
            }
            else
            {
                serializableQuickSlotIcon._itemID = -1;
            }
            return serializableQuickSlotIcon;
        }

    }
}