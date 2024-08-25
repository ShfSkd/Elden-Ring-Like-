using SKD.World_Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD
{
    public class FogWallIntractable : NetworkBehaviour
    {
        [Header("Fog")]
        [SerializeField] GameObject[] _fogGameObjectArray;

        [Header("ID")]
        public int _fogWallID;

        [Header("Active")]
        public NetworkVariable<bool> _isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
         
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChange(false,_isActive.Value);
            _isActive.OnValueChanged += OnIsActiveChange;
            WorldObjectManager.Instance.AddFogWallToList(this); 
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            _isActive.OnValueChanged -= OnIsActiveChange;
            WorldObjectManager.Instance.RemoveFogWallFromList(this);
        }
        private void OnIsActiveChange(bool oldStatus,bool newStatus)
        {
            if(_isActive.Value)
            {
                foreach(var fog in _fogGameObjectArray)
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
    }
}
