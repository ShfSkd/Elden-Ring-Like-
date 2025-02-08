using SKD.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Effects
{
    public class StaticCharacterEffect : ScriptableObject
    {
        [FormerlySerializedAs("_effectID")]
        [Header("Effect ID")]
        public int _staticEffectID;

        public virtual void ProcessStaticEffect(CharacterManager character)
        {

        }
        public virtual void RemoveStaticEffect(CharacterManager character)
        {

        }
    }
}