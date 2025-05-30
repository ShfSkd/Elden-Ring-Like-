using System.Collections;
using System.Collections.Generic;
using SKD.Character.Player;
using UnityEngine;

namespace Skd.Animator
{
    public class ToggleNotchedArrowMovement : StateMachineBehaviour
    {
        PlayerManager _player;

        [SerializeField] bool _allowMovement = true;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_player == null)
                _player = animator.GetComponent<PlayerManager>();

            if (_player == null)
                return;

            _player._playerLocomotionManager._canMove = _allowMovement;
            _player._playerLocomotionManager._canRotate = _allowMovement;
            _player._playerLocomotionManager._canRun = !_allowMovement;
            _player._isPerformingAction = !_allowMovement;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}