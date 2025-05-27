using SKD.UI.PlayerUI;
using SKD.WorldManager;
using SKD.Items.Weapons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SKD.Character.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        PlayerControls _playerControls;
        [FormerlySerializedAs("_playerManager")] public PlayerManager _player;

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
        [SerializeField] bool _switchRightWeapon_Input;
        [SerializeField] bool _switchLeftWeapon_Input;
        [SerializeField] bool _interactInput;
        [SerializeField] bool _useItem_Input = false;

        [Header("Qued Inputs")]
        private bool _input_Que_IsActive;
        [SerializeField] float _default_Que_Input_Time = 0.35f;
        [SerializeField] float _que_Input_Timer;
        [SerializeField] bool _que_RB_Input;
        [SerializeField] bool _que_RT_Input;

        [Header("Bumper Inputs")]
        [SerializeField] bool _RB_Input;
        [SerializeField] bool _LB_Input;
        [SerializeField] bool _hold_RB_Input;
        [SerializeField] bool _hold_LB_Input;

        [Header("Two Handed Inputs")]
        [SerializeField] bool _two_Hand_Input;
        [SerializeField] bool _two_Hand_Right_Weapon_Input;
        [SerializeField] bool _two_Hand_Left_Input;

        [Header("Trigger Inputs")]
        [SerializeField] bool _RT_Input;
        [SerializeField] bool _LT_Input;
        [SerializeField] bool _holdRT_Input;

        [Header("UI Inputs")]
        [SerializeField] bool _openCharcterMenuInput;
        [SerializeField] bool _closeMenuInput;


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

                // Actions
                _playerControls.PlayerActions.Dodge.performed += i => _dodgeInput = true;
                _playerControls.PlayerActions.Sprint.performed += i => _sprintInput = true;
                _playerControls.PlayerActions.SwitchRightWeapon.performed += i => _switchRightWeapon_Input = true;
                _playerControls.PlayerActions.SwitchLeftWeapon.performed += i => _switchLeftWeapon_Input = true;
                _playerControls.PlayerActions.Interact.performed += i => _interactInput = true;
                _playerControls.PlayerActions.X.performed += i => _useItem_Input = true;

                // Bumpers
                _playerControls.PlayerActions.RB.performed += i => _RB_Input = true;
                _playerControls.PlayerActions.LB.performed += i => _LB_Input = true;
                _playerControls.PlayerActions.LB.canceled += i => _player._playerNetworkManager._isBlocking.Value = false;
                _playerControls.PlayerActions.LB.canceled += i => _player._playerNetworkManager._isAiming.Value = false;
                _playerControls.PlayerActions.HoldRB.performed += i => _hold_RB_Input = true;
                _playerControls.PlayerActions.HoldRB.canceled += i => _hold_RB_Input = false;
                _playerControls.PlayerActions.HoldLB.performed += i => _hold_LB_Input = true;
                _playerControls.PlayerActions.HoldLB.canceled += i => _hold_LB_Input = false;

                // Triggers
                _playerControls.PlayerActions.RT.performed += i => _RT_Input = true;
                _playerControls.PlayerActions.LT.performed += i => _LT_Input = true;
                _playerControls.PlayerActions.HoldRT.performed += i => _holdRT_Input = true;
                _playerControls.PlayerActions.HoldRT.canceled += i => _holdRT_Input = false;

                // Two Hand
                _playerControls.PlayerActions.TwoHandedWeapon.performed += i => _two_Hand_Input = true;
                _playerControls.PlayerActions.TwoHandedWeapon.canceled += i => _two_Hand_Input = false;
                _playerControls.PlayerActions.TwoHandRightWeapon.performed += i => _two_Hand_Right_Weapon_Input = true;
                _playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => _two_Hand_Right_Weapon_Input = false;
                _playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => _two_Hand_Left_Input = true;
                _playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => _two_Hand_Left_Input = false;

                // Lock On
                _playerControls.PlayerActions.LockOn.performed += i => _lockOnInput = true;
                _playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => _lockOnLeftInput = true;
                _playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => _lockOnRightInput = true;

                // Holding the input, sets the bool to true
                _playerControls.PlayerActions.Jump.performed += i => _jumpInput = true;
                // Releasing the input, sets the bool to false
                _playerControls.PlayerActions.Sprint.canceled += i => _sprintInput = false;

                // Qued Inputs
                _playerControls.PlayerActions.QueRB.performed += i => QueInput(ref _que_RB_Input);
                _playerControls.PlayerActions.QueRT.performed += i => QueInput(ref _que_RT_Input);

                // UI Inputs
                _playerControls.PlayerActions.Dodge.performed += i => _closeMenuInput = true;
                _playerControls.PlayerActions.OpenCharacterMenu.performed += i => _openCharcterMenuInput = true;
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
            HandleUseItemInput();
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandlePlayerMovementInput();
            HandleCameraMovmentInput();
            HandleRoleInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleHoldRBInput();
            HandleLBInput();
            HandleHoldLBInput();
            HandleRTInput();
            HandleLTInput();
            HandleChargeRTInput();
            HandleSwitchRightInput();
            HandleSwitchLeftInput();
            HanleQueInputs();
            HandleInteractInput();
            HandleCloseUIInputs();
            HandleOpenCharacterMenuInputs();
        }
        //  USE ITEM
        private void HandleUseItemInput()
        {
            if (_useItem_Input)
            {
                _useItem_Input = false;

                if (PlayerUIManager.Instance._menuWindowIsOpen)
                    return;

                if (_player._playerInventoryManager._currentQuickSlotItem != null)
                {
                    _player._playerInventoryManager._currentQuickSlotItem.AttemptToUseItem(_player);

                    //  SEND SERVER RPC SO OUR PLAYER PERFORMS ITEM ACTION ON OTHER CLIENTS GAME WINDOWS
                }
            }
        }

        //Two Hand
        private void HandleTwoHandInput()
        {
            if (!_two_Hand_Input)
                return;

            if (_two_Hand_Right_Weapon_Input)
            {
                // If we are using the two hand input and pressing the right two hand button we want to stop the regular RB input (Or else we should attack)
                _RB_Input = false;
                _two_Hand_Right_Weapon_Input = false;
                _player._playerNetworkManager._isBlocking.Value = false;

                if (_player._playerNetworkManager._isTwoHandingWeapon.Value)
                {
                    // If we are two handing a weapon already, change the twoHanding to false  which trigger an "On Value Changed" function, which un_TwooHanded current Weapon
                    _player._playerNetworkManager._isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    _player._playerNetworkManager._isTwoHandingRightWepoen.Value = true;
                    return;
                }
            }
            else if (_two_Hand_Left_Input)
            {
                // If we are using the two hand input and pressing the right two hand button we want to stop the regular RB input (Or else we should attack)
                _LB_Input = false;
                _two_Hand_Left_Input = false;
                _player._playerNetworkManager._isBlocking.Value = false;

                if (_player._playerNetworkManager._isTwoHandingWeapon.Value)
                {
                    // If we are two handing a weapon already, change the twoHanding to false  which trigger an "On Value Changed" function, which un_TwooHanded current Weapon
                    _player._playerNetworkManager._isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    _player._playerNetworkManager._isTwoHandingLeftWeapon.Value = true;
                    return;
                }
            }
        }

        // Lock On
        private void HandleLockOnInput()
        {
            // Check for dead character
            if (_player._playerNetworkManager._isLockOn.Value)
            {
                if (_player._playerCombatManager._currentTarget == null)
                    return;

                if (_player._playerCombatManager._currentTarget._isDead.Value)
                {
                    _player._playerNetworkManager._isLockOn.Value = false;
                }

                // Attempt to find new targets
                // This assures us that the coroutine never runs multiple times overlapping itself
                if (_lockOnCoroutine != null)
                    StopCoroutine(_lockOnCoroutine);

                _lockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitThenFindNewTarget());
            }

            if (_lockOnInput && _player._playerNetworkManager._isLockOn.Value)
            {
                _lockOnInput = false;
                PlayerCamera.Instance.ClearLockOnTargets();
                _player._playerNetworkManager._isLockOn.Value = false;
                // Disable Lock on
                return;
            }

            if (_lockOnInput && !_player._playerNetworkManager._isLockOn.Value)
            {
                _lockOnInput = false;
                // Enable Lock on

                PlayerCamera.Instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.Instance._nearstLockOnTarget != null)
                {
                    // Set The Target as our current target 
                    _player._playerCombatManager.SetTarget(PlayerCamera.Instance._nearstLockOnTarget);
                    _player._playerNetworkManager._isLockOn.Value = true;
                }
            }
        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (_lockOnLeftInput)
            {
                _lockOnLeftInput = false;

                if (_player._playerNetworkManager._isLockOn.Value)
                {
                    PlayerCamera.Instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.Instance._leftLockOnTarget != null)
                    {
                        _player._playerCombatManager.SetTarget(PlayerCamera.Instance._leftLockOnTarget);
                    }
                }
            }

            if (_lockOnRightInput)
            {
                _lockOnRightInput = false;

                if (_player._playerNetworkManager._isLockOn.Value)
                {
                    PlayerCamera.Instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.Instance._rightLockOnTarget != null)
                    {
                        _player._playerCombatManager.SetTarget(PlayerCamera.Instance._rightLockOnTarget);
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
            if (_player == null) return;

            if (_moveAmount != 0)
                _player._playerNetworkManager._isMoving.Value = true;
            else
                _player._playerNetworkManager._isMoving.Value = false;

            if (!_player._playerLocomotionManager._canRun)
            {
                if (_moveAmount > 0.5f)
                    _moveAmount = 0.5f;

                if (_verticalInput > 0.5f)
                    _verticalInput = 0.5f;

                if (_horizontalInput > 0.5f)
                    _horizontalInput = 0.5f;
            }

            // If we are locked on, only use the move amount 
            if (_player._playerNetworkManager._isLockOn.Value &&
                !_player._playerNetworkManager._isSprinting.Value)
            {
                _player._playerAnimationManager.UpdateAnimatorMovementParameters(_horizontalInput,
                    _verticalInput, _player._playerNetworkManager._isSprinting.Value);
                
                return;
            }
            if (_player._playerNetworkManager._isAiming.Value)
            {
                _player._playerAnimationManager.UpdateAnimatorMovementParameters(_horizontalInput,
                    _verticalInput, _player._playerNetworkManager._isSprinting.Value);
                
                return;
            }
            // If we are not locked on, only use the move amount
            _player._playerAnimationManager.UpdateAnimatorMovementParameters(0, _moveAmount,
                _player._playerNetworkManager._isSprinting.Value);
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

                _player._playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (_sprintInput)
            {
                _player._playerLocomotionManager.HandleSprinting();
            }
            else
            {
                _player._playerNetworkManager._isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (_jumpInput)
            {
                _jumpInput = false;
                // If we have UI window Open, simply return without doing nothing
                if (PlayerUIManager.Instance._menuWindowIsOpen)
                    return;

                // Attempt to perform jump
                _player._playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleRBInput()
        {
            if (_two_Hand_Input)
                return;

            if (_RB_Input)
            {
                _RB_Input = false;
                // TODO: If we have UI Window open return and do nothing

                _player._playerNetworkManager.SetCharacterActionHand(true);

                _player._playerCombatManager.PerformWeaponBasedAction(
                    _player._playerInventoryManager._currentRightHandWeapon._Oh_RB_Action,
                    _player._playerInventoryManager._currentRightHandWeapon);
            }
        }
        private void HandleHoldRBInput()
        {
            if (_hold_RB_Input)
            {
                _player._playerNetworkManager._isChargingRightSpell.Value = true;
                _player._playerNetworkManager._isHoldingArrow.Value = true;
            }
            else
            {
                _player._playerNetworkManager._isChargingRightSpell.Value = false;
                _player._playerNetworkManager._isHoldingArrow.Value = false;

            }
        }
        private void HandleHoldLBInput()
        {
            if (_hold_LB_Input)
            {
                _player._playerNetworkManager._isChargingLeftSpell.Value = true;
            }
            else
            {
                _player._playerNetworkManager._isChargingLeftSpell.Value = false;
            }
        }

        private void HandleLBInput()
        {
            if (_two_Hand_Input)
                return;

            if (_LB_Input)
            {
                _LB_Input = false;
                // TODO: If we have UI Window open return and do nothing

                _player._playerNetworkManager.SetCharacterActionHand(false);

                // If we are two handing the weapon, use the two-handed action
                if (_player._playerNetworkManager._isTwoHandingRightWepoen.Value)
                {
                    _player._playerCombatManager.PerformWeaponBasedAction(
                        _player._playerInventoryManager._currentRightHandWeapon._Oh_LB_Action,
                        _player._playerInventoryManager._currentRightHandWeapon);
                }
                else
                {

                    _player._playerCombatManager.PerformWeaponBasedAction(
                        _player._playerInventoryManager._currentLeftHandWeapon._Oh_LB_Action,
                        _player._playerInventoryManager._currentLeftHandWeapon);

                }
            }
        }

        private void HandleRTInput()
        {
            if (_RT_Input)
            {
                _RT_Input = false;
                // TODO: If we have UI Window open return and do nothing

                _player._playerNetworkManager.SetCharacterActionHand(true);

                _player._playerCombatManager.PerformWeaponBasedAction(
                    _player._playerInventoryManager._currentRightHandWeapon._Oh_RT_Action,
                    _player._playerInventoryManager._currentRightHandWeapon);
            }
        }
        private void HandleLTInput()
        {
            if (_LT_Input)
            {
                _LT_Input = false;

                WeaponItem weaponPerformingAction = _player._playerCombatManager.SelectWeaponToPerformAshOfWar();

                weaponPerformingAction._ashesOfWarAction.AttemptToPerformAction(_player);
            }
        }
        private void HandleChargeRTInput()
        {
            // we only want to check for a charge if we are in an action thats requires it (attacking)
            if (_player._isPerformingAction)
            {
                if (_player._playerNetworkManager._isUsingRightHand.Value)
                {
                    _player._playerNetworkManager._isChargingAttack.Value = _holdRT_Input;
                }
            }
        }

        private void HandleSwitchRightInput()
        {
            if (_switchRightWeapon_Input)
            {
                _switchRightWeapon_Input = false;

                if (PlayerUIManager.Instance._menuWindowIsOpen)
                    return;

                _player._playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftInput()
        {
            if (_switchLeftWeapon_Input)
            {
                _switchLeftWeapon_Input = false;

                if (PlayerUIManager.Instance._menuWindowIsOpen)
                    return;

                _player._playerEquipmentManager.SwitchLeftWeapon();
            }
        }

        private void QueInput(ref bool que_RB_Input)// Passing a reference means we pass a specific bool, and not the value of the bool(true, false)
        {
            // Reset all qued inputs so only one can que at a time
            _que_RB_Input = false;
            _que_RT_Input = false;

            //_que_LB_Input = false;
            //_que_LT_Input = false;

            if (_player._isPerformingAction || _player._playerNetworkManager._isJumping.Value)
            {
                que_RB_Input = true;
                _que_Input_Timer = _default_Que_Input_Time;
                _input_Que_IsActive = true;
            }
        }

        private void ProcessQuedInputs()
        {
            if (_player._isDead.Value)
                return;

            if (_que_RB_Input)
                _RB_Input = true;

            if (_que_RT_Input)
                _RT_Input = true;
        }

        private void HanleQueInputs()
        {
            if (_input_Que_IsActive)
            {
                if (_que_Input_Timer > 0)
                {
                    _que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInputs();
                }
                else
                {
                    _que_RB_Input = false;
                    _que_RT_Input = false;
                    _input_Que_IsActive = false;
                    _que_Input_Timer = 0;
                }
            }
        }

        private void HandleInteractInput()
        {
            if (_interactInput)
            {
                _interactInput = false;

                _player._playerInteractionManager.Interact();
            }
        }
        private void HandleOpenCharacterMenuInputs()
        {
            if (_openCharcterMenuInput)
            {
                _openCharcterMenuInput = false;

                PlayerUIManager.Instance._playerUIPopUpManager.CloseAllPopUpsWindows();
                PlayerUIManager.Instance.ClosAllMenuWindows();
                PlayerUIManager.Instance._playerUICharacterMenuManager.OpenCharacterMenu();
            }
        }
        private void HandleCloseUIInputs()
        {
            if (_closeMenuInput)
            {
                _closeMenuInput = false;

                if (PlayerUIManager.Instance._menuWindowIsOpen)
                {
                    PlayerUIManager.Instance.ClosAllMenuWindows();
                }
            }
        }
    }
}