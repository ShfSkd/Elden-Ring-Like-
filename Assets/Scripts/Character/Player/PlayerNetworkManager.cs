using SKD.Character.Player.PlayerUI;
using SKD.Items;
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
        public NetworkVariable<int> _currentRightWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentLeftWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
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
        public void OnCurrentRightHandWeaponIDChanged(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _playerManager._playerInventoryManager._currentRightHandWeapon = newWeapon;
            _playerManager._playerEquiqmentManager.LoadRightWepon();
        }
        public void OnCurrentLedtHandWeaponIDChanged(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _playerManager._playerInventoryManager._currentLeftHandWeapon = newWeapon;
            _playerManager._playerEquiqmentManager.LoadLeftWepon();
        }
    }
}