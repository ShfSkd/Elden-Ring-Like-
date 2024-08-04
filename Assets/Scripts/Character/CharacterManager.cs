using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> _isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController _characterController;
        [HideInInspector] public Animator _animator;

        [HideInInspector] public CharacterEffectsManager _characterEffectsManager;
        [HideInInspector] public CharacterNetworkManager _characterNetworkManager;
        [HideInInspector] public CharacterAnimationManager _characterAnimationManager;

        [Header("Flags")]
        public bool _isPerfomingAction = false;
        public bool _isJumping = false;
        public bool _isGrounded = true;
        public bool _applyRootMotion = false;
        public bool _canRotate = true;
        public bool _canMove = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
            _characterEffectsManager = GetComponent<CharacterEffectsManager>();
            _characterAnimationManager = GetComponent<CharacterAnimationManager>();
        }
        protected virtual void Update()
        {
            _animator.SetBool("IsGrounded", _isGrounded);
            //  If this character is being control from our side, then assign its network position to the position of our transform
            if (IsOwner)
            {
                _characterNetworkManager._networkPosition.Value = transform.position;
                _characterNetworkManager._networkRotation.Value = transform.rotation;
            }
            //  If this character is being control from else where, then assign its  position here locally by the position of network transform 
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, _characterNetworkManager._networkPosition.Value,
                  ref _characterNetworkManager._networkPositionVelocity, _characterNetworkManager._networkPositionSmoothTime);

                transform.rotation = Quaternion.Slerp(transform.rotation, _characterNetworkManager._networkRotation.Value, _characterNetworkManager._networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                _characterNetworkManager._currentHealth.Value = 0;
                _isDead.Value = true;

                // Reset any flags you need to reset

                // If we are not grounded, play an aerial death animation
                if (!manuallySelectDeathAnimation)
                {
                    _characterAnimationManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }
            yield return new WaitForSeconds(5);
        }

        public virtual void ReviveCharacter()
        {

        }

    }

}
