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
        [Header("Menus")]
        [SerializeField] GameObject _tiltleScreenMainMenu;
        [SerializeField] GameObject _tiltleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button _loadMenuReturnButton;
        [SerializeField] Button _mainMenuLoadGameButton;
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
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
    }

}
