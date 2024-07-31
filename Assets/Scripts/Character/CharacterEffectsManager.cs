using SKD.Effects;
using System.Collections;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Process instant effects (Take Damage, Heal)

        // Process times effects (Poison, Build Ups)

        // Process static effects (adding or removing buffs from rings etc)

        CharacterManager _charcterManager;
        protected virtual void Awake()
        {
            _charcterManager = GetComponent<CharacterManager>(); 
        }

        public virtual void ProceesInstanceEffect(InstantCharacterEffect effects)
        {
            // Take in an effect
            effects.ProccesEffect(_charcterManager);
            // Process it 
        }
    }
}