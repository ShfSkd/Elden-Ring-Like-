using SKD.Character;
using UnityEngine;

namespace SKD.Effects
{
    [CreateAssetMenu(menuName = "CharacterEffect/StaticEffect/Two Handing Effect")]
    public class TwoHandingEffect : StaticCharacterEffect
    {
        [SerializeField] int _strengthGainedFromTwoHandingWeapon;

        public override void ProcessStaticEffect(CharacterManager character)
        {
            base.ProcessStaticEffect(character);

            if (character.IsOwner)
            {
                _strengthGainedFromTwoHandingWeapon = Mathf.RoundToInt(character._characterNetworkManager._strength.Value / 2);
                Debug.Log("Strength Gained " + _strengthGainedFromTwoHandingWeapon);
                character._characterNetworkManager._strengthModifier.Value += _strengthGainedFromTwoHandingWeapon;
            }
        }
        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);
            
            if (character.IsOwner)
            {
                character._characterNetworkManager._strengthModifier.Value -= _strengthGainedFromTwoHandingWeapon;
            }
        }
    }
}