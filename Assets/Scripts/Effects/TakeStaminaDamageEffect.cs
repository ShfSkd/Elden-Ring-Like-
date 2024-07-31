using SKD.Character;
using System.Collections;
using UnityEngine;

namespace SKD.Effects
{
    [CreateAssetMenu(menuName ="Character Effects/Instant Effect/Take Stamina Damage ")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float _staminaDamage;

        public override void ProccesEffect(CharacterManager characterManager)
        {
            base.ProccesEffect(characterManager);

            CalculateStaminaDamage(characterManager);
        }
        private void CalculateStaminaDamage(CharacterManager characterManager)
        {
            if (characterManager.IsOwner)
            {
                characterManager._characterNetworkManager._currentStamina.Value -= _staminaDamage;
            }
        }
    }
}