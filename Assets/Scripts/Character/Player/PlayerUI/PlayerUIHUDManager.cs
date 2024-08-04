using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Character.Player.PlayerUI
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar _healthBar;
        [SerializeField] UI_StatBar _staminaBar;

        public void RefreshHUD()
        {
            _healthBar.gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(true);

            _staminaBar.gameObject.SetActive(false);
            _staminaBar.gameObject.SetActive(true);

        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            _healthBar.SetStat(newValue);
        }
        public void SetMaxHealthValue(int maxHealth)
        {
            _healthBar.SetMaxStat(maxHealth);
        }
        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            _staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            _staminaBar.SetMaxStat(maxStamina);
        }

    }

}
