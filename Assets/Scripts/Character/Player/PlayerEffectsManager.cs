using SKD.Effects;
using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] InstantCharacterEffect _effectToTest;
        [SerializeField] bool _proccesEffect;

        private void Update()
        {
            if(_proccesEffect)
            {
                _proccesEffect = false;
                InstantCharacterEffect effect = Instantiate(_effectToTest);
                ProceesInstanceEffect(effect);
            }
        }
    }
}