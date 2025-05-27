using SKD.GameSaving;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using SKD.Items;
using SKD.Items.Equipment;
using SKD.Items.Weapons;
using SKD.Spells.Items;
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
        [HideInInspector] public PlayerEquipmentManager _playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager _playerCombatManager;
        [HideInInspector] public PlayerInteractionManager _playerInteractionManager;
        [HideInInspector] public PlayerEffectsManager _playerEffectsManager;
        [HideInInspector] public PlayerBodyManager _playerBodyManager;

        protected override void Awake()
        {
            base.Awake();

            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();

            _playerAnimationManager = GetComponent<PlayerAnimatorManager>();

            _playerNetworkManager = GetComponent<PlayerNetworkManager>();

            _playerStatsManager = GetComponent<PlayerStatsManager>();

            _playerInventoryManager = GetComponent<PlayerInventoryManager>();

            _playerEquipmentManager = GetComponent<PlayerEquipmentManager>();

            _playerCombatManager = GetComponent<PlayerCombatManager>();
            _playerInteractionManager = GetComponent<PlayerInteractionManager>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
            _playerBodyManager = GetComponent<PlayerBodyManager>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
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
                PlayerCamera.Instance._player = this;
                PlayerInputManager.Instance._player = this;
                WorldSaveGameManager.Instance._playerManager = this;

                // Update the total amount of health or stamina when the stat linked to either changes
                _playerNetworkManager._vitality.OnValueChanged += _playerNetworkManager.SetNewMaxHealthValue;
                _playerNetworkManager._endurance.OnValueChanged += _playerNetworkManager.SetNewMaxStaminaValue;
                _playerNetworkManager._mind.OnValueChanged += _playerNetworkManager.SetNewMaxFocusPointsValue;

                // Updated UI stat bars when a stat changes(Health or stamina)
                _playerNetworkManager._currentHealth.OnValueChanged += PlayerUIManager.Instance._playerUIHUDManager.SetNewHealthValue;
                _playerNetworkManager._currentStamina.OnValueChanged += PlayerUIManager.Instance._playerUIHUDManager.SetNewStaminaValue;
                _playerNetworkManager._currentStamina.OnValueChanged += _playerStatsManager.ResetStaminaReganTimer;
                _playerNetworkManager._currentFocusPoints.OnValueChanged += PlayerUIManager.Instance._playerUIHUDManager.SetNewFocusPointsValue;
                
                // Reset camera rotation to a standard when aiming is disabled 
                _playerNetworkManager._isAiming.OnValueChanged += _playerNetworkManager.OnIsAimingChanged;


            }

            // Only Update Hp bar if this character is not the local players character
            if (!IsOwner)
                _characterNetworkManager._currentHealth.OnValueChanged += _characterUIManager.OnHPChanged;

            // Body types
            _playerNetworkManager._isMale.OnValueChanged += _playerNetworkManager.OnIsMaleChanged;

            // Stats 
            _playerNetworkManager._currentHealth.OnValueChanged += _playerNetworkManager.CheckHP;

            // Lock On 
            _playerNetworkManager._isLockOn.OnValueChanged += _playerNetworkManager.OnIsLockOnChanged;
            _playerNetworkManager._currentTargetNetworkObjectID.OnValueChanged += _playerNetworkManager.OnLockOnTargetIDChange;

            // Equipments
            _playerNetworkManager._currentRightHandWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            _playerNetworkManager._currentLeftWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            _playerNetworkManager._currentQuickSlotItemID.OnValueChanged += _playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            _playerNetworkManager._currentWeaponBeingUsed.OnValueChanged += _playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            _playerNetworkManager._currentSpellID.OnValueChanged += _playerNetworkManager.OnCurrentSpellIDChange;
            _playerNetworkManager._isBlocking.OnValueChanged += _playerNetworkManager.OnIsBlockingChanged;
            _playerNetworkManager._headEquipmentID.OnValueChanged += _playerNetworkManager.OnHeadEquipmentChanged;
            _playerNetworkManager._bodyEquipmentID.OnValueChanged += _playerNetworkManager.OnBodyEquipmentChanged;
            _playerNetworkManager._legEquipmentID.OnValueChanged += _playerNetworkManager.OnLegEquipmentChanged;
            _playerNetworkManager._handEquipmentID.OnValueChanged += _playerNetworkManager.OnHandEquipmentChanged;

            // Projectiles
            _playerNetworkManager._mainProjectileID.OnValueChanged += _playerNetworkManager.OnMainProjectileIDChange;
            _playerNetworkManager._secondaryProjectileID.OnValueChanged += _playerNetworkManager.OnSecondaryProjectileIDChange;
            _playerNetworkManager._isHoldingArrow.OnValueChanged += _playerNetworkManager.OnIsHoldingArrowChange;

            // Spells
            _playerNetworkManager._isChargingRightSpell.OnValueChanged += _playerNetworkManager.OnIsChargingRightSpellChange;
            _playerNetworkManager._isChargingLeftSpell.OnValueChanged += _playerNetworkManager.OnIsChargingLeftSpellChange;

            // Two Hand
            _playerNetworkManager._isTwoHandingWeapon.OnValueChanged += _playerNetworkManager.OnIsTwoHandingWeaponChanged;
            _playerNetworkManager._isTwoHandingRightWepoen.OnValueChanged += _playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            _playerNetworkManager._isTwoHandingLeftWeapon.OnValueChanged += _playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

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
                _playerNetworkManager._mind.OnValueChanged -= _playerNetworkManager.SetNewMaxFocusPointsValue;


                // Updated UI stat bars when a stat changes(Health or stamina)
                _playerNetworkManager._currentHealth.OnValueChanged -= PlayerUIManager.Instance._playerUIHUDManager.SetNewHealthValue;
                _playerNetworkManager._currentStamina.OnValueChanged -= PlayerUIManager.Instance._playerUIHUDManager.SetNewStaminaValue;
                _playerNetworkManager._currentFocusPoints.OnValueChanged -= PlayerUIManager.Instance._playerUIHUDManager.SetNewFocusPointsValue;
                _playerNetworkManager._currentStamina.OnValueChanged -= _playerStatsManager.ResetStaminaReganTimer;
    
                // Reset camera rotation to a standard when aiming is disabled 
                _playerNetworkManager._isAiming.OnValueChanged -= _playerNetworkManager.OnIsAimingChanged;
            }

            if (!IsOwner)
                _characterNetworkManager._currentHealth.OnValueChanged -= _characterUIManager.OnHPChanged;

            // Body types
            _playerNetworkManager._isMale.OnValueChanged -= _playerNetworkManager.OnIsMaleChanged;

            // Stats 
            _playerNetworkManager._currentHealth.OnValueChanged -= _playerNetworkManager.CheckHP;

            // Lock On 
            _playerNetworkManager._isLockOn.OnValueChanged -= _playerNetworkManager.OnIsLockOnChanged;
            _playerNetworkManager._currentTargetNetworkObjectID.OnValueChanged -= _playerNetworkManager.OnLockOnTargetIDChange;

            // Equipments
            _playerNetworkManager._currentRightHandWeaponID.OnValueChanged -= _playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            _playerNetworkManager._currentQuickSlotItemID.OnValueChanged -= _playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            _playerNetworkManager._currentLeftWeaponID.OnValueChanged -= _playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            _playerNetworkManager._currentWeaponBeingUsed.OnValueChanged -= _playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            _playerNetworkManager._currentSpellID.OnValueChanged -= _playerNetworkManager.OnCurrentSpellIDChange;
            _playerNetworkManager._isBlocking.OnValueChanged -= _playerNetworkManager.OnIsBlockingChanged;
            _playerNetworkManager._headEquipmentID.OnValueChanged -= _playerNetworkManager.OnHeadEquipmentChanged;
            _playerNetworkManager._bodyEquipmentID.OnValueChanged -= _playerNetworkManager.OnBodyEquipmentChanged;
            _playerNetworkManager._legEquipmentID.OnValueChanged -= _playerNetworkManager.OnLegEquipmentChanged;
            _playerNetworkManager._handEquipmentID.OnValueChanged -= _playerNetworkManager.OnHandEquipmentChanged;

            // Projectiles
            _playerNetworkManager._mainProjectileID.OnValueChanged -= _playerNetworkManager.OnMainProjectileIDChange;
            _playerNetworkManager._secondaryProjectileID.OnValueChanged -= _playerNetworkManager.OnSecondaryProjectileIDChange;
            _playerNetworkManager._isHoldingArrow.OnValueChanged = _playerNetworkManager.OnIsHoldingArrowChange;


            // Spells
            _playerNetworkManager._isChargingRightSpell.OnValueChanged -= _playerNetworkManager.OnIsChargingRightSpellChange;
            _playerNetworkManager._isChargingLeftSpell.OnValueChanged -= _playerNetworkManager.OnIsChargingLeftSpellChange;

            // Two Hand
            _playerNetworkManager._isTwoHandingWeapon.OnValueChanged -= _playerNetworkManager.OnIsTwoHandingWeaponChanged;
            _playerNetworkManager._isTwoHandingRightWepoen.OnValueChanged -= _playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            _playerNetworkManager._isTwoHandingLeftWeapon.OnValueChanged -= _playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

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
                PlayerUIManager.Instance._playerUIPopUpManager.SendYouDiedPopUp();
            }


            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currenCharacterSaveData)
        {
            currenCharacterSaveData._sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currenCharacterSaveData._characterName = _playerNetworkManager._characterName.Value.ToString();
            currenCharacterSaveData._isMale = _playerNetworkManager._isMale.Value;

            currenCharacterSaveData._xPosition = transform.position.x;
            currenCharacterSaveData._yPosition = transform.position.y;
            currenCharacterSaveData._zPosition = transform.position.z;

            currenCharacterSaveData._currentHealth = _playerNetworkManager._currentHealth.Value;
            currenCharacterSaveData._currentStamina = _playerNetworkManager._currentStamina.Value;
            currenCharacterSaveData._currentFocusPoints = _playerNetworkManager._currentFocusPoints.Value;

            currenCharacterSaveData._vitality = _playerNetworkManager._vitality.Value;
            currenCharacterSaveData._endurance = _playerNetworkManager._endurance.Value;
            currenCharacterSaveData._mind = _playerNetworkManager._mind.Value;

            // Equipment
            currenCharacterSaveData._headEquipment = _playerNetworkManager._headEquipmentID.Value;
            currenCharacterSaveData._bodyEquipment = _playerNetworkManager._bodyEquipmentID.Value;
            currenCharacterSaveData._legEquipment = _playerNetworkManager._legEquipmentID.Value;
            currenCharacterSaveData._handEquipment = _playerNetworkManager._handEquipmentID.Value;

            currenCharacterSaveData._rightWeaponIndex = _playerInventoryManager._rightHandWeaponIndex;
            currenCharacterSaveData._rightWeapon01 = _playerInventoryManager._weaponInRigthHandSlots[0]._itemID;
            currenCharacterSaveData._rightWeapon02 = _playerInventoryManager._weaponInRigthHandSlots[1]._itemID;
            currenCharacterSaveData._rightWeapon03 = _playerInventoryManager._weaponInRigthHandSlots[2]._itemID;

            currenCharacterSaveData._leftWeaponIndex = _playerInventoryManager._leftHandWeaponIndex;
            currenCharacterSaveData._leftWeapon01 = _playerInventoryManager._weaponInLefthHandSlots[0]._itemID;
            currenCharacterSaveData._leftWeapon02 = _playerInventoryManager._weaponInLefthHandSlots[1]._itemID;
            currenCharacterSaveData._leftWeapon03 = _playerInventoryManager._weaponInLefthHandSlots[2]._itemID;

            if (_playerInventoryManager._currentSpell != null)
                currenCharacterSaveData._currentSpell = _playerInventoryManager._currentSpell._itemID;

        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currenCharacterSaveData)
        {
            _playerNetworkManager._characterName.Value = currenCharacterSaveData._characterName;
            _playerNetworkManager._isMale.Value = currenCharacterSaveData._isMale;
            _playerBodyManager.ToggleBodyType(currenCharacterSaveData._isMale);// Toggle in case the value is the same as default (OnValueChanged only works when the value is changed)
            Vector3 myPosition = new Vector3(currenCharacterSaveData._xPosition, currenCharacterSaveData._yPosition, currenCharacterSaveData._zPosition);
            transform.position = myPosition;

            _playerNetworkManager._vitality.Value = currenCharacterSaveData._vitality;
            _playerNetworkManager._endurance.Value = currenCharacterSaveData._endurance;
            _playerNetworkManager._mind.Value = currenCharacterSaveData._mind;

            // This will be moved when saving and loading is added
            _playerNetworkManager._maxHealth.Value = _playerStatsManager.CalculateHealthBasedOnVitalityLevel(_playerNetworkManager._vitality.Value);
            _playerNetworkManager._maxStamina.Value = _playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(_playerNetworkManager._endurance.Value);
            _playerNetworkManager._maxFocusPoints.Value = _playerStatsManager.CalculateFucosPointsBasedOnMindLevel(_playerNetworkManager._mind.Value);
            _playerNetworkManager._currentHealth.Value = currenCharacterSaveData._currentHealth;
            _playerNetworkManager._currentStamina.Value = currenCharacterSaveData._currentStamina;
            _playerNetworkManager._currentFocusPoints.Value = currenCharacterSaveData._currentFocusPoints;

            // PlayerUIManger.instance._playerUIHUDManager.SetMaxStaminaValue(_playerNetworkManager._maxStamina.Value);

            // Equipment
            if (WorldItemDatabase.Instance.GetHeadEquipmentByID(currenCharacterSaveData._headEquipment))
            {
                HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.Instance.GetHeadEquipmentByID(currenCharacterSaveData._headEquipment));
                _playerInventoryManager._headEquipment = headEquipment;
            }
            else
            {
                _playerInventoryManager._headEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetBodyEquipmentByID(currenCharacterSaveData._bodyEquipment))
            {
                BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.Instance.GetBodyEquipmentByID(currenCharacterSaveData._bodyEquipment));
                _playerInventoryManager._bodyEquipment = bodyEquipment;
            }
            else
            {
                _playerInventoryManager._bodyEquipment = null;
            }
            if (WorldItemDatabase.Instance.GetLegEquipmentByID(currenCharacterSaveData._legEquipment))
            {
                LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.Instance.GetLegEquipmentByID(currenCharacterSaveData._legEquipment));
                _playerInventoryManager._legEquipment = legEquipment;
            }
            else
            {
                _playerInventoryManager._legEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetHandEquipmentByID(currenCharacterSaveData._handEquipment))
            {
                HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.Instance.GetHandEquipmentByID(currenCharacterSaveData._handEquipment));
                _playerInventoryManager._handEquipment = handEquipment;
            }
            else
            {
                _playerInventoryManager._handEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._rightWeapon01))
            {
                WeaponItem rightWeapon01 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._rightWeapon01));
                _playerInventoryManager._weaponInRigthHandSlots[0] = rightWeapon01;
            }
            else
            {
                _playerInventoryManager._weaponInRigthHandSlots[0] = null;

            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._rightWeapon02))
            {
                WeaponItem rightWeapon02 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._rightWeapon02));
                _playerInventoryManager._weaponInRigthHandSlots[1] = rightWeapon02;
            }
            else
            {
                _playerInventoryManager._weaponInRigthHandSlots[1] = null;
            }

            if (WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._rightWeapon03))
            {
                WeaponItem rightWeapon03 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._rightWeapon03));
                _playerInventoryManager._weaponInRigthHandSlots[2] = rightWeapon03;
            }
            else
            {
                _playerInventoryManager._weaponInRigthHandSlots[2] = null;
            }

            if (WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._leftWeapon01))
            {
                WeaponItem leftWeapon01 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._leftWeapon01));
                _playerInventoryManager._weaponInLefthHandSlots[0] = leftWeapon01;
            }
            else
            {
                _playerInventoryManager._weaponInLefthHandSlots[0] = null;
            }

            if (WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._leftWeapon02))
            {
                WeaponItem leftWeapon02 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._leftWeapon02));
                _playerInventoryManager._weaponInLefthHandSlots[1] = leftWeapon02;
            }
            else
            {
                _playerInventoryManager._weaponInLefthHandSlots[1] = null;
            }

            if (WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._leftWeapon03))
            {
                WeaponItem leftWeapon03 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currenCharacterSaveData._leftWeapon03));
                _playerInventoryManager._weaponInLefthHandSlots[2] = leftWeapon03;
            }
            else
            {
                _playerInventoryManager._weaponInLefthHandSlots[2] = null;
            }


            if (WorldItemDatabase.Instance.GetSpellByID(currenCharacterSaveData._currentSpell))
            {
                SpellItem currentSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(currenCharacterSaveData._currentSpell));
                _playerNetworkManager._currentSpellID.Value = currentSpell._itemID;
            }
            else
            {
                _playerNetworkManager._currentSpellID.Value = -1;

            }


            // _playerEquipmentManager.EquipArmor();

            _playerInventoryManager._rightHandWeaponIndex = currenCharacterSaveData._rightWeaponIndex;

            if (currenCharacterSaveData._rightWeaponIndex >= 0)
            {
                _playerNetworkManager._currentRightHandWeaponID.Value = _playerInventoryManager._weaponInRigthHandSlots[currenCharacterSaveData._rightWeaponIndex]._itemID;
            }
            else
            {
                _playerNetworkManager._currentRightHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
            }

            _playerInventoryManager._leftHandWeaponIndex = currenCharacterSaveData._leftWeaponIndex;

            if (currenCharacterSaveData._leftWeaponIndex >= 0)
            {
                _playerNetworkManager._currentLeftWeaponID.Value = _playerInventoryManager._weaponInLefthHandSlots[currenCharacterSaveData._leftWeaponIndex]._itemID;
            }
            else
            {
                _playerNetworkManager._currentLeftWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
            }


            //   _playerBodyManager.ToggleBodyType(currenCharacterSaveData._isMale);
        }
        private void LoadOtherCharacterPlayerCharaceterWhenJoininigServer()
        {
            // Sync body types
            _playerNetworkManager.OnIsMaleChanged(false, _playerNetworkManager._isMale.Value);

            // Sync weapons 
            _playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, _playerNetworkManager._currentRightHandWeaponID.Value);
            _playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, _playerNetworkManager._currentLeftWeaponID.Value);
            _playerNetworkManager.OnCurrentSpellIDChange(0, _playerNetworkManager._currentSpellID.Value);

            // Sync armor 
            _playerNetworkManager.OnHeadEquipmentChanged(0, _playerNetworkManager._headEquipmentID.Value);
            _playerNetworkManager.OnBodyEquipmentChanged(0, _playerNetworkManager._bodyEquipmentID.Value);
            _playerNetworkManager.OnLegEquipmentChanged(0, _playerNetworkManager._legEquipmentID.Value);
            _playerNetworkManager.OnHandEquipmentChanged(0, _playerNetworkManager._handEquipmentID.Value);

            // Sync Projectile
            _playerNetworkManager.OnMainProjectileIDChange(0, _playerNetworkManager._mainProjectileID.Value);
            _playerNetworkManager.OnSecondaryProjectileIDChange(0, _playerNetworkManager._secondaryProjectileID.Value);
            _playerNetworkManager.OnIsHoldingArrowChange(false, _playerNetworkManager._isHoldingArrow.Value);

            // Sync two hand status 
            _playerNetworkManager.OnIsTwoHandingRightWeaponChanged(false, _playerNetworkManager._isTwoHandingRightWepoen.Value);
            _playerNetworkManager.OnIsTwoHandingLeftWeaponChanged(false, _playerNetworkManager._isTwoHandingLeftWeapon.Value);

            // Sync blocking
            _playerNetworkManager.OnIsBlockingChanged(false, _playerNetworkManager._isBlocking.Value);

            // Lock On 
            if (_playerNetworkManager._isLockOn.Value)
            {
                _playerNetworkManager.OnLockOnTargetIDChange(0, _playerNetworkManager._currentTargetNetworkObjectID.Value);
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
                _playerEquipmentManager.SwitchRightWeapon();
            }
        }
        private void PlayDamageSFX()
        {
            // Priorities: If fire damage is grater then 0 , play burn SFX (and for lightning..etc)  
        }

    }
}