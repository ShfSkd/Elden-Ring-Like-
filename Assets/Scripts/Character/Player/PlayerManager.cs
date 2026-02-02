using SKD.GameSaving;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using SKD.Items;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
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
                PlayerUIManager.Instance._localPlayer = this;
                WorldSaveGameManager.Instance._playerManager = this;

                // Update the total amount of health or stamina when the stat linked to either changes
                _playerNetworkManager._vigor.OnValueChanged += _playerNetworkManager.SetNewMaxHealthValue;
                _playerNetworkManager._mind.OnValueChanged += _playerNetworkManager.SetNewMaxFocusPointsValue;
                _playerNetworkManager._endurance.OnValueChanged += _playerNetworkManager.SetNewMaxStaminaValue;

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

            // Body
            _playerNetworkManager._hairStyleID.OnValueChanged += _playerNetworkManager.OnHairStyleChanged;
            _playerNetworkManager._hairColorRed.OnValueChanged += _playerNetworkManager.OnHairColorRedChanged;
            _playerNetworkManager._hairColorGreen.OnValueChanged += _playerNetworkManager.OnHairColorGreenChanged;
            _playerNetworkManager._hairColorBlue.OnValueChanged += _playerNetworkManager.OnHairColorBlueChanged;

            // Equipments
            _playerNetworkManager._currentRightHandWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            _playerNetworkManager._currentLeftHandWeaponID.OnValueChanged += _playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            _playerNetworkManager._currentQuickSlotItemID.OnValueChanged += _playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            _playerNetworkManager._isChugging.OnValueChanged += _playerNetworkManager.OnIsChuggingChanged;
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
                _playerNetworkManager._vigor.OnValueChanged -= _playerNetworkManager.SetNewMaxHealthValue;
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
            
            // Body
            _playerNetworkManager._hairStyleID.OnValueChanged -= _playerNetworkManager.OnHairStyleChanged;
            _playerNetworkManager._hairColorRed.OnValueChanged -= _playerNetworkManager.OnHairColorRedChanged;
            _playerNetworkManager._hairColorGreen.OnValueChanged -= _playerNetworkManager.OnHairColorGreenChanged;
            _playerNetworkManager._hairColorBlue.OnValueChanged -= _playerNetworkManager.OnHairColorBlueChanged;

            // Equipments
            _playerNetworkManager._currentRightHandWeaponID.OnValueChanged -= _playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            _playerNetworkManager._currentLeftHandWeaponID.OnValueChanged -= _playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            _playerNetworkManager._currentQuickSlotItemID.OnValueChanged -= _playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            _playerNetworkManager._isChugging.OnValueChanged -= _playerNetworkManager.OnIsChuggingChanged;
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
                        player.LoadOtherCharacterPlayerCharacterWhenJoiningServer();
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

            currenCharacterSaveData._vigor = _playerNetworkManager._vigor.Value;
            currenCharacterSaveData._mind = _playerNetworkManager._mind.Value;
            currenCharacterSaveData._endurance = _playerNetworkManager._endurance.Value;
            currenCharacterSaveData._strength = _playerNetworkManager._strength.Value;
            currenCharacterSaveData._dexterty = _playerNetworkManager._dexterty.Value;
            currenCharacterSaveData._intelligence = _playerNetworkManager._intelligence.Value;
            currenCharacterSaveData._faith = _playerNetworkManager._faith.Value;
            
            currenCharacterSaveData._currentHelathFlaskRemaining = _playerNetworkManager._remainingHealthFlasks.Value;
            currenCharacterSaveData._currentPocusPointFlaskRemaining = _playerNetworkManager._remainingFocusPointsFlasks.Value;

            // Body
            currenCharacterSaveData._hairStyleID = _playerNetworkManager._hairStyleID.Value;
            currenCharacterSaveData._hairColorRedID = _playerNetworkManager._hairColorRed.Value;
            currenCharacterSaveData._hairColorGreenID=_playerNetworkManager._hairColorGreen.Value;
            currenCharacterSaveData._hairColorBlueID = _playerNetworkManager._hairColorBlue.Value;

            // Equipment
            currenCharacterSaveData._headEquipment = _playerNetworkManager._headEquipmentID.Value;
            currenCharacterSaveData._bodyEquipment = _playerNetworkManager._bodyEquipmentID.Value;
            currenCharacterSaveData._legEquipment = _playerNetworkManager._legEquipmentID.Value;
            currenCharacterSaveData._handEquipment = _playerNetworkManager._handEquipmentID.Value;

            currenCharacterSaveData._rightWeaponIndex = _playerInventoryManager._rightHandWeaponIndex;
            currenCharacterSaveData._rightWeapon01 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(_playerInventoryManager._weaponInRigthHandSlots[0]);
            currenCharacterSaveData._rightWeapon02 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(_playerInventoryManager._weaponInRigthHandSlots[1]);
            currenCharacterSaveData._rightWeapon03 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(_playerInventoryManager._weaponInRigthHandSlots[2]);

            currenCharacterSaveData._leftWeaponIndex = _playerInventoryManager._leftHandWeaponIndex;
            currenCharacterSaveData._leftWeapon01 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(_playerInventoryManager._weaponInLeftHandSlots[0]);
            currenCharacterSaveData._leftWeapon02 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(_playerInventoryManager._weaponInLeftHandSlots[1]);
            currenCharacterSaveData._leftWeapon03 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(_playerInventoryManager._weaponInLeftHandSlots[2]);

            currenCharacterSaveData._quickSlotIndex = _playerInventoryManager._quickSlotItemIndex;
            currenCharacterSaveData._quickSlotItem01 = WorldSaveGameManager.Instance.GetSerializableQuickSlotIconFromQuickSLotIcon(_playerInventoryManager._quickSlotItemInQuickSlots[0]);
            currenCharacterSaveData._quickSlotItem02 = WorldSaveGameManager.Instance.GetSerializableQuickSlotIconFromQuickSLotIcon(_playerInventoryManager._quickSlotItemInQuickSlots[1]);
            currenCharacterSaveData._quickSlotItem03 = WorldSaveGameManager.Instance.GetSerializableQuickSlotIconFromQuickSLotIcon(_playerInventoryManager._quickSlotItemInQuickSlots[2]);

            currenCharacterSaveData._mainProjectile = WorldSaveGameManager.Instance.GetSerializableRangedProjectileFromRangedProjectileItem(_playerInventoryManager._mainProjectile);
            currenCharacterSaveData._secondaryProjectile = WorldSaveGameManager.Instance.GetSerializableRangedProjectileFromRangedProjectileItem(_playerInventoryManager._secondaryProjectile);

            if (_playerInventoryManager._currentSpell != null)
                currenCharacterSaveData._currentSpell = _playerInventoryManager._currentSpell._itemID;

            // Clear List before save
            currenCharacterSaveData._weaponInInventory = new List<SerializableWeapon>();
            currenCharacterSaveData._projectileInInventory = new List<SerializableRangedProjectile>();
            currenCharacterSaveData._quickSlotItemInInventory = new List<SerializableQuickSlotIcon>();
            currenCharacterSaveData._headEquipmentInInventory = new List<int>();
            currenCharacterSaveData._bodyEquipmentInInventory = new List<int>();
            currenCharacterSaveData._legEquipmentInInventory = new List<int>();
            currenCharacterSaveData._handsEquipmentInInventory = new List<int>();

            for (int i = 0; i < _playerInventoryManager._itemInTheInventory.Count; i++)
            {
                if (_playerInventoryManager._itemInTheInventory[i] == null)
                    continue;

                WeaponItem weaponInInventory = _playerInventoryManager._itemInTheInventory[i] as WeaponItem;
                RangedProjectileItem projectileInInventory = _playerInventoryManager._itemInTheInventory[i] as RangedProjectileItem;
                QuickSlotItem quickSlotItemInInventory = _playerInventoryManager._itemInTheInventory[i] as QuickSlotItem;
                HeadEquipmentItem headEquipmentInInventory = _playerInventoryManager._itemInTheInventory[i] as HeadEquipmentItem;
                BodyEquipmentItem bodyEquipmentInInventory = _playerInventoryManager._itemInTheInventory[i] as BodyEquipmentItem;
                LegEquipmentItem legEquipmentInInventory = _playerInventoryManager._itemInTheInventory[i] as LegEquipmentItem;
                HandEquipmentItem handEquipmentInInventory = _playerInventoryManager._itemInTheInventory[i] as HandEquipmentItem;

                if (weaponInInventory != null)
                    currenCharacterSaveData._weaponInInventory.Add(WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));

                if (projectileInInventory != null)
                    currenCharacterSaveData._projectileInInventory.Add(WorldSaveGameManager.Instance.GetSerializableRangedProjectileFromRangedProjectileItem(projectileInInventory));

                if (quickSlotItemInInventory != null)
                    currenCharacterSaveData._quickSlotItemInInventory.Add(WorldSaveGameManager.Instance.GetSerializableQuickSlotIconFromQuickSLotIcon(quickSlotItemInInventory));

                if (headEquipmentInInventory != null)
                    currenCharacterSaveData._headEquipmentInInventory.Add(headEquipmentInInventory._itemID);

                if (bodyEquipmentInInventory != null)
                    currenCharacterSaveData._bodyEquipmentInInventory.Add(bodyEquipmentInInventory._itemID);

                if (legEquipmentInInventory != null)
                    currenCharacterSaveData._legEquipmentInInventory.Add(legEquipmentInInventory._itemID);

                if (handEquipmentInInventory != null)
                    currenCharacterSaveData._handsEquipmentInInventory.Add(handEquipmentInInventory._itemID);
            }

        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterSaveData)
        {
            _playerNetworkManager._characterName.Value = currentCharacterSaveData._characterName;
            _playerNetworkManager._isMale.Value = currentCharacterSaveData._isMale;
            _playerBodyManager.ToggleBodyType(currentCharacterSaveData._isMale);// Toggle in case the value is the same as default (OnValueChanged only works when the value is changed)
            Vector3 myPosition = new Vector3(currentCharacterSaveData._xPosition, currentCharacterSaveData._yPosition, currentCharacterSaveData._zPosition);
            transform.position = myPosition;

            _playerNetworkManager._vigor.Value = currentCharacterSaveData._vigor;
            _playerNetworkManager._endurance.Value = currentCharacterSaveData._endurance;
            _playerNetworkManager._mind.Value = currentCharacterSaveData._mind;
            _playerNetworkManager._strength.Value = currentCharacterSaveData._strength;
            _playerNetworkManager._dexterty.Value = currentCharacterSaveData._dexterty;
            _playerNetworkManager._intelligence.Value = currentCharacterSaveData._intelligence;
            _playerNetworkManager._faith.Value = currentCharacterSaveData._faith;
            
            // Body
            _playerNetworkManager._hairStyleID.Value = currentCharacterSaveData._hairStyleID;   
            _playerNetworkManager._hairColorRed.Value = currentCharacterSaveData._hairColorRedID;
            _playerNetworkManager._hairColorGreen.Value = currentCharacterSaveData._hairColorGreenID;
            _playerNetworkManager._hairColorBlue.Value = currentCharacterSaveData._hairColorBlueID;

            // This will be moved when saving and loading is added
            _playerNetworkManager._maxHealth.Value = _playerStatsManager.CalculateHealthBasedOnVigorLevel(_playerNetworkManager._vigor.Value);
            _playerNetworkManager._maxStamina.Value = _playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(_playerNetworkManager._endurance.Value);
            _playerNetworkManager._maxFocusPoints.Value = _playerStatsManager.CalculateFucosPointsBasedOnMindLevel(_playerNetworkManager._mind.Value);
            _playerNetworkManager._currentHealth.Value = currentCharacterSaveData._currentHealth;
            _playerNetworkManager._currentStamina.Value = currentCharacterSaveData._currentStamina;
            _playerNetworkManager._currentFocusPoints.Value = currentCharacterSaveData._currentFocusPoints;

            _playerNetworkManager._remainingHealthFlasks.Value = currentCharacterSaveData._currentHelathFlaskRemaining;
            _playerNetworkManager._remainingFocusPointsFlasks.Value = currentCharacterSaveData._currentFocusPoints;

            // PlayerUIManger.instance._playerUIHUDManager.SetMaxStaminaValue(_playerNetworkManager._maxStamina.Value);

            // Equipment
            if (WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterSaveData._headEquipment))
            {
                HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterSaveData._headEquipment));
                _playerInventoryManager._headEquipment = headEquipment;
            }
            else
            {
                _playerInventoryManager._headEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterSaveData._bodyEquipment))
            {
                BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterSaveData._bodyEquipment));
                _playerInventoryManager._bodyEquipment = bodyEquipment;
            }
            else
            {
                _playerInventoryManager._bodyEquipment = null;
            }
            if (WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterSaveData._legEquipment))
            {
                LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterSaveData._legEquipment));
                _playerInventoryManager._legEquipment = legEquipment;
            }
            else
            {
                _playerInventoryManager._legEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterSaveData._handEquipment))
            {
                HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterSaveData._handEquipment));
                _playerInventoryManager._handEquipment = handEquipment;
            }
            else
            {
                _playerInventoryManager._handEquipment = null;
            }

            // Weapon
            _playerInventoryManager._rightHandWeaponIndex = currentCharacterSaveData._rightWeaponIndex;
            _playerInventoryManager._weaponInRigthHandSlots[0] = currentCharacterSaveData._rightWeapon01.GetWeapon();
            _playerInventoryManager._weaponInRigthHandSlots[1] = currentCharacterSaveData._rightWeapon02.GetWeapon();
            _playerInventoryManager._weaponInRigthHandSlots[2] = currentCharacterSaveData._rightWeapon03.GetWeapon();
            _playerInventoryManager._leftHandWeaponIndex = currentCharacterSaveData._leftWeaponIndex;
            _playerInventoryManager._weaponInLeftHandSlots[0] = currentCharacterSaveData._leftWeapon01.GetWeapon();
            _playerInventoryManager._weaponInLeftHandSlots[1] = currentCharacterSaveData._leftWeapon02.GetWeapon();
            _playerInventoryManager._weaponInLeftHandSlots[2] = currentCharacterSaveData._leftWeapon03.GetWeapon();

            // Quick Slot Item
            _playerInventoryManager._quickSlotItemIndex = currentCharacterSaveData._quickSlotIndex;
            _playerInventoryManager._quickSlotItemInQuickSlots[0] = currentCharacterSaveData._quickSlotItem01.GetQuickSlotItem();
            _playerInventoryManager._quickSlotItemInQuickSlots[1] = currentCharacterSaveData._quickSlotItem02.GetQuickSlotItem();
            _playerInventoryManager._quickSlotItemInQuickSlots[2] = currentCharacterSaveData._quickSlotItem03.GetQuickSlotItem();
            // THis refresh the HUD
            _playerEquipmentManager.LoadQuickSlotEquipment(_playerInventoryManager._quickSlotItemInQuickSlots[_playerInventoryManager._quickSlotItemIndex]);

            _playerInventoryManager._rightHandWeaponIndex = currentCharacterSaveData._rightWeaponIndex;

            if (currentCharacterSaveData._rightWeaponIndex >= 0)
            {
                _playerInventoryManager._currentRightHandWeapon = _playerInventoryManager._weaponInRigthHandSlots[currentCharacterSaveData._rightWeaponIndex];
                _playerNetworkManager._currentRightHandWeaponID.Value = _playerInventoryManager._weaponInRigthHandSlots[currentCharacterSaveData._rightWeaponIndex]._itemID;
            }
            else
            {
                _playerNetworkManager._currentRightHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
            }

            _playerInventoryManager._leftHandWeaponIndex = currentCharacterSaveData._leftWeaponIndex;

            if (currentCharacterSaveData._leftWeaponIndex >= 0)
            {
                _playerInventoryManager._currentLeftHandWeapon = _playerInventoryManager._weaponInLeftHandSlots[currentCharacterSaveData._leftWeaponIndex];
                _playerNetworkManager._currentLeftHandWeaponID.Value = _playerInventoryManager._weaponInLeftHandSlots[currentCharacterSaveData._leftWeaponIndex]._itemID;
            }
            else
            {
                _playerNetworkManager._currentLeftHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
            }


            if (WorldItemDatabase.Instance.GetSpellByID(currentCharacterSaveData._currentSpell))
            {
                SpellItem currentSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(currentCharacterSaveData._currentSpell));
                _playerNetworkManager._currentSpellID.Value = currentSpell._itemID;
            }
            else
            {
                _playerNetworkManager._currentSpellID.Value = -1;

            }

            for (int i = 0; i < currentCharacterSaveData._weaponInInventory.Count; i++)
            {
                WeaponItem weapon = currentCharacterSaveData._weaponInInventory[i].GetWeapon();
                _playerInventoryManager.AddItemsToInventory(weapon);
            }
            for (int i = 0; i < currentCharacterSaveData._projectileInInventory.Count; i++)
            {
                RangedProjectileItem projectile = currentCharacterSaveData._projectileInInventory[i].GetProjectile();
                _playerInventoryManager.AddItemsToInventory(projectile);
            }
            for (int i = 0; i < currentCharacterSaveData._headEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterSaveData._headEquipmentInInventory[i]);
                _playerInventoryManager.AddItemsToInventory(equipment);
            }
            for (int i = 0; i < currentCharacterSaveData._bodyEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterSaveData._bodyEquipmentInInventory[i]);
                _playerInventoryManager.AddItemsToInventory(equipment);
            }
            for (int i = 0; i < currentCharacterSaveData._legEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterSaveData._legEquipmentInInventory[i]);
                _playerInventoryManager.AddItemsToInventory(equipment);
            }
            for (int i = 0; i < currentCharacterSaveData._handsEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterSaveData._handsEquipmentInInventory[i]);
                _playerInventoryManager.AddItemsToInventory(equipment);
            }
            for (int i = 0; i < currentCharacterSaveData._quickSlotItemInInventory.Count; i++)
            {
                QuickSlotItem quickSlotItem = currentCharacterSaveData._quickSlotItemInInventory[i].GetQuickSlotItem();
                _playerInventoryManager.AddItemsToInventory(quickSlotItem);
            }
            // _playerEquipmentManager.EquipArmor();

            _playerEquipmentManager.LoadMainProjectileEquipment(currentCharacterSaveData._mainProjectile.GetProjectile());
            _playerEquipmentManager.LoadSecondaryProjectileEquipment(currentCharacterSaveData._secondaryProjectile.GetProjectile());


            //   _playerBodyManager.ToggleBodyType(currenCharacterSaveData._isMale);
        }
        private void LoadOtherCharacterPlayerCharacterWhenJoiningServer()
        {
            // Sync body types
            _playerNetworkManager.OnIsMaleChanged(false, _playerNetworkManager._isMale.Value);
            _playerNetworkManager.OnHairStyleChanged(0, _playerNetworkManager._hairStyleID.Value);
            _playerNetworkManager.OnHairColorRedChanged(0, _playerNetworkManager._hairColorRed.Value);
            _playerNetworkManager.OnHairColorGreenChanged(0,_playerNetworkManager._hairColorGreen.Value);
            _playerNetworkManager.OnHairColorBlueChanged(0, _playerNetworkManager._hairColorBlue.Value);

            // Sync weapons 
            _playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, _playerNetworkManager._currentRightHandWeaponID.Value);
            _playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, _playerNetworkManager._currentLeftHandWeaponID.Value);
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