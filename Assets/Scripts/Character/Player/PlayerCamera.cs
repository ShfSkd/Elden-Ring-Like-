using SKD.World_Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

namespace SKD.Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        public PlayerManager _player;
        public Camera _cameraObject;
        public Transform _cameraPivotTransform;
        public float _cameraPivotYPositionOffset = 1.5f;
        [Header("Camera Settings")]
        [SerializeField] float _cameraSmoothSpeed = 1f;// The bigger this number, the longer for the camera to reach its position during movement
        [SerializeField] float _leftAndRightRotationSpeed = 220f;
        [SerializeField] float _upAndDownRotationSpeed = 220f;
        [SerializeField] float _minimumPivot = -30f;// The lowest point to look down
        [SerializeField] float _maximumPivot = 60f;// The Highs point to look up
        [SerializeField] float _cameraCollosionRaduis = 0.2f;
        [SerializeField] LayerMask _collideWithLayers;

        [Header("Camera Values")]
        private Vector3 _cameraVelocity;
        private Vector3 _cameraObjectPosition;// Used for camera Collision (Moved the camera object to this position upon collision 
        [SerializeField] float _leftAndRightLookAngle;
        [SerializeField] float _uptAndDownLookAngle;
        private float _cameraZPosition;
        private float _targetCameraZPosition;

        [Header("Lock on")]
        [SerializeField] float _lockOnRaduis = 20f;
        [SerializeField] float _minimumViewableAngle = -50f;
        [SerializeField] float _maxuimuViewableAngle = 50f;
        [SerializeField] float _lockOnTargetFollowSpeed = 0.2f;
        [SerializeField] float _setCameraSpeed = 0.75f;
        [SerializeField] float _unlockCameraHeigth = 1.65f;
        [SerializeField] float _lockedCameraHeigth = 2.0f;
        private Coroutine _cameraLockOnHightCoroutine;
        private List<CharacterManager> _availableTargetsList = new List<CharacterManager>();
        public CharacterManager _nearstLockOnTarget;
        public CharacterManager _leftLockOnTarget;
        public CharacterManager _rightLockOnTarget;

        [Header("Ranged Aim")]
        private Transform _followTransformWhenAiming;
        public Vector3 _aimDirection;

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
            if (_player._playerNetworkManager._isAiming.Value)
            {
                Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, _player._playerCombatManager._lockOnTransform.position, ref _cameraVelocity, _cameraSmoothSpeed * Time.deltaTime);
                transform.position = targetCameraPosition;
            }
            else
            {
                Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, _player.transform.position, ref _cameraVelocity, _cameraSmoothSpeed * Time.deltaTime);
                transform.position = targetCameraPosition;
            }
        }
        private void HandleRotations()
        {
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
            // If locked on, force rotation toward target
            if (_player._playerNetworkManager._isLockOn.Value)
            {
                // This rotate this gameobject
                Vector3 rotationDirection = _player._playerCombatManager._currentTarget._characterCombatManager._lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _lockOnTargetFollowSpeed);

                // This rotate the pivot
                rotationDirection = _player._playerCombatManager._currentTarget._characterCombatManager._lockOnTransform.position - _cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                _cameraPivotTransform.transform.rotation = Quaternion.Slerp(_cameraPivotTransform.rotation, targetRotation, _lockOnTargetFollowSpeed);

                // Save rotation yo our look angles, so when we unlock it doesn't snap to far away
                _leftAndRightLookAngle = transform.eulerAngles.y;
                _uptAndDownLookAngle = transform.eulerAngles.y;
            }
            // Else rotate normally 
            else
            {

                _leftAndRightLookAngle += (PlayerInputManager.Instance._cameraHorizontalInput * _leftAndRightRotationSpeed) * Time.deltaTime;

                _uptAndDownLookAngle -= (PlayerInputManager.Instance._cameraVerticalInput * _upAndDownRotationSpeed) * Time.deltaTime;


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

        }
        private void HandleAimRotation()
        {
            if (!_player._playerLocomotionManager._isGrounded)
                _player._playerNetworkManager._isAiming.Value = false;

            _aimDirection = _cameraObject.transform.forward.normalized;

            if (_player._isPerformingAction)
                return;
            // Left and right look
            Vector3 cameraRotationY = Vector3.zero;
            // up and down look
            Vector3 cameraRotationX = Vector3.zero;

            _leftAndRightLookAngle += (PlayerInputManager.Instance._cameraHorizontalInput * _leftAndRightRotationSpeed) * Time.deltaTime;
            _uptAndDownLookAngle -= (PlayerInputManager.Instance._cameraVerticalInput * _upAndDownRotationSpeed) * Time.deltaTime;
            _uptAndDownLookAngle = Mathf.Clamp(_uptAndDownLookAngle, _minimumPivot, _maximumPivot);

            cameraRotationY.y = _leftAndRightLookAngle;
            cameraRotationX.y = _uptAndDownLookAngle;

            _cameraObject.transform.localEulerAngles = new Vector3(_uptAndDownLookAngle, _leftAndRightLookAngle, 0);
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
            if (_player._playerNetworkManager._isAiming.Value)
            {
                _cameraObjectPosition.z = 0f;
                _cameraObject.transform.localPosition = _cameraObjectPosition;
                return;
            }
            _cameraObjectPosition.z = Mathf.Lerp(_cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
            _cameraObject.transform.localPosition = _cameraObjectPosition;
        }
        public void HandleLocatingLockOnTargets()
        {
            float shortesDistance = Mathf.Infinity;// Will be used to determine the target closest to us 
            float shortDistanceOfTheRigthTarget = Mathf.Infinity;// Will be used to determine shortest distance on one axis to the right of current target(closest target to the right of current target)
            float shortesDistanceOfLeftTarget = -Mathf.Infinity;//Will be used to determine shortest distance on one axis to the left of current target (-)

            Collider[] colliders = Physics.OverlapSphere(_player.transform.position, _lockOnRaduis, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {

                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
                if (lockOnTarget != null)
                {
                    // Check if they are within our field of view
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - _player.transform.position;
                    float distanceFromTarget = Vector3.Distance(_player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, _cameraObject.transform.forward);

                    // Cannot lock on dead character
                    if (lockOnTarget._isDead.Value)
                        continue;

                    // The player cannot lock on himself 
                    if (lockOnTarget.transform.root == _player.transform.root)
                    {
                        continue;
                    }

                    // Lastly if the target is outside field of view or is blocked by enviro, check the next potential  target
                    if (viewableAngle > _minimumViewableAngle && viewableAngle < _maxuimuViewableAngle)
                    {
                        RaycastHit hit;

                        if (Physics.Linecast(_player._playerCombatManager._lockOnTransform.position, lockOnTarget._characterCombatManager._lockOnTransform.transform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {

                            // We have hit something, we cannot see our lock on target 
                            continue;
                        }
                        else
                        {
                            // Otherwise, add them to the list
                            _availableTargetsList.Add(lockOnTarget);
                        }
                    }
                }
            }
            // We now sort through our potential targets to see which one we lock into first
            for (int k = 0; k < _availableTargetsList.Count; k++)
            {
                if (_availableTargetsList[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(_player.transform.position, _availableTargetsList[k].transform.position);
                    // Vector3 lockTargetDirection = _availableTargetsList[k].transform.position - _playerManager.transform.position;

                    if (distanceFromTarget < shortesDistance)
                    {
                        shortesDistance = distanceFromTarget;
                        _nearstLockOnTarget = _availableTargetsList[k];
                    }
                    // If we are already lock On when searching for targets, search for our nearest left/right targets
                    if (_player._playerNetworkManager._isLockOn.Value)
                    {
                        Vector3 relativeEnemyPositon = _player.transform.InverseTransformPoint(_availableTargetsList[k].transform.position);
                        float distanceFromleftTarget = relativeEnemyPositon.x;
                        float distanceFromRightTarget = relativeEnemyPositon.x;

                        if (_availableTargetsList[k] == _player._playerCombatManager._currentTarget)
                            continue;

                        // Check for the left side for targets
                        if (relativeEnemyPositon.x <= 0.00f && distanceFromleftTarget > shortesDistanceOfLeftTarget)
                        {
                            shortesDistanceOfLeftTarget = distanceFromleftTarget;
                            _leftLockOnTarget = _availableTargetsList[k];
                        }
                        // Check for the right side for targets
                        else if (relativeEnemyPositon.x >= 0.00f && distanceFromRightTarget < shortDistanceOfTheRigthTarget)
                        {
                            shortDistanceOfTheRigthTarget = distanceFromRightTarget;
                            _rightLockOnTarget = _availableTargetsList[k];
                        }
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    _player._playerNetworkManager._isLockOn.Value = false;
                }
            }
        }

        public void SetLockOnCameraHeight()
        {
            if (_cameraLockOnHightCoroutine != null)
                StopCoroutine(_cameraLockOnHightCoroutine);

            _cameraLockOnHightCoroutine = StartCoroutine(SetCameraHeight());
        }


        public void ClearLockOnTargets()
        {
            _nearstLockOnTarget = null;
            _leftLockOnTarget = null;
            _rightLockOnTarget = null;
            _availableTargetsList.Clear();
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while(_player._isPerformingAction)
            {
                yield return null;
            }
            ClearLockOnTargets();
            HandleLocatingLockOnTargets();

            if (_nearstLockOnTarget != null)
            {
                _player._playerCombatManager.SetTarget(_nearstLockOnTarget);
                _player._playerNetworkManager._isLockOn.Value = true;
            }
            yield return null;
        }
        private IEnumerator SetCameraHeight()
        {
            float duration = 1f;
            float timer = 0f;

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeigth = new Vector3(_cameraPivotTransform.transform.localPosition.x, _lockedCameraHeigth);
            Vector3 newUnlockedCameraHeigth = new Vector3(_cameraPivotTransform.transform.localPosition.x, _unlockCameraHeigth);

            while(timer < duration)
            {
                timer += Time.deltaTime;

                if (_player != null)
                {
                    if (_player._playerCombatManager != null)
                    {
                        _cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newLockedCameraHeigth, ref velocity, _setCameraSpeed);

                        _cameraPivotTransform.transform.localRotation = Quaternion.Slerp(_cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), _lockOnTargetFollowSpeed);
                    }
                    else
                    {
                        _cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newUnlockedCameraHeigth, ref velocity, _setCameraSpeed);
                    }

                }
                yield return null;
            }
            if (_player != null)
            {
                if (_player._playerCombatManager != null)
                {
                    _cameraPivotTransform.transform.localPosition = newLockedCameraHeigth;
                    _cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    _cameraPivotTransform.transform.localPosition = newUnlockedCameraHeigth;
                }

            }
            yield return null;
        }
    }
}