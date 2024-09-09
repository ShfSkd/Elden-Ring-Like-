using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SKD.Character.AI_Character.States
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aICharacter)
        {
            // Check if we are performing an action (if so do nothing until action is complete)
            if (aICharacter._isPerformingAction)
                return this;

            // Check if our target is null, if we do not have a target, return to idle state 
            if (aICharacter._aICharacterCombatManager._currentTarget == null)
                return SwitchState(aICharacter, aICharacter._idle);

            // Make sure our navmesh agent is active, if its not enable it 
            if (!aICharacter._navMeshAgent.enabled)
                aICharacter._navMeshAgent.enabled = true;

            // if our target goes outside if the character F.O.V pivot to face them
            if (aICharacter._aICharacterCombatManager._enablePivot)
            {
                if (aICharacter._aICharacterCombatManager._viewableAngle < aICharacter._aICharacterCombatManager._minimumFieldOfView || aICharacter._aICharacterCombatManager._viewableAngle > aICharacter._aICharacterCombatManager._maximumFieldOfView)
                {
                    aICharacter._aICharacterCombatManager.PivotTowardsTarget(aICharacter);
                }
            }


            aICharacter._aICharacterLocomotionManager.RotateTowardAgent(aICharacter);

            // If we are within the combat range of target, switch state to combat stance state
            if (aICharacter._aICharacterCombatManager._distanceFromTarget <= aICharacter._navMeshAgent.stoppingDistance)
                return SwitchState(aICharacter, aICharacter._combatStance);

            // Pursue the target
            // Option 1
            //  aICharacter._navMeshAgent.SetDestination(aICharacter._aICharacterCombatManager._currentTarget.transform.position);

            // Option 2
            NavMeshPath path = new NavMeshPath();
            aICharacter._navMeshAgent.CalculatePath(aICharacter._aICharacterCombatManager._currentTarget.transform.position, path);
            aICharacter._navMeshAgent.SetPath(path);

            return this;
        }

    }
}