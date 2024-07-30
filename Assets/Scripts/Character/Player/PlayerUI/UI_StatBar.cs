using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.Character.Player.PlayerUI
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider _slider;

        protected virtual void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        public virtual void SetStat(int newValue)
        {
            _slider.value = newValue;
        }
        public virtual void SetMaxStat(int maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
        }
    }

}
