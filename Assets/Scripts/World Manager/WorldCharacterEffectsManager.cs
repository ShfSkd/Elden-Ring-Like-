using SKD.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.WorldManager
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager Instance;

        [Header("VFX")] 
        public GameObject _bloodSplatterVFX;

        [Header("Damage")]
        public TakeDamageEffect _takeDamageEffect;
        public TakeBlockedDamageEffect _takeBlockedDamageEffect;

        [SerializeField] List<InstantCharacterEffect> _instantEffects;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            GenerateEffectIds();
        }

        private void GenerateEffectIds()
        {
            for (int i = 0; i < _instantEffects.Count; i++)
            {
                _instantEffects[i]._instantEffectID = i;
            }
        }
    }
}