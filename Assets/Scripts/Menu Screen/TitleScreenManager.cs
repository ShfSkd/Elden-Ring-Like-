using SKD.Character;
using SKD.Character.Player;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.WorldManager;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SKD.MenuScreen
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        // Main Menu
        [Header("Main Menu  Menus")]
        [SerializeField] GameObject _titleScreenMainMenu;
        [SerializeField] GameObject _titleScreenLoadMenu;
        [SerializeField] GameObject _titleScreenCharacterCreationMenu;

        [Header("Main Menu Buttons")]
        [SerializeField] Button _loadMenuReturnButton;
        [SerializeField] Button _mainMenuLoadGameButton;
        [SerializeField] Button _mainMenuNewGameButton;
        [SerializeField] Button _deleteCharacterPopUpConfirmButton;

        [Header("Main Menu Pop Ups")]
        [SerializeField] GameObject _noCharacetSlotPopUp;
        [SerializeField] Button _noCharacterSlotsOkeyButton;
        [SerializeField] GameObject _deleteCharcterSlotPopUp;

        [Header("Character Creation Main Panel Buttons")]
        [SerializeField] Button _characterNameButton;
        [SerializeField] Button _characterClassButton;
        [SerializeField] Button _characterHairButton;
        [SerializeField] Button _characterHairColorButton;
        [SerializeField] Button _characterSexButton;
        [SerializeField] TextMeshProUGUI _characterSexText;
        [SerializeField] Button _startGameButton;

        [Header("Character Creation Class Panel Buttons")]
        [SerializeField] Button[] _characterClassButtons;
        [SerializeField] Button[] _characterHairButtons;
        [SerializeField] Button[] _characterHairColorsButtons;

        [Header("Character Creation Secondary Panel Menus")]
        [SerializeField] GameObject _characterClassMenu;
        [SerializeField] GameObject _characterHairMenu;
        [SerializeField] GameObject _characterHairColorMenu;
        [SerializeField] GameObject _charcterNameMenu;
        [SerializeField] TMP_InputField _characterNameInputField;

        [Header("Color Sliders")]
        [SerializeField] Slider _redSlider;
        [SerializeField] Slider _greenSlider;
        [SerializeField] Slider _blueSlider;

        [Header("Hidden Gear")]
        HeadEquipmentItem _hiddenHelmet;

        [Header("Character Slots")]
        public CharacterSlot _currentSelectedSlot = CharacterSlot.No_Slot;

        [Header("Classes")]
        public CharacterClass[] _startingClasses;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

        }
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        public void AttemptToCreateNewCharacter()
        {
            if (WorldSaveGameManager.Instance.HasFreeCharacterSlot())
            {
                OpenCharacterCreationMenu();
            }
            else
            {
                DisplayNoFreeCharacterSlotsPopUp();
            }
        }
        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            // Close Main menu
            _titleScreenMainMenu.SetActive(false);
            // Open Load menu 
            _titleScreenLoadMenu.SetActive(true);

            // Select the return button first
            _loadMenuReturnButton.Select();

        }
        public void CloseLoadGameMenu()
        {
            // Close Load menu 
            _titleScreenLoadMenu.SetActive(false);
            // Open Main menu
            _titleScreenMainMenu.SetActive(true);

            // Select the Load button
            _mainMenuLoadGameButton.Select();
        }

        public void ToggleBodyType()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player._playerNetworkManager._isMale.Value = !player._playerNetworkManager._isMale.Value;

            _characterSexText.text = player._playerNetworkManager._isMale.Value ? "Male" : "Female";

        }
        public void OpenTitleScreenMainMenu()
        {
            _titleScreenMainMenu.SetActive(true);
        }
        public void CloseTitleScreenMainMenu()
        {
            _titleScreenMainMenu.SetActive(false);
        }
        private void OpenCharacterCreationMenu()
        {
            CloseTitleScreenMainMenu();
            _titleScreenCharacterCreationMenu.SetActive(true);

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Sets Default body type
            player._playerBodyManager.ToggleBodyType(true);
        }
        public void CloseCharacterCreationMenu()
        {
            _titleScreenCharacterCreationMenu.SetActive(false);
            OpenTitleScreenMainMenu();
        }
        public void OpenChooseCharacterClassSubMenu()
        {
            ToggleCharacterCreationScreenMainMenuButton(false);
            _characterClassMenu.SetActive(true);

            if (_characterClassButtons.Length > 0)
            {
                _characterClassButtons[0].Select();
                _characterClassButtons[0].OnSelect(null);
            }
        }
        public void CloseChooseCharacterClassSubMenu()
        {
            ToggleCharacterCreationScreenMainMenuButton(true);
            _characterClassMenu.SetActive(false);
            _characterClassButton.Select();
            _characterClassButton.OnSelect(null);

        }
        public void OpenChooseHairStyleSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButton(false);
            _characterHairMenu.SetActive(true);

            if (_characterHairButtons.Length > 0)
            {
                _characterHairButtons[0].Select();
                _characterHairButtons[0].OnSelect(null);
            }

            // Store the helmet the player had on
            if (player._playerInventoryManager._headEquipment != null)
                _hiddenHelmet = Instantiate(player._playerInventoryManager._headEquipment);

            // Unequip the helmet and restore the gear
            player._playerInventoryManager._headEquipment = null;
            player._playerEquipmentManager.EquipArmor();
        }
        public void CloseChooseHairStyleSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButton(true);
            _characterHairMenu.SetActive(false);
            _characterHairButton.Select();
            _characterHairButton.OnSelect(null);

            if (_hiddenHelmet != null)
                player._playerInventoryManager._headEquipment = _hiddenHelmet;

            player._playerEquipmentManager.EquipArmor();

        }

        public void OpenChooseHairColorSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButton(false);
            _characterHairColorMenu.SetActive(true);

            if (_characterHairColorsButtons.Length > 0)
            {
                _characterHairColorsButtons[0].Select();
                _characterHairColorsButtons[0].OnSelect(null);
            }

            // Store the helmet the player had on
            if (player._playerInventoryManager._headEquipment != null)
                _hiddenHelmet = Instantiate(player._playerInventoryManager._headEquipment);

            // Unequip the helmet and restore the gear
            player._playerInventoryManager._headEquipment = null;
            player._playerEquipmentManager.EquipArmor();
        }
        public void CloseChooseHairColorSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButton(true);
            _characterHairColorMenu.SetActive(false);
            _characterHairColorButton.Select();
            _characterHairColorButton.OnSelect(null);

            if (_hiddenHelmet != null)
                player._playerInventoryManager._headEquipment = _hiddenHelmet;

            player._playerEquipmentManager.EquipArmor();

        }

        public void OpenChooseNameMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // 1. Disable all main menu button gameObject
            ToggleCharacterCreationScreenMainMenuButton(false);

            // 2. Disable name Button gameObject, and replace it with name field game object 
            _characterNameButton.gameObject.SetActive(false);
            _charcterNameMenu.SetActive(true);

            // 3. Select name Field Object 
            _characterNameInputField.Select();
        }
        public void CloseChooseNameSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // 1. Re-enable all main menu button gameObject
            ToggleCharacterCreationScreenMainMenuButton(true);

            // 2. Enable name Button gameObject, and disable  name field game object 
            _characterNameButton.gameObject.SetActive(false);
            _charcterNameMenu.SetActive(true);

            // 3. Select name Button 
            _characterNameButton.Select();

            player._playerNetworkManager._characterName.Value = _characterNameInputField.text;

        }
        private void ToggleCharacterCreationScreenMainMenuButton(bool status)
        {
            _characterNameButton.enabled = status;
            _characterClassButton.enabled = status;
            _characterHairButton.enabled = status;
            _characterHairColorButton.enabled = status;
            _characterSexButton.enabled = status;
            _startGameButton.enabled = status;
        }
        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            _noCharacetSlotPopUp.SetActive(true);
            _noCharacterSlotsOkeyButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            _noCharacetSlotPopUp.gameObject.SetActive(false);
            _mainMenuNewGameButton.Select();
        }
        // Character Slots
        public void SelectedCharacterSlot(CharacterSlot characterSlot)
        {
            _currentSelectedSlot = characterSlot;
        }
        public void SelectNoSlot()
        {
            _currentSelectedSlot = CharacterSlot.No_Slot;
        }
        public void AttampToDeleteCharacterSlot()
        {
            if (_currentSelectedSlot != CharacterSlot.No_Slot)
            {

                _deleteCharcterSlotPopUp.gameObject.SetActive(true);
                _deleteCharacterPopUpConfirmButton.Select();
            }
        }
        public void DeleteCharacterSlot()
        {
            _deleteCharcterSlotPopUp.gameObject.SetActive(false);
            WorldSaveGameManager.Instance.DeleteGame(_currentSelectedSlot);
            // We disable and then enable the load menu,to refresh the slots (the deleted slots will now become inactive)
            _titleScreenLoadMenu.gameObject.SetActive(false);
            _titleScreenLoadMenu.gameObject.SetActive(true);

            _loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            _deleteCharcterSlotPopUp.gameObject.SetActive(false);
            _loadMenuReturnButton.Select();
        }

        public void SelectClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();


            if (_startingClasses.Length <= 0)
                return;

            _startingClasses[classID].SetClass(player);
            CloseChooseCharacterClassSubMenu();
        }
        public void PreviewClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();


            if (_startingClasses.Length <= 0)
                return;

            _startingClasses[classID].SetClass(player);
        }
        // Character Class
        public void SetCharacterClass(PlayerManager player, int vitality, int endurance, int mind, int strength, int dexterity, int intelligence, int faith,
            WeaponItem[] mainWeaponItems, WeaponItem[] offHandWeaponItems,
            HeadEquipmentItem headEquipment, BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment, HandEquipmentItem handEquipment,
            QuickSlotItem[] quickSlotItems)
        {
            // 0. Clear The Hidden Helmet
            _hiddenHelmet = null;

            // 1. Set this stats
            player._playerNetworkManager._vitality.Value = vitality;
            player._playerNetworkManager._endurance.Value = endurance;
            player._playerNetworkManager._mind.Value = mind;
            player._playerNetworkManager._strength.Value = strength;
            player._playerNetworkManager._dexterty.Value = dexterity;
            player._playerNetworkManager._intelligence.Value = intelligence;
            player._playerNetworkManager._faith.Value = faith;

            // 2. Set the weapons
            player._playerInventoryManager._weaponInRigthHandSlots[0] = Instantiate(mainWeaponItems[0]);
            player._playerInventoryManager._weaponInRigthHandSlots[1] = Instantiate(mainWeaponItems[1]);
            player._playerInventoryManager._weaponInRigthHandSlots[2] = Instantiate(mainWeaponItems[2]);
            player._playerInventoryManager._currentRightHandWeapon = player._playerInventoryManager._weaponInRigthHandSlots[0];
            player._playerNetworkManager._currentRightHandWeaponID.Value = player._playerInventoryManager._weaponInRigthHandSlots[0]._itemID;

            player._playerInventoryManager._weaponInLeftHandSlots[0] = Instantiate(offHandWeaponItems[0]);
            player._playerInventoryManager._weaponInLeftHandSlots[1] = Instantiate(offHandWeaponItems[1]);
            player._playerInventoryManager._weaponInLeftHandSlots[2] = Instantiate(offHandWeaponItems[2]);

            player._playerInventoryManager._currentLeftHandWeapon = player._playerInventoryManager._weaponInLeftHandSlots[0];
            player._playerNetworkManager._currentLeftHandWeaponID.Value = player._playerInventoryManager._weaponInLeftHandSlots[0]._itemID;

            // 3. Set the armor
            if (headEquipment != null)
            {
                HeadEquipmentItem equipment = Instantiate(headEquipment);
                player._playerInventoryManager._headEquipment = equipment;
            }
            else
            {
                player._playerInventoryManager._headEquipment = null;
            }
            if (bodyEquipment != null)
            {
                BodyEquipmentItem equipment = Instantiate(bodyEquipment);
                player._playerInventoryManager._bodyEquipment = equipment;
            }
            else
            {
                player._playerInventoryManager._bodyEquipment = null;
            }
            if (legEquipment != null)
            {
                LegEquipmentItem equipment = Instantiate(legEquipment);
                player._playerInventoryManager._legEquipment = equipment;
            }
            else
            {
                player._playerInventoryManager._legEquipment = null;
            }
            if (handEquipment != null)
            {
                HandEquipmentItem equipment = Instantiate(handEquipment);
                player._playerInventoryManager._handEquipment = equipment;
            }
            else
            {
                player._playerInventoryManager._handEquipment = null;
            }

            player._playerEquipmentManager.EquipArmor();

            // 4. Set the quickSlot item
            player._playerInventoryManager._quickSlotItemIndex = 0;

            if (quickSlotItems[0] != null)
                player._playerInventoryManager._quickSlotItemInQuickSlots[0] = Instantiate(quickSlotItems[0]);
            if (quickSlotItems[1] != null)
                player._playerInventoryManager._quickSlotItemInQuickSlots[1] = Instantiate(quickSlotItems[1]);
            if (quickSlotItems[2] != null)
                player._playerInventoryManager._quickSlotItemInQuickSlots[2] = Instantiate(quickSlotItems[2]);

            player._playerEquipmentManager.LoadQuickSlotEquipment(player._playerInventoryManager._quickSlotItemInQuickSlots[player._playerInventoryManager._quickSlotItemIndex]);
        }

        // Character Hair
        public void SelectHair(int hairID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player._playerNetworkManager._hairStyleID.Value = hairID;
            CloseChooseHairStyleSubMenu();
        }
        public void PreviewHair(int hairID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player._playerNetworkManager._hairStyleID.Value = hairID;
        }
        public void SelectHairColor()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player._playerNetworkManager._hairColorRed.Value = _redSlider.value;
            player._playerNetworkManager._hairColorGreen.Value = _greenSlider.value;
            player._playerNetworkManager._hairColorBlue.Value = _blueSlider.value;

            CloseChooseHairColorSubMenu();
        }
        public void PreviewHairColor()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player._playerNetworkManager._hairColorRed.Value = _redSlider.value;
            player._playerNetworkManager._hairColorGreen.Value = _greenSlider.value;
            player._playerNetworkManager._hairColorBlue.Value = _blueSlider.value;

        }
        public void SetRedColorsSlider(float redValue)
        {
            _redSlider.value = redValue;
        }
        public void SetGreenColorsSlider(float greenValue)
        {
            _greenSlider.value = greenValue;
        }
        public void SetBlueColorsSlider(float blueValue)
        {
            _blueSlider.value = blueValue;
        }
    }

}