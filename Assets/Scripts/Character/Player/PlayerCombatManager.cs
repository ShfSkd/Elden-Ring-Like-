using SKD.Items;
using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

namespace SKD.Character.Player
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager _playerManager;
        public WeaponItem _currentWeaponBeingUsed;

        [Header("Flags")]
        public bool _canComboWithMainHandWeapon;
        // public bool _canComboWithOffHandWeapon;
        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
        }
        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (_playerManager.IsOwner)
            {
                // perform the action
                weaponAction.AttampToPerformedAction(_playerManager, weaponPerformingAction);

                // Notify the server we have performed the action, so we perform it from their perspective also 
                _playerManager._playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction._actionID, weaponPerformingAction._itemID);
            }


        }
        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!_playerManager.IsOwner)
                return;
            if (_currentWeaponBeingUsed == null)
                return;

            float staminaDetucted = 0f;

            switch (_currentAttacktype)
            {
                case AttackType.LigthAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }
            Debug.Log("Stamina Drained " + staminaDetucted);
            _playerManager._playerNetworkManager._currentStamina.Value -= Mathf.RoundToInt(staminaDetucted);
        }
        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if (_playerManager.IsOwner)
            {
                PlayerCamera.Instance.SetLockOnCameraHeight();
            }
        }
  
    }
}