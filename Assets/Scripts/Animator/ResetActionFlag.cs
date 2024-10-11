using SKD.Character;
using UnityEngine;


public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager _characterManager;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_characterManager == null)
        {
            _characterManager = animator.GetComponent<CharacterManager>();
        }

        _characterManager._isPerformingAction = false;
        _characterManager._characterAnimationManager._applyRootMotion = false;
        _characterManager._characterLocomotionManager._canRotate = true;
        _characterManager._characterLocomotionManager._canMove = true;
        _characterManager._characterLocomotionManager._isRolling = false;
        _characterManager._characterAnimationManager.DisableCanDoCombo();

        if (_characterManager.IsOwner)
        {
            _characterManager._characterNetworkManager._isJumping.Value = false;
            _characterManager._characterNetworkManager._isInvulnerable.Value = false;
            _characterManager._characterNetworkManager._isAttacking.Value = false;
        }

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

