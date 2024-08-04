using System.Collections;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterLocamotionManager : MonoBehaviour
    {
        CharacterManager _characterManger;

        [Header("Ground Check & jump")]
        [SerializeField] protected float _gravityForce = -5.55f;
        [SerializeField] LayerMask _groundLayer;
        [SerializeField] float _groundCheckSphereRaduis = 1;
        [SerializeField] protected Vector3 _yVelocity;// This is the force at which our character is pulled up or down)Jumping or Falling)
        [SerializeField] protected float _groundedYVelocity = -20f;// The force at which our character is sticking to the ground whilst they are grounded
        [SerializeField] protected float _fallStartYVelocity = -5f;// the force at which our character begins to fall when they become underground (Rises as they fall longer)
        protected bool _fallingVelocityhasBeenSet;
        protected float _inAirTimer = 0f;
        protected virtual void Awake()
        {
            _characterManger = GetComponent<CharacterManager>();
        }
        protected virtual void Update()
        {
            HandleGroundCheck();

            if (_characterManger._isGrounded)
            {
                // If we are not attempting to jump or move upward
                if (_yVelocity.y < 0)
                {
                    _inAirTimer = 0;
                    _fallingVelocityhasBeenSet = false;
                    _yVelocity.y = _groundedYVelocity;
                }
            }
            else
            {
                // If we are not jumping, and our falling velocity has not been set 
                if (!_characterManger._isJumping && !_fallingVelocityhasBeenSet)
                {
                    _fallingVelocityhasBeenSet = true;
                    _yVelocity.y = _fallStartYVelocity;
                }
                _inAirTimer += Time.deltaTime;
                _characterManger._animator.SetFloat("InAirTimer", _inAirTimer);

                _yVelocity.y += _gravityForce * Time.deltaTime;

            }
            // There should always be some force applied to the Y velocity
            _characterManger._characterController.Move(_yVelocity * Time.deltaTime);
        }
        protected void HandleGroundCheck()
        {
            _characterManger._isGrounded = Physics.CheckSphere(_characterManger.transform.position, _groundCheckSphereRaduis, _groundLayer);
        }
        // Draw our ground check sphere in scene view
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(_characterManger.transform.position, _groundCheckSphereRaduis); 
        }
    }
}