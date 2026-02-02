using System.Collections;
using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUICharacterMenuManager : PlayerUIMenu
    {

        public override void OpenMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = true;
            _menu.SetActive(true);
        }
        public override void CloseMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            StartCoroutine(WaitThenCloseMenu());
        }
        protected override IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();
            
            PlayerUIManager.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }
    }
}