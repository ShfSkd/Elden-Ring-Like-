using SKD.Items.Weapon_Actions;
using System.Linq;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager Instance;

        [Header("Weapon Item Actions")]
        public WeaponItemAction [] _weaponItemAction;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            for (int i = 0; i < _weaponItemAction.Length; i++)
            {
                _weaponItemAction[i]._actionID = i;
            }
        }

        public WeaponItemAction GetWeponActionItemByID(int id)
        {
            return _weaponItemAction.FirstOrDefault(action => action._actionID == id);
        }
    }
}