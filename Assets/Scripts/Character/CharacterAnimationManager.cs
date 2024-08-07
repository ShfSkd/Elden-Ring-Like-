using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager _characterManager;
        int _vertical;
        int _horizontal;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            _horizontal = Animator.StringToHash("Horizontal");
            _vertical = Animator.StringToHash("Vertical");
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isPrinting)
        {
            float horizontalAmount = horizontalMovement;
            float verticalAmpunt = verticalMovement;

            if (isPrinting)
            {
                verticalAmpunt = 2f;
            }
            _characterManager._animator.SetFloat(_horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            _characterManager._animator.SetFloat(_vertical, verticalAmpunt, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            _characterManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(targetAnimationName, 0.2f);
            // Can be used to stop character from attempting new actions
            // for example:If you get damaged, and begin performing a damage animation this flag will turn true if you are stunned  
            // we can them check for this before attempting new actions
            _characterManager._isPerfomingAction = isPerformingAction;
            _characterManager._canRotate = canRotate;
            _characterManager._canMove = canMove;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            _characterManager._characterNetworkManager.NotifyTheServerofActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimationName, applyRootMotion);
        }
        public void PlayTargetAttackActioAnimation(AttackType attackType,   string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // Keep track of the last attack perform(for combos)
            // Keep track of current attack type(light,heavy,etc)
            // Update animation set to current weapons animations
            // Deiced if our attack can be parried
            // Tell the network our "IsAttacking" flag
            _characterManager._characterCombatManager._currentAttacktype = attackType; 
            _characterManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(targetAnimationName, 0.2f);
            _characterManager._isPerfomingAction = isPerformingAction;
            _characterManager._canRotate = canRotate;
            _characterManager._canMove = canMove;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            _characterManager._characterNetworkManager.NotifyTheServerofActionAttackAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimationName, applyRootMotion);
        }

    }
}