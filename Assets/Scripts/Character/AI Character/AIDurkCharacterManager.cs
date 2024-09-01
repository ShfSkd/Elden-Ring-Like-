using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character.Durk
{
    public class AIDurkCharacterManager : AIBossCharacterManager
    {
        [HideInInspector] public AIDurkSoundFXManager _aIDurkSoundFXManager;
        [HideInInspector] public AIDurkCombatManager _aIDurkCombatManager;

        protected override void Awake()
        {
            base.Awake();

            _aIDurkSoundFXManager = GetComponent<AIDurkSoundFXManager>();
            _aIDurkCombatManager = GetComponent<AIDurkCombatManager>();
        }


    }
}