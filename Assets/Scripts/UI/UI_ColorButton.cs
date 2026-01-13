using System;
using SKD.MenuScreen;
using UnityEngine;
using UnityEngine.UI;
namespace SKD.UI
{
    public class UI_ColorButton : MonoBehaviour
    {
        public float _redValue;
        public float _greenValue;
        public float _blueValue;

        [SerializeField] Image _colorImage;

        void Awake()
        {
            _redValue = _colorImage.color.r * 255;
            _greenValue = _colorImage.color.g * 255;
            _blueValue = _colorImage.color.b * 255;
        }
        public void SetSlidersValuesToColor()
        {
            TitleScreenManager.Instance.SetRedColorsSlider(_redValue);
            TitleScreenManager.Instance.SetGreenColorsSlider(_greenValue);
            TitleScreenManager.Instance.SetBlueColorsSlider(_blueValue);
            TitleScreenManager.Instance.SelectHairColor();
        }
        public void ConfirmColor()
        {
            TitleScreenManager.Instance.CloseChooseHairColorSubMenu(); 
        }
    }
}