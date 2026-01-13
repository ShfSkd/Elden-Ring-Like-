using SKD.Items.Equipment;
using SKD.Items.Weapons;
using SKD.World_Manager;
using UnityEngine;
namespace SKD.GameSaving
{
    [System.Serializable]
    public class SerializableRangedProjectile : ISerializationCallbackReceiver
    {
        [SerializeField] public int _itemID;
        [SerializeField] public int _itemAmount;

        public RangedProjectileItem GetProjectile()
        {
            RangedProjectileItem projectile = WorldItemDatabase.Instance.GetRangedWeaponFromSerializedData(this);
            return projectile;
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
        }
    }
}