using SKD.Character;
using UnityEngine;


public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager _character;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_character == null)
        {
            _character = animator.GetComponent<CharacterManager>();
        }

        _character._isPerformingAction = false;
        _character._characterAnimationManager._applyRootMotion = false;
        _character._characterLocomotionManager._canRotate = true;
        _character._characterLocomotionManager._canMove = true;
        _character._characterLocomotionManager._canRun = true;
        _character._characterLocomotionManager._isRolling = false;
        _character._characterAnimationManager.DisableCanDoCombo();
        _character._characterCombatManager.DisableCanDoRollingAttack();
        _character._characterCombatManager.DisableCanDoBackstepAttack();
        
        if(_character._characterEffectsManager._activeSpellWarmUpFX!=null)
            Destroy(_character._characterEffectsManager._activeSpellWarmUpFX);
        
        if (_character._characterEffectsManager._activeQuickSlotItemFX != null)
            Destroy(_character._characterEffectsManager._activeQuickSlotItemFX);

        if (_character.IsOwner)
        {
            _character._characterNetworkManager._isJumping.Value = false;
            _character._characterNetworkManager._isInvulnerable.Value = false;
            _character._characterNetworkManager._isAttacking.Value = false;
            _character._characterNetworkManager._isRipostable.Value = false;
            _character._characterNetworkManager._isBeingCrititcalDamged.Value = false;
            _character._characterNetworkManager._isParrying.Value = false;
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