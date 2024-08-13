using SKD.Items;
using SKD.World_Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.Character.Player.PlayerUI
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar _healthBar;
        [SerializeField] UI_StatBar _staminaBar;

        [Header("Quick Slots")]
        [SerializeField] Image _rightWeaponQuickSlotIcon;
        [SerializeField] Image _leftWeaponQuickSlotIcon;

        public void RefreshHUD()
        {
            _healthBar.gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(true);

            _staminaBar.gameObject.SetActive(false);
            _staminaBar.gameObject.SetActive(true);

        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            _healthBar.SetStat(newValue);
        }
        public void SetMaxHealthValue(int maxHealth)
        {
            _healthBar.SetMaxStat(maxHealth);
        }
        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            _staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            _staminaBar.SetMaxStat(maxStamina);
        }
        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is Null");
                _rightWeaponQuickSlotIcon.enabled = false;
                _rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon._itemIcon == null)
            {
                Debug.Log("Item Has No Icon");
                _rightWeaponQuickSlotIcon.enabled = false;
                _rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            //  This is where you would check to see if you meet the item requirements if you want to create the warning foe not being able to wield it in the UI


            _rightWeaponQuickSlotIcon.sprite = weapon._itemIcon;
            _rightWeaponQuickSlotIcon.enabled = true;
        }
        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is Null");
                _leftWeaponQuickSlotIcon.enabled = false;
                _leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon._itemIcon == null)
            {
                Debug.Log("Item Has No Icon");
                _leftWeaponQuickSlotIcon.enabled = false;
                _leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            //  This is where you would check to see if you meet the item requirements if you want to create the warning foe not being able to wield it in the UI


            _leftWeaponQuickSlotIcon.sprite = weapon._itemIcon;
            _leftWeaponQuickSlotIcon.enabled = true;
        }
    }

}
