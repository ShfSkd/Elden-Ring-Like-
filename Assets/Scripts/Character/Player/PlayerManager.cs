using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocamotionManager _playerLocamotionManager;
        protected override void Awake()
        {
            base.Awake();

            _playerLocamotionManager = GetComponent<PlayerLocamotionManager>();
        }
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameObject, we do not control or edit it
            if (!IsOwner)
                return;

            _playerLocamotionManager.HandleAllMovement();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.instance._player = this;
            }
        }
        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActiond();
        }
    }
}