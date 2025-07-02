using SKD.Character.Player;
using SKD.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using SKD.Items.Weapons;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager _character;
        int _vertical;
        int _horizontal;

        [Header("Flags")]
        public bool _applyRootMotion = false;

        [Header("Damage Animations")]
        public string _lastDamageAnimationPlayed;

        [Header("Ping Damage Animations")]
        [SerializeField] string _hit_Forward_Ping_01 = "Hit_Forward_Ping_01";

        [SerializeField] string _hit_Forward_Ping_02 = "Hit_Forward_Ping_02";

        [SerializeField] string _hit_Backward_Ping_01 = "Hit_Backward_Ping_01";
        [SerializeField] string _hit_Backward_Ping_02 = "Hit_Backward_Ping_02";

        [SerializeField] string _hit_Left_Ping_01 = "Hit_Left_Ping_01";
        [SerializeField] string _hit_Left_Ping_02 = "Hit_Left_Ping_02";

        [SerializeField] string _hit_Right_Ping_01 = "Hit_Right_Ping_01";
        [SerializeField] string _hit_Right_Ping_02 = "Hit_Right_Ping_02";

        public List<string> _forward_Oing_Damage_List;
        public List<string> _forward_Ping_Damage_List;
        public List<string> _left_Ping_damage_List;
        public List<string> _right_Ping_damage_List;
        public List<string> _backeard_Ping_damage_List;

        [Header("Medium Damage Animations")]
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
            _character = GetComponent<CharacterManager>();
            _horizontal = Animator.StringToHash("Horizontal");
            _vertical = Animator.StringToHash("Vertical");
        }

        protected virtual void Start()
        {
            // Medium
            _forward_Medium_Damage_List.Add(_hit_Forward_Medium_01);
            _forward_Medium_Damage_List.Add(_hit_Forward_Medium_02);

            _backeard_Medium_damage_List.Add(_hit_Backward_Medium_01);
            _backeard_Medium_damage_List.Add(_hit_Backward_Medium_02);

            _left_Medium_damage_List.Add(_hit_Left_Medium_01);
            _left_Medium_damage_List.Add(_hit_Left_Medium_02);

            _right_Medium_damage_List.Add(_hit_Right_Medium_01);
            _right_Medium_damage_List.Add(_hit_Right_Medium_02);

            // Ping
            _forward_Ping_Damage_List.Add(_hit_Forward_Ping_01);
            _forward_Ping_Damage_List.Add(_hit_Forward_Ping_02);

            _backeard_Ping_damage_List.Add(_hit_Backward_Ping_01);
            _backeard_Ping_damage_List.Add(_hit_Backward_Ping_02);

            _left_Ping_damage_List.Add(_hit_Left_Ping_01);
            _left_Ping_damage_List.Add(_hit_Left_Ping_02);

            _right_Ping_damage_List.Add(_hit_Right_Ping_01);
            _right_Ping_damage_List.Add(_hit_Right_Ping_02);
        }

        public virtual void EnableCanDoCombo()
        {
        }

        public virtual void DisableCanDoCombo()
        {
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach (string animation in animationList)
            {
                finalList.Add(animation);
            }

            // Check if we already played this damage animation so it doesn't repeat 
            finalList.Remove(_lastDamageAnimationPlayed);

            // Check the list for null entries,and remove them
            for (int i = finalList.Count - 1; i < -1; i--)
            {
                if (finalList[i] == null)
                    finalList.RemoveAt(i);
            }

            int ranomValue = UnityEngine.Random.Range(0, finalList.Count);

            return finalList[ranomValue];
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
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


            if (isSprinting)
            {
                snappedVertical = 2f;
            }

            _character._animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            _character._animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false,
            bool canRun = true,
            bool canRoll = false)
        {
            _applyRootMotion = applyRootMotion;

            _character._animator.CrossFade(targetAnimationName, 0.2f);
            // Can be used to stop character from attempting new actions
            // for example:If you get damaged, and begin performing a damage animation this flag will turn true if you are stunned  
            // we can them check for this before attempting new actions
            _character._isPerformingAction = isPerformingAction;
            _character._characterLocomotionManager._canRotate = canRotate;
            _character._characterLocomotionManager._canMove = canMove;
            _character._characterLocomotionManager._canRun = canRun;
            _character._characterLocomotionManager._canRun = canRoll;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            _character._characterNetworkManager.NotifyTheServerofActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, targetAnimationName, applyRootMotion);
        }
        public void PlayTargetActionAnimationInstantly(
            string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false,
            bool canRun = true,
            bool canRoll = false)
        {
            _applyRootMotion = applyRootMotion;
            _character._animator.Play(targetAnimationName);
            // Can be used to stop character from attempting new actions
            // for example:If you get damaged, and begin performing a damage animation this flag will turn true if you are stunned  
            // we can them check for this before attempting new actions
            _character._isPerformingAction = isPerformingAction;
            _character._characterLocomotionManager._canRotate = canRotate;
            _character._characterLocomotionManager._canMove = canMove;
            _character._characterLocomotionManager._canRun = canRun;
            _character._characterLocomotionManager._canRoll = canRoll;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            _character._characterNetworkManager.NotifyTheServerOfInstantActionAttackAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, targetAnimationName, applyRootMotion);
        }
        public void PlayTargetAttackActionAnimation(WeaponItem weapon, AttackType attackType,
            string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false,
            bool canRoll = false)
        {
            this._applyRootMotion = applyRootMotion;
            // Keep track of the last attack perform(for combos)
            // Keep track of current attack type(light,heavy,etc)
            // Update animation set to current weapons animations
            // Deiced if our attack can be parried
            // Tell the network our "IsAttacking" flag
            _character._characterCombatManager._currentAttackType = attackType;
            _character._characterCombatManager._lastAttackAnimationPerformed = targetAnimationName;
            UpdateAnimatorController(weapon._weaponAnimator);
            
            _character._animator.CrossFade(targetAnimationName, 0.2f);
            _character._isPerformingAction = isPerformingAction;
            _character._characterLocomotionManager._canRotate = canRotate;
            _character._characterLocomotionManager._canMove = canMove;
            _character._characterLocomotionManager._canRoll = canRoll;
            _character._characterNetworkManager._isAttacking.Value = true;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            _character._characterNetworkManager.NotifyTheServerOfActionAttackAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, targetAnimationName, applyRootMotion);
        }

        public void UpdateAnimatorController(AnimatorOverrideController weaponController)
        {
            _character._animator.runtimeAnimatorController = weaponController;
        }

    }
}