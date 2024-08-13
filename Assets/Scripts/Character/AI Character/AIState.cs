using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aICharacter)
        {
           
            // To some logic to find the player 

            // If we have found the player,return the pursue target state instead

            // If we haven't find the player, continue to return to the idle state 
            return this;
        }
        protected virtual AIState SwitchState(AICharacterManager aICharacter,AIState newState)
        {
            ResetStateFlags(aICharacter);
            return newState;
        }
        protected virtual void ResetStateFlags(AICharacterManager aICharacter)
        {
            // Reset any state flags here so when you return to the state, they are blank once again
        }
    }
}