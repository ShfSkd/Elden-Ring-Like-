using SKD.Character.Player.PlayerUI;
using SKD.Items;
using SKD.Items.WeaponItems;
using SKD.World_Manager;
using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager _playerManager;

        public NetworkVariable<FixedString64Bytes> _characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> _currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentRightWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentLeftWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
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
            _maxHealth.Value = _playerManager._playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManger.instance._playerUIHUDManager.SetMaxHealthValue(_maxHealth.Value);
            _currentHealth.Value = _maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            _maxStamina.Value = _playerManager._playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(newEndurance);
            PlayerUIManger.instance._playerUIHUDManager.SetMaxStaminaValue(_maxStamina.Value);
            _currentStamina.Value = _maxStamina.Value;
        }
        public void OnCurrentRightHandWeaponIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _playerManager._playerInventoryManager._currentRightHandWeapon = newWeapon;
            _playerManager._playerEquiqmentManager.LoadRightWepon();

            if (_playerManager.IsOwner)
            {
                PlayerUIManger.instance._playerUIHUDManager.SetRightWeaponQuickSlotIcon(newId);
            }
        }
        public void OnCurrentLeftHandWeaponIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _playerManager._playerInventoryManager._currentLeftHandWeapon = newWeapon;
            _playerManager._playerEquiqmentManager.LoadLeftWepon();

            if (_playerManager.IsOwner)
            {
                PlayerUIManger.instance._playerUIHUDManager.SetLeftWeaponQuickSlotIcon(newId);
            }
        }
        public void OnCurrentWeaponBeingUsedIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _playerManager._playerCombatManager._currentWeaponBeingUsed = newWeapon;

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

            if(weaponAction != null)
            {
                weaponAction.AttampToPerformedAction(_playerManager,WorldItemDatabase.Instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action Is Null, Cannot be performed");
            }
        }
    }
}