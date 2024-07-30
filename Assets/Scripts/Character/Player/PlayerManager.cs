using SKD.Character.Player.PlayerUI;
using SKD.Game_Saving;
using SKD.WorldManager;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SKD.Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimationManager _playerAnimationManager;
        [HideInInspector] public PlayerLocamotionManager _playerLocamotionManager;
        [HideInInspector] public PlayerNetworkManager _playerNetworkManager;
        [HideInInspector] public PlayerStatsManager _playerStatsManager;
        protected override void Awake()
        {
            base.Awake();

            _playerLocamotionManager = GetComponent<PlayerLocamotionManager>();

            _playerAnimationManager = GetComponent<PlayerAnimationManager>();

            _playerNetworkManager = GetComponent<PlayerNetworkManager>();

            _playerStatsManager = GetComponent<PlayerStatsManager>();
        }
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameObject, we do not control or edit it
            if (!IsOwner)
                return;

            _playerLocamotionManager.HandleAllMovement();
            // Regenerate Stamina
            _playerStatsManager.RegenerateStamina();
        }
        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActiond();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.instance._player = this;
                PlayerInputManager.instance._playerManager = this;
                WorldSaveGameManager.Instance._playerManager = this;

                _playerNetworkManager._currentStamina.OnValueChanged += PlayerUIManger.instance._playerUIHUDManager.SetNewStaminaValue;
                _playerNetworkManager._currentStamina.OnValueChanged += _playerStatsManager.ResetStaminaReganTimer;

                // This will be moved when saving and loading is added
                _playerNetworkManager._maxStamina.Value = _playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(_playerNetworkManager._endurance.Value);

                _playerNetworkManager._currentStamina.Value = _playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(_playerNetworkManager._endurance.Value);

                PlayerUIManger.instance._playerUIHUDManager.SetMaxStaminaValue(_playerNetworkManager._maxStamina.Value);

            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currenCharacterSaveData)
        {
            currenCharacterSaveData._sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currenCharacterSaveData._characterName = _playerNetworkManager._characterName.Value.ToString();

            currenCharacterSaveData._xPosition = transform.position.x;
            currenCharacterSaveData._yPosition = transform.position.y;
            currenCharacterSaveData._zPosition = transform.position.z;
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currenCharacterSaveData)
        {
            _playerNetworkManager._characterName.Value = currenCharacterSaveData._characterName;

            Vector3 myPosition = new Vector3(currenCharacterSaveData._xPosition, currenCharacterSaveData._yPosition, currenCharacterSaveData._zPosition);
            transform.position = myPosition;
        }
    }
}