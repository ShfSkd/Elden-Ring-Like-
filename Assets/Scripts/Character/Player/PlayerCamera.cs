using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public PlayerManager _player;
        public static PlayerCamera instance;
        public Camera _cameraObject;
        [SerializeField] Transform _cameraPivotTransform;

        [Header("Camera Settings")]
        private float _cameraSmoothSpeed = 1f; // The bigger this number, the longer for the camera to reach its position during movement
        [SerializeField] float _leftAndRightRotationSpeed = 220f;
        [SerializeField] float _upAndDownRotationSpeed = 220f;
        [SerializeField] float _minimumPivot = -30f; // The lowest point to look down
        [SerializeField] float _maximumPivot = 60f; // The Highs point to look up
        [SerializeField] float _cameraCollosionRaduis = 0.2f;
        [SerializeField] LayerMask _collideWithLayers;

        [Header("Camera Values")]
        private Vector3 _cameraVelocity;
        private Vector3 _cameraObjectPosition;// Used for camera Collision (Moved the camera object to this position upon collision 
        [SerializeField] float _leftAndRightLookAngle;
        [SerializeField] float _uptAndDownLookAngle;
        private float _cameraZPosition;
        private float _targetCameraZPosition;

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
            _cameraZPosition = _cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActiond()
        {
            if (_player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }
        }
        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, _player.transform.position, ref _cameraVelocity, _cameraSmoothSpeed * Time.deltaTime);

            transform.position = targetCameraPosition;
        }
        private void HandleRotations()
        {
            _leftAndRightLookAngle += (PlayerInputManager.instance._cameraHorizontalInput * _leftAndRightRotationSpeed) * Time.deltaTime;

            _uptAndDownLookAngle -= (PlayerInputManager.instance._cameraVerticalInput * _upAndDownRotationSpeed) * Time.deltaTime;


            // Clamp the up and down between min and max
            _uptAndDownLookAngle = Mathf.Clamp(_uptAndDownLookAngle, _minimumPivot, _maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // Rotate this gameobject left and right 
            cameraRotation.y = _leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // Rotate this gameobject up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = _uptAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            _cameraPivotTransform.localRotation = targetRotation;

        }
        private void HandleCollisions()
        {
            _targetCameraZPosition = _cameraZPosition;
            RaycastHit hit;
            // Direction for collision Check
            Vector3 direction = _cameraObject.transform.position - _cameraPivotTransform.position;

            direction.Normalize();

            // If we have an object front of the our desire Direction 
            if (Physics.SphereCast(_cameraPivotTransform.position, _cameraCollosionRaduis, direction, out hit, Mathf.Abs(_targetCameraZPosition), 0))
            {
                // If there is, we get our distance from it
                float distanceFromHitObject = Vector3.Distance(_cameraPivotTransform.position, hit.point);

                // We then equate our target z position to the following
                _targetCameraZPosition = -(distanceFromHitObject - _cameraCollosionRaduis);
            }
            // If out target position is less then our collision radius, we subtract our collision radius (making it snap back) 
            if (Mathf.Abs(_targetCameraZPosition) < _cameraCollosionRaduis)
            {
                _targetCameraZPosition = -_cameraCollosionRaduis;
            }

            // We then apply our final position using a lerp over a time of 0.2
            _cameraObjectPosition.z = Mathf.Lerp(_cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);

            _cameraObject.transform.localPosition = _cameraObjectPosition; 
        }
    }
}