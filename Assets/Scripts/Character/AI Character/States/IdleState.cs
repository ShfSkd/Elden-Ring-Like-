using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character.States
{
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aICharacter)
        {
            if (aICharacter._characterCombatManager._currentTarget != null)
            {
                // Return the pursue target state(Change The state to the pursue target state)
                return SwitchState(aICharacter, aICharacter._pursueTarget);
                 
            }
            else
            {
                // Return this state, to continually search for a target (keep the state here, until a target is found
                aICharacter._aICharacterCombatManager.FindATargetViaLineOfSight(aICharacter);
                return this;
            }
        }
    }
}