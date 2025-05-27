using System.Collections;
using SKD.Character.AI_Character;
using SKD.Character.Player;
using SKD.Interacts;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace SKD.Items
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType _pickUpType;

        [Header("Item")]
        [SerializeField] Item _item;

        [Header("Creature Loot Pickup")]
        public NetworkVariable<int> _itemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Vector3> _networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<ulong> _droppingCreatureID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public bool _trackDroppingCreaturePosition = true;

        [Header("World Spawn Pick Up")]
        [SerializeField] int _worldSpawnInteractableID;// This is a unique given to each world spawn item, so you may not loot them more then once
        [SerializeField] bool _hasBeenLooted;

        [Header("Drop SFX")]
        [SerializeField] AudioClip _itemDropSFX;
        private AudioSource _audioSource;
        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }
        protected override void Start()
        {
            base.Start();

            if (_pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _itemID.OnValueChanged += OnItemIDChanged;
            _networkPosition.OnValueChanged += OnNetworkPositionChanged;
            _droppingCreatureID.OnValueChanged += OnDroppingCreatureIDChanged;

            if (_pickUpType == ItemPickUpType.CharacterDrop)
                _audioSource.PlayOneShot(_itemDropSFX);

            if (!IsOwner)
            {
                OnItemIDChanged(0, _itemID.Value);
                OnNetworkPositionChanged(Vector3.zero, _networkPosition.Value);
                OnDroppingCreatureIDChanged(0, _droppingCreatureID.Value); 
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _itemID.OnValueChanged -= OnItemIDChanged;
            _networkPosition.OnValueChanged -= OnNetworkPositionChanged;
            _droppingCreatureID.OnValueChanged -= OnDroppingCreatureIDChanged;
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
            if (!WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.ContainsKey(_worldSpawnInteractableID))
            {
                WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.Add(_worldSpawnInteractableID, false);
            }
            _hasBeenLooted = WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted[_worldSpawnInteractableID];

            // 2. If it has been looted, hide the game object  
            if (_hasBeenLooted)
                gameObject.SetActive(false);

            // 3. If it has not been looted, Activate the game object

        }

        public override void Interact(PlayerManager player)
        {
            if (player._isPerformingAction)
                return;

            base.Interact(player);

            // 1. Play SFX
            player._characterSoundFXManager.PlaySoundFX((WorldSoundFXManager.instance._pickUpItemSFX));
            
            // Play Animation
            player._playerAnimationManager.PlayTargetActionAnimation("Pick_Up_Item_01", true); 
            
            // 2. Add item to inventory
            player._playerInventoryManager.AddItemsToInventory(_item);
            // 3. Display a UI pop up showing item's name and picture 
            PlayerUIManager.Instance._playerUIPopUpManager.SendItemPopUp(_item, 1);

            // 4. Save loot status
            if (_pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.ContainsKey(_worldSpawnInteractableID))
                {
                    WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.Remove(_worldSpawnInteractableID);
                }
                WorldSaveGameManager.Instance._currentCharacterData._worldItemLooted.Add(_worldSpawnInteractableID, true);
            }
            // 5. Hide or destroy game object
            
            DestroyThisNetworkObjectServerRpc();
        }

        protected void OnItemIDChanged(int oldValue, int newValue)
        {
            if (_pickUpType != ItemPickUpType.CharacterDrop)
                return;

            _item = WorldItemDatabase.Instance.GetItemByID(_itemID.Value);

        }
        protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newNewPosition)
        {
            if (_pickUpType != ItemPickUpType.CharacterDrop)
                return;

            transform.position = _networkPosition.Value;
        }
        protected void OnDroppingCreatureIDChanged(ulong oldID, ulong newID)
        {
            if (_pickUpType != ItemPickUpType.CharacterDrop)
                return;

            if (_trackDroppingCreaturePosition)
                StartCoroutine(TrackDroppingCreaturePosition());
        }
        protected IEnumerator TrackDroppingCreaturePosition()
        {
            AICharacterManager droppingCreature = NetworkManager.Singleton.SpawnManager.SpawnedObjects[_droppingCreatureID.Value].GetComponent<AICharacterManager>();
            bool trackCreature = droppingCreature != null;

            if (trackCreature)
            {
                while(gameObject.activeInHierarchy)
                {
                    transform.position = droppingCreature._characterCombatManager._lockOnTransform.position;
                    yield return null;

                }
            }
            yield return null;
        }
        [ServerRpc(RequireOwnership = false)]
        protected void DestroyThisNetworkObjectServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }

}