using System.Collections;
using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUICharacterMenuManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject _menu;

        public void OpenCharacterMenu()
        {
            PlayerUIManger.Instance._menuWindowIsOpen = true;
            _menu.SetActive(true);
        }
        public void CloseCharacterMenu()
        {
            PlayerUIManger.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }

        public void CloseCharacterMenuAfterFixedUpdate()
        {
            StartCoroutine(WaitThenCloseMenu());
        }
        private IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();
            
            PlayerUIManger.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }
    }
}