using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerLocamotionManager : CharacterLocamotionManager
    {
        PlayerManager _playerManager;
        // This values will take from the input manager
        public float _verticalMovement;
        public float _horizontalMovement;
        public float _moveAmount;

        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        [SerializeField] float _walkingSpeed = 2f;
        [SerializeField] float _runningSpeed = 5f;
        [SerializeField] float _rotationSpeed = 15f;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetVerticalAndHorizontalInputs()
        {
            _verticalMovement = PlayerInputManager.instance._verticalInput;
            _horizontalMovement = PlayerInputManager.instance._horizontalInput;
        }
        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();

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
    }
}