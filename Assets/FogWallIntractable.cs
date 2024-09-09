using SKD.Character.Player;
using SKD.Interacts;
using SKD.World_Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD
{
    public class FogWallIntractable : Interactable
    {
        [Header("Fog")]
        [SerializeField] GameObject[] _fogGameObjectArray;

        [Header("Collision")]
        [SerializeField] Collider _fogWallCollider;

        [Header("ID")]
        public int _fogWallID;

        [Header("Sound")]
        private AudioSource _fogwallAudioSource;
        [SerializeField]AudioClip _fogwallSFX;

        [Header("Active")]
        public NetworkVariable<bool> _isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            _fogwallAudioSource = gameObject.GetComponent<AudioSource>();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            Vector3 wallRight = transform.forward;
            wallRight.y = 0;
            wallRight.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(wallRight);
            player.transform.rotation = targetRotation;

            AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);
            player._playerAnimationManager.PlayTargetActionAnimation("Pass_Through_Fog_01", true);
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChange(false, _isActive.Value);
            _isActive.OnValueChanged += OnIsActiveChange;
            WorldObjectManager.Instance.AddFogWallToList(this);
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            _isActive.OnValueChanged -= OnIsActiveChange;
            WorldObjectManager.Instance.RemoveFogWallFromList(this);
        }
        private void OnIsActiveChange(bool oldStatus, bool newStatus)
        {
            if (_isActive.Value)
            {
                foreach (var fog in _fogGameObjectArray)
                {
                    fog.SetActive(true);
                }
            }
            else
            {
                foreach (var fog in _fogGameObjectArray)
                {
                    fog.SetActive(false);
                }
            }
        }

        // When a server Rpc does not require ownership, A non owner can activate the function(client player does not own fog wall, as they are not the host
        [ServerRpc(RequireOwnership = false)]
        private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)
        {
            if (IsServer)
                AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
        }
        [ClientRpc]
        private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

            _fogwallAudioSource.PlayOneShot(_fogwallSFX);

            if (player != null)
                StartCoroutine(DisableCollisionForTime(player));
        }

        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            Physics.IgnoreCollision(player._characterController, _fogWallCollider, true);
            yield return new WaitForSeconds(3);
            Physics.IgnoreCollision(player._characterController, _fogWallCollider, false);
        }
    }
}
