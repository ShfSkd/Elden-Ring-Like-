using SKD.Effects;
using SKD.Items;
using SKD.Items.WeaponItems;
using SKD.Spells.Items;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        static readonly int IsTwoHandedWeapon = Animator.StringToHash("IsTwoHandedWeapon");
        PlayerManager _player;

        public NetworkVariable<FixedString64Bytes> _characterName = new NetworkVariable<FixedString64Bytes>("Character",
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> _currentWeaponBeingUsed =
            new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> _currentRightHandWeaponID = new NetworkVariable<int>(0,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> _currentLeftWeaponID = new NetworkVariable<int>(0,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<bool> _isUsingRightHand = new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentSpellID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<bool> _isUsingLeftHand = new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Two Handed Weapons")]
        public NetworkVariable<int> _currentWeaponBeingTwoHanded = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isTwoHandingWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isTwoHandingRightWepoen = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isTwoHandingLeftWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Spells")]
        public NetworkVariable<bool> _isChargingRightSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isChargingLeftSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Armor")]
        public NetworkVariable<bool> _isMale = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _headEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _bodyEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _legEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _handEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<PlayerManager>();
        }
        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if (rightHandedAction)
            {
                _isUsingLeftHand.Value = false;
                _isUsingRightHand.Value = true;
            }
            else
            {
                _isUsingRightHand.Value = false;
                _isUsingLeftHand.Value = true;
            }
        }
        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            _maxHealth.Value = _player._playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManger.instance._playerUIHUDManager.SetMaxHealthValue(_maxHealth.Value);
            _currentHealth.Value = _maxHealth.Value;
        }
        public void SetNewMaxFocusPointsValue(int oldMind, int newMind)
        {
            _maxFocusPoints.Value = _player._playerStatsManager.CalculateFucosPointsBasedOnMindLevel(newMind);
            PlayerUIManger.instance._playerUIHUDManager.SetMaxFocusPointsValue(_maxFocusPoints.Value);
            _currentHealth.Value = _maxFocusPoints.Value;
        }
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            _maxStamina.Value = _player._playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(newEndurance);
            PlayerUIManger.instance._playerUIHUDManager.SetMaxStaminaValue(_maxStamina.Value);
            _currentStamina.Value = _maxStamina.Value;
        }
        public void OnCurrentRightHandWeaponIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _player._playerInventoryManager._currentRightHandWeapon = newWeapon;
            _player._playerEquipmentManager.LoadRightWepon();

            if (_player.IsOwner)
            {
                PlayerUIManger.instance._playerUIHUDManager.SetRightWeaponQuickSlotIcon(newId);
            }
        }
        public void OnCurrentSpellIDChange(int oldId, int newId)
        {
            SpellItem newSpell = null;

            if (WorldItemDatabase.Instance.GetSpellByID(newId))
                newSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(newId));
            
            if (newSpell != null)
                _player._playerInventoryManager._currentSpell = newSpell;

            if (newSpell != null)
            {
                _player._playerInventoryManager._currentSpell = newSpell;

                if (_player.IsOwner)
                    PlayerUIManger.instance._playerUIHUDManager.SetSpellItemQuickSlotIcon(newId);
            }


        }
        public void OnIsChargingRightSpellChange(bool oldValue, bool newValue)
        {
            _player._animator.SetBool("IsChargingRightSpell", _isChargingRightSpell.Value);
        }
        public void OnIsChargingLeftSpellChange(bool oldValue, bool newValue)
        {
            _player._animator.SetBool("IsChargingLeftSpell", _isChargingLeftSpell.Value);
        }
        public void OnCurrentLeftHandWeaponIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _player._playerInventoryManager._currentLeftHandWeapon = newWeapon;
            _player._playerEquipmentManager.LoadLeftWeapon();

            if (_player.IsOwner)
            {
                PlayerUIManger.instance._playerUIHUDManager.SetLeftWeaponQuickSlotIcon(newId);
            }
        }
        public void OnCurrentWeaponBeingUsedIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _player._playerCombatManager._currentWeaponBeingUsed = newWeapon;

            if (_player.IsOwner)
                return;

            if (_player._playerCombatManager._currentWeaponBeingUsed != null)
                _player._playerAnimationManager.UpdateAnimatorController(_player._playerCombatManager
                    ._currentWeaponBeingUsed._weaponAnimator);
        }
        public void OnIsTwoHandingWeaponChanged(bool oldValue, bool newValue)
        {
            if (!_isTwoHandingWeapon.Value)
            {
                if (IsOwner)
                {
                    _isTwoHandingLeftWeapon.Value = false;
                    _isTwoHandingRightWepoen.Value = false;
                }
                _player._playerEquipmentManager.UnTwoHandWeapon();
                _player._playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.Instance._twoHandingEffect._staticEffectID);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.Instance._twoHandingEffect);
                _player._playerEffectsManager.AddStaticEffect(twoHandEffect);
            }

            _player._animator.SetBool(IsTwoHandedWeapon, _isTwoHandingWeapon.Value);
        }
        public void OnIsTwoHandingRightWeaponChanged(bool oldValue, bool newValue)
        {
            if (_isTwoHandingRightWepoen.Value)
                return;

            if (IsOwner)
            {
                _currentWeaponBeingTwoHanded.Value = _currentRightHandWeaponID.Value;
                _isTwoHandingWeapon.Value = true;
            }

            _player._playerInventoryManager._currentTwoHandWeapon = _player._playerInventoryManager._currentRightHandWeapon;
            _player._playerEquipmentManager.TwoHandRightWeapon();
        }
        public void OnIsTwoHandingLeftWeaponChanged(bool oldValue, bool newValue)
        {
            if (!_isTwoHandingLeftWeapon.Value)
                return;

            if (IsOwner)
            {
                _currentWeaponBeingTwoHanded.Value = _currentLeftWeaponID.Value;
                _isTwoHandingWeapon.Value = true;
            }
            _player._playerInventoryManager._currentTwoHandWeapon =
                _player._playerInventoryManager._currentLeftHandWeapon;
            _player._playerEquipmentManager.TwoHandLeftWeapon();
        }
        public void OnHeadEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            HeadEquipmentItem equipment = WorldItemDatabase.Instance.GetHeadEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadHeadEquipment(null);
            }
        }
        public void OnBodyEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            BodyEquipmentItem equipment = WorldItemDatabase.Instance.GetBodyEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadBodyEquipment(null);
            }
        }
        public void OnLegEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            LegEquipmentItem equipment = WorldItemDatabase.Instance.GetLegEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadLegEquipment(null);
            }
        }
        public void OnHandEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            HandEquipmentItem equipment = WorldItemDatabase.Instance.GetHandEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadHandEquipment(null);
            }
        }
        public void OnIsMaleChanged(bool oldStatus, bool newStatus)
        {
            _player._playerBodyManager.ToggleBodyType(_isMale.Value);
        }
        public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsBlockingChanged(oldStatus, newStatus);

            if (IsOwner)
            {
                _player._playerStatsManager._blockingPhysicalAbsorption = _player._playerCombatManager
                    ._currentWeaponBeingUsed._physicalBaseDamageAbsorption;
                _player._playerStatsManager._blockingMagicAbsorption = _player._playerCombatManager
                    ._currentWeaponBeingUsed._magicBaseDamageAbsorption;
                _player._playerStatsManager._blockingFireAbsorption =
                    _player._playerCombatManager._currentWeaponBeingUsed._fireBaseDamageAbsorption;
                _player._playerStatsManager._blockingLightningAbsorption = _player._playerCombatManager
                    ._currentWeaponBeingUsed._lightingBaseDamageAbsorption;
                _player._playerStatsManager._blockingHolyAbsorption =
                    _player._playerCombatManager._currentWeaponBeingUsed._holyBaseDamageAbsorption;
                _player._playerStatsManager._blockingStability =
                    _player._playerCombatManager._currentWeaponBeingUsed._stability;
            }
        }

        // Item Actions
        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            if (IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            // We do not play the action again for the character who called it, because they already played it locally
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.Instance.GetWeponActionItemByID(weaponID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformedAction(_player, WorldItemDatabase.Instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action Is Null, Cannot be performed");
            }
        }
    }
}