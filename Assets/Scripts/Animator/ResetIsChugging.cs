using System.Collections;
using System.Collections.Generic;
using Items.Flasks;
using SKD.Character.Player;
using UnityEngine;

public class ResetIsChugging : StateMachineBehaviour
{
    PlayerManager _player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
            _player = animator.GetComponent<PlayerManager>();

        if (_player == null)
            return;

        // If we are out of flasks, play the empty animation && Hide weapons (Owner only)
        if (_player._playerNetworkManager._isChugging.Value && _player.IsOwner)
        {
            FlaskItem currentFlask = _player._playerInventoryManager._currentQuickSlotItem as FlaskItem;

            if (currentFlask != null && currentFlask._isHealthFlask)
            {
                if (_player._playerNetworkManager._remainingHealthFlasks.Value <= 0)
                {
                    _player._characterAnimationManager.PlayTargetActionAnimation(currentFlask._emptyFlaskAnimation, false, false, true, true, false);
                    _player._playerNetworkManager.HideWeaponsServerRpc();
                }
            }
            else if (currentFlask != null)
            {
                if (_player._playerNetworkManager._remainingFocusPointsFlasks.Value <= 0)
                {
                    if (currentFlask != null)
                        _player._characterAnimationManager.PlayTargetActionAnimation(currentFlask._emptyFlaskAnimation, false, false, true, true, false);
                    
                    _player._playerNetworkManager.HideWeaponsServerRpc();

                }
            }
        }
        // If we are out of flasks, instantiate the empty flask
        if (_player._playerNetworkManager._isChugging.Value)
        {
            FlaskItem currentFlask = _player._playerInventoryManager._currentQuickSlotItem as FlaskItem;

            if (currentFlask != null && currentFlask._isHealthFlask)
            {
                if (_player._playerNetworkManager._remainingHealthFlasks.Value <= 0)
                {
                    Destroy(_player._playerEffectsManager._activeQuickSlotItemFX);
                    GameObject emptyFlask = Instantiate(currentFlask._emptyFlaskItem, _player._playerEquipmentManager._rightHandWeaponSlot.transform);
                    _player._playerEffectsManager._activeQuickSlotItemFX = emptyFlask;
                }
            }
            else if (currentFlask != null)
            {
                if (_player._playerNetworkManager._remainingFocusPointsFlasks.Value <= 0)
                {
                    Destroy(_player._playerEffectsManager._activeQuickSlotItemFX);
                    GameObject emptyFlask = Instantiate(currentFlask._emptyFlaskItem, _player._playerEquipmentManager._rightHandWeaponSlot.transform);
                    _player._playerEffectsManager._activeQuickSlotItemFX = emptyFlask;
                }
            }
            // Reset isChugging
            if (_player.IsOwner)
                _player._playerNetworkManager._isChugging.Value = false;
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