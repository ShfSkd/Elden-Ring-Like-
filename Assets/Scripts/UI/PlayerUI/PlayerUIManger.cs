using System;
using Unity.Netcode;
using UnityEngine;

namespace SKD.UI.PlayerUI
{
    public class PlayerUIManger : MonoBehaviour
    {
        public static PlayerUIManger instance;

        [Header("Network Join")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHUDManager _playerUIHUDManager;
        [HideInInspector] public PlayerUIPopUpManager _playerUIPopUpManager;

        [Header("UI Flags")]
        public bool _menuWindowIsOpen; // Inventory Screen , Equipment menu, Shop,etc.
        public bool _popUpWindowIsOpen;// Item pick ups,dialogged pop up etc

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            _playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
            _playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
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
    }

}
