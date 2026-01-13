using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace SKD.MenuScreen
{
    public class TitleMenuPlayerPreviewRotator : MonoBehaviour
    {
        PlayerControls _playerControls;
        
        [Header("Camera Input")]
        [SerializeField] Vector2 _cameraInput;
        [SerializeField] float _horizontalInput;
        
        [Header("Rotation")]
        [SerializeField] float _lookAngle;
        [SerializeField] float _rotationSpeed = 5f;

        void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();

                _playerControls.PlayerCamera.Movement.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }

            _playerControls.Enable();
        }
        void OnDisable()
        {
            _playerControls.Disable();
        }
        void Update()
        {
            _horizontalInput = _cameraInput.x;

            _lookAngle += (_horizontalInput * _rotationSpeed) * Time.deltaTime;

            Vector3 cameraRotation = Vector3.zero;
            cameraRotation.y = _lookAngle;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
        }

    }
}