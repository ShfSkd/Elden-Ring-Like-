using SKD.Character.Player;
using SKD.Interacts;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using UnityEngine;
using Unity.Netcode;

namespace SKD.Items
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType _pickUpType;

        [Header("Item")]
        [SerializeField] Item _item;

        [Header("World Spawn Pick Up")]
        [SerializeField] int _itemID;
        [SerializeField] bool _hasBeenLooted;

        protected override void Start()
        {
            base.Start();

            if (_pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }
        private void CheckIfWorldItemWasAlreadyLooted()
        {
            // 0. If the player isn't the host, hide the item
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }
            
            // 1. Compare The data of looted items Id's with this item id
            if (!WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.ContainsKey(_itemID))
            {
                WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.Add(_itemID, false);
            }
            _hasBeenLooted = WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted[_itemID];

            // 2. If it has been looted, hide the game object  
            if (_hasBeenLooted)
                gameObject.SetActive(false);

            // 3. If it has not been looted, Activate the game object

        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            
            // 1. Play SFX
            player._characterSoundFXManager.PlaySoundFX((WorldSoundFXManager.instance._pickUpItemSFX));
            // 2. Add item to inventory
            player._playerInventoryManager.AddItemsToInventory(_item);
            // 3. Display a UI pop up showing item's name and picture 
            PlayerUIManger.instance._playerUIPopUpManager.SendItemPopUp(_item,1);
            
            // 4. Save loot status
            if (_pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.ContainsKey(_itemID))
                {
                    WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.Remove(_itemID);
                }
                WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.Add(_itemID, true);
            }
            // 5. Hide or destroy game object
            Destroy(gameObject);
        }
    }

}