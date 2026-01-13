using SKD.Character.Player;
using SKD.Items;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.Spells.Items;
using SKD.World_Manager;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.UI.PlayerUI
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup[] _canvasGroup;

        [Header("Stats Bars")]
        [SerializeField] UI_StatBar _healthBar;
        [SerializeField] UI_StatBar _staminaBar;
        [SerializeField] UI_StatBar _focusPointBar;

        [Header("Quick Slots")]
        [SerializeField] Image _rightWeaponQuickSlotIcon;
        [SerializeField] Image _leftWeaponQuickSlotIcon;
        [SerializeField] Image _spellQuickSlotIcon;
        [SerializeField] Image _quickSlotItemQuickSlotIcon;
        [SerializeField] TextMeshProUGUI _quickSlotItemCountText;
        [SerializeField] GameObject _projectileQuickSlotsGameObject;
        [SerializeField] Image _mainProjectileQuickSlotIcon;
        [SerializeField] TextMeshProUGUI _mainProjectileCountText;
        [SerializeField] Image _secondaryProjectileQuickSlotIcon;
        [SerializeField] TextMeshProUGUI _secondaryProjectileCountText;

        [Header("Boss Health Bar")]
        public Transform _bossHealthBarParent;
        public GameObject _bossHealthBarObject;

        [Header("Crosshair")]
        public GameObject _crosshair;

        public void ToggleHud(bool status)
        {
            if (status)
            {
                foreach (var canvas in _canvasGroup)
                {
                    canvas.alpha = 1;
                }
            }
            else
            {
                foreach (var canvas in _canvasGroup)
                {
                    canvas.alpha = 0;
                }
            }
        }
        public void RefreshHUD()
        {
            _healthBar.gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(true);

            _staminaBar.gameObject.SetActive(false);
            _staminaBar.gameObject.SetActive(true);

            _focusPointBar.gameObject.SetActive(false);
            _focusPointBar.gameObject.SetActive(true);

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
        public void SetNewFocusPointsValue(int oldValue, int newValue)
        {
            _focusPointBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxFocusPointsValue(int maxFocusPoints)
        {
            _focusPointBar.SetMaxStat(maxFocusPoints);
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
        public void SetSpellItemQuickSlotIcon(int spellID)
        {
            SpellItem spell = WorldItemDatabase.Instance.GetSpellByID(spellID);
            if (spell == null)
            {
                Debug.Log("Spell is Null");
                _spellQuickSlotIcon.enabled = false;
                _spellQuickSlotIcon.sprite = null;
                return;
            }

            if (spell._itemIcon == null)
            {
                Debug.Log("Item Has No Icon");
                _spellQuickSlotIcon.enabled = false;
                _spellQuickSlotIcon.sprite = null;
                return;
            }
            //  This is where you would check to see if you meet the item requirements if you want to create the warning foe not being able to wield it in the UI


            _spellQuickSlotIcon.sprite = spell._itemIcon;
            _spellQuickSlotIcon.enabled = true;
        }
        public void SetQuickSlotItemQuickSlotIcon(QuickSlotItem quickSlotItem)
        {
            if (quickSlotItem == null)
            {
                Debug.Log("ITEM IS NULL");
                _quickSlotItemQuickSlotIcon.enabled = false;
                _quickSlotItemQuickSlotIcon.sprite = null;
                _quickSlotItemCountText.enabled = false;
                return;
            }

            if (quickSlotItem._itemIcon == null)
            {
                Debug.Log("ITEM HAS NO ICON");
                _quickSlotItemQuickSlotIcon.enabled = false;
                _quickSlotItemQuickSlotIcon.sprite = null;
                return;
            }

//  TO DO, UPDATE QUANTITY LEFT, SHOW IN UI
//  FADE OUT ICON IF NONE REMAINING

            _quickSlotItemQuickSlotIcon.sprite = quickSlotItem._itemIcon;
            _quickSlotItemQuickSlotIcon.enabled = true;

            if (quickSlotItem._isConsumable)
            {
                var player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
                _quickSlotItemCountText.text = quickSlotItem.GetCurrentAmount(player).ToString();
                _quickSlotItemCountText.enabled = true;
            }
            else
            {
                _quickSlotItemCountText.enabled = false;
            }
        }
        public void SetMainProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
        {
            if (projectileItem == null)
            {
                Debug.Log("ITEM IS NULL");
                _mainProjectileQuickSlotIcon.enabled = false;
                _mainProjectileQuickSlotIcon.sprite = null;
                _mainProjectileCountText.enabled = false;
                return;
            }

            if (projectileItem._itemIcon == null)
            {
                Debug.Log("ITEM HAS NO ICON");
                _mainProjectileQuickSlotIcon.enabled = false;
                _mainProjectileQuickSlotIcon.sprite = null;
                _mainProjectileCountText.enabled = false;
                return;
            }

//  TO DO, UPDATE QUANTITY LEFT, SHOW IN UI
//  FADE OUT ICON IF NONE REMAINING

            _mainProjectileQuickSlotIcon.sprite = projectileItem._itemIcon;
            _mainProjectileCountText.text = projectileItem._currentAmmoAmount.ToString();
            _mainProjectileQuickSlotIcon.enabled = true;
            _mainProjectileCountText.enabled = true;
        }
        public void SetSecondaryProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
        {
            if (projectileItem == null)
            {
                Debug.Log("ITEM IS NULL");
                _secondaryProjectileQuickSlotIcon.enabled = false;
                _secondaryProjectileQuickSlotIcon.sprite = null;
                _secondaryProjectileCountText.enabled = false;
                return;
            }

            if (projectileItem._itemIcon == null)
            {
                Debug.Log("ITEM HAS NO ICON");
                _secondaryProjectileQuickSlotIcon.enabled = false;
                _secondaryProjectileQuickSlotIcon.sprite = null;
                _secondaryProjectileCountText.enabled = true;
                return;
            }

//  TO DO, UPDATE QUANTITY LEFT, SHOW IN UI
//  FADE OUT ICON IF NONE REMAINING

            _secondaryProjectileQuickSlotIcon.sprite = projectileItem._itemIcon;
            _secondaryProjectileCountText.text = projectileItem._currentAmmoAmount.ToString();
            _secondaryProjectileQuickSlotIcon.enabled = true;
            _mainProjectileCountText.enabled = true;
        }
        public void ToggleProjectileQuickSlotVisibility(bool status)
        {
            _projectileQuickSlotsGameObject.SetActive(status);
        }
    }

}