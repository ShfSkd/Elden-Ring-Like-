using System;
using SKD.Items;
using SKD.World_Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character.Player
{
    public class PlayerEquipmentManager : CharacterEqiqmentManager
    {
        PlayerManager _player;

        [Header("Weapon Model Instantiation Slot ")]
        public WeaponModelInstantiationSlot _rightHandWeaponSlot;

        public WeaponModelInstantiationSlot _leftHandWeaponSlot;
        public WeaponModelInstantiationSlot _leftHandShieldSlot;
        public WeaponModelInstantiationSlot _backSlot;

        [Header("Weapon Model")]
        public GameObject _rightHandWeaponModel;
        public GameObject _leftHandWeaponModel;

        [Header("Weapon Manager")] [SerializeField]
        WeaponManager _rightWeaponManager;

        [SerializeField] WeaponManager _leftWeaponManager;

        [Header("Debug")]
        [SerializeField] bool _equipNewItems;

        [Header("Male Equipment Models")]
        public GameObject _maleFullHelmetObject;
        public GameObject[] _maleHeadFullHelmets;

        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<PlayerManager>();

            // Get our slots 
            InitializeWeaponSlots();

            List<GameObject> maleFullHelmetsList = new List<GameObject>();

            foreach (Transform child in _maleFullHelmetObject.transform)
            {
                maleFullHelmetsList.Add(child.gameObject);
            }
            _maleHeadFullHelmets = maleFullHelmetsList.ToArray();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothhands();
        }
        void Update()
        {
            if (_equipNewItems)
            {
                _equipNewItems = false;
                DebugNewItems();
            }
        }
        private void DebugNewItems()
        {
            Debug.Log("Equipping New Items ");
             
            LoadHeadEquipment(_player._playerInventoryManager._headEquipment);

            if (_player._playerInventoryManager._bodyEquipment != null)
                LoadBodyEquipment(_player._playerInventoryManager._bodyEquipment);

            if (_player._playerInventoryManager._handEquipment != null)
                LoadHandEquipment(_player._playerInventoryManager._handEquipment);

            if (_player._playerInventoryManager._legEquipment != null)
                LoadLegEquipment(_player._playerInventoryManager._legEquipment);
        }

        // Equipment 
        public void LoadHeadEquipment(HeadEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            UnloadHeadEquipmentModels();
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._handEquipmentID.Value = -1; // -1 will never be an Item ID, so it will always ne null

                _player._playerInventoryManager._headEquipment = null;
                return;
            }
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            _player._playerInventoryManager._headEquipment = equipment;
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            // 6. Load head equipment models
            foreach (var model in equipment._equipmentModels)
            {
                model.LoadModel(_player, true);
            }
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();
            
            if (_player.IsOwner)
                _player._playerNetworkManager._headEquipmentID.Value = equipment._itemID;
        }

        private void UnloadHeadEquipmentModels()
        {
            foreach (var model in _maleHeadFullHelmets)
            {
                model.SetActive(false);
            }
            // Re-enable head,hair 
        }
        public void LoadBodyEquipment(BodyEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            // 6. Load head equipment models
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();
        }
        public void LoadLegEquipment(LegEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            // 6. Load head equipment models
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();
        }
        public void LoadHandEquipment(HandEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            // 6. Load head equipment models
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();
        }
        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponsSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponsSlots)
            {
                if (weaponSlot._weaponSlot == WeaponModelSlot.RightHandWeaponSlot)
                {
                    _rightHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot._weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
                {
                    _leftHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot._weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
                {
                    _leftHandShieldSlot = weaponSlot;
                }
                else if (weaponSlot._weaponSlot == WeaponModelSlot.BackSlot)
                {
                    _backSlot = weaponSlot;
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
            if (_player._playerInventoryManager._currentRightHandWeapon != null)
            {
                // Remove the old weapon
                _rightHandWeaponSlot.UnloadWeaponModel();

                // Bring in the new weapon
                _rightHandWeaponModel =
                    Instantiate(_player._playerInventoryManager._currentRightHandWeapon._weaponModel);
                _rightHandWeaponSlot.PlaceWeaponModelIntoSlot(_rightHandWeaponModel);
                _rightWeaponManager = _rightHandWeaponModel.GetComponent<WeaponManager>();
                _rightWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentRightHandWeapon);
                _player._playerAnimationManager.UpdateAnimatorController(_player._playerInventoryManager
                    ._currentRightHandWeapon._weaponAnimator);
                // Assign weapons damage, to its collider 
            }
        }

        public void SwitchRightWeapon()
        {
            if (!_player.IsOwner)
                return;

            _player._playerAnimationManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            // Elden Rings Weapon Swapping:
            // 1. Check if we have another weapon beside our main weapon, if we do ,Never swap to unarmed, rotate between weapon 1 & 2
            // 2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not proceed both empty slots before returning to the main weapon

            WeaponItem selectedWeapon = null;

            // Disable two handing if we are two handing

            // Add one to our index to switch ti the next potential weapon
            _player._playerInventoryManager._rightHandWeaponIndex += 1;
            // If our index number is out of bounds, reset it to position #1(0)
            if (_player._playerInventoryManager._rightHandWeaponIndex < 0 ||
                _player._playerInventoryManager._rightHandWeaponIndex > 2)
            {
                _player._playerInventoryManager._rightHandWeaponIndex = 0;

                // We check if we are holding more then one weapon 
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < _player._playerInventoryManager._weaponInRigthHandSlots.Length; i++)
                {
                    if (_player._playerInventoryManager._weaponInRigthHandSlots[i]._itemID !=
                        WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = _player._playerInventoryManager._weaponInRigthHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    _player._playerInventoryManager._rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance._unarmedWeapon;
                    _player._playerNetworkManager._currentRightWeaponID.Value = selectedWeapon._itemID;
                }
                else
                {
                    _player._playerInventoryManager._rightHandWeaponIndex = firstWeaponPosition;
                    _player._playerNetworkManager._currentRightWeaponID.Value = firstWeapon._itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in _player._playerInventoryManager._weaponInRigthHandSlots)
            {
                // Check to see if the next potential weapon is not the "unarmed" weapon
                if (_player._playerInventoryManager
                        ._weaponInRigthHandSlots[_player._playerInventoryManager._rightHandWeaponIndex]._itemID !=
                    WorldItemDatabase.Instance._unarmedWeapon._itemID)
                {
                    selectedWeapon =
                        _player._playerInventoryManager._weaponInRigthHandSlots[
                            _player._playerInventoryManager._rightHandWeaponIndex];
                    // Assign the network weapon ID so it switch for all connected clients 
                    _player._playerNetworkManager._currentRightWeaponID.Value = _player._playerInventoryManager
                        ._weaponInRigthHandSlots[_player._playerInventoryManager._rightHandWeaponIndex]._itemID;
                    return;
                }
            }

            if (selectedWeapon == null && _player._playerInventoryManager._rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }

        //Left Weapon
        public void LoadLeftWeapon()
        {
            if (_player._playerInventoryManager._currentLeftHandWeapon != null)
            {
                // Remove the old weapon
                if (_leftHandWeaponSlot._currentWeaponModel != null)
                    _leftHandWeaponSlot.UnloadWeaponModel();

                if (_leftHandShieldSlot._currentWeaponModel != null)
                    _leftHandShieldSlot.UnloadWeaponModel();

                // Bring in the new weapon
                _leftHandWeaponModel = Instantiate(_player._playerInventoryManager._currentLeftHandWeapon._weaponModel);

                switch (_player._playerInventoryManager._currentLeftHandWeapon._weaponModelType)
                {
                    case WeaponModelType.Weapon:
                        _leftHandWeaponSlot.PlaceWeaponModelIntoSlot(_leftHandWeaponModel);
                        break;
                    case WeaponModelType.Shield:
                        _leftHandShieldSlot.PlaceWeaponModelIntoSlot(_leftHandWeaponModel);
                        break;
                    default:
                        break;
                }

                _leftWeaponManager = _leftHandWeaponModel.GetComponent<WeaponManager>();
                _leftWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentLeftHandWeapon);
                // Assign weapons damage, to its collider 
            }
        }

        public void SwitchLeftWeapon()
        {
            if (!_player.IsOwner)
                return;

            _player._playerAnimationManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            // Elden Rings Weapon Swapping:
            // 1. Check if we have another weapon beside our main weapon, if we do ,Never swap to unarmed, rotate between weapon 1 & 2
            // 2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not proceed both empty slots before returning to the main weapon

            WeaponItem selectedWeapon = null;

            // Disable two handing if we are two handing

            // Add one to our index to switch ti the next potential weapon
            _player._playerInventoryManager._leftHandWeaponIndex += 1;
            // If our index number is out of bounds, reset it to position #1(0)
            if (_player._playerInventoryManager._leftHandWeaponIndex < 0 ||
                _player._playerInventoryManager._leftHandWeaponIndex > 2)
            {
                _player._playerInventoryManager._leftHandWeaponIndex = 0;

                // We check if we are holding more then one weapon 
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < _player._playerInventoryManager._weaponInLefthHandSlot.Length; i++)
                {
                    if (_player._playerInventoryManager._weaponInLefthHandSlot[i]._itemID !=
                        WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = _player._playerInventoryManager._weaponInLefthHandSlot[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    _player._playerInventoryManager._leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance._unarmedWeapon;
                    _player._playerNetworkManager._currentLeftWeaponID.Value = selectedWeapon._itemID;
                }
                else
                {
                    _player._playerInventoryManager._leftHandWeaponIndex = firstWeaponPosition;
                    _player._playerNetworkManager._currentLeftWeaponID.Value = firstWeapon._itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in _player._playerInventoryManager._weaponInLefthHandSlot)
            {
                // Check to see if the next potential weapon is not the "unarmed" weapon
                if (_player._playerInventoryManager
                        ._weaponInLefthHandSlot[_player._playerInventoryManager._leftHandWeaponIndex]._itemID !=
                    WorldItemDatabase.Instance._unarmedWeapon._itemID)
                {
                    selectedWeapon =
                        _player._playerInventoryManager._weaponInLefthHandSlot[
                            _player._playerInventoryManager._leftHandWeaponIndex];
                    // Assign the network weapon ID so it switch for all connected clients 
                    _player._playerNetworkManager._currentLeftWeaponID.Value = _player._playerInventoryManager
                        ._weaponInLefthHandSlot[_player._playerInventoryManager._leftHandWeaponIndex]._itemID;
                    return;
                }
            }

            if (selectedWeapon == null && _player._playerInventoryManager._leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        // Two Hand
        public void UnTwoHandWeapon()
        {
            // Update Animator Controller to current main hand Weapon 
            _player._playerAnimationManager.UpdateAnimatorController(_player._playerInventoryManager._currentRightHandWeapon._weaponAnimator);

            // Remove the strength bonus (Two handing a weapon makes your strength level (strength + (strength * 0.5)

            // Un-Two hand the model and move the model that isnt being two handed back to its hand

            // Left Hand
            if (_player._playerInventoryManager._currentLeftHandWeapon._weaponModelType == WeaponModelType.Weapon)
            {
                _leftHandWeaponSlot.PlaceWeaponModelIntoSlot(_leftHandWeaponModel);
            }
            else if (_player._playerInventoryManager._currentLeftHandWeapon._weaponModelType == WeaponModelType.Shield)
            {
                _leftHandShieldSlot.PlaceWeaponModelIntoSlot(_leftHandWeaponModel);

            }
            // Right hand
            _rightHandWeaponSlot.PlaceWeaponModelIntoSlot(_rightHandWeaponModel);
            // Refresh damage collider calculation (strength scaling would be effected since the strength bonus was remove)
            _rightWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentRightHandWeapon);
            _leftWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentLeftHandWeapon);
        }

        public void TwoHandRightWeapon()
        {
            // Check for unTwoHandable item(like unarm) if we are attempting to two hand unarmed, return 
            if (_player._playerInventoryManager._currentRightHandWeapon == WorldItemDatabase.Instance._unarmedWeapon)
            {
                // If we are returning and not two handing this weapon, reset bool status 
                if (_player.IsOwner)
                {
                    _player._playerNetworkManager._isTwoHandingRightWepoen.Value = false;
                    _player._playerNetworkManager._isTwoHandingWeapon.Value = false;
                }

                return;
            }

            // Update animator 
            _player._playerAnimationManager.UpdateAnimatorController(_player._playerInventoryManager
                ._currentRightHandWeapon._weaponAnimator);

            // Place the Non-Two handed weapon model in the back slot or hip slot 
            _backSlot.PlaceWeaponModelInUnequippedSlot(_leftHandWeaponModel,
                _player._playerInventoryManager._currentLeftHandWeapon._weaponClass, _player);

            // Place the handed weapon model in the main (Right hand)
            _rightHandWeaponSlot.PlaceWeaponModelIntoSlot(_rightHandWeaponModel);

            // if you are two handed the left weapon, place the left weapon model in the character right hand
            _rightWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentRightHandWeapon);
            _leftWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentLeftHandWeapon);
        }

        public void TwoHandLeftWeapon()
        {
            //  check for unTwoHandable item(like unarm) if we are attempting to two hand unarmed, return 
            if (_player._playerInventoryManager._currentLeftHandWeapon == WorldItemDatabase.Instance._unarmedWeapon)
            {
                if (_player.IsOwner)
                {
                    //  If we are returning and not two handing this weapon, reset bool status 
                    _player._playerNetworkManager._isTwoHandingLeftWeapon.Value = false;
                    _player._playerNetworkManager._isTwoHandingWeapon.Value = false;
                }

                return;
            }

            // Update animator 
            _player._playerAnimationManager.UpdateAnimatorController(_player._playerInventoryManager
                ._currentLeftHandWeapon._weaponAnimator);


            //  Place the Non-Two handed weapon model in the back slot or hip slot 
            _backSlot.PlaceWeaponModelInUnequippedSlot(_rightHandWeaponModel,
                _player._playerInventoryManager._currentRightHandWeapon._weaponClass, _player);
            //  place the handed weapon model in the main (Right hand)
            _rightHandWeaponSlot.PlaceWeaponModelIntoSlot(_leftHandWeaponModel);

            //  if you are two handed the right weapon, place the left weapon model in the character left hand
            _rightWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentRightHandWeapon);
            _leftWeaponManager.SetWeaponDamage(_player, _player._playerInventoryManager._currentLeftHandWeapon);
        }

        // Damage Colliders
        public void OpenDamageCollider()
        {
            if (_player._playerNetworkManager._isUsingRightHand.Value)
            {
                _rightWeaponManager._meleeDamageCollider.EnableDamageCollider();
                _player._characterSoundFXManager.PlaySoundFX(
                    WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_player._playerInventoryManager
                        ._currentRightHandWeapon._whooshes));
            }
            else if (_player._playerNetworkManager._isUsingLeftHand.Value)
            {
                _leftWeaponManager._meleeDamageCollider.EnableDamageCollider();
                _player._characterSoundFXManager.PlaySoundFX(
                    WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_player._playerInventoryManager
                        ._currentLeftHandWeapon._whooshes));
            }
        }

        public void CloseDamageCollider()
        {
            if (_player._playerNetworkManager._isUsingRightHand.Value)
            {
                _rightWeaponManager._meleeDamageCollider.DisableDamageCollider();
            }
            else if (_player._playerNetworkManager._isUsingLeftHand.Value)
            {
                _leftWeaponManager._meleeDamageCollider.DisableDamageCollider();
            }
        }
    }
}