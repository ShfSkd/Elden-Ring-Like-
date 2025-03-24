using System;
using UnityEngine;
using UnityEngine.UI;
namespace SKD.UI.PlayerUI
{
    public class PlayerUISelectedButtonOnEnable : MonoBehaviour
    {
        private Button _button;

        void Awake()
        {
            _button = GetComponent<Button>();
        }
        void OnEnable()
        {
            _button.Select();
            _button.OnSelect(null);
        }
    }
}