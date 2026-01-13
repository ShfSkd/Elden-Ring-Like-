using Items.Flasks;
using SKD.Items.Quick_Item_Slot;
using SKD.World_Manager;
using UnityEngine;
namespace SKD.GameSaving
{
    [System.Serializable]
    public class SerializableQuickSlotIcon : ISerializationCallbackReceiver
    {
        [SerializeField] public int _itemID;
        [SerializeField] public int _itemAmount;

        public QuickSlotItem GetQuickSlotItem()
        {
            FlaskItem flask = WorldItemDatabase.Instance.GetFlaskFromSerializedData(this);
            return flask;
        }
        
        
        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
        }
    }
}