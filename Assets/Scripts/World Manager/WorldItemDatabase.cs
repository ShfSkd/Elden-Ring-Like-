using SKD.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem _unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> _weapons = new List<WeaponItem>();

        // A list of every item that we have in the game 
        private List<Item> _items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            // Add all of our weapons to the list of items 
            foreach (var weapon in _weapons)
            {
                _items.Add(weapon);
            }
            // Assign all of our items a unique  item ID
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i]._itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int id)
        {
            return _weapons.FirstOrDefault(weapon => weapon._itemID == id);
        }
    }
}