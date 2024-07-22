using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController _characterController;

        CharacterNetworkManager _characterNetworkManager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            _characterController = GetComponent<CharacterController>();
            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }
        protected virtual void Update()
        {
            //  If this character is being control from our side, then assign its network position to the position of our transform
            if (IsOwner)
            {
                _characterNetworkManager._networkPosition.Value = transform.position;
                _characterNetworkManager._networkRotation.Value = transform.rotation;
            }
            //  If this character is being control from else where, then assign its  position here locally by the position of network transform 
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, _characterNetworkManager._networkPosition.Value,
                  ref _characterNetworkManager._networkPositionVelocity, _characterNetworkManager._networkPositionSmoothTime);

                transform.rotation = Quaternion.Slerp(transform.rotation, _characterNetworkManager._networkRotation.Value, _characterNetworkManager._networkRotationSmoothTime);
            }
        }
    }

}
