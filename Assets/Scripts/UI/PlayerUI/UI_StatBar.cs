using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.UI.PlayerUI
{
    public class UI_StatBar : MonoBehaviour
    {
        protected Slider _slider;
        protected RectTransform _rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool _scaleBarLengthWithStats = true;
        [SerializeField] protected float _widthScaleMultiplier = 1f;

        protected virtual void Awake()
        {
            _slider = GetComponent<Slider>();
            _rectTransform = GetComponent<RectTransform>();
        }
        protected virtual void Start()
        {

        }
        public virtual void SetStat(int newValue)
        {
            _slider.value = newValue;
        }
        public virtual void SetMaxStat(int maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;

            if (_scaleBarLengthWithStats)
            {
                // Scale the transform of this object
                _rectTransform.sizeDelta = new Vector2(maxValue * _widthScaleMultiplier, _rectTransform.sizeDelta.y);

                // Reset the position of the bars based on their layout group's settings
                PlayerUIManager.Instance._playerUIHUDManager.RefreshHUD();
            }
        }
    }

}
