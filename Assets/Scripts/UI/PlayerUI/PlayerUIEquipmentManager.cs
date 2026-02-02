using System;
using System.Collections;
using System.Collections.Generic;
using SKD.Items.Weapons;
using SKD.Character.Player;
using SKD.Items;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.World_Manager;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace SKD.UI.PlayerUI
{
    public class PlayerUIEquipmentManager : PlayerUIMenu
    {
        [Header("Weapon Slots")]
        [SerializeField] Image _rightHandSlot01;
        private Button _rightHandSlot01Button;
        [SerializeField] Image _rightHandSlot02;
        private Button _rightHandSlot02Button;
        [SerializeField] Image _rightHandSlot03;
        private Button _rightHandSlot03Button;
        [SerializeField] Image _leftHandSlot01;
        private Button _leftHandSlot01Button;
        [SerializeField] Image _leftHandSlot02;
        private Button _leftHandSlot02Button;
        [SerializeField] Image _leftHandSlot03;
        private Button _leftHandSlot03Button;

        [Header("Armor Slots")]
        [SerializeField] Image _headEquipmentSlot;
        private Button _headEquipmentSlotButton;
        [SerializeField] Image _bodyEquipmentSlot;
        private Button _bodyEquipmentSlotButton;
        [SerializeField] Image _legEquipmentSlot;
        private Button _legEquipmentSlotButton;
        [SerializeField] Image _handEquipmentSlot;
        private Button _handEquipmentSlotButton;

        [Header("Projectile Slots")]
        [SerializeField] Image _mainProjectileEquipmentSlot;
        [SerializeField] TextMeshProUGUI _mainProjectileCountText;
        private Button _mainProjectileEquipmentSlotButton;
        [SerializeField] Image _secondaryProjectileEquipmentSlot;
        [SerializeField] TextMeshProUGUI _secondaryProjectileCountText;
        private Button _secondaryProjectileEquipmentSlotButton;

        [Header("Quick Slots")]
        [SerializeField] Image _quickSlot01EquipmentSlot;
        [SerializeField] TextMeshProUGUI _quickSlot01CountText;
        private Button _quickSlot01Button;
        [SerializeField] Image _quickSlot02EquipmentSlot;
        [SerializeField] TextMeshProUGUI _quickSlot02CountText;
        private Button _quickSlot02Button;
        [SerializeField] Image _quickSlot03EquipmentSlot;
        [SerializeField] TextMeshProUGUI _quickSlot03CountText;
        private Button _quickSlot03Button;

        // This inventory populates with related items when changing equipment
        [Header("Equipment Inventory")]
        public EquipmentType _currentSelectedEquipmentSlot;
        [SerializeField] GameObject _equipmentInventoryWindow;
        [SerializeField] GameObject _equipmentInventorySlotPrefab;
        [SerializeField] Transform _equipmentInventoryContentWindow;
        [SerializeField] Item _currentSelectedItem;

        private void Awake()
        {
            _rightHandSlot01Button = _rightHandSlot01.GetComponentInParent<Button>(true);
            _rightHandSlot02Button = _rightHandSlot02.GetComponentInParent<Button>(true);
            _rightHandSlot03Button = _rightHandSlot03.GetComponentInParent<Button>(true);

            _leftHandSlot01Button = _leftHandSlot01.GetComponentInParent<Button>(true);
            _leftHandSlot02Button = _leftHandSlot02.GetComponentInParent<Button>(true);
            _leftHandSlot03Button = _leftHandSlot03.GetComponentInParent<Button>(true);

            _headEquipmentSlotButton = _headEquipmentSlot.GetComponentInParent<Button>(true);
            _bodyEquipmentSlotButton = _bodyEquipmentSlot.GetComponentInParent<Button>(true);
            _handEquipmentSlotButton = _handEquipmentSlot.GetComponentInParent<Button>(true);
            _legEquipmentSlotButton = _legEquipmentSlot.GetComponentInParent<Button>(true);
            _mainProjectileEquipmentSlotButton = _mainProjectileEquipmentSlot.GetComponentInParent<Button>(true);
            _secondaryProjectileEquipmentSlotButton = _secondaryProjectileEquipmentSlot.GetComponentInParent<Button>(true);

            _quickSlot01Button = _quickSlot01EquipmentSlot.GetComponentInParent<Button>(true);
            _quickSlot02Button = _quickSlot02EquipmentSlot.GetComponentInParent<Button>(true);
            _quickSlot03Button = _quickSlot03EquipmentSlot.GetComponentInParent<Button>(true);
        }
        public void OpenEquipmentManagerMenu()
        {
        
        }
        public override void OpenMenu()
        {
            base.OpenMenu();
            
            ToggleEquipmentButtons(true);
            _equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        
        }
        public void RefreshMenu()
        {
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }
        private void ToggleEquipmentButtons(bool isEnabled)
        {
            _rightHandSlot01Button.enabled = isEnabled;
            _rightHandSlot02Button.enabled = isEnabled;
            _rightHandSlot03Button.enabled = isEnabled;

            _leftHandSlot01Button.enabled = isEnabled;
            _leftHandSlot02Button.enabled = isEnabled;
            _leftHandSlot03Button.enabled = isEnabled;

            _headEquipmentSlotButton.enabled = isEnabled;
            _bodyEquipmentSlotButton.enabled = isEnabled;
            _handEquipmentSlotButton.enabled = isEnabled;
            _legEquipmentSlotButton.enabled = isEnabled;
            _mainProjectileEquipmentSlotButton.enabled = isEnabled;
            _secondaryProjectileEquipmentSlotButton.enabled = isEnabled;

            _quickSlot01Button.enabled = isEnabled;
            _quickSlot02Button.enabled = isEnabled;
            _quickSlot03Button.enabled = isEnabled;
        }

        // This function simply returns you to the last selected button when you are finished equipping a new item 
        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButton = null;
            ToggleEquipmentButtons(true);
            switch (_currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    lastSelectedButton = _rightHandSlot01Button;
                    break;
                case EquipmentType.RightWeapon02:
                    lastSelectedButton = _rightHandSlot02Button;
                    break;
                case EquipmentType.RightWeapon03:
                    lastSelectedButton = _rightHandSlot03Button;
                    break;
                case EquipmentType.LeftWeapon01:
                    lastSelectedButton = _leftHandSlot01Button;
                    break;
                case EquipmentType.LeftWeapon02:
                    lastSelectedButton = _leftHandSlot02Button;
                    break;
                case EquipmentType.LeftWeapon03:
                    lastSelectedButton = _leftHandSlot03Button;
                    break;
                case EquipmentType.Head:
                    lastSelectedButton = _headEquipmentSlotButton;
                    break;
                case EquipmentType.Body:
                    lastSelectedButton = _bodyEquipmentSlotButton;
                    break;
                case EquipmentType.Legs:
                    lastSelectedButton = _legEquipmentSlotButton;
                    break;
                case EquipmentType.Hands:
                    lastSelectedButton = _handEquipmentSlotButton;
                    break;
                case EquipmentType.MainProjectile:
                    lastSelectedButton = _mainProjectileEquipmentSlotButton;
                    break;
                case EquipmentType.SecondaryProjectile:
                    lastSelectedButton = _secondaryProjectileEquipmentSlotButton;
                    break;
                case EquipmentType.QuickSlot01:
                    lastSelectedButton = _quickSlot01Button;
                    break;
                case EquipmentType.QuickSlot02:
                    lastSelectedButton = _quickSlot02Button;
                    break;
                case EquipmentType.QuickSlot03:
                    lastSelectedButton = _quickSlot03Button;
                    break;
                default:
                    break;
            }
            if (lastSelectedButton != null)
            {
                lastSelectedButton.Select();
                lastSelectedButton.OnSelect(null);
            }
            _equipmentInventoryWindow.SetActive(false);
        }
        private void RefreshEquipmentSlotIcons()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            WeaponItem rightHandWeapon01 = player._playerInventoryManager._weaponInRigthHandSlots[0];
            if (rightHandWeapon01._itemIcon != null)
            {
                _rightHandSlot01.enabled = true;
                _rightHandSlot01.sprite = rightHandWeapon01._itemIcon;
            }
            else
            {
                _rightHandSlot01.enabled = false;
            }

            WeaponItem rightHandWeapon02 = player._playerInventoryManager._weaponInRigthHandSlots[1];
            if (rightHandWeapon02._itemIcon != null)
            {
                _rightHandSlot02.enabled = true;
                _rightHandSlot02.sprite = rightHandWeapon02._itemIcon;
            }
            else
            {
                _rightHandSlot02.enabled = false;
            }

            WeaponItem rightHandWeapon03 = player._playerInventoryManager._weaponInRigthHandSlots[2];
            if (rightHandWeapon03._itemIcon != null)
            {
                _rightHandSlot03.enabled = true;
                _rightHandSlot03.sprite = rightHandWeapon03._itemIcon;
            }
            else
            {
                _rightHandSlot03.enabled = false;
            }

            WeaponItem leftHandWeapon01 = player._playerInventoryManager._weaponInLeftHandSlots[0];
            if (leftHandWeapon01._itemIcon != null)
            {
                _leftHandSlot01.enabled = true;
                _leftHandSlot01.sprite = leftHandWeapon01._itemIcon;
            }
            else
            {
                _leftHandSlot01.enabled = false;
            }

            WeaponItem leftHandWeapon02 = player._playerInventoryManager._weaponInLeftHandSlots[1];
            if (leftHandWeapon02._itemIcon != null)
            {
                _leftHandSlot02.enabled = true;
                _leftHandSlot02.sprite = leftHandWeapon02._itemIcon;
            }
            else
            {
                _leftHandSlot02.enabled = false;
            }

            WeaponItem leftHandWeapon03 = player._playerInventoryManager._weaponInLeftHandSlots[2];
            if (leftHandWeapon03._itemIcon != null)
            {
                _leftHandSlot03.enabled = true;
                _leftHandSlot03.sprite = leftHandWeapon03._itemIcon;
            }
            else
            {
                _leftHandSlot03.enabled = false;
            }

            HeadEquipmentItem headEquipment = player._playerInventoryManager._headEquipment;
            if (headEquipment != null)
            {
                _headEquipmentSlot.enabled = true;
                _headEquipmentSlot.sprite = headEquipment._itemIcon;
            }
            else
            {
                _headEquipmentSlot.enabled = false;
            }

            BodyEquipmentItem bodyEquipment = player._playerInventoryManager._bodyEquipment;
            if (bodyEquipment != null)
            {
                _bodyEquipmentSlot.enabled = true;
                _bodyEquipmentSlot.sprite = bodyEquipment._itemIcon;
            }
            else
            {
                _bodyEquipmentSlot.enabled = false;
            }

            LegEquipmentItem legEquipment = player._playerInventoryManager._legEquipment;
            if (legEquipment != null)
            {
                _legEquipmentSlot.enabled = true;
                _legEquipmentSlot.sprite = legEquipment._itemIcon;
            }
            else
            {
                _legEquipmentSlot.enabled = false;
            }

            HandEquipmentItem handEquipment = player._playerInventoryManager._handEquipment;
            if (handEquipment != null)
            {
                _handEquipmentSlot.enabled = true;
                _handEquipmentSlot.sprite = handEquipment._itemIcon;
            }
            else
            {
                _handEquipmentSlot.enabled = false;
            }

            // Projectile
            RangedProjectileItem mainProjectileEquipment = player._playerInventoryManager._mainProjectile;
            if (mainProjectileEquipment != null)
            {
                _mainProjectileEquipmentSlot.enabled = true;
                _mainProjectileEquipmentSlot.sprite = mainProjectileEquipment._itemIcon;
                _mainProjectileCountText.enabled = true;
                _mainProjectileCountText.text = mainProjectileEquipment._currentAmmoAmount.ToString();
            }
            else
            {
                _mainProjectileEquipmentSlot.enabled = false;
                _mainProjectileCountText.enabled = false;
            }
            RangedProjectileItem secondaryProjectileEquipment = player._playerInventoryManager._secondaryProjectile;
            if (secondaryProjectileEquipment != null)
            {
                _secondaryProjectileEquipmentSlot.enabled = true;
                _secondaryProjectileEquipmentSlot.sprite = secondaryProjectileEquipment._itemIcon;
                _secondaryProjectileCountText.enabled = true;
                _secondaryProjectileCountText.text = secondaryProjectileEquipment._currentAmmoAmount.ToString();
            }
            else
            {
                _secondaryProjectileEquipmentSlot.enabled = false;
                _secondaryProjectileCountText.enabled = false;
            }
            QuickSlotItem quickSlotEquipment01 = player._playerInventoryManager._quickSlotItemInQuickSlots[0];
            if (quickSlotEquipment01 != null)
            {
                _quickSlot01EquipmentSlot.enabled = true;
                _quickSlot01EquipmentSlot.sprite = quickSlotEquipment01._itemIcon;

                if (quickSlotEquipment01._isConsumable)
                {
                    _quickSlot01CountText.enabled = true;
                    _quickSlot01CountText.text = quickSlotEquipment01.GetCurrentAmount(player).ToString();
                }
                else
                {
                    _quickSlot01CountText.enabled = false;
                }

            }
            else
            {
                _quickSlot01EquipmentSlot.enabled = false;
                _quickSlot01CountText.enabled = false;
            }
            QuickSlotItem quickSlotEquipment02 = player._playerInventoryManager._quickSlotItemInQuickSlots[1];
            if (quickSlotEquipment02 != null)
            {
                _quickSlot02EquipmentSlot.enabled = true;
                _quickSlot02EquipmentSlot.sprite = quickSlotEquipment02._itemIcon;

                if (quickSlotEquipment02._isConsumable)
                {
                    _quickSlot02CountText.enabled = true;
                    _quickSlot02CountText.text = quickSlotEquipment02.GetCurrentAmount(player).ToString();
                }
                else
                {
                    _quickSlot02CountText.enabled = false;
                }

            }
            else
            {
                _quickSlot02EquipmentSlot.enabled = false;
                _quickSlot02CountText.enabled = false;
            }
            QuickSlotItem quickSlotEquipment03 = player._playerInventoryManager._quickSlotItemInQuickSlots[2];
            if (quickSlotEquipment03 != null)
            {
                _quickSlot03EquipmentSlot.enabled = true;
                _quickSlot03EquipmentSlot.sprite = quickSlotEquipment03._itemIcon;

                if (quickSlotEquipment03._isConsumable)
                {
                    _quickSlot03CountText.enabled = true;
                    _quickSlot03CountText.text = quickSlotEquipment03.GetCurrentAmount(player).ToString();
                }
                else
                {
                    _quickSlot03CountText.enabled = false;
                }

            }
            else
            {
                _quickSlot03EquipmentSlot.enabled = false;
                _quickSlot03CountText.enabled = false;
            }
        }
        private void ClearEquipmentInventory()
        {
            foreach (Transform item in _equipmentInventoryContentWindow)
            {
                Destroy(item.gameObject);
            }
        }
        public void LoadEquipmentInventory()
        {
            ToggleEquipmentButtons(false);
            _equipmentInventoryWindow.SetActive(true);

            switch (_currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.Head:
                    LoadHeadEquipmentInventory();
                    break;
                case EquipmentType.Body:
                    LoadBodyEquipmentInventory();
                    break;
                case EquipmentType.Legs:
                    LoadLegEquipmentInventory();
                    break;
                case EquipmentType.Hands:
                    LoadHandEquipmentInventory();
                    break;
                case EquipmentType.MainProjectile:
                    LoadProjectileInventory();
                    break;
                case EquipmentType.SecondaryProjectile:
                    LoadProjectileInventory();
                    break;
                case EquipmentType.QuickSlot01:
                    LoadQuickSlotInventory();
                    break;
                case EquipmentType.QuickSlot02:
                    LoadQuickSlotInventory();
                    break;
                case EquipmentType.QuickSlot03:
                    LoadQuickSlotInventory();
                    break;
                default:
                    break;
            }
        }
        private void LoadHeadEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<HeadEquipmentItem> headEquipmentInventory = new List<HeadEquipmentItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                HeadEquipmentItem equipment = player._playerInventoryManager._itemInTheInventory[i] as HeadEquipmentItem;

                if (equipment != null)
                    headEquipmentInventory.Add(equipment);
            }

            if (headEquipmentInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < headEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(headEquipmentInventory[i]);

                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
            ;
        }
        private void LoadBodyEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<BodyEquipmentItem> bodyEquipmentInventory = new List<BodyEquipmentItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                BodyEquipmentItem equipment = player._playerInventoryManager._itemInTheInventory[i] as BodyEquipmentItem;

                if (equipment != null)
                    bodyEquipmentInventory.Add(equipment);
            }

            if (bodyEquipmentInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < bodyEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(bodyEquipmentInventory[i]);

                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
        private void LoadLegEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<LegEquipmentItem> headEquipmentInventory = new List<LegEquipmentItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                LegEquipmentItem equipment = player._playerInventoryManager._itemInTheInventory[i] as LegEquipmentItem;

                if (equipment != null)
                    headEquipmentInventory.Add(equipment);
            }

            if (headEquipmentInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < headEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(headEquipmentInventory[i]);

                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
        private void LoadHandEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<HandEquipmentItem> handEquipmentInventory = new List<HandEquipmentItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                HandEquipmentItem equipment = player._playerInventoryManager._itemInTheInventory[i] as HandEquipmentItem;

                if (equipment != null)
                    handEquipmentInventory.Add(equipment);
            }

            if (handEquipmentInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < handEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(handEquipmentInventory[i]);

                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
        private void LoadWeaponInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                WeaponItem weapon = player._playerInventoryManager._itemInTheInventory[i] as WeaponItem;

                if (weapon != null)
                    weaponsInInventory.Add(weapon);
            }

            if (weaponsInInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < weaponsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(weaponsInInventory[i]);

                Debug.Log("1");
                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
        private void LoadProjectileInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<RangedProjectileItem> projectileInInventory = new List<RangedProjectileItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                RangedProjectileItem projectile = player._playerInventoryManager._itemInTheInventory[i] as RangedProjectileItem;

                if (projectile != null)
                    projectileInInventory.Add(projectile);
            }

            if (projectileInInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < projectileInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(projectileInInventory[i]);

                Debug.Log("1");
                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
        private void LoadQuickSlotInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<QuickSlotItem> quickSlotItemsInInventory = new List<QuickSlotItem>();

            // Search our entire inventory, and out of all the items in our inventory if the item is a weapon add it to our weapon list
            for (int i = 0; i < player._playerInventoryManager._itemInTheInventory.Count; i++)
            {
                QuickSlotItem quickSlotItem = player._playerInventoryManager._itemInTheInventory[i] as QuickSlotItem;

                if (quickSlotItem != null)
                    quickSlotItemsInInventory.Add(quickSlotItem);
            }

            if (quickSlotItemsInInventory.Count <= 0)
            {
                _equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < quickSlotItemsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(quickSlotItemsInInventory[i]);

                Debug.Log("1");
                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
        public void SelectEquipmentSlot(int equipmentSlot)
        {
            _currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
        }
        public void UnEquipSelectedItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item unequippedItem;
            switch (_currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    unequippedItem = player._playerInventoryManager._weaponInRigthHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInRigthHandSlots[0] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._rightHandWeaponIndex == 0)
                            player._playerNetworkManager._currentRightHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.RightWeapon02:
                    unequippedItem = player._playerInventoryManager._weaponInRigthHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInRigthHandSlots[1] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._rightHandWeaponIndex == 1)
                            player._playerNetworkManager._currentRightHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.RightWeapon03:
                    unequippedItem = player._playerInventoryManager._weaponInRigthHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInRigthHandSlots[2] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._rightHandWeaponIndex == 2)
                            player._playerNetworkManager._currentRightHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon01:
                    unequippedItem = player._playerInventoryManager._weaponInLeftHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInLeftHandSlots[0] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._leftHandWeaponIndex == 0)
                            player._playerNetworkManager._currentLeftHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon02:
                    unequippedItem = player._playerInventoryManager._weaponInLeftHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInLeftHandSlots[1] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._leftHandWeaponIndex == 1)
                            player._playerNetworkManager._currentLeftHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon03:
                    unequippedItem = player._playerInventoryManager._weaponInLeftHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInLeftHandSlots[2] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._leftHandWeaponIndex == 2)
                            player._playerNetworkManager._currentLeftHandWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.Head:
                    unequippedItem = player._playerInventoryManager._headEquipment;
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._headEquipment = null;
                    player._playerEquipmentManager.LoadHeadEquipment(player._playerInventoryManager._headEquipment);
                    break;
                case EquipmentType.Body:
                    unequippedItem = player._playerInventoryManager._bodyEquipment;
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._bodyEquipment = null;
                    player._playerEquipmentManager.LoadBodyEquipment(player._playerInventoryManager._bodyEquipment);
                    break;
                case EquipmentType.Legs:
                    unequippedItem = player._playerInventoryManager._legEquipment;
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._legEquipment = null;
                    player._playerEquipmentManager.LoadLegEquipment(player._playerInventoryManager._legEquipment);
                    break;
                case EquipmentType.Hands:
                    unequippedItem = player._playerInventoryManager._handEquipment;
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._headEquipment = null;
                    player._playerEquipmentManager.LoadHandEquipment(player._playerInventoryManager._handEquipment);
                    break;
                case EquipmentType.MainProjectile:
                    unequippedItem = player._playerInventoryManager._mainProjectile;
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._mainProjectile = null;
                    player._playerEquipmentManager.LoadMainProjectileEquipment(player._playerInventoryManager._mainProjectile);
                    break;
                case EquipmentType.SecondaryProjectile:
                    unequippedItem = player._playerInventoryManager._secondaryProjectile;
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._secondaryProjectile = null;
                    player._playerEquipmentManager.LoadSecondaryProjectileEquipment(player._playerInventoryManager._mainProjectile);
                    break;
                case EquipmentType.QuickSlot01:
                    unequippedItem = player._playerInventoryManager._quickSlotItemInQuickSlots[0];
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._quickSlotItemInQuickSlots[0] = null;

                    if (player._playerInventoryManager._quickSlotItemIndex == 0)
                        player._playerNetworkManager._currentQuickSlotItemID.Value = -1;
                    
                    break;
                case EquipmentType.QuickSlot02:
                    unequippedItem = player._playerInventoryManager._quickSlotItemInQuickSlots[1];
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._quickSlotItemInQuickSlots[2] = null;

                    if (player._playerInventoryManager._quickSlotItemIndex == 2)
                        player._playerNetworkManager._currentQuickSlotItemID.Value = -1;
                    break;
                case EquipmentType.QuickSlot03:
                    unequippedItem = player._playerInventoryManager._quickSlotItemInQuickSlots[2];
                    if (unequippedItem != null)
                        player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                    player._playerInventoryManager._quickSlotItemInQuickSlots[2] = null;

                    if (player._playerInventoryManager._quickSlotItemIndex == 2  )
                        player._playerNetworkManager._currentQuickSlotItemID.Value = -1;
                    break;
                default:
                    break;
            }
            // Refresh Menu
            RefreshMenu();
        }
    }
}