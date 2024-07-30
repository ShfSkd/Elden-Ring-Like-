using Unity.Collections;
using Unity.Netcode;

namespace SKD.Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        public NetworkVariable<FixedString64Bytes> _characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }
}