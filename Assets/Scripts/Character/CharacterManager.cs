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
        [HideInInspector] public CharacterAnimatorManager _characterAnimationManager;
        [HideInInspector] public CharacterCombatManager _characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager _characterSoundFXManager;
        [HideInInspector] public CharacterLocamotionManager _characterLocomotionManager;

        [Header("Character Group")]
        public CharacterGruop _characterGroup;

        [Header("Flags")]
        public bool _isPerformingAction = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
            _characterEffectsManager = GetComponent<CharacterEffectsManager>();
            _characterAnimationManager = GetComponent<CharacterAnimatorManager>();
            _characterCombatManager = GetComponent<CharacterCombatManager>();
            _characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            _characterLocomotionManager = GetComponent<CharacterLocamotionManager>();
        }
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }
        protected virtual void Update()
        {
            _animator.SetBool("IsGrounded", _characterLocomotionManager._isGrounded);
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
        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _animator.SetBool("IsMoving", _characterNetworkManager._isMoving.Value);
            _characterNetworkManager.OnIsActiveChange(false, _characterNetworkManager._isActive.Value);

            _characterNetworkManager._isMoving.OnValueChanged += _characterNetworkManager.OnIsMovingChanged;
            _characterNetworkManager._isActive.OnValueChanged += _characterNetworkManager.OnIsActiveChange;
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            _characterNetworkManager._isMoving.OnValueChanged -= _characterNetworkManager.OnIsMovingChanged;
            _characterNetworkManager._isActive.OnValueChanged -= _characterNetworkManager.OnIsActiveChange;
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
        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damagbleCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            // Add all of our damageable  character colliders, to the list that will be used to ignore collisions 
            foreach (Collider collider in damagbleCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }
            // Add our character controller coliider to the list that will be used to ignore collisions
            ignoreColliders.Add(characterControllerCollider);

            // Goes through every collider on the list, and ignore collision with each other
            foreach (Collider collider in ignoreColliders)
            {
                foreach (Collider otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }

    }

}
