using System;
using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUIToggleHud : MonoBehaviour
    {
        void OnEnable()
        {
            // Hide the HUD
            PlayerUIManger.instance._playerUIHUDManager.ToggleHud(false);
        }
        void OnDisable()
        {
            // Being the HUD back
            PlayerUIManger.instance._playerUIHUDManager.ToggleHud(true);

        }
    }
}