using SKD.Interacts;
using SKD.World_Manager;
using UnityEngine;
using UnityEngine.UI;
namespace SKD.UI.PlayerUI
{
    public class PlayerUITeleportLocationManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject _menu;

        [SerializeField] GameObject[] _teleportLocations;
        public void OpenTeleportLocationManagerMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = true;
            _menu.SetActive(true);

            CheckForUnlockTeleports();
        }
        public void CloseTeleportLocationMenu()
        {
            PlayerUIManager.Instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
        }

        private void CheckForUnlockTeleports()
        {
            bool hasFirstSelectedButton = false;
            for (int i = 0; i < _teleportLocations.Length; i++)
            {
                for (int s = 0; s < WorldObjectManager.Instance._sitesOfGraceList.Count; s++)
                {
                    if (WorldObjectManager.Instance._sitesOfGraceList[s]._siteOfGraceID == i)
                    {
                        if (WorldObjectManager.Instance._sitesOfGraceList[s]._isActivated.Value)
                        {
                            _teleportLocations[i].SetActive(true);
                            if (!hasFirstSelectedButton)
                            {
                                hasFirstSelectedButton = true;
                                _teleportLocations[i].GetComponent<Button>().Select();
                                _teleportLocations[i].GetComponent<Button>().OnSelect(null);
                            }
                        }
                        else
                        {
                            _teleportLocations[i].SetActive(false);
                            hasFirstSelectedButton = false;
                        }
                    }
                }
            }
        }
        public void TeleportToSiteOfGrace(int siteID)
        {
            foreach (var siteOfGrace in WorldObjectManager.Instance._sitesOfGraceList)
            {
                if (siteOfGrace._siteOfGraceID == siteID)
                {
                    // Teleport
                    siteOfGrace.TeleportToSiteOfGrace();
                    return;
                }
            }
        }

    }
}