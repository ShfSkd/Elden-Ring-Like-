using SKD.Character.Player;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Interacts
{
    public class SiteOfGraceInteractable : Interactable
    {
        [Header("Site Of Grace")]
        public int _siteOfGraceID;
        public NetworkVariable<bool> _isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("VFX")]
        [SerializeField] GameObject _activatedParticles;

        [Header("Interaction Text")]
        [SerializeField] string _unactivatedInteractionText = "Restore Site Of Grace";
        [SerializeField] string _activatedInteractionText = "Rest";

        [Header("Teleport Transform")]
        [SerializeField] Transform _teleportTransform;

        protected override void Start()
        {
            base.Start();

            if (IsOwner)
            {
                if (WorldSaveGameManager.Instance._currentCharacterData._siteOfGrace.ContainsKey(_siteOfGraceID))
                {
                    _isActivated.Value = WorldSaveGameManager.Instance._currentCharacterData._siteOfGrace[_siteOfGraceID];
                }
                else
                {
                    _isActivated.Value = false;
                }
            }
            if (_isActivated.Value)
            {
                _interactableText = _activatedInteractionText;
            }
            else
            {
                _interactableText = _unactivatedInteractionText;
            }
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
                OnIsActiveChanged(false, _isActivated.Value);

            _isActivated.OnValueChanged += OnIsActiveChanged;

            WorldObjectManager.Instance.AddSiteOfGraceToList(this);
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            // If we join when the status has already changed, we force the OnChange function to run here upon joining
            if (!IsOwner)
                OnIsActiveChanged(false, _isActivated.Value);

            _isActivated.OnValueChanged -= OnIsActiveChanged;
        }
        private void RestoreSiteOfGrace(PlayerManager player)
        {
            _isActivated.Value = true;

            // If our save file contains info on the site of grace, remove it
            if (WorldSaveGameManager.Instance._currentCharacterData._siteOfGrace.ContainsKey(_siteOfGraceID))
                WorldSaveGameManager.Instance._currentCharacterData._siteOfGrace.Remove(_siteOfGraceID);

            // Then re-add it with the value of "True" (IsActovated)
            WorldSaveGameManager.Instance._currentCharacterData._siteOfGrace.Add(_siteOfGraceID, true);

            player._playerAnimationManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);

            PlayerUIManager.Instance._playerUIPopUpManager.SendGraceRestoredPopUp("Site of Grace Restored");

            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
        }
        private void RestAtSiteOfGrace(PlayerManager player)
        {
            PlayerUIManager.Instance._playerUISiteOfGraceManager.OpenSiteOfGraceManagerMenu();

            Debug.Log("Resting");
            _interactableCollider.enabled = true;
            player._playerNetworkManager._currentHealth.Value = player._playerNetworkManager._maxHealth.Value;
            player._playerNetworkManager._currentStamina.Value = player._playerNetworkManager._maxStamina.Value;
            // Reset Monsters/Characters locations
            WorldAIManager.Instance.ResetAllCharacters();

        }
        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(2);
            _interactableCollider.enabled = true;
        }
        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (_isActivated.Value)
            {
                _activatedParticles.SetActive(true);
                _interactableText = _activatedInteractionText;

            }
            else
            {
                _interactableText = _unactivatedInteractionText;
            }
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (player._isPerformingAction)
                return;

            if (player._playerCombatManager._isUsingItem)
                return;

            if (!_isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                RestAtSiteOfGrace(player);
            }
        }
        public void TeleportToSiteOfGrace()
        {
            // The Player is only able to teleport when not in a co-op game so we can grab the local player from the network manager
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            
            // Enable loading screen
            PlayerUIManager.Instance._playerUILoadingScreenManager.ActivateLoadingScreen();
            
            // Teleport Player
            player.transform.position = _teleportTransform.position;
            
            // Disable Loading Screen
            PlayerUIManager.Instance._playerUILoadingScreenManager.DeactivateLoadingScreen();
        }
    }
}