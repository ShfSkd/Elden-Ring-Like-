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

        [SerializeField] Vector2 _movementInput;
        public float _verticalInput;
        public float _horizontalInput;
        public float _moveAmount;

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
            HandleMovementInput();
        }
        private void HandleMovementInput()
        {
            _verticalInput = _movementInput.y;
            _horizontalInput = _movementInput.x;

            _moveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));

            if (_moveAmount <= 0.5f && _moveAmount > 0)
                _moveAmount = 0.5f;
            else if (_moveAmount > 0.5f && _moveAmount <= 1)
                _moveAmount = 1;
        }
    }

}
