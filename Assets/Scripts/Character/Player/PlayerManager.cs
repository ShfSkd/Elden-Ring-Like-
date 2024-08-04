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
        [Header("Debug Menu")]
        [SerializeField] bool _respawnCharacter = false;
        [SerializeField] bool _switchRightWeapon = false;

        [HideInInspector] public PlayerAnimationManager _playerAnimationManager;
        [HideInInspector] public PlayerLocamotionManager _playerLocamotionManager;
        [HideInInspector] public PlayerNetworkManager _playerNetworkManager;
        [HideInInspector] public PlayerStatsManager _playerStatsManager;
        [HideInInspector] public PlayerInventoryManager _playerInventoryManager;
        [HideInInspector] public PlayerEquiqmentManager _playerEquiqmentManager;

        protected override void Awake()
        {
            base.Awake();

            _playerLocamotionManager = GetComponent<PlayerLocamotionManager>();

            _playerAnimationManager = GetComponent<PlayerAnimationManager>();

            _playerNetworkManager = GetComponent<PlayerNetworkManager>();

            _playerStatsManager = GetComponent<PlayerStatsManager>();

            _playerInventoryManager = GetComponent<PlayerInventoryManager>();

            _playerEquiqmentManager = GetComponent<PlayerEquiqmentManager>();
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

            DebugMenu();
        }
        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;
            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActiond();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.Instance._player = this;
                PlayerInputManager.Instance._playerManager = this;
                WorldSaveGameManager.Instance._playerManager = this;

                // Update the total amount of health or stamina when the stat linked to either changes
                _playerNetworkManager._vitality.OnValueChanged += _playerNetworkManager.SetNewMaxHealthValue;
                _playerNetworkManager._endurance.OnValueChanged += _playerNetworkManager.SetNewMaxStaminaValue;

                // Updated UI stat bars when a stat changes(Health or stamina)
                _playerNetworkManager._currentHealth.OnValueChanged += PlayerUIManger.instance._playerUIHUDManager.SetNewHealthValue;
                _playerNetworkManager._currentStamina.OnValueChanged += PlayerUIManger.instance._playerUIHUDManager.SetNewStaminaValue;
                _playerNetworkManager._currentStamina.OnValueChanged += _playerStatsManager.ResetStaminaReganTimer;

            }
            // Stats 
            _playerNetworkManager._currentHealth.OnValueChanged += _playerNetworkManager.CheckHP;

            // Equipments
            _playerNetworkManager._currentRightWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentRightHandWeaponIDChanged;
            _playerNetworkManager._currentLeftWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentLedtHandWeaponIDChanged;
        }
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
            if (IsOwner)
            {
                _playerNetworkManager._currentHealth.Value = _playerNetworkManager._maxHealth.Value;
                _playerNetworkManager._currentStamina.Value = _playerNetworkManager._maxStamina.Value;
                _playerAnimationManager.PlayTargetActionAnimation("Empty", false);
                Debug.Log(_isDead.Value);
            }
        }
        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManger.instance._playerUIPopUpmanager.SendYouDiedPopUp();
            }


            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currenCharacterSaveData)
        {
            currenCharacterSaveData._sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currenCharacterSaveData._characterName = _playerNetworkManager._characterName.Value.ToString();

            currenCharacterSaveData._xPosition = transform.position.x;
            currenCharacterSaveData._yPosition = transform.position.y;
            currenCharacterSaveData._zPosition = transform.position.z;

            currenCharacterSaveData._currentHealth = _playerNetworkManager._currentHealth.Value;
            currenCharacterSaveData._currentStamina = _playerNetworkManager._currentStamina.Value;

            currenCharacterSaveData._vitality = _playerNetworkManager._vitality.Value;

            currenCharacterSaveData._endurance = _playerNetworkManager._endurance.Value;
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currenCharacterSaveData)
        {
            _playerNetworkManager._characterName.Value = currenCharacterSaveData._characterName;

            Vector3 myPosition = new Vector3(currenCharacterSaveData._xPosition, currenCharacterSaveData._yPosition, currenCharacterSaveData._zPosition);
            transform.position = myPosition;

            _playerNetworkManager._vitality.Value = currenCharacterSaveData._vitality;

            _playerNetworkManager._endurance.Value = currenCharacterSaveData._endurance;

            // This will be moved when saving and loading is added
            _playerNetworkManager._maxHealth.Value = _playerStatsManager.CalculateHealthBasedOnVitalityLevel(_playerNetworkManager._vitality.Value);


            _playerNetworkManager._maxStamina.Value = _playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(_playerNetworkManager._endurance.Value);

            _playerNetworkManager._currentHealth.Value = currenCharacterSaveData._currentHealth;

            _playerNetworkManager._currentStamina.Value = currenCharacterSaveData._currentStamina;

            PlayerUIManger.instance._playerUIHUDManager.SetMaxStaminaValue(_playerNetworkManager._maxStamina.Value);

        }

        public void DebugMenu()
        {
            if (_respawnCharacter)
            {
                _respawnCharacter = false;
                ReviveCharacter();
            }
            if (_switchRightWeapon)
            {
                _switchRightWeapon = false;
                _playerEquiqmentManager.SwitchRightWeapon();
            }
        }

    }
}