using SKD.Character.Player;
using SKD.UI.PlayerUI;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Interacts
{
    public class Interactable : NetworkBehaviour
    {
        public string _interactableText;
        [SerializeField] protected Collider _interactableCollider;
        [SerializeField] protected bool _hostOnlyInteractable = true;

        protected virtual void Awake()
        {
            if (_interactableCollider == null)
                _interactableCollider = GetComponent<Collider>();
        }
        protected virtual void Start()
        {

        }
        public virtual void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            _interactableCollider.enabled = false;
            player._playerInteractionManager.RemoveInteractionList(this);
            PlayerUIManager.Instance._playerUIPopUpManager.CloseAllPopUpsWindows();
        }
        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player._playerNetworkManager.IsHost && _hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                // Pass the interaction to the player
                player._playerInteractionManager.AddInteractionList(this);
            }
        }
        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player._playerNetworkManager.IsHost && _hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                // Remove the interaction to the player
                player._playerInteractionManager.RemoveInteractionList(this);

                PlayerUIManager.Instance._playerUIPopUpManager.CloseAllPopUpsWindows();
            }
        }
    }
}