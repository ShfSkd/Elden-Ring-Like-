using SKD.Game_Saving;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SKD.Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] bool _respawnCharacter = false;
        [SerializeField] bool _switchRightWeapon = false;

        [HideInInspector] public PlayerAnimatorManager _playerAnimationManager;
        [HideInInspector] public PlayerLocomotionManager _playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager _playerNetworkManager;
        [HideInInspector] public PlayerStatsManager _playerStatsManager;
        [HideInInspector] public PlayerInventoryManager _playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager _playerEquiqmentManager;
        [HideInInspector] public PlayerCombatManager _playerCombatManager;

        protected override void Awake()
        {
            base.Awake();

            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();

            _playerAnimationManager = GetComponent<PlayerAnimatorManager>();

            _playerNetworkManager = GetComponent<PlayerNetworkManager>();

            _playerStatsManager = GetComponent<PlayerStatsManager>();

            _playerInventoryManager = GetComponent<PlayerInventoryManager>();

            _playerEquiqmentManager = GetComponent<PlayerEquipmentManager>();

            _playerCombatManager = GetComponent<PlayerCombatManager>();
        }
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameObject, we do not control or edit it
            if (!IsOwner)
                return;

            _playerLocomotionManager.HandleAllMovement();
            // Regenerate Stamina
            _playerStatsManager.RegenerateStamina();

          //  DebugMenu();
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
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            Debug.Log("Network object spawned. Setting up event handlers.");

            // If this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.Instance._playerManager = this;
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

            // Lock On 
            _playerNetworkManager._isLockOn.OnValueChanged += _playerNetworkManager.OnIsLockOnChanged;
            _playerNetworkManager._currentTargetNetworkObjectID.OnValueChanged += _playerNetworkManager.OnLockOnTargetIDChange;

            // Equipments
            _playerNetworkManager._currentRightWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            _playerNetworkManager._currentLeftWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            _playerNetworkManager._currentWeaponBeingUsed.OnValueChanged += _playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

            // Flags
            _playerNetworkManager._isChargingAttack.OnValueChanged += _playerNetworkManager.OnIsCharagingAttackChanged;

            // Upon connecting, If we are the owner of this character, But we are not the server, reload our character data to this newly instantiated character
            // We don't run it if we are the server, because since they are the host, they are already loaded in and don't need to reload their data
            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.Instance._currentCharacterData);
            }
            Debug.Log("Event handlers set up completed.");
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

            // If this is the player object owned by this client
            if (IsOwner)
            {
                // Update the total amount of health or stamina when the stat linked to either changes
                _playerNetworkManager._vitality.OnValueChanged -= _playerNetworkManager.SetNewMaxHealthValue;
                _playerNetworkManager._endurance.OnValueChanged -= _playerNetworkManager.SetNewMaxStaminaValue;

                // Updated UI stat bars when a stat changes(Health or stamina)
                _playerNetworkManager._currentHealth.OnValueChanged -= PlayerUIManger.instance._playerUIHUDManager.SetNewHealthValue;
                _playerNetworkManager._currentStamina.OnValueChanged -= PlayerUIManger.instance._playerUIHUDManager.SetNewStaminaValue;
                _playerNetworkManager._currentStamina.OnValueChanged -= _playerStatsManager.ResetStaminaReganTimer;

            }
            // Stats 
            _playerNetworkManager._currentHealth.OnValueChanged -= _playerNetworkManager.CheckHP;

            // Lock On 
            _playerNetworkManager._isLockOn.OnValueChanged -= _playerNetworkManager.OnIsLockOnChanged;
            _playerNetworkManager._currentTargetNetworkObjectID.OnValueChanged -= _playerNetworkManager.OnLockOnTargetIDChange;

            // Equipments
            _playerNetworkManager._currentRightWeaponID.OnValueChanged -= _playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            _playerNetworkManager._currentLeftWeaponID.OnValueChanged -= _playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            _playerNetworkManager._currentWeaponBeingUsed.OnValueChanged -= _playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

            // Flags
            _playerNetworkManager._isChargingAttack.OnValueChanged -= _playerNetworkManager.OnIsCharagingAttackChanged;
        }
        private void OnClientConnectedCallback(ulong clientID)
        {
            WorldGameSessionManager.Instance.AddPlayerToActivePlayerList(this);
            // keep a list of active players in the game
            // If we are the server, we are the host, so we don't need to load players to sync them
            // You only need to load other players gear to sync it if you join a game thats already been active without you being present 
            if (!IsServer && IsOwner)
            {
                foreach (PlayerManager player in WorldGameSessionManager.Instance._players)
                {
                    if (player != this)
                    {
                        player.LoadOtherCharacterPlayerCharaceterWhenJoininigServer();
                    }
                }
            }
        }
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
            if (IsOwner)
            {
                _isDead.Value = false;
                _playerNetworkManager._currentHealth.Value = _playerNetworkManager._maxHealth.Value;
                _playerNetworkManager._currentStamina.Value = _playerNetworkManager._maxStamina.Value;
                _playerAnimationManager.PlayTargetActionAnimation("Empty", false);
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
        private void LoadOtherCharacterPlayerCharaceterWhenJoininigServer()
        {
            // Sync weapons 
            _playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, _playerNetworkManager._currentRightWeaponID.Value);
            _playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, _playerNetworkManager._currentLeftWeaponID.Value);


            // Lock On 
            if(_playerNetworkManager._isLockOn.Value)
            {
                _playerNetworkManager.OnLockOnTargetIDChange(0,_playerNetworkManager._currentTargetNetworkObjectID.Value);
            }
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
        private void PlayDamageSFX()
        {
            // Priorities: If fire damage is grater then 0 , play burn SFX (and for lightning..etc)  
        }

    }
}