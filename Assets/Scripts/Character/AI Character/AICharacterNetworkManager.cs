using System.Collections;
using SKD.UI.PlayerUI;
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

            if (_aiCharacter._isDead.Value)
            {
                _aiCharacter._aICharacterInventoryManager.DropItem();
                _aiCharacter._aICharacterCombatManager.AwardRunesOnDeath(PlayerUIManager.Instance._localPlayer);
            }
        }
    }
}