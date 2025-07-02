using Unity.VisualScripting;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerLocomotionManager : CharacterLocamotionManager
    {
        PlayerManager _player;

        // This values will take from the input manager
        [HideInInspector] public float _verticalMovement;
        [HideInInspector] public float _horizontalMovement;
        [HideInInspector] public float _moveAmount;

        [Header("Movement Settings")] private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        [SerializeField] float _walkingSpeed = 2f;
        [SerializeField] float _runningSpeed = 5f;
        [SerializeField] float _sprintingSpeed = 6.5f;
        [SerializeField] float _rotationSpeed = 15f;
        [SerializeField] int _sprintingStaminaCost = 2;

        [Header("Jump")] [SerializeField] float _jumpStaminaCost = 25f;
        [SerializeField] float _jumpHeight = 4;
        [SerializeField] float _jumpForwardSpeed = 5;
        [SerializeField] float _freeFallSpeed = 2;
        private Vector3 _jumpDirection;


        [Header("Dodge")] private Vector3 _rollDirection;
        [SerializeField] float _dodgeStaminaCost = 25f;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (_player.IsOwner)
            {
                _player._characterNetworkManager._verticalMovement.Value = _verticalMovement;
                _player._characterNetworkManager._horizontalMovement.Value = _horizontalMovement;
                _player._characterNetworkManager._moveAmount.Value = _moveAmount;
            }
            else
            {
                _verticalMovement = _player._characterNetworkManager._verticalMovement.Value;
                _horizontalMovement = _player._characterNetworkManager._horizontalMovement.Value;
                _moveAmount = _player._characterNetworkManager._moveAmount.Value;

                // If not locked on, pass move amount
                if (!_player._playerNetworkManager._isLockOn.Value ||
                    _player._playerNetworkManager._isSprinting.Value)
                {
                    _player._playerAnimationManager.UpdateAnimatorMovementParameters(0, _moveAmount,
                        _player._playerNetworkManager._isSprinting.Value);
                }
                // if locked on, pass horizontal and vertical
                else
                {
                    _player._playerAnimationManager.UpdateAnimatorMovementParameters(_horizontalMovement,
                        _verticalMovement, _player._playerNetworkManager._isSprinting.Value);
                }
            }
        }

        public void HandleAllMovement()
        {
            if (_player._isPerformingAction)
                return;

            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovment();
        }

        private void GetMovementValues()
        {
            _verticalMovement = PlayerInputManager.Instance._verticalInput;
            _horizontalMovement = PlayerInputManager.Instance._horizontalInput;
            _moveAmount = PlayerInputManager.Instance._moveAmount;
        }

        private void HandleGroundedMovement()
        {
            if (_player._playerLocomotionManager._canMove || _player._playerLocomotionManager._canRotate)
                GetMovementValues();

            if (!_player._playerLocomotionManager._canMove)
                return;

            if (_player._playerNetworkManager._isAiming.Value)
            {
                _moveDirection = transform.forward * _verticalMovement;
                _moveDirection += transform.right * _horizontalMovement;
                _moveDirection.Normalize();
                _moveDirection.y = 0;

            }
            else
            {
                _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
                _moveDirection += PlayerCamera.Instance.transform.right * _horizontalMovement;
                _moveDirection.Normalize();
                _moveDirection.y = 0;

            }

            if (_player._playerNetworkManager._isSprinting.Value)
            {
                _player._characterController.Move(_moveDirection * (_sprintingSpeed * Time.deltaTime));
            }
            else
            {
                if (PlayerInputManager.Instance._moveAmount > 0.5f)
                {
                    // Move at running speed
                    _player._characterController.Move(_moveDirection * (_runningSpeed * Time.deltaTime));
                }
                else if ((PlayerInputManager.Instance._moveAmount <= 0.5f))
                {
                    // Move at walking speed
                    _player._characterController.Move(_moveDirection * (_walkingSpeed * Time.deltaTime));
                }
            }
        }

        private void HandleJumpingMovement()
        {
            if (_player._playerNetworkManager._isJumping.Value)
            {
                _player._characterController.Move(_jumpDirection * (_jumpForwardSpeed * Time.deltaTime));
            }
        }

        private void HandleFreeFallMovment()
        {
            if (!_player._playerLocomotionManager._isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection =
                    PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance._verticalInput;
                freeFallDirection += PlayerInputManager.Instance.transform.right *
                                     PlayerInputManager.Instance._horizontalInput;

                freeFallDirection.y = 0f;

                _player._characterController.Move(freeFallDirection * (_freeFallSpeed * Time.deltaTime));
            }
        }

        private void HandleRotation()
        {
            if (_player._isDead.Value)
                return;

            if (!_player._playerLocomotionManager._canRotate)
                return;

            if (_player._playerNetworkManager._isAiming.Value)
            {
                HandleAimRotation();
            }
            else
            {
                HandleStandardRotation();
            }

        }
        private void HandleStandardRotation()
        {
            if (_player._playerNetworkManager._isLockOn.Value)
            {
                if (_player._playerNetworkManager._isSprinting.Value ||
                    _player._playerLocomotionManager._isRolling)
                {
                    Vector3 targetDirection;
                    targetDirection = PlayerCamera.Instance._cameraObject.transform.forward * _verticalMovement;
                    targetDirection += PlayerCamera.Instance._cameraObject.transform.right * _horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0f;

                    if (targetDirection == Vector3.zero)
                        targetDirection = transform.forward;

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                        _rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if (_player._playerCombatManager._currentTarget == null)
                        return;

                    Vector3 targetDirection;
                    targetDirection = _player._playerCombatManager._currentTarget.transform.position -
                                      transform.position;
                    targetDirection.Normalize();
                    targetDirection.y = 0f;

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                        _rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
                _targetRotationDirection = Vector3.zero;
                _targetRotationDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;

                _targetRotationDirection += PlayerCamera.Instance._cameraObject.transform.right * _horizontalMovement;

                _targetRotationDirection.y = 0;
                _targetRotationDirection.Normalize();

                if (_targetRotationDirection == Vector3.zero)
                {
                    _targetRotationDirection = transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);

                Quaternion targetRotation =
                    Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * Time.deltaTime);

                transform.rotation = targetRotation;
            }
        }
        private void HandleAimRotation()
        {
            Vector3 targetDirection = Vector3.zero;
            targetDirection = PlayerCamera.Instance._cameraObject.transform.forward;
            targetDirection.Normalize();
            targetDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                _rotationSpeed * Time.deltaTime);
            transform.rotation = finalRotation;
        }

        public void HandleSprinting()
        {
            if (_player._isPerformingAction)
            {
                // Set sprinting to false
                _player._playerNetworkManager._isSprinting.Value = false;
            }

            // If we are out of stamina, set sprinting to false
            if (_player._playerNetworkManager._currentStamina.Value <= 0)
            {
                _player._playerNetworkManager._isSprinting.Value = false;
                return;
            }

            // If we are moving set sprinting to true
            if (_moveAmount >= 0.5f)
                _player._playerNetworkManager._isSprinting.Value = true;
            // if we are stationary set sprinting to false
            else
                _player._playerNetworkManager._isSprinting.Value = false;


            if (_player._playerNetworkManager._isSprinting.Value)
                _player._playerNetworkManager._currentStamina.Value -= _sprintingStaminaCost * Time.deltaTime;
        }

        public void AttemptToPerformDodge()
        {
            if (!_canRoll)
                return;

            if (_player._playerNetworkManager._currentStamina.Value <= 0)
                return;
            // If we are moving when we attempt to dodge, we perform a roll
            if (_moveAmount > 0)
            {
                _rollDirection = (PlayerCamera.Instance._cameraObject.transform.forward * _verticalMovement) +
                                 (PlayerCamera.Instance.transform.right * _horizontalMovement);


                _rollDirection.y = 0;
                _rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(_rollDirection);
                _player.transform.rotation = playerRotation;

                _player._playerAnimationManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
                _player._playerLocomotionManager._isRolling = true;
            }
            //  If we are stationary, we performed a backstep
            else
            {
                _player._playerAnimationManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }

            _player._playerNetworkManager._currentStamina.Value -= _dodgeStaminaCost;
            _player._playerNetworkManager.DestroyALlCurrentActionFXServerRpc();
        }

        public void AttemptToPerformJump()
        {
            // If we are performing a general action, we do not want to allow a jump (will change when combat is added)
            if (_player._isPerformingAction)
                return;
            
            if(_player._playerCombatManager._isUsingItem)
                return;
            
            // If we are out of stamina, we do not wish to allow a jump
            if (_player._playerNetworkManager._currentStamina.Value <= 0)
                return;

            // If we are already in a jump, we do not wish to allow a jump again until the current jump has finished
            if (_player._playerNetworkManager.IsOwner && _player._playerNetworkManager._isJumping.Value)
                return;

            // If we are not grounded, we do not wish to allow a jump
            if (!_player._playerLocomotionManager._isGrounded)
                return;
            // If we are 2 handed our weapon, play the 2 handed animation , otherwise play the 1 handed animation (TODO) 
            _player._playerAnimationManager.PlayTargetActionAnimation("Main_Jump_01", false);

            _player._playerNetworkManager._isJumping.Value = true;

            _player._playerNetworkManager._currentStamina.Value -= _jumpStaminaCost;

            _jumpDirection = PlayerCamera.Instance._cameraObject.transform.forward *
                             PlayerInputManager.Instance._verticalInput;
            _jumpDirection += PlayerCamera.Instance._cameraObject.transform.right *
                              PlayerInputManager.Instance._horizontalInput;

            _jumpDirection.y = 0;

            if (_jumpDirection != Vector3.zero)
            {
                // If we are sprinting,jump direction is at full distance
                if (_player._playerNetworkManager._isSprinting.Value)
                {
                    _jumpDirection *= 1;
                }
                // If we are running,jump direction is at half a distance
                else if (PlayerInputManager.Instance._moveAmount > 0.5f)
                {
                    _jumpDirection *= 0.5f;
                }
                // If we are walking,jump direction is at quarter distance
                else if (PlayerInputManager.Instance._moveAmount <= 0.5f)
                {
                    _jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyJumpingVelocity()
        {
            // Apply An Upward Velocity
            _yVelocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravityForce);
            Debug.Log(_player._characterController.velocity);
        }
    }
}