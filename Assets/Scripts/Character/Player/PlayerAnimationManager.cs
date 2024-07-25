using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerAnimationManager : CharacterAnimationManager 
    {
        PlayerManager _playerManager;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }
        private void OnAnimatorMove()
        {
            if(_playerManager._applyRootMotion)
            {
                Vector3 velocity = _playerManager._animator.deltaPosition;
                _playerManager._characterController.Move(velocity);
                _playerManager.transform.rotation *= _playerManager._animator.deltaRotation;
            }
        }
    }
}