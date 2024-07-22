using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SKD.Character.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        PlayerControls _playerControls;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 _movementInput;
        public float _verticalInput;
        public float _horizontalInput;
        public float _moveAmount; 

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 _cameraInput;
        public float _cameraVerticalInput;
        public float _cameraHorizontalInput;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
            instance.enabled = false;
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }
        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.PlayerMovement.Movment.performed += i => _movementInput = i.ReadValue<Vector2>();

                _playerControls.PlayerCamera.Movement.performed += i => _cameraInput = i.ReadValue<Vector2>();
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
            if(enabled)
            {
                if (focus)
                    _playerControls.Enable();
                else
                    _playerControls.Disable();
            }
        }
        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovmentInput();
        }
        private void HandlePlayerMovementInput()
        {
            _verticalInput = _movementInput.y;
            _horizontalInput = _movementInput.x;

            _moveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));

            if (_moveAmount <= 0.5f && _moveAmount > 0)
                _moveAmount = 0.5f;
            else if (_moveAmount > 0.5f && _moveAmount <= 1)
                _moveAmount = 1;
        }

        private void HandleCameraMovmentInput()
        {
            _cameraVerticalInput = _cameraInput.y;
            _cameraHorizontalInput = _cameraInput.x;
        }

    }

}
