using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerLocamotionManager : CharacterLocamotionManager
    {
        PlayerManager _playerManager;

        // This values will take from the input manager
        [HideInInspector] public float _verticalMovement;
        [HideInInspector] public float _horizontalMovement;
        [HideInInspector] public float _moveAmount;

        [Header("Movement Settings")]
        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        [SerializeField] float _walkingSpeed = 2f;
        [SerializeField] float _runningSpeed = 5f;
        [SerializeField] float _rotationSpeed = 15f;

        [Header("Dodge")]
        private Vector3 _rollDirection;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }
        protected override void Update()
        {
            base.Update();

            if (_playerManager.IsOwner)
            {
                _playerManager._characterNetworkManager._verticalMovement.Value = _verticalMovement;
                _playerManager._characterNetworkManager._horizontalMovement.Value = _horizontalMovement;
                _playerManager._characterNetworkManager._moveAmount.Value = _moveAmount;
            }
            else
            {

                _verticalMovement = _playerManager._characterNetworkManager._verticalMovement.Value;

                _horizontalMovement = _playerManager._characterNetworkManager._horizontalMovement.Value;

                _moveAmount = _playerManager._characterNetworkManager._moveAmount.Value;
                // If not locked on, pass move amount
                _playerManager._playerAnimationManager.UpdateAnimatorMovementParameters(0, _moveAmount);

                // if locked on, pass horizontal and vertical
            }

        }
        public void HandleAllMovement()
        {
            if (_playerManager._isPerfomingAction)
                return;

            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetMovementValues()
        {
            _verticalMovement = PlayerInputManager.instance._verticalInput;
            _horizontalMovement = PlayerInputManager.instance._horizontalInput;
            _moveAmount = PlayerInputManager.instance._moveAmount;
        }
        private void HandleGroundedMovement()
        {
            if (!_playerManager._canMove)
                return;

            GetMovementValues();

            _moveDirection = PlayerCamera.instance.transform.forward * _verticalMovement;

            _moveDirection += PlayerCamera.instance.transform.right * _horizontalMovement;

            _moveDirection.Normalize();
            _moveDirection.y = 0;

            if (PlayerInputManager.instance._moveAmount > 0.5f)
            {
                // Move at running speed
                _playerManager._characterController.Move(_moveDirection * _runningSpeed * Time.deltaTime);
            }
            else if ((PlayerInputManager.instance._moveAmount <= 0.5f))
            {
                // Move at walking speed
                _playerManager._characterController.Move(_moveDirection * _walkingSpeed * Time.deltaTime);
            }
        }
        private void HandleRotation()
        {
            if (!_playerManager._canRotate)
                return;

            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = PlayerCamera.instance.transform.forward * _verticalMovement;

            _targetRotationDirection += PlayerCamera.instance._cameraObject.transform.right * _horizontalMovement;

            _targetRotationDirection.y = 0;
            _targetRotationDirection.Normalize();

            if (_targetRotationDirection == Vector3.zero)
            {
                _targetRotationDirection = transform.forward;
            }
            Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);

            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;

        }
        public void AttampToPerformDodge()
        {
            if (_playerManager._isPerfomingAction)
                return;
            // If we are moving when we attempt to dodge, we perform a roll
            if (_moveAmount > 0)
            {
                _rollDirection = PlayerCamera.instance._cameraObject.transform.forward * _verticalMovement;
                _rollDirection = PlayerCamera.instance.transform.right * _horizontalMovement;

                _rollDirection.y = 0;
                _rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(_rollDirection);
                _playerManager.transform.rotation = playerRotation;

                _playerManager._playerAnimationManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            //  If we are stationary, we performed a backstep
            else
            {
                _playerManager._playerAnimationManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }
        }
    }
}