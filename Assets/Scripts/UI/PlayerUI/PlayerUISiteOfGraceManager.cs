using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUISiteOfGraceManager : PlayerUIMenu
    { 
        public void OpenTeleportLocationMenu()
        {
            CloseMenu();
            PlayerUIManager.Instance._playerUITeleportLocationManager.OpenMenu();
        }
        public void OpenLevelUpMenu()
        {
            CloseMenu();
            PlayerUIManager.Instance._playerUILevelUpManager.OpenMenu();
        }
    }
}