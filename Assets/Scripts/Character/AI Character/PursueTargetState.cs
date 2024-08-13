using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SKD.Character.AI_Character
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aICharacter)
        {
            // Check if we are performing an action (if so do nothing until action is complete)
            if (aICharacter._isPerfomingAction)
                return this;

            // Check if our target is null, if we do not have a target, return to idle state 
            if (aICharacter._aICharcterCombatManager._currentTarget == null)
                return SwitchState(aICharacter, aICharacter._idle);

            // Make sure our navmesh agent is active, if its not enable it 
            if (!aICharacter._navMeshAgent.enabled)
                aICharacter._navMeshAgent.enabled = true;

            aICharacter._aICharacterLocomotionManager.RotateTowardAgent(aICharacter);

            // Pursue the target
            // Option 1
          //  aICharacter._navMeshAgent.SetDestination(aICharacter._aICharcterCombatManager._currentTarget.transform.position);

            // Option 2
            NavMeshPath path = new NavMeshPath();
            aICharacter._navMeshAgent.CalculatePath(aICharacter._aICharcterCombatManager._currentTarget.transform.position, path);
            aICharacter._navMeshAgent.SetPath(path);

            return this;
        }
        
    }
}