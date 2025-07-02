using System;
using SKD.Items;
using SKD.World_Manager;
using System.Collections;
using System.Collections.Generic;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.Weapons.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character.Player
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager _player;

        [Header("Weapon Model Instantiation Slot ")]
        [HideInInspector] public WeaponModelInstantiationSlot _rightHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot _leftHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot _leftHandShieldSlot;
        [HideInInspector] public WeaponModelInstantiationSlot _backSlot;

        [Header("Weapon Model")]
        [HideInInspector] public GameObject _rightHandWeaponModel;
        [HideInInspector] public GameObject _leftHandWeaponModel;

        [Header("Weapon Manager")]
        public WeaponManager _rightWeaponManager;
        public WeaponManager _leftWeaponManager;

        [Header("Debug")]
        [SerializeField] bool _equipNewItems;

        [Header("General Equipment Models")]
        public GameObject _hatsObject;
        [HideInInspector] public GameObject[] _hats;
        public GameObject _hoodObject;
        [HideInInspector] public GameObject[] _hoods;
        public GameObject _faceCoverObject;
        [HideInInspector] public GameObject[] _faceCovers;
        public GameObject _helmetAccessoriesObject;
        [HideInInspector] public GameObject[] _helmetAccessories;
        public GameObject _backAccessoriesObject;
        [HideInInspector] public GameObject[] _backAccessories;
        public GameObject _hipAccessoriesObject;
        [HideInInspector] public GameObject[] _hipAccessories;
        public GameObject _rightShoulderObject;
        [HideInInspector] public GameObject[] _rightShoulders;
        public GameObject _rightElbowObject;
        [HideInInspector] public GameObject[] _rightElbows;
        public GameObject _rightKneeObject;
        [HideInInspector] public GameObject[] _rightKnees;
        public GameObject _leftShoulderObject;
        [HideInInspector] public GameObject[] _leftShoulders;
        public GameObject _leftElbowObject;
        [HideInInspector] public GameObject[] _leftElbows;
        public GameObject _leftKneeObject;
        [HideInInspector] public GameObject[] _leftKnees;

        [Header("Male Equipment Models")]
        public GameObject _maleFullHelmetObject;
        [HideInInspector] public GameObject[] _maleHeadFullHelmets;
        public GameObject _maleFullBodyObject;
        [HideInInspector] public GameObject[] _maleBodies;
        public GameObject _maleRightUpperArmObject;
        [HideInInspector] public GameObject[] _maleRightUpperArms;
        public GameObject _maleRightLowerArmObject;
        [HideInInspector] public GameObject[] _maleRightLowerArms;
        public GameObject _maleRightHandObject;
        [HideInInspector] public GameObject[] _maleRightHands;
        public GameObject _maleLeftUpperArmObject;
        [HideInInspector] public GameObject[] _maleLeftUpperArms;
        public GameObject _maleLeftLowerArmObject;
        [HideInInspector] public GameObject[] _maleLeftLowerArms;
        public GameObject _maleLeftHandObject;
        [HideInInspector] public GameObject[] _maleLeftHands;
        public GameObject _maleHipObject;
        [HideInInspector] public GameObject[] _maleHips;
        public GameObject _maleRightLegObject;
        [HideInInspector] public GameObject[] _maleRightLegs;
        public GameObject _maleLeftLegObject;
        [HideInInspector] public GameObject[] _maleLeftLegs;

        [Header("Female Equipment Models")]
        public GameObject _femaleFullHelmetObject;
        [HideInInspector] public GameObject[] _femaleHeadFullHelmets;
        public GameObject _femaleFullBodyObject;
        [HideInInspector] public GameObject[] _femaleBodies;
        public GameObject _femaleRightUpperArmObject;
        [HideInInspector] public GameObject[] _femaleRightUpperArms;
        public GameObject _femaleRightLowerArmObject;
        [HideInInspector] public GameObject[] _femaleRightLowerArms;
        public GameObject _femaleRightHandObject;
        [HideInInspector] public GameObject[] _femaleRightHands;
        public GameObject _femaleLeftUpperArmObject;
        [HideInInspector] public GameObject[] _femaleLeftUpperArms;
        public GameObject _femaleLeftLowerArmObject;
        [HideInInspector] public GameObject[] _femaleLeftLowerArms;
        public GameObject _femaleLeftHandObject;
        [HideInInspector] public GameObject[] _femaleLeftHands;
        public GameObject _femaleHipObject;
        [HideInInspector] public GameObject[] _femaleHips;
        public GameObject _femaleRightLegObject;
        [HideInInspector] public GameObject[] _femaleRightLegs;
        public GameObject _femaleLeftLegObject;
        [HideInInspector] public GameObject[] _femaleLeftLegs;
        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<PlayerManager>();
            //    InitializeArmorModels();
            // Get our slots 
            InitializeWeaponSlots();

            // TO DO- when we have skins for the player body we can uncomment those line + InitializeArmorModels(
            /*List<GameObject> hoodsList = new List<GameObject>();
            foreach (Transform child in _hoodObject.transform)
            {
                hoodsList.Add(child.gameObject);
            }
            _hoods = hoodsList.ToArray();

            List<GameObject> maleFullHelmetsList = new List<GameObject>();
            foreach (Transform child in _maleFullHelmetObject.transform)
            {
                maleFullHelmetsList.Add(child.gameObject);
            }
            _maleHeadFullHelmets = maleFullHelmetsList.ToArray();

            List<GameObject> faceCoverList = new List<GameObject>();
            foreach (Transform child in _faceCoverObject.transform)
            {
                faceCoverList.Add(child.gameObject);
            }
            _faceCovers = faceCoverList.ToArray();


            List<GameObject> helmetAccessoriesList = new List<GameObject>();
            foreach (Transform child in _helmetAccessoriesObject.transform)
            {
                helmetAccessoriesList.Add(child.gameObject);
            }
            _helmetAccessories = helmetAccessoriesList.ToArray();

            List<GameObject> maleBodiesList = new List<GameObject>();
            foreach (Transform child in _maleFullHelmetObject.transform)
            {
                maleBodiesList.Add(child.gameObject);
            }
            _maleBodies = maleBodiesList.ToArray();

            List<GameObject> backAccessoriesList = new List<GameObject>();
            foreach (Transform child in _backAccessoriesObject.transform)
            {
                backAccessoriesList.Add(child.gameObject);
            }
            _backAccessories = backAccessoriesList.ToArray();

            List<GameObject> hipsAccessoriesList = new List<GameObject>();
            foreach (Transform child in _hipAccessoriesObject.transform)
            {
                hipsAccessoriesList.Add(child.gameObject);
            }
            _hipAccessories = hipsAccessoriesList.ToArray();

            List<GameObject> rightShoulderList = new List<GameObject>();
            foreach (Transform child in _rightShoulderObject.transform)
            {
                rightShoulderList.Add(child.gameObject);
            }
            _rightShoulders = rightShoulderList.ToArray();

            List<GameObject> rightElbowList = new List<GameObject>();
            foreach (Transform child in _rightElbowObject.transform)
            {
                rightElbowList.Add(child.gameObject);
            }
            _rightElbows = rightElbowList.ToArray();

            List<GameObject> rightKneeList = new List<GameObject>();
            foreach (Transform child in _rightKneeObject.transform)
            {
                rightKneeList.Add(child.gameObject);
            }
            _rightKnees = rightKneeList.ToArray();

            List<GameObject> leftShoulderList = new List<GameObject>();
            foreach (Transform child in _leftShoulderObject.transform)
            {
                leftShoulderList.Add(child.gameObject);
            }
            _leftShoulders = leftShoulderList.ToArray();

            List<GameObject> leftElbowList = new List<GameObject>();
            foreach (Transform child in _leftElbowObject.transform)
            {
                leftElbowList.Add(child.gameObject);
            }
            _leftElbows = leftElbowList.ToArray();

            List<GameObject> leftKneeList = new List<GameObject>();
            foreach (Transform child in _leftKneeObject.transform)
            {
                leftKneeList.Add(child.gameObject);
            }
            _leftKnees = leftKneeList.ToArray();*/
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }
        void Update()
        {
            if (_equipNewItems)
            {
                _equipNewItems = false;
                EquipArmor();
            }
        }
        public void EquipArmor()
        {
            Debug.Log("Equipping New Items ");

            if (_player._playerInventoryManager._headEquipment != null)
                LoadHeadEquipment(_player._playerInventoryManager._headEquipment);

            if (_player._playerInventoryManager._bodyEquipment != null)
                LoadBodyEquipment(_player._playerInventoryManager._bodyEquipment);

            if (_player._playerInventoryManager._handEquipment != null)
                LoadHandEquipment(_player._playerInventoryManager._handEquipment);

            if (_player._playerInventoryManager._legEquipment != null)
                LoadLegEquipment(_player._playerInventoryManager._legEquipment);
        }

        // Quick Slot
        public void SwitchQuickSlotItem()
        {
            if (!_player.IsOwner)
                return;

            QuickSlotItem selectedQuickSlotItem = null;

            // Disable two handing if we are two handing

            // Add one to our index to switch ti the next potential item
            _player._playerInventoryManager._quickSlotItemIndex += 1;
            // If our index number is out of bounds, reset it to position #1(0)
            if (_player._playerInventoryManager._quickSlotItemIndex is < 0 or > 2)
            {
                _player._playerInventoryManager._quickSlotItemIndex = 0;

                // We check if we are holding more then one weapon 
                float quickSlotCount = 0;
                QuickSlotItem firstQuickSlot = null;
                int firstQuickSlotPosition = 0;

                for (int i = 0; i < _player._playerInventoryManager._quickSlotItemInQuickSlots.Length; i++)
                {
                    if (_player._playerInventoryManager._quickSlotItemInQuickSlots[i] != null)
                    {
                        quickSlotCount += 1;
                        if (firstQuickSlot == null)
                        {
                            firstQuickSlot = _player._playerInventoryManager._quickSlotItemInQuickSlots[i];
                            firstQuickSlotPosition = i;
                        }
                    }
                }

                if (quickSlotCount <= 1)
                {
                    _player._playerInventoryManager._quickSlotItemIndex = -1;
                    selectedQuickSlotItem = null;
                    _player._playerNetworkManager._currentQuickSlotItemID.Value = -1;
                }
                else
                {
                    _player._playerInventoryManager._quickSlotItemIndex = firstQuickSlotPosition;
                    if (firstQuickSlot != null)
                        _player._playerNetworkManager._currentQuickSlotItemID.Value = firstQuickSlot._itemID;
                }

                return;
            }


            // Check to see if the next potential weapon is not the "unarmed" weapon
            if (_player._playerInventoryManager
                    ._quickSlotItemInQuickSlots[_player._playerInventoryManager._quickSlotItemIndex] != null)
            {
                selectedQuickSlotItem =
                    _player._playerInventoryManager._quickSlotItemInQuickSlots[
                        _player._playerInventoryManager._quickSlotItemIndex];
                // Assign the network weapon ID so it switch for all connected clients 
                _player._playerNetworkManager._currentQuickSlotItemID.Value = _player._playerInventoryManager
                    ._quickSlotItemInQuickSlots[_player._playerInventoryManager._rightHandWeaponIndex]._itemID;

            }
            else
            {
                _player._playerNetworkManager._currentQuickSlotItemID.Value = -1;
            }

            if (selectedQuickSlotItem == null && _player._playerInventoryManager._quickSlotItemInQuickSlots.Length <= 2)
                SwitchQuickSlotItem();

        }

        // Equipment 
        private void InitializeArmorModels()
        {
            List<GameObject> maleBodiesList = new List<GameObject>();
            foreach (Transform child in _maleFullBodyObject.transform)
            {
                maleBodiesList.Add(child.gameObject);
            }
            _maleBodies = maleBodiesList.ToArray();

            List<GameObject> maleRightUpperHandList = new List<GameObject>();
            foreach (Transform child in _maleRightHandObject.transform)
            {
                maleRightUpperHandList.Add(child.gameObject);
            }
            _maleRightUpperArms = maleRightUpperHandList.ToArray();

            List<GameObject> maleLowerRightHandsList = new List<GameObject>();
            foreach (Transform child in _maleRightLowerArmObject.transform)
            {
                maleLowerRightHandsList.Add(child.gameObject);
            }
            _maleRightLowerArms = maleLowerRightHandsList.ToArray();

            List<GameObject> maleRightLowerArmList = new List<GameObject>();
            foreach (Transform child in _maleRightLowerArmObject.transform)
            {
                maleRightLowerArmList.Add(child.gameObject);
            }
            _maleLeftLowerArms = maleRightLowerArmList.ToArray();

            List<GameObject> maleRightHandList = new List<GameObject>();
            foreach (Transform child in _maleRightHandObject.transform)
            {
                maleRightHandList.Add(child.gameObject);
            }
            _maleRightHands = maleRightHandList.ToArray();

            List<GameObject> maleUpperLeftHandList = new List<GameObject>();

            foreach (Transform child in _maleLeftUpperArmObject.transform)
            {
                maleUpperLeftHandList.Add(child.gameObject);
            }
            _maleLeftUpperArms = maleUpperLeftHandList.ToArray();

            List<GameObject> maleLeftLowerArmList = new List<GameObject>();
            foreach (Transform child in _maleLeftLowerArmObject.transform)
            {
                maleLeftLowerArmList.Add(child.gameObject);
            }
            _maleLeftLowerArms = maleLeftLowerArmList.ToArray();

            List<GameObject> maleLeftHandList = new List<GameObject>();
            foreach (Transform child in _maleLeftHandObject.transform)
            {
                maleLeftHandList.Add(child.gameObject);
            }
            _maleLeftHands = maleLeftHandList.ToArray();

            List<GameObject> maleHipsList = new List<GameObject>();
            foreach (Transform child in _maleHipObject.transform)
            {
                maleHipsList.Add(child.gameObject);
            }
            _maleHips = maleHipsList.ToArray();

            List<GameObject> maleRightLegList = new List<GameObject>();
            foreach (Transform child in _maleRightLegObject.transform)
            {
                maleRightLegList.Add(child.gameObject);
            }
            _maleRightLegs = maleRightLegList.ToArray();

            List<GameObject> maleLeftLegList = new List<GameObject>();
            foreach (Transform child in _maleLeftLegObject.transform)
            {
                maleLeftLegList.Add(child.gameObject);
            }
            _maleLeftLegs = maleLeftLegList.ToArray();

            List<GameObject> femaleBodiesList = new List<GameObject>();
            foreach (Transform child in _femaleFullBodyObject.transform)
            {
                femaleBodiesList.Add(child.gameObject);
            }
            _femaleBodies = femaleBodiesList.ToArray();

            List<GameObject> femaleRightUpperHandList = new List<GameObject>();
            foreach (Transform child in _femaleRightUpperArmObject.transform)
            {
                femaleRightUpperHandList.Add(child.gameObject);
            }
            _femaleRightUpperArms = femaleRightUpperHandList.ToArray();

            List<GameObject> femaleLowerRightHandsList = new List<GameObject>();
            foreach (Transform child in _femaleRightLowerArmObject.transform)
            {
                femaleLowerRightHandsList.Add(child.gameObject);
            }
            _femaleRightLowerArms = femaleLowerRightHandsList.ToArray();

            List<GameObject> femaleRightHandList = new List<GameObject>();
            foreach (Transform child in _femaleRightHandObject.transform)
            {
                femaleRightHandList.Add(child.gameObject);
            }
            _femaleRightHands = femaleRightHandList.ToArray();

            List<GameObject> femaleUpperLeftHandList = new List<GameObject>();

            foreach (Transform child in _femaleLeftUpperArmObject.transform)
            {
                femaleUpperLeftHandList.Add(child.gameObject);
            }
            _femaleLeftUpperArms = femaleUpperLeftHandList.ToArray();

            List<GameObject> femaleRightLowerArmList = new List<GameObject>();
            foreach (Transform child in _femaleRightLowerArmObject.transform)
            {
                femaleRightLowerArmList.Add(child.gameObject);
            }
            _femaleRightLowerArms = femaleRightLowerArmList.ToArray();

            List<GameObject> femaleLeftLowerArmList = new List<GameObject>();
            foreach (Transform child in _femaleLeftLowerArmObject.transform)
            {
                femaleLeftLowerArmList.Add(child.gameObject);
            }
            _femaleLeftLowerArms = femaleLeftLowerArmList.ToArray();

            List<GameObject> femaleLeftHandList = new List<GameObject>();
            foreach (Transform child in _femaleLeftHandObject.transform)
            {
                femaleLeftHandList.Add(child.gameObject);
            }
            _femaleLeftHands = femaleLeftHandList.ToArray();

            List<GameObject> femaleHipsList = new List<GameObject>();
            foreach (Transform child in _femaleHipObject.transform)
            {
                femaleHipsList.Add(child.gameObject);
            }
            _femaleHips = femaleHipsList.ToArray();

            List<GameObject> femaleRightLegList = new List<GameObject>();
            foreach (Transform child in _femaleRightLegObject.transform)
            {
                femaleRightLegList.Add(child.gameObject);
            }
            _femaleRightLegs = femaleRightLegList.ToArray();

            List<GameObject> femaleLeftLegList = new List<GameObject>();
            foreach (Transform child in _femaleLeftLegObject.transform)
            {
                femaleLeftLegList.Add(child.gameObject);
            }
            _femaleLeftLegs = femaleLeftLegList.ToArray();

            List<GameObject> femaleFullHelmetList = new List<GameObject>();
            foreach (Transform child in _femaleFullHelmetObject.transform)
            {
                femaleFullHelmetList.Add(child.gameObject);
            }
            _femaleHeadFullHelmets = femaleFullHelmetList.ToArray();
        }
        public void LoadHeadEquipment(HeadEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            UnloadHeadEquipmentModels();
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._handEquipmentID.Value = -1;// -1 will never be an Item ID, so it will always ne null

                _player._playerInventoryManager._headEquipment = null;
                return;
            }
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            _player._playerInventoryManager._headEquipment = equipment;
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now

            switch (equipment._HeadEquipmentType)
            {
                case HeadEquipmentType.FullHelmet:
                    _player._playerBodyManager.DisableHair();
                    _player._playerBodyManager.DisableHead();
                    break;
                case HeadEquipmentType.Hat:
                    break;
                case HeadEquipmentType.Hood:
                    _player._playerBodyManager.DisableHair();
                    break;
                case HeadEquipmentType.FaceCover:
                    _player._playerBodyManager.DisableFacialHair();
                    break;
                default:
                    break;
            }
            // 6. Load head equipment models
            foreach (var model in equipment._equipmentModels)
            {
                model.LoadModel(_player, _player._playerNetworkManager._isMale.Value);
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
            foreach (var model in _femaleHeadFullHelmets)
            {
                model.SetActive(false);
            }
            foreach (var model in _hats)
            {
                model.SetActive(false);
            }
            foreach (var model in _faceCovers)
            {
                model.SetActive(false);
            }
            foreach (var model in _hoods)
            {
                model.SetActive(false);
            }
            foreach (var model in _helmetAccessories)
            {
                model.SetActive(false);
            }
            // Re-enable head,hair 
            _player._playerBodyManager.EnableHead();
            _player._playerBodyManager.EnableHair();
        }

        public void LoadBodyEquipment(BodyEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            UnloadBodyEquipmentModels();
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._bodyEquipmentID.Value = -1;

                _player._playerInventoryManager._bodyEquipment = null;
            }
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            _player._playerInventoryManager._bodyEquipment = equipment;
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            _player._playerBodyManager.DisableBody();
            // 6. Load head equipment models
            foreach (var model in equipment._equipmentModels)
            {
                model.LoadModel(_player, _player._playerNetworkManager._isMale.Value);
            }
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();

            if (_player.IsOwner)
                _player._playerNetworkManager._bodyEquipmentID.Value = equipment._itemID;
        }
        private void UnloadBodyEquipmentModels()
        {
            foreach (var model in _rightShoulders)
            {
                model.SetActive(false);
            }
            foreach (var model in _rightElbows)
            {
                model.SetActive(false);
            }
            foreach (var model in _leftShoulders)
            {
                model.SetActive(false);
            }
            foreach (var model in _leftElbows)
            {
                model.SetActive(false);
            }
            foreach (var model in _backAccessories)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleBodies)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleLeftUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleBodies)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleLeftUpperArms)
            {
                model.SetActive(false);
            }
            _player._playerBodyManager.EnableBody();
        }
        public void LoadLegEquipment(LegEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            UnloadLegEquipment();
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._legEquipmentID.Value = -1;

                _player._playerInventoryManager._legEquipment = null;
                return;
            }
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function 
            _player._playerInventoryManager._legEquipment = equipment;
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            _player._playerBodyManager.DisableLowerBody();
            // 6. Load head equipment models
            foreach (var model in equipment._equipmentModels)
            {
                model.LoadModel(_player, _player._playerNetworkManager._isMale.Value);
            }
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();

            if (_player.IsOwner)
                _player._playerNetworkManager._legEquipmentID.Value = equipment._itemID;
        }
        private void UnloadLegEquipment()
        {
            foreach (var model in _maleHips)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleLeftLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleRightLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in _rightKnees)
            {
                model.SetActive(false);
            }
            foreach (var model in _leftKnees)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleHips)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleLeftLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleRightLegs)
            {
                model.SetActive(false);
            }
            _player._playerBodyManager.EnableLowerBody();
        }
        public void LoadHandEquipment(HandEquipmentItem equipment)
        {
            // 1. Unload old head equipment(if any)
            UnloadHandEquipmentModels();
            // 2. If you have an "OnItemEquipped" call on your equipment, run it now 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._handEquipmentID.Value = -1;

                _player._playerInventoryManager._handEquipment = null;
                return;
            }
            // 3. If equipment is NULL simply set equipment in inventory to null and return  
            // 4. Set current head equipment in player inventory to the equipment that is passed to this function
            _player._playerInventoryManager._handEquipment = equipment;
            // 5. if you need to check for head equipment type to disable certain body features (hood disabling hair etc, full helms disabling heads) Do it now
            _player._playerBodyManager.DisableArms();
            // 6. Load head equipment models
            foreach (var model in equipment._equipmentModels)
            {
                model.LoadModel(_player, _player._playerNetworkManager._isMale.Value);
            }
            // 7. Calculate total equipment load (weight of all of your equipment. This impact roll speed and at extreme weight, movement speed)
            // 8. Calculate total armor absorption 
            _player._playerStatsManager.CalculateTotalArmorAbsorption();

            if (_player.IsOwner)
                _player._playerNetworkManager._handEquipmentID.Value = equipment._itemID;

        }
        private void UnloadHandEquipmentModels()
        {
            foreach (var model in _maleLeftLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleRightLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleLeftUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleRightHands)
            {
                model.SetActive(false);
            }
            foreach (var model in _maleLeftHands)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleLeftLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleRightLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleLeftUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleRightHands)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleLeftHands)
            {
                model.SetActive(false);
            }
            _player._playerBodyManager.EnableArms();
        }

        // Enable Body Fetures

        // Projectile
        public void LoadMainProjectileEquipment(RangedProjectileItem equipment)
        {
            // 1. If equipment is null simply set equipment in inventory to null and return 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._mainProjectileID.Value = -1;// -1 will never be an item id,so is always be null

                _player._playerInventoryManager._mainProjectile = null;
                return;
            }
            // 2. If you have an "OnItemEquipped", run it now
            // 3. Set current projectile equipment in player inventory to the equipment that is passed to this function 
            _player._playerInventoryManager._mainProjectile = equipment;

            if (_player.IsOwner)
                _player._playerNetworkManager._mainProjectileID.Value = equipment._itemID;

        }
        public void LoadSecondaryProjectileEquipment(RangedProjectileItem equipment)
        {
            // 1. If equipment is null simply set equipment in inventory to null and return 
            if (equipment == null)
            {
                if (_player.IsOwner)
                    _player._playerNetworkManager._secondaryProjectileID.Value = -1;// -1 will never be an item id,so is always be null

                _player._playerInventoryManager._secondaryProjectile = null;
                return;
            }
            // 2. If you have an "OnItemEquipped", run it now
            // 3. Set current projectile equipment in player inventory to the equipment that is passed to this function 
            _player._playerInventoryManager._secondaryProjectile = equipment;

            if (_player.IsOwner)
                _player._playerNetworkManager._secondaryProjectileID.Value = equipment._itemID;

        }
        // Weapons
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

        private void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        // Right Wepaon
        public void LoadRightWeapon()
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

            _player._playerNetworkManager._isTwoHandingWeapon.Value = false;

            _player._playerAnimationManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            // Elden Rings Weapon Swapping:
            // 1. Check if we have another weapon beside our main weapon, if we do ,Never swap to unarmed, rotate between weapon 1 & 2
            // 2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not proceed both empty slots before returning to the main weapon

            WeaponItem selectedWeapon = null;

            // Disable two handing if we are two handing

            // Add one to our index to switch ti the next potential weapon
            _player._playerInventoryManager._rightHandWeaponIndex += 1;
            // If our index number is out of bounds, reset it to position #1(0)
            if (_player._playerInventoryManager._rightHandWeaponIndex is < 0 or > 2)
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
                        weaponCount += 1;
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
                    _player._playerNetworkManager._currentRightHandWeaponID.Value = selectedWeapon._itemID;
                }
                else
                {
                    _player._playerInventoryManager._rightHandWeaponIndex = firstWeaponPosition;
                    if (firstWeapon != null)
                        _player._playerNetworkManager._currentRightHandWeaponID.Value = firstWeapon._itemID;
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
                    _player._playerNetworkManager._currentRightHandWeaponID.Value = _player._playerInventoryManager
                        ._weaponInRigthHandSlots[_player._playerInventoryManager._rightHandWeaponIndex]._itemID;
                    return;
                }
            }

            if (_player._playerInventoryManager._rightHandWeaponIndex <= 2)
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

            _player._playerNetworkManager._isTwoHandingWeapon.Value = false;
            
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

                for (int i = 0; i < _player._playerInventoryManager._weaponInLefthHandSlots.Length; i++)
                {
                    if (_player._playerInventoryManager._weaponInLefthHandSlots[i]._itemID !=
                        WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = _player._playerInventoryManager._weaponInLefthHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    _player._playerInventoryManager._leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance._unarmedWeapon;
                    _player._playerNetworkManager._currentLeftHandWeaponID.Value = selectedWeapon._itemID;
                }
                else
                {
                    _player._playerInventoryManager._leftHandWeaponIndex = firstWeaponPosition;
                    if (firstWeapon != null)
                        _player._playerNetworkManager._currentLeftHandWeaponID.Value = firstWeapon._itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in _player._playerInventoryManager._weaponInLefthHandSlots)
            {
                // Check to see if the next potential weapon is not the "unarmed" weapon
                if (_player._playerInventoryManager
                        ._weaponInLefthHandSlots[_player._playerInventoryManager._leftHandWeaponIndex]._itemID !=
                    WorldItemDatabase.Instance._unarmedWeapon._itemID)
                {
                    selectedWeapon =
                        _player._playerInventoryManager._weaponInLefthHandSlots[
                            _player._playerInventoryManager._leftHandWeaponIndex];
                    // Assign the network weapon ID so it switch for all connected clients 
                    _player._playerNetworkManager._currentLeftHandWeaponID.Value = _player._playerInventoryManager
                        ._weaponInLefthHandSlots[_player._playerInventoryManager._leftHandWeaponIndex]._itemID;
                    return;
                }
            }

            if (_player._playerInventoryManager._leftHandWeaponIndex <= 2)
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
                    WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_player._playerInventoryManager
                        ._currentRightHandWeapon._whooshes));
            }
            else if (_player._playerNetworkManager._isUsingLeftHand.Value)
            {
                _leftWeaponManager._meleeDamageCollider.EnableDamageCollider();
                _player._characterSoundFXManager.PlaySoundFX(
                    WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_player._playerInventoryManager
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

        // Unhide weapons
        public void UnhideWeapons()
        {
            if (_player._playerEquipmentManager._rightHandWeaponModel != null)
                _player._playerEquipmentManager._rightHandWeaponModel.SetActive(true);

            if (_player._playerEquipmentManager._leftHandWeaponModel != null)
                _player._playerEquipmentManager._leftHandWeaponModel.SetActive(true);
        }
    }
}