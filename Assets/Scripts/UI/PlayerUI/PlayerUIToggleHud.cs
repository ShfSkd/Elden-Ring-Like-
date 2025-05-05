using System;
using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUIToggleHud : MonoBehaviour
    {
        void OnEnable()
        {
            // Hide the HUD
            PlayerUIManger.Instance._playerUIHUDManager.ToggleHud(false);
        }
        void OnDisable()
        {
            // Being the HUD back
            PlayerUIManger.Instance._playerUIHUDManager.ToggleHud(true);

        }
    }
}