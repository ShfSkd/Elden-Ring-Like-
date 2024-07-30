using SKd;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.MenuScreen
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        [Header("Menus")]
        [SerializeField] GameObject _tiltleScreenMainMenu;
        [SerializeField] GameObject _tiltleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button _loadMenuReturnButton;
        [SerializeField] Button _mainMenuLoadGameButton;
        [SerializeField] Button _mainMenuNewGameButton;
        [SerializeField] Button _deleteCharacterPopUpConfirmButton;

        [Header("Pop Ups")]
        [SerializeField] GameObject _noCharacetSlotPopUp;
        [SerializeField] Button _noCharacterSlotsOkeyButton;
        [SerializeField] GameObject _deleteCharcterSlotPopUp;

        [Header("Character Slots")]
        public CharacterSlot _currentSelectedSlot = CharacterSlot.No_Slot;

       
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
        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.AttempToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            // Close Main menu
            _tiltleScreenMainMenu.SetActive(false);
            // Open Load menu 
            _tiltleScreenLoadMenu.SetActive(true);

            // Select the return button first
            _loadMenuReturnButton.Select();

        }
        public void CloseLoadGameMenu()
        {
            // Close Load menu 
            _tiltleScreenLoadMenu.SetActive(false);
            // Open Main menu
            _tiltleScreenMainMenu.SetActive(true);

            // Select the Load button
            _mainMenuLoadGameButton.Select();
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
            _tiltleScreenLoadMenu.gameObject.SetActive(false);
            _tiltleScreenLoadMenu.gameObject.SetActive(true);

            _loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            _deleteCharcterSlotPopUp.gameObject.SetActive(false );
            _loadMenuReturnButton.Select();
        }
    }

}
