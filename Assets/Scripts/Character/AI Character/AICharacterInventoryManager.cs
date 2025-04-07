using SKD.Items;
using SKD.World_Manager;
using Unity.Netcode;
using UnityEngine;
namespace SKD.Character.AI_Character
{
    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        AICharacterManager _aiCharacter;
        [Header("Loot Chance")]
        public int _dropItemChance = 10;
        [SerializeField] Item[] _droppableItem;

        protected override void Awake()
        {
            base.Awake();

            _aiCharacter = GetComponent<AICharacterManager>();
        }
        public void DropItem()
        {
            if (!_aiCharacter.IsOwner)
                return;

            bool wilDropItem = false;
            int itemChanceRoll = Random.Range(0, 100);

            if (itemChanceRoll <= _dropItemChance)
                wilDropItem = true;

            if (!wilDropItem)
                return;

            Item generatItem = _droppableItem[Random.Range(0, _droppableItem.Length)];

            if (generatItem == null)
                return;

            GameObject itemPickUpInteractableGameObject = Instantiate(WorldItemDatabase.Instance._pickUpItemPrefab);
            PickUpItemInteractable pickUpItemInteractable = itemPickUpInteractableGameObject.GetComponent<PickUpItemInteractable>();
            pickUpItemInteractable.GetComponent<NetworkObject>().Spawn();
            pickUpItemInteractable._itemID.Value = generatItem._itemID;
            pickUpItemInteractable._networkPosition.Value = transform.position;
            pickUpItemInteractable._droppingCreatureID.Value = _aiCharacter.NetworkObjectId;


        }
    }
}