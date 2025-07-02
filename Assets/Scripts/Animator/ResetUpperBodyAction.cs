using SKD.Character.Player;
using UnityEngine;

public class ResetUpperBodyAction : StateMachineBehaviour
{
    PlayerManager _player;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
            _player = animator.GetComponent<PlayerManager>();

        if (_player == null)
            return;

        if (_player._playerEffectsManager._activeQuickSlotItemFX != null)
            Destroy(_player._playerEffectsManager._activeQuickSlotItemFX);

        _player._playerLocomotionManager._canRun = true;
        _player._playerEquipmentManager.UnhideWeapons();

        if (_player._playerEffectsManager._activeQuickSlotItemFX != null)
            Destroy(_player._playerEffectsManager._activeQuickSlotItemFX);
        
        // we check if the player is using an item 
        if (_player._playerCombatManager._isUsingItem)
        {
            _player._playerCombatManager._isUsingItem = false;

            if (!_player._isPerformingAction)
                _player._playerLocomotionManager._canRoll = true;
        }
    }

    /*public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }*/
}