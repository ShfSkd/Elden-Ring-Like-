using System;
using SKD.Character.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.UI.PlayerUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance;
        [HideInInspector] public PlayerManager _localPlayer;
        
        [Header("Network Join")]
        [SerializeField] bool _startGameAsClient;

        [HideInInspector] public PlayerUIHUDManager _playerUIHUDManager;
        [HideInInspector] public PlayerUIPopUpManager _playerUIPopUpManager;
        [HideInInspector] public PlayerUICharacterMenuManager _playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager _playerUIEquipmentManager;
        [HideInInspector] public PlayerUISiteOfGraceManager _playerUISiteOfGraceManager;
        [HideInInspector] public PlayerUITeleportLocationManager _playerUITeleportLocationManager;
        [HideInInspector] public PlayerUILoadingScreenManager _playerUILoadingScreenManager;
        [HideInInspector] public PlayerUILevelUpManager _playerUILevelUpManager;

        [Header("UI Flags")]
        public bool _menuWindowIsOpen;// Inventory Screen , Equipment menu, Shop,etc.
        public bool _popUpWindowIsOpen;// Item pick ups,dialogged pop up etc

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
            _playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            _playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            _playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
            _playerUISiteOfGraceManager = GetComponentInChildren<PlayerUISiteOfGraceManager>();
            _playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
            _playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
            _playerUILevelUpManager = GetComponentInChildren<PlayerUILevelUpManager>();
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (_startGameAsClient)
            {
                _startGameAsClient = false;
                //we must first shut down, because we have to start a host during the tile screen
                NetworkManager.Singleton.Shutdown();
                // We restart as a client
                NetworkManager.Singleton.StartClient();
            }
        }
        public void ClosAllMenuWindows()
        {
            _playerUICharacterMenuManager.CloseMenuAfterFixedUpdate();
            _playerUIEquipmentManager.CloseMenuAfterFixedUpdate();
            _playerUISiteOfGraceManager.CloseMenuAfterFixedUpdate();
            _playerUITeleportLocationManager.CloseMenuAfterFixedUpdate();
            _playerUILevelUpManager.CloseMenuAfterFixedUpdate();
        }
    }

}