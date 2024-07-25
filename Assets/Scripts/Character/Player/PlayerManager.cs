using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimationManager _playerAnimationManager;
        [HideInInspector] public PlayerLocamotionManager _playerLocamotionManager;
        protected override void Awake()
        {
            base.Awake();

            _playerLocamotionManager = GetComponent<PlayerLocamotionManager>(); 

            _playerAnimationManager = GetComponent<PlayerAnimationManager>();
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
                PlayerInputManager.instance._player = this;
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