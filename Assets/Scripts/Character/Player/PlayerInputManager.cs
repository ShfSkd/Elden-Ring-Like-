using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SKD.Character.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        PlayerControls _playerControls;
        public PlayerManager _playerManager;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 _movementInput;
        public float _verticalInput;
        public float _horizontalInput;
        public float _moveAmount;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 _cameraInput;
        public float _cameraVerticalInput;
        public float _cameraHorizontalInput;

        [Header("Lock On")]
        [SerializeField] bool _lockOnInput;
        [SerializeField] bool _lockOnLeftInput;
        [SerializeField] bool _lockOnRightInput;
        private Coroutine _lockOnCoroutine;

        [Header("Player Actions Inputs")]
        [SerializeField] bool _dodgeInput;
        [SerializeField] bool _sprintInput;
        [SerializeField] bool _jumpInput;
        [SerializeField] bool _RB_Input;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
            Instance.enabled = false;

            if (_playerControls != null)
                _playerControls.Disable();
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldIndex())
            {
                Instance.enabled = true;

                if (_playerControls != null)
                    _playerControls.Enable();
            }
            else
            {
                Instance.enabled = false;

                if (_playerControls != null)
                    _playerControls.Disable();
            }
        }
        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.PlayerMovement.Movment.performed += i => _movementInput = i.ReadValue<Vector2>();

                _playerControls.PlayerCamera.Movement.performed += i => _cameraInput = i.ReadValue<Vector2>();

                _playerControls.PlayerActions.Dodge.performed += i => _dodgeInput = true;
                // Holding the input, sets the bool to true
                _playerControls.PlayerActions.Sprint.performed += i => _sprintInput = true;
                // Releasing the input, sets the bool to false
                _playerControls.PlayerActions.Sprint.canceled += i => _sprintInput = false;
                // Holding the input, sets the bool to true
                _playerControls.PlayerActions.Jump.performed += i => _jumpInput = true;

                _playerControls.PlayerActions.RB.performed += i => _RB_Input = true;

                // Lock On
                _playerControls.PlayerActions.LockOn.performed += i => _lockOnInput = true;
                _playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => _lockOnLeftInput = true;
                _playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => _lockOnRightInput = true;
            }
            _playerControls.Enable();
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        // If we minimized or lower the window, stop adjusting inputs
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                    _playerControls.Enable();
                else
                    _playerControls.Disable();
            }
        }
        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandlePlayerMovementInput();
            HandleCameraMovmentInput();
            HandleRoleInput();
            HandleSptintInput();
            HandleJumpInput();
            HandleRBInput();
        }
        // Lock On
        private void HandleLockOnInput()
        {
            // Check for dead character
            if (_playerManager._playerNetworkManager._isLockOn.Value)
            {
                if (_playerManager._playerCombatManager._currentTarget == null)
                    return;

                if (_playerManager._playerCombatManager._currentTarget._isDead.Value)
                {
                    _playerManager._playerNetworkManager._isLockOn.Value = false;
                }
                // Attempt to find new targets
                // This assures us that the coroutine never runs multiple times overlapping itself
                if(_lockOnCoroutine!=null)
                    StopCoroutine(_lockOnCoroutine);

                _lockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitThenFindNewTarget());
            }
            if (_lockOnInput && _playerManager._playerNetworkManager._isLockOn.Value)
            {
                _lockOnInput = false;
                PlayerCamera.Instance.ClearLockOnTargets();
                _playerManager._playerNetworkManager._isLockOn.Value = false;
                // Disable Lock on
                return;
            }
            if (_lockOnInput && !_playerManager._playerNetworkManager._isLockOn.Value)
            {
                _lockOnInput = false;
                // Enable Lock on

                PlayerCamera.Instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.Instance._nearstLockOnTarget != null)
                {
                    // Set The Target as our current target 
                    _playerManager._playerCombatManager.SetTarget(PlayerCamera.Instance._nearstLockOnTarget);
                    _playerManager._playerNetworkManager._isLockOn.Value = true;
                }
            }
        }
        private void HandleLockOnSwitchTargetInput()
        {
            if (_lockOnLeftInput)
            {
                _lockOnLeftInput = false;

                if (_playerManager._playerNetworkManager._isLockOn.Value)
                {
                    PlayerCamera.Instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.Instance._leftLockOnTarget != null)
                    {
                        _playerManager._playerCombatManager.SetTarget(PlayerCamera.Instance._leftLockOnTarget);
                    }
                }
            }

            if (_lockOnRightInput)
            {
                _lockOnRightInput = false;

                if (_playerManager._playerNetworkManager._isLockOn.Value)
                {
                    PlayerCamera.Instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.Instance._rightLockOnTarget != null)
                    {
                        _playerManager._playerCombatManager.SetTarget(PlayerCamera.Instance._rightLockOnTarget);
                    }
                }
            }
        }
        // Movement
        private void HandlePlayerMovementInput()
        {
            _verticalInput = _movementInput.y;
            _horizontalInput = _movementInput.x;

            _moveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));

            if (_moveAmount <= 0.5f && _moveAmount > 0)
                _moveAmount = 0.5f;
            else if (_moveAmount > 0.5f && _moveAmount <= 1)
                _moveAmount = 1;


            // Why do we pass 0 on the horizontal? because we only want non-strafing movement 
            // We use the horizontal when we are strafing or locked on 
            if (_playerManager == null) return;
            // If we are not locked on, only use the move amount 
            if (!_playerManager._playerNetworkManager._isLockOn.Value || _playerManager._playerNetworkManager._isSprinting.Value)
            {
                _playerManager._playerAnimationManager.UpdateAnimatorMovementParameters(0, _moveAmount, _playerManager._playerNetworkManager._isSprinting.Value);
            }
            else
            // If are locked on pass the horizontal as well 
            {
                _playerManager._playerAnimationManager.UpdateAnimatorMovementParameters(_horizontalInput, _verticalInput, _playerManager._playerNetworkManager._isSprinting.Value);

            }

        }

        private void HandleCameraMovmentInput()
        {
            _cameraVerticalInput = _cameraInput.y;
            _cameraHorizontalInput = _cameraInput.x;
        }

        // Actions
        private void HandleRoleInput()
        {
            if (_dodgeInput)
            {
                _dodgeInput = false;

                _playerManager._playerLocamotionManager.AttampToPerformDodge();
            }
        }
        private void HandleSptintInput()
        {
            if (_sprintInput)
            {
                _playerManager._playerLocamotionManager.HandleSprinting();
            }
            else
            {
                _playerManager._playerNetworkManager._isSprinting.Value = false;
            }
        }
        private void HandleJumpInput()
        {
            if (_jumpInput)
            {
                _jumpInput = false;

                // If we have UI window Open, simply return without doing nothing

                // Attempt to perform jump
                _playerManager._playerLocamotionManager.AttampToPerformJump();
            }
        }
        private void HandleRBInput()
        {
            if (_RB_Input && Application.isPlaying)
            {
                _RB_Input = false;
                // TODO: If we have UI Window open return and do nothing

                _playerManager._playerNetworkManager.SetCharacterActionHand(true);

                _playerManager._playerCombatManager.PerformWeaponBasedAction(_playerManager._playerInventoryManager._currentRightHandWeapon._keyboard_RB_Action, _playerManager._playerInventoryManager._currentRightHandWeapon);
            }
        }
    }

}
