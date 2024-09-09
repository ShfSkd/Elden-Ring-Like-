using SKD.GameSaving;
using SKD.MenuScreen;
using SKD.WorldManager;
using TMPro;
using UnityEngine;

namespace SKD.MenuScreen
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter _saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot _characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI _characterName;
        public TextMeshProUGUI _timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }
        private void LoadSaveSlots()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter._saveDataDirectoryPath = Application.persistentDataPath;

            // Save Slot 01
            if (_characterSlot == CharacterSlot.CharacterSlot_01)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if(_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot01._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 02
            else if (_characterSlot == CharacterSlot.CharacterSlot_02)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot02._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 03
            else if (_characterSlot == CharacterSlot.CharacterSlot_03)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot03._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 04
            else if (_characterSlot == CharacterSlot.CharacterSlot_04)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot04._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 05
            else if (_characterSlot == CharacterSlot.CharacterSlot_05)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot05._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 06
            else if (_characterSlot == CharacterSlot.CharacterSlot_06)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot06._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }

            }
            // Save Slot 07
            else if (_characterSlot == CharacterSlot.CharacterSlot_07)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot07._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 08
            else if (_characterSlot == CharacterSlot.CharacterSlot_08)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot08._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 09
            else if (_characterSlot == CharacterSlot.CharacterSlot_09)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot09._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // Save Slot 10
            else if (_characterSlot == CharacterSlot.CharacterSlot_10)
            {
                _saveFileDataWriter._saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(_characterSlot);

                // If the file exist, get information from it 
                if (_saveFileDataWriter.ChechTooSeeFileExists())
                {
                    _characterName.text = WorldSaveGameManager.Instance._characterSlot10._characterName;
                }
                // If it is not, disable the GameObject
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance._currentCharacterSlotBeingUsed = _characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }
        public void SelecteCurrentSlot()
        {
            TitleScreenManager.Instance.SelectedCharacterSlot(_characterSlot);
                
        }
    }
}