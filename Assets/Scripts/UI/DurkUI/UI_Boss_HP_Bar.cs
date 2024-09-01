using SKD.Character.AI_Character;
using SKD.UI.PlayerUI;
using System;
using TMPro;
using UnityEngine;

namespace SKD.UI.DurkUI
{
    public class UI_Boss_HP_Bar : UI_StatBar
    {
        [SerializeField] AIBossCharacterManager _bossCharacter;
        public void EnableBossHPBar(AIBossCharacterManager aIBoss)
        {
            _bossCharacter = aIBoss;
            _bossCharacter._aICharacterNetworkManager._currentHealth.OnValueChanged += OnBossHPChanged;
            SetMaxStat(_bossCharacter._aICharacterNetworkManager._maxHealth.Value);
            SetStat(_bossCharacter._aICharacterNetworkManager._currentHealth.Value);
            GetComponentInChildren<TextMeshProUGUI>().text = _bossCharacter._characterName;
        }
        private void OnDestroy()
        {

            _bossCharacter._aICharacterNetworkManager._currentHealth.OnValueChanged -= OnBossHPChanged;
        }
        private void OnBossHPChanged(int previousValue, int newValue)
        {
            SetStat(newValue);

            if (newValue <= 0)
            {
                RemoveHPBar(2.5f);
            }
        }
        public void RemoveHPBar(float time)
        {
            Destroy(gameObject, time);
        }
    }
}