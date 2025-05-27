using System;
using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUIToggleHud : MonoBehaviour
    {
        void OnEnable()
        {
            // Hide the HUD
            PlayerUIManager.Instance._playerUIHUDManager.ToggleHud(false);
        }
        void OnDisable()
        {
            // Being the HUD back
            PlayerUIManager.Instance._playerUIHUDManager.ToggleHud(true);

        }
    }
}