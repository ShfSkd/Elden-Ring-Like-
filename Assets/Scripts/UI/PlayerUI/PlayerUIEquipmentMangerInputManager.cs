using System;
using UnityEngine;
namespace SKD.UI.PlayerUI
{
    public class PlayerUIEquipmentMangerInputManager : MonoBehaviour
    {
        PlayerControls _playerControls;

        PlayerUIEquipmentManager _playerUIEquipmentManager;

        [Header("Inputs")]
        [SerializeField] bool _unequipItemInput;

        void Awake()
        {
            _playerUIEquipmentManager = GetComponentInParent<PlayerUIEquipmentManager>();
        }
        void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();

                _playerControls.PlayerActions.X.performed += i => _unequipItemInput = true;
            }
            _playerControls.Enable();   
        }
        void OnDisable()
        {
          _playerControls.Disable();  
        }
        void Update()
        {
            HandlePlayerUIEquipmentMangerInputs();
        }
        void HandlePlayerUIEquipmentMangerInputs()
        {
            if (_unequipItemInput)
            {
                _unequipItemInput = false;
                _playerUIEquipmentManager.UnEquipSelectedItem();
            }
        }
    }
}