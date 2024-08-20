using SKD.Items;
using SKD.World_Manager;
using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerEquipmentManager : CharacterEqiqmentManager
    {
        PlayerManager _playerManager;

        public WeaponModelInstantationSlot _rightHandSlot;
        public WeaponModelInstantationSlot _leftHandSlot;

        [SerializeField] WeaponManager _rightWeaponManager;
        [SerializeField] WeaponManager _leftWeaponManager;

        public GameObject _rightHandWeaponModel;
        public GameObject _leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();

            // Get our slots 
            InitializeWeaponSlots();
        }
        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothhands();
        }
        private void InitializeWeaponSlots()
        {
            WeaponModelInstantationSlot[] weaponsSlots = GetComponentsInChildren<WeaponModelInstantationSlot>();

            foreach (var weaponSlot in weaponsSlots)
            {
                if (weaponSlot._weaponSlot == WeaponModelSlot.RightHand)
                {
                    _rightHandSlot = weaponSlot;
                }
                else if (weaponSlot._weaponSlot == WeaponModelSlot.LeftHand)
                {
                    _leftHandSlot = weaponSlot;
                }
            }
        }
        public void LoadWeaponsOnBothhands()
        {
            LoadRightWepon();
            LoadLeftWeapon();
        }
        // Right Wepaon
        public void LoadRightWepon()
        {
            if (_playerManager._playerInventoryManager._currentRightHandWeapon != null)
            {
                // Remove the old weapon
                _rightHandSlot.UnloadWeaponModel();

                // Bring in the new weapon
                _rightHandWeaponModel = Instantiate(_playerManager._playerInventoryManager._currentRightHandWeapon._weaponModel);
                _rightHandSlot.LoadWeaponModel(_rightHandWeaponModel);
                _rightWeaponManager = _rightHandWeaponModel.GetComponent<WeaponManager>();
                _rightWeaponManager.SetWeaponDamage(_playerManager, _playerManager._playerInventoryManager._currentRightHandWeapon);
                // Assign weapons damage, to its collider 
            }
        }
        public void SwitchRightWeapon()
        {
            if (!_playerManager.IsOwner)
                return;

            _playerManager._playerAnimationManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            // Elden Rings Weapon Swapping:
            // 1. Check if we have another weapon beside our main weapon, if we do ,Never swap to unarmed, rotate between weapon 1 & 2
            // 2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not proceed both empty slots before returning to the main weapon

            WeaponItem selectedWeapon = null;

            // Disable two handing if we are two handing

            // Add one to our index to switch ti the next potential weapon
            _playerManager._playerInventoryManager._rightHandWeaponIndex += 1;
            // If our index number is out of bounds, reset it to position #1(0)
            if (_playerManager._playerInventoryManager._rightHandWeaponIndex < 0 || _playerManager._playerInventoryManager._rightHandWeaponIndex > 2)
            {
                _playerManager._playerInventoryManager._rightHandWeaponIndex = 0;

                // We check if we are holding more then one weapon 
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < _playerManager._playerInventoryManager._weaponInRigthHandSlots.Length; i++)
                {
                    if (_playerManager._playerInventoryManager._weaponInRigthHandSlots[i]._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = _playerManager._playerInventoryManager._weaponInRigthHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }
                if (weaponCount <= 1)
                {
                    _playerManager._playerInventoryManager._rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance._unarmedWeapon;
                    _playerManager._playerNetworkManager._currentRightWeaponID.Value = selectedWeapon._itemID;

                }
                else
                {
                    _playerManager._playerInventoryManager._rightHandWeaponIndex = firstWeaponPosition;
                    _playerManager._playerNetworkManager._currentRightWeaponID.Value = firstWeapon._itemID;
                }
                return;
            }
            foreach (WeaponItem weapon in _playerManager._playerInventoryManager._weaponInRigthHandSlots)
            {
                // Check to see if the next potential weapon is not the "unarmed" weapon
                if (_playerManager._playerInventoryManager._weaponInRigthHandSlots[_playerManager._playerInventoryManager._rightHandWeaponIndex]._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                {
                    selectedWeapon = _playerManager._playerInventoryManager._weaponInRigthHandSlots[_playerManager._playerInventoryManager._rightHandWeaponIndex];
                    // Assign the network weapon ID so it switch for all connected clients 
                    _playerManager._playerNetworkManager._currentRightWeaponID.Value = _playerManager._playerInventoryManager._weaponInRigthHandSlots[_playerManager._playerInventoryManager._rightHandWeaponIndex]._itemID;
                    return;
                }
            }
            if (selectedWeapon == null && _playerManager._playerInventoryManager._rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }
        //Left Weapon
        public void LoadLeftWeapon()
        {
            if (_playerManager._playerInventoryManager._currentLeftHandWeapon != null)
            {
                // Remove the old weapon
                _leftHandSlot.UnloadWeaponModel();

                // Bring in the new weapon
                _leftHandWeaponModel = Instantiate(_playerManager._playerInventoryManager._currentLeftHandWeapon._weaponModel);
                _leftHandSlot.LoadWeaponModel(_leftHandWeaponModel);
                _leftWeaponManager = _leftHandWeaponModel.GetComponent<WeaponManager>();
                _leftWeaponManager.SetWeaponDamage(_playerManager, _playerManager._playerInventoryManager._currentLeftHandWeapon);
                // Assign weapons damage, to its collider 
            }
        }
        public void SwitchLeftWeapon()
        {
            if (!_playerManager.IsOwner)
                return;

            _playerManager._playerAnimationManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            // Elden Rings Weapon Swapping:
            // 1. Check if we have another weapon beside our main weapon, if we do ,Never swap to unarmed, rotate between weapon 1 & 2
            // 2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not proceed both empty slots before returning to the main weapon

            WeaponItem selectedWeapon = null;

            // Disable two handing if we are two handing

            // Add one to our index to switch ti the next potential weapon
            _playerManager._playerInventoryManager._leftHandWeaponIndex += 1;
            // If our index number is out of bounds, reset it to position #1(0)
            if (_playerManager._playerInventoryManager._leftHandWeaponIndex < 0 || _playerManager._playerInventoryManager._leftHandWeaponIndex > 2)
            {
                _playerManager._playerInventoryManager._leftHandWeaponIndex = 0;

                // We check if we are holding more then one weapon 
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < _playerManager._playerInventoryManager._weaponInLefthHandSlot.Length; i++)
                {
                    if (_playerManager._playerInventoryManager._weaponInLefthHandSlot[i]._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = _playerManager._playerInventoryManager._weaponInLefthHandSlot[i];
                            firstWeaponPosition = i;
                        }
                    }
                }
                if (weaponCount <= 1)
                {
                    _playerManager._playerInventoryManager._leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance._unarmedWeapon;
                    _playerManager._playerNetworkManager._currentLeftWeaponID.Value = selectedWeapon._itemID;

                }
                else
                {
                    _playerManager._playerInventoryManager._leftHandWeaponIndex = firstWeaponPosition;
                    _playerManager._playerNetworkManager._currentLeftWeaponID.Value = firstWeapon._itemID;
                }
                return;
            }
            foreach (WeaponItem weapon in _playerManager._playerInventoryManager._weaponInLefthHandSlot)
            {
                // Check to see if the next potential weapon is not the "unarmed" weapon
                if (_playerManager._playerInventoryManager._weaponInLefthHandSlot[_playerManager._playerInventoryManager._leftHandWeaponIndex]._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                {
                    selectedWeapon = _playerManager._playerInventoryManager._weaponInLefthHandSlot[_playerManager._playerInventoryManager._leftHandWeaponIndex];
                    // Assign the network weapon ID so it switch for all connected clients 
                    _playerManager._playerNetworkManager._currentLeftWeaponID.Value = _playerManager._playerInventoryManager._weaponInLefthHandSlot[_playerManager._playerInventoryManager._leftHandWeaponIndex]._itemID;
                    return;
                }
            }
            if (selectedWeapon == null && _playerManager._playerInventoryManager._leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        // Damage Colliders
        public void OpenDamageCollider()
        {
            if (_playerManager._playerNetworkManager._isUsingRightHand.Value)
            {
                _rightWeaponManager._meleeDamageCollider.EnableDamageCollider();
                _playerManager._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_playerManager._playerInventoryManager._currentRightHandWeapon._whooshes));
            }
            else if (_playerManager._playerNetworkManager._isUsingLeftHand.Value)
            {
                _leftWeaponManager._meleeDamageCollider.EnableDamageCollider();
                _playerManager._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_playerManager._playerInventoryManager._currentLeftHandWeapon._whooshes));
            }
        }
        public void CloseDamageCollider()
        {
            if (_playerManager._playerNetworkManager._isUsingRightHand.Value)
            {
                _rightWeaponManager._meleeDamageCollider.DisableDamageCollider();
            }
            else if (_playerManager._playerNetworkManager._isUsingLeftHand.Value)
            {
                _leftWeaponManager._meleeDamageCollider.DisableDamageCollider();
            }
        }
    }
}