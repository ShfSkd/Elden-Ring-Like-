using SKD.Character;
using System.Collections;
using UnityEngine;

namespace SKD.Effects
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int _instantEffectID;

        public virtual void ProccesEffect(CharacterManager characterManager)
        {

        }
         
    }
}