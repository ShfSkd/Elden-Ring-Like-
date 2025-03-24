using System;
using System.Collections;
using System.Collections.Generic;
using SKD.Character.Player;
using SKD.Items;
using SKD.World_Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace SKD.UI.PlayerUI
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject _menu;

        [Header("Weapon Slots")]
        [SerializeField] Image _rightHandSlot01;
        [SerializeField] Image _rightHandSlot02;
        [SerializeField] Image _rightHandSlot03;
        [SerializeField] Image _leftHandSlot01;
        [SerializeField] Image _leftHandSlot02;
        [SerializeField] Image _leftHandSlot03;
        [SerializeField] Image _headEquipmentSlot;
        [SerializeField] Image _bodyEquipmentSlot;
        [SerializeField] Image _legEquipmentSlot;
        [SerializeField] Image _handEquipmentSlot;

        // This inventory populate with related items when changing equipment
        [Header("Equipment Inventory")]
        public EquipmentType _currentSelectedEquipmentSlot;
        [SerializeField] GameObject _equipmentInventoryWindow;
        [SerializeField] GameObject _equipmentInventorySlotPrefab;
        [SerializeField] Transform _equipmentInventoryContentWindow;
        [SerializeField] Item _currentSelectedItem;
        public void OpenEquipmentManagerMenu()
        {
            PlayerUIManger.instance._menuWindowIsOpen = true;
            _menu.SetActive(true);
            _equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }
        public void RefreshMenu()
        {
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }

        // This function simply returns you to the last selected button when you are finished equipping a new item 
        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButton = null;
            switch (_currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    lastSelectedButton = _rightHandSlot01.GetComponentInParent<Button>();
                    break;
                case EquipmentType.RightWeapon02:
                    lastSelectedButton = _rightHandSlot02.GetComponentInParent<Button>();
                    break;
                case EquipmentType.RightWeapon03:
                    lastSelectedButton = _rightHandSlot03.GetComponentInParent<Button>();
                    break;
                case EquipmentType.LeftWeapon01:
                    lastSelectedButton = _leftHandSlot01.GetComponentInParent<Button>();
                    break;
                case EquipmentType.LeftWeapon02:
                    lastSelectedButton = _leftHandSlot02.GetComponentInParent<Button>();
                    break;
                case EquipmentType.LeftWeapon03:
                    lastSelectedButton = _leftHandSlot03.GetComponentInParent<Button>();
                    break;
                case EquipmentType.Head:
                    lastSelectedButton = _headEquipmentSlot.GetComponentInParent<Button>();
                    break;
                case EquipmentType.Body:
                    lastSelectedButton = _bodyEquipmentSlot.GetComponentInParent<Button>();
                    break;
                case EquipmentType.Legs:
                    lastSelectedButton = _legEquipmentSlot.GetComponentInParent<Button>();
                    break;
                case EquipmentType.Hands:
                    lastSelectedButton = _handEquipmentSlot.GetComponentInParent<Button>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (lastSelectedButton != null)
            {
                lastSelectedButton.Select();
                lastSelectedButton.OnSelect(null);
            }
        }
        public void CloseEquipmentManagerMenu()
        {
            PlayerUIManger.instance._menuWindowIsOpen = false;
            _menu.SetActive(false);
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

            WeaponItem leftHandWeapon01 = player._playerInventoryManager._weaponInLefthHandSlots[0];
            if (leftHandWeapon01._itemIcon != null)
            {
                _leftHandSlot01.enabled = true;
                _leftHandSlot01.sprite = leftHandWeapon01._itemIcon;
            }
            else
            {
                _leftHandSlot01.enabled = false;
            }

            WeaponItem leftHandWeapon02 = player._playerInventoryManager._weaponInLefthHandSlots[1];
            if (leftHandWeapon02._itemIcon != null)
            {
                _leftHandSlot02.enabled = true;
                _leftHandSlot02.sprite = leftHandWeapon02._itemIcon;
            }
            else
            {
                _leftHandSlot02.enabled = false;
            }

            WeaponItem leftHandWeapon03 = player._playerInventoryManager._weaponInLefthHandSlots[2];
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
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < headEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.Additem(headEquipmentInventory[i]);

                // This will select the first button in the list 
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            };
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
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < bodyEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.Additem(bodyEquipmentInventory[i]);

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
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < headEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.Additem(headEquipmentInventory[i]);

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
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < handEquipmentInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.Additem(handEquipmentInventory[i]);

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
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < weaponsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(_equipmentInventorySlotPrefab, _equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.Additem(weaponsInInventory[i]);

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
                    unequippedItem = player._playerInventoryManager._weaponInLefthHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInLefthHandSlots[0] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._leftHandWeaponIndex == 0)
                            player._playerNetworkManager._currentLeftWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon02:
                    unequippedItem = player._playerInventoryManager._weaponInLefthHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInLefthHandSlots[1] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._leftHandWeaponIndex == 1)
                            player._playerNetworkManager._currentLeftWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon03:
                    unequippedItem = player._playerInventoryManager._weaponInLefthHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player._playerInventoryManager._weaponInLefthHandSlots[2] = Instantiate(WorldItemDatabase.Instance._unarmedWeapon);

                        if (unequippedItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                            player._playerInventoryManager.AddItemsToInventory(unequippedItem);

                        if (player._playerInventoryManager._leftHandWeaponIndex == 2)
                            player._playerNetworkManager._currentLeftWeaponID.Value = WorldItemDatabase.Instance._unarmedWeapon._itemID;
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
                default:
                    break;
            }
            // Refresh Menu
            RefreshMenu();
        }
    }
}