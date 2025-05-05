using System.Collections.Generic;
using System.Linq;
using SKD.Items;
using SKD.Items.AshesOfWar;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapons;
using SKD.Spells.Items;
using UnityEngine;


namespace SKD.World_Manager
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem _unarmedWeapon;

        public GameObject _pickUpItemPrefab;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> _weapons = new List<WeaponItem>();

        [Header("Head Equipment")]
        [SerializeField] List<HeadEquipmentItem> _headEquipment = new List<HeadEquipmentItem>();

        [Header("Body Equipment")]
        [SerializeField] List<BodyEquipmentItem> _bodyEqipment = new List<BodyEquipmentItem>();

        [Header("Leg Equipment")]
        [SerializeField] List<LegEquipmentItem> _legEqipment = new List<LegEquipmentItem>();

        [Header("Hand Equipment")]
        [SerializeField] List<HandEquipmentItem> _handEqipment = new List<HandEquipmentItem>();

        [Header("Ashes Of War")]
        [SerializeField] List<AshOfWar> _ashOfWar = new List<AshOfWar>();

        [Header("Spells")]
        [SerializeField] List<SpellItem> _spells = new List<SpellItem>();
        
        [Header("Projectiles")]
        [SerializeField] List<RangedProjectileItem> _projectiles = new List<RangedProjectileItem>();

        [Header("Quick Slots Item")]
        [SerializeField] List<QuickSlotItem> _quickSlotsItem = new List<QuickSlotItem>();

        [Header("Items")]
        private List<Item> _items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            foreach (var weapon in _weapons)
                _items.Add(weapon);

            foreach (var item in _headEquipment)
                _items.Add(item);

            foreach (var item in _bodyEqipment)
                _items.Add(item);

            foreach (var item in _legEqipment)
                _items.Add(item);

            foreach (var item in _handEqipment)
                _items.Add(item);

            foreach (var item in _ashOfWar)
                _items.Add(item);

            foreach (var item in _spells)
                _items.Add(item);

            foreach (var projectile in _projectiles)
                _items.Add(projectile);

            foreach (var item in _quickSlotsItem)
                _items.Add(item);
            
       
            // Assign all of our items a unique  item ID
            for (int i = 0; i < _items.Count; i++)
                _items[i]._itemID = i;
        }

        public Item GetItemByID(int ID) =>
            _items.FirstOrDefault(item => item._itemID == ID);

        public WeaponItem GetWeaponByID(int id) =>
            _weapons.FirstOrDefault(weapon => weapon._itemID == id);

        public HeadEquipmentItem GetHeadEquipmentByID(int id) =>
            _headEquipment.FirstOrDefault(equipment => equipment._itemID == id);

        public BodyEquipmentItem GetBodyEquipmentByID(int id) =>
            _bodyEqipment.FirstOrDefault(equipment => equipment._itemID == id);

        public LegEquipmentItem GetLegEquipmentByID(int id) =>
            _legEqipment.FirstOrDefault(equipment => equipment._itemID == id);

        public HandEquipmentItem GetHandEquipmentByID(int id) =>
            _handEqipment.FirstOrDefault(equipment => equipment._itemID == id);

        public AshOfWar GetAshOfWarByID(int id) =>
            _ashOfWar.FirstOrDefault(ash => ash._itemID == id);

        public SpellItem GetSpellByID(int id) =>
            _spells.FirstOrDefault(spell => spell._itemID == id);

        public RangedProjectileItem GetProjectileByID(int id) =>
            _projectiles.FirstOrDefault(projectile => projectile._itemID == id);

        public QuickSlotItem GetQuickSlotItem(int id) =>
            _quickSlotsItem.FirstOrDefault(flask => flask._itemID == id);
    }
}
