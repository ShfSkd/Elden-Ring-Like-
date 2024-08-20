using SKD.Effects;
using SKD.WorldManager;
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

        [Header("VFX")]
        [SerializeField] GameObject _bloodSplatterVFX;
        protected virtual void Awake()
        {
            _charcterManager = GetComponent<CharacterManager>();
        }

        public virtual void ProceesInstanceEffect(InstantCharacterEffect effects)
        {
            // Take in an effect
            effects.ProcessesEffect(_charcterManager);
            // Process it 
        }
        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // If we have manually have placed a blood splatter VFX on this mode, Play this version
            if (_charcterManager != null)
            {
                GameObject bloodSplatter = Instantiate(_bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // Else , use the generic (default version) we have elsewhere
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance._bloodSplatterVFX, contactPoint, Quaternion.identity);

            }
        }
    }
}