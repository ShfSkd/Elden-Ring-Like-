using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [Header("Position")]
        public NetworkVariable<Vector3> _networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<Quaternion> _networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        public Vector3 _networkPositionVelocity;
        public float _networkPositionSmoothTime = 0.1f;
        public float _networkRotationSmoothTime = 0.1f;
    }
}