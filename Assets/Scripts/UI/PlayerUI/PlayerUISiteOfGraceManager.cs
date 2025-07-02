using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUISiteOfGraceManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject _menu;
        
        public void OpenSiteOfGraceManagerMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = true;
            _menu.SetActive(true);
        }
        public void CloseSiteOfGraceManagerMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }

        public void OpenTeleportLocationMenu()
        {
            CloseSiteOfGraceManagerMenu();
            PlayerUIManager.Instance._playerUITeleportLocationManager.OpenTeleportLocationManagerMenu();
        }
    }
}