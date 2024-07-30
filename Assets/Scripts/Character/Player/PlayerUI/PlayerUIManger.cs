using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character.Player.PlayerUI
{
    public class PlayerUIManger : MonoBehaviour
    {
        public static PlayerUIManger instance;

        [Header("Network Join")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHUDManager _playerUIHUDManager;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            _playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
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
