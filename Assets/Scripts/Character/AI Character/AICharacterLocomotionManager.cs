using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AICharacterLocomotionManager : CharacterLocamotionManager
    {
        public void RotateTowardAgent(AICharacterManager aICharacter)
        {
            if(aICharacter._aICharacterNetworkManager._isMoving.Value)
            {
                aICharacter.transform.rotation=aICharacter._navMeshAgent.transform.rotation;
            }
        }
    }
}