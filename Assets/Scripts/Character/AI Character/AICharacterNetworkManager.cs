using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        AICharacterManager _aiCharacter;
        protected override void Awake()
        {
            base.Awake();
            
            _aiCharacter = GetComponent<AICharacterManager>();
        }
        public override void OnisDeadChanged(bool oldState, bool newState)
        {
            base.OnisDeadChanged(oldState, newState);
            
            _aiCharacter._aICharacterInventoryManager.DropItem();
        }
    }
}