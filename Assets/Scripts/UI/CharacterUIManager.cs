using System.Collections;
using UnityEngine;

namespace SKD.UI
{
    public class CharacterUIManager : MonoBehaviour
    {
        [Header("UI")]
        public bool _hasFloatingHPBar = true;
        public UI_CharacterHPBar _characterHPBar;

        public void OnHPChanged(int oldValue, int newValue)
        {
            _characterHPBar._oldHealthValue = oldValue;
            _characterHPBar.SetStat(newValue);
        }
    }
}