using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager _characterManager;
        int _vertical;
        int _horizontal;

        [Header("Damage Animations")]
        public string _lastDamageAniumationPlayed;
        [SerializeField] string _hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
        [SerializeField] string _hit_Forward_Medium_02 = "Hit_Forward_Medium_02";

        [SerializeField] string _hit_Backward_Medium_01 = "Hit_Backward_Medium_01";
        [SerializeField] string _hit_Backward_Medium_02 = "Hit_Backward_Medium_02";

        [SerializeField] string _hit_Left_Medium_01 = "Hit_Left_Medium_01";
        [SerializeField] string _hit_Left_Medium_02 = "Hit_Left_Medium_02";

        [SerializeField] string _hit_Right_Medium_01 = "Hit_Right_Medium_01";
        [SerializeField] string _hit_Right_Medium_02 = "Hit_Right_Medium_02";

        public List<string> _forward_Medium_Damage_List = new List<string>();
        public List<string> _backeard_Medium_damage_List = new List<string>();
        public List<string> _left_Medium_damage_List = new List<string>();
        public List<string> _right_Medium_damage_List = new List<string>();
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            _horizontal = Animator.StringToHash("Horizontal");
            _vertical = Animator.StringToHash("Vertical");
        }

        protected virtual void Start()
        {
            _forward_Medium_Damage_List.Add(_hit_Forward_Medium_01);
            _forward_Medium_Damage_List.Add(_hit_Forward_Medium_02);

            _backeard_Medium_damage_List.Add(_hit_Backward_Medium_01);
            _backeard_Medium_damage_List.Add(_hit_Backward_Medium_02);

            _left_Medium_damage_List.Add(_hit_Left_Medium_01);
            _left_Medium_damage_List.Add(_hit_Left_Medium_02);

            _right_Medium_damage_List.Add(_hit_Right_Medium_01);
            _right_Medium_damage_List.Add(_hit_Right_Medium_02);

        }
        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach (string animation in animationList)
            {
                finalList.Add(animation);
            }
            // Check if we already played this damage animation so it doesn't repeat 
            finalList.Remove(_lastDamageAniumationPlayed);

            // Check the list for null entries,and remove them
            for (int i = finalList.Count - 1; i < -1; i--)
            {
                if (finalList[i] == null)
                    finalList.RemoveAt(i);
            }
            int ranomValue = UnityEngine.Random.Range(0, finalList.Count);

            return finalList[ranomValue];
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isPrinting)
        {
            float snappedHorizontal;
            float snappedVertical;

            // This is chain will round the horizontal movement to -1,-0.5,0,0.5 or 1
            if (horizontalMovement > 0f && horizontalMovement <= 0.5f)
                snappedHorizontal = 0.5f;

            else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
                snappedHorizontal = 1;

            else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
                snappedHorizontal = -0.5f;

            else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
                snappedHorizontal = -1f;
            else
                snappedHorizontal = 0;

            // This is chain will round the vertical movement to -1,-0.5,0,0.5 or 1 
            if (verticalMovement > 0f && verticalMovement <= 0.5f)
                snappedVertical = 0.5f;

            else if (verticalMovement > 0.5f && verticalMovement <= 1)
                snappedVertical = 1;

            else if (verticalMovement < 0 && verticalMovement >= -0.5f)
                snappedVertical = -0.5f;

            else if (verticalMovement < -0.5f && verticalMovement >= -1)
                snappedVertical = -1f;
            else
                snappedVertical = 0;


            if (isPrinting)
            {
                snappedVertical = 2f;
            }
            _characterManager._animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            _characterManager._animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            Debug.Log("Playing Animation: " + targetAnimationName);
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
        public void PlayTargetAttackActioAnimation(AttackType attackType, string targetAnimationName,
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