using SKD.Items.Weapons;
using SKD.World_Manager;
using UnityEngine;
namespace SKD.GameSaving
{
    [System.Serializable]
    public class SerializableWeapon : ISerializationCallbackReceiver
    {
        [SerializeField] public int _itemID;
        [SerializeField] public int _ashOfWarID;

        public WeaponItem GetWeapon()
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponFromSerializedData(this);
            return weapon;
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
        }
    }
}