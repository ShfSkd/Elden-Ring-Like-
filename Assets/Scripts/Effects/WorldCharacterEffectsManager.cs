using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Effects
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        private static WorldCharacterEffectsManager _instance;
        [SerializeField] List<InstantCharacterEffect> _instantEffects;

        public static WorldCharacterEffectsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("WorldCharacterEffectsManager instance is null. Ensure it is instantiated properly.");
                }
                return _instance;
            }
        }

        private void Awake()
        {
          if(_instance == null)
                _instance = this;
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