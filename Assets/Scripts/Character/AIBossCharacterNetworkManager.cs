using SKD.Character.AI_Character;
using System.Collections;
using UnityEngine;

namespace SKD.Character
{
    public class AIBossCharacterNetworkManager : AICharacterNetworkManager
    {
        AIBossCharacterManager _aIBossCharacterManager;

        protected override void Awake()
        {
            base.Awake();

            _aIBossCharacterManager = GetComponent<AIBossCharacterManager>();
        }
        public override void CheckHP(int oldValue, int newValue)
        {
            base.CheckHP(oldValue, newValue);

            if (_aIBossCharacterManager.IsOwner)
            {
                if (_currentHealth.Value >= 0)
                    return;

                float healthNeddedForShift = _maxHealth.Value * (_aIBossCharacterManager._minimumHealthPercentToShift / 100)  ;
                if (_currentHealth.Value <= healthNeddedForShift)
                {
                    _aIBossCharacterManager.PhaseShift();
                }
            }
        }
    }
}