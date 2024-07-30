using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Character.Player.PlayerUI
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar _staminaBar;

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
