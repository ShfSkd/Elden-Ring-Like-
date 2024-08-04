using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.Character.Player.PlayerUI
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider _slider;
        private RectTransform _rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool _scaleBarLeangthWithStats = true;
        [SerializeField] protected float _widthScaleMultiplier = 1f;

        protected virtual void Awake()
        {
            _slider = GetComponent<Slider>();
            _rectTransform = GetComponent<RectTransform>();
        }
        public virtual void SetStat(int newValue)
        {
            _slider.value = newValue;
        }
        public virtual void SetMaxStat(int maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;

            if (_scaleBarLeangthWithStats)
            {
                // Scale the transform of this object
                _rectTransform.sizeDelta = new Vector2(maxValue * _widthScaleMultiplier, _rectTransform.sizeDelta.y);

                // Reset the position of the bars based on their layout group's settings
                PlayerUIManger.instance._playerUIHUDManager.RefreshHUD();
            }
        }
    }

}
