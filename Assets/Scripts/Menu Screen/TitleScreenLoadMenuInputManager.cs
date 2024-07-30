using SKD.MenuScreen;
using System.Collections;
using UnityEngine;

namespace SKD.Menu_Screen
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        PlayerControls _playerControls;

        [Header("Title Screen Inputs")]
        [SerializeField] bool _deleteCharacterSlot;

        private void Update()
        {
            if (_deleteCharacterSlot)
            {
                _deleteCharacterSlot = false;
                TitleScreenManager.Instance.AttampToDeleteCharacterSlot();
            }
        }
        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.UI.Delete.performed += i => _deleteCharacterSlot = true;
            }

            _playerControls.Enable(); 
        }
        private void OnDisable()
        {
            _playerControls.Disable();
        }
    }
}