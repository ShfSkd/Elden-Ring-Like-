using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager _aiCharacter;
        protected override void Awake()
        {
            base.Awake();
            _aiCharacter = GetComponent<AICharacterManager>();
        }
        private void OnAnimatorMove()
        {
            // Host
            if (_aiCharacter.IsOwner)
            {
                if (!_aiCharacter._characterLocomotionManager._isGrounded)
                    return;

                Vector3 velocity = _aiCharacter._animator.deltaPosition;

                _aiCharacter._characterController.Move(velocity);
                _aiCharacter.transform.rotation *= _aiCharacter._animator.deltaRotation;
            }
            // Client
            else
            {
                if (!_aiCharacter._characterLocomotionManager._isGrounded)
                    return;

                Vector3 velocity = _aiCharacter._animator.deltaPosition;

                _aiCharacter._characterController.Move(velocity);
                _aiCharacter.transform.position = Vector3.SmoothDamp(transform.position, _aiCharacter._characterNetworkManager._networkPosition.Value, ref _aiCharacter._characterNetworkManager._networkPositionVelocity, _aiCharacter._characterNetworkManager._networkPositionSmoothTime);
                _aiCharacter.transform.rotation *= _aiCharacter._animator.deltaRotation;
            }
        }
    }
}