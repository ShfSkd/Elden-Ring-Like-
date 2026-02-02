using System.Collections;
using UnityEngine;

namespace SKD.UI.PlayerUI
{
    public class PlayerUIMenu : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] protected GameObject _menu;
        
        public virtual void OpenMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = true;
            _menu.SetActive(true);
        }
        public virtual void CloseMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }

        public virtual void CloseMenuAfterFixedUpdate()
        {
            if(!_menu.activeInHierarchy)
                return;
            
            StartCoroutine(WaitThenCloseMenu());
        }
        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();
            
            PlayerUIManager.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }
    }
}