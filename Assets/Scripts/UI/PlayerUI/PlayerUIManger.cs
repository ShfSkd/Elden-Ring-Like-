using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.UI.PlayerUI
{
    public class PlayerUIManger : MonoBehaviour
    {
        public static PlayerUIManger instance;

        [Header("Network Join")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHUDManager _playerUIHUDManager;
        [HideInInspector] public PlayerUIPopUpManager _playerUIPopUpManager;
        [HideInInspector] public PlayerUICharacterMenuManager _playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager _playerUIEquipmentManager;

        [Header("UI Flags")]
        public bool _menuWindowIsOpen;// Inventory Screen , Equipment menu, Shop,etc.
        public bool _popUpWindowIsOpen;// Item pick ups,dialogged pop up etc

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            _playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
            _playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            _playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            _playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                //we must first shut down, because we have to start a host during the tile screen
                NetworkManager.Singleton.Shutdown();
                // We restart as a client
                NetworkManager.Singleton.StartClient();
            }
        }
        public void ClosAllMenuWindows()
        {
            _playerUICharacterMenuManager.CloseCharacterMenu();
            _playerUIEquipmentManager.CloseEquipmentManagerMenu();
        }
    }

}