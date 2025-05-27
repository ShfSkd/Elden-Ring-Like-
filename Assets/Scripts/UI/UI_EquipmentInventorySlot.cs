using System;
using SKD.Items.Weapons;
using SKD.Character.Player;
using SKD.Items;
using SKD.Items.Equipment;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
namespace SKD.UI
{
    public class UI_EquipmentInventorySlot : MonoBehaviour
    {
        public Image _itemIcon;
        public Image _highlightedIcon;
        [SerializeField] public Item _currentItem;

        public void Additem(Item item)
        {
            if (item == null)
            {
                _itemIcon.enabled = false;
                return;
            }
            _itemIcon.enabled = true;

            _currentItem = item;
            _itemIcon.sprite = item._itemIcon;
        }

        public void SelectSlot()
        {
            _highlightedIcon.enabled = true;
        }
        public void DeselectSlot()
        {
            _highlightedIcon.enabled = false;
        }

        public void EquipeItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item equippedtItem;
            switch (PlayerUIManager.Instance._playerUIEquipmentManager._currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    equippedtItem = player._playerInventoryManager._weaponInRigthHandSlots[0];

                    if (equippedtItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._weaponInRigthHandSlots[0] = _currentItem as WeaponItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    if (player._playerInventoryManager._rightHandWeaponIndex == 0)
                        player._playerNetworkManager._currentRightHandWeaponID.Value = _currentItem._itemID;

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.RightWeapon02:
                    equippedtItem = player._playerInventoryManager._weaponInRigthHandSlots[1];

                    if (equippedtItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._weaponInRigthHandSlots[1] = _currentItem as WeaponItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    if (player._playerInventoryManager._rightHandWeaponIndex == 1)
                        player._playerNetworkManager._currentRightHandWeaponID.Value = _currentItem._itemID;

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.RightWeapon03:
                    equippedtItem = player._playerInventoryManager._weaponInRigthHandSlots[2];

                    if (equippedtItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._weaponInRigthHandSlots[2] = _currentItem as WeaponItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    if (player._playerInventoryManager._rightHandWeaponIndex == 2)
                        player._playerNetworkManager._currentRightHandWeaponID.Value = _currentItem._itemID;

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon01:
                    equippedtItem = player._playerInventoryManager._weaponInLefthHandSlots[0];

                    if (equippedtItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._weaponInLefthHandSlots[0] = _currentItem as WeaponItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    if (player._playerInventoryManager._leftHandWeaponIndex == 0)
                        player._playerNetworkManager._currentLeftWeaponID.Value = _currentItem._itemID;

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon02:
                    equippedtItem = player._playerInventoryManager._weaponInLefthHandSlots[1];

                    if (equippedtItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._weaponInLefthHandSlots[1] = _currentItem as WeaponItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    if (player._playerInventoryManager._leftHandWeaponIndex == 1)
                        player._playerNetworkManager._currentLeftWeaponID.Value = _currentItem._itemID;

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon03:
                    // If our Current Equipment is this slot, is not a null item, add it to our inventory
                    equippedtItem = player._playerInventoryManager._weaponInLefthHandSlots[2];
                    if (equippedtItem._itemID != WorldItemDatabase.Instance._unarmedWeapon._itemID)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    // Then assign the slot our new Weapon
                    player._playerInventoryManager._weaponInLefthHandSlots[2] = _currentItem as WeaponItem;
                    // Then remove the new weapon from our inventory
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    // Re-equip new weapon if we are holding the current weapon uin this slot (if you change weapon 3, and you are holding right weapon 1 nothing would happen here)
                    if (player._playerInventoryManager._leftHandWeaponIndex == 2)
                        player._playerNetworkManager._currentLeftWeaponID.Value = _currentItem._itemID;

                    // Refresh equipment window
                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;

                case EquipmentType.Head:

                    // If our Current Equipment is this slot, is not a null item, add it to our inventory
                    equippedtItem = player._playerInventoryManager._headEquipment;

                    if (equippedtItem != null)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    // Then assign the slot our new item
                    player._playerInventoryManager._headEquipment = _currentItem as HeadEquipmentItem;
                    // Then remove the new item from our inventory
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);
                    // Re-equip new item happen
                    player._playerEquipmentManager.LoadHeadEquipment(player._playerInventoryManager._headEquipment);
                    // Refresh equipment window
                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Body:
                    equippedtItem = player._playerInventoryManager._bodyEquipment;

                    if (equippedtItem != null)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._bodyEquipment = _currentItem as BodyEquipmentItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    player._playerEquipmentManager.LoadBodyEquipment(player._playerInventoryManager._bodyEquipment);

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Legs:
                    equippedtItem = player._playerInventoryManager._legEquipment;

                    if (equippedtItem != null)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._legEquipment = _currentItem as LegEquipmentItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    player._playerEquipmentManager.LoadLegEquipment(player._playerInventoryManager._legEquipment);

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Hands:
                    equippedtItem = player._playerInventoryManager._handEquipment;

                    if (equippedtItem != null)
                    {
                        player._playerInventoryManager.AddItemsToInventory(equippedtItem);
                    }
                    player._playerInventoryManager._handEquipment = _currentItem as HandEquipmentItem;
                    player._playerInventoryManager.RemoveItemFromInventory(_currentItem);

                    player._playerEquipmentManager.LoadHandEquipment(player._playerInventoryManager._handEquipment);

                    PlayerUIManager.Instance._playerUIEquipmentManager.RefreshMenu();
                    break;
                default:
                    break;
            }

            PlayerUIManager.Instance._playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
        }
    }
}