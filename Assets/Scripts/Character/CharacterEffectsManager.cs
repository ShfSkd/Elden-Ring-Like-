using SKD.Effects;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Process instant effects (Take Damage, Heal)

        // Process times effects (Poison, Build Ups)

        // Process static effects (adding or removing buffs from rings etc)

        CharacterManager _charcter;

        [Header("VFX")]
        [SerializeField] GameObject _bloodSplatterVFX;
        
        [Header("Static Effects")]
        public List<StaticCharacterEffect> _staticEffectsList = new List<StaticCharacterEffect>();
        protected virtual void Awake()
        {
            _charcter = GetComponent<CharacterManager>();
        }

        public virtual void ProceesInstanceEffect(InstantCharacterEffect effects)
        {
            // Take in an effect
            effects.ProcessesEffect(_charcter);
            // Process it 
        }
        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // If we have manually have placed a blood splatter VFX on this mode, Play this version
            if (_charcter != null)
            {
                GameObject bloodSplatter = Instantiate(_bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // Else , use the generic (default version) we have elsewhere
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance._bloodSplatterVFX, contactPoint, Quaternion.identity);

            }
        }

        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            // 1. Add static effect to the character
            _staticEffectsList.Add(effect);

            // 2. Process its effect
            effect.ProcessStaticEffect(_charcter);

            // 3. Check for null entries in your list and remove them 
            for (int i = _staticEffectsList.Count - 1; i > -1; i--)
            {
                if (_staticEffectsList[i] == null)
                {
                    _staticEffectsList.RemoveAt(i);
                }
            }
        }
        public void RemoveStaticEffect(int effectID)
        {
            // 1. Remove static effect to the character
            StaticCharacterEffect effect;

            for (int i = 0; i < _staticEffectsList.Count; i++)
            {
                if (_staticEffectsList[i] != null)
                {
                    if (_staticEffectsList[i]._staticEffectID == effectID)
                    {
                        effect = _staticEffectsList[i];
                        // 1. Remove Static Effect from character
                        effect.RemoveStaticEffect(_charcter);
                        // 2. Remove Static Effect from list
                        _staticEffectsList.Remove(effect);
                    }
                }
            }
            // Check for null entries in your list and remove them 
            for (int i = _staticEffectsList.Count - 1; i > -1; i--)
            {
                if (_staticEffectsList[i] == null)
                {
                    _staticEffectsList.RemoveAt(i);
                }
            }
        }
    }
}