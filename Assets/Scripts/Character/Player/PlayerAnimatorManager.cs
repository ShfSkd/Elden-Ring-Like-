using System;
using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager _playerManager;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }


        private void OnAnimatorMove()
        {
            if (_playerManager._applyRootMotion)
            {
                Vector3 velocity = _playerManager._animator.deltaPosition;
                _playerManager._characterController.Move(velocity);
                _playerManager.transform.rotation *= _playerManager._animator.deltaRotation;
            }
        }
        // Action Event Calls 
        public override void EnableCanDoCombo()
        {
            if (_playerManager._playerNetworkManager._isUsingRightHand.Value)
            {
                _playerManager._playerCombatManager._canComboWithMainHandWeapon = true;
            }
        }
        public override void DisableCanDoCombo()
        {
            _playerManager._playerCombatManager._canComboWithMainHandWeapon = false;
            // _canComboWithOffHandWeapon = false;
        }
    }
}