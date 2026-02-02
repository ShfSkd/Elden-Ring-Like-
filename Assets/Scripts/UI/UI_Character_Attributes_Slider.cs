using SKD.UI.PlayerUI;
using UnityEngine;
namespace SKD.UI
{
    public class UI_Character_Attributes_Slider : MonoBehaviour
    {
        [SerializeField] CharacterAttributes _sliderAttributes;

        public void SetCurrentSelectedAttributes()
        {
            PlayerUIManager.Instance._playerUILevelUpManager._currentSelectedAttribute = _sliderAttributes;
        }
    }
}