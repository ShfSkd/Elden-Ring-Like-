using SKD.Effects;
using SKD.WorldManager;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager _characterManager;

        [Header("Position")]
        public NetworkVariable<Vector3> _networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<Quaternion> _networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        public Vector3 _networkPositionVelocity;
        public float _networkPositionSmoothTime = 0.1f;
        public float _networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<float> _horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<float> _verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<float> _moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> _isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Recurses")]
        public NetworkVariable<int> _currentHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _maxHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> _currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> _vitality = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        public void CheckHP(int oldValue, int newValue)
        {
            if (_currentHealth.Value <= 0)
            {
                StartCoroutine(_characterManager.ProcessDeathEvent());
            }
            // Prevents us from over-healing 
            if (_characterManager.IsOwner)
            {
                if (_currentHealth.Value <= 0)
                {

                }
            }
        }
        // A server RPC is a function called from client, to the server (in our case the host)
        [ServerRpc]
        public void NotifyTheServerofActionAnimationServerRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            // If this client is the host/server , then activate the client RPC
            if (IsServer)
            {
                PlayActionAnimationForAllClientsClientRpc(clientId, animationID, applyRootMotion);
            }
        }
        // A client RPC is sent To all clients present, from the server 
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it (so we don't play the animation twice)
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }
        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            _characterManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(animationID, 0.2f);
        }
        // Attack Animations
        [ServerRpc]
        public void NotifyTheServerofActionAttackAnimationServerRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            // If this client is the host/server , then activate the client RPC
            if (IsServer)
            {
                PlayActionAttackAnimationForAllClientsClientRpc(clientId, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayActionAttackAnimationForAllClientsClientRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it (so we don't play the animation twice)
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAttackAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformActionAttackAnimationFromServer(string animationID, bool applyRootMotion)
        {
            _characterManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(animationID, 0.2f);
        }

        // Damage
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerofCharacterdamageServerRpc(ulong damageCharacterID,
            ulong charcterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float ligthningDamage,
            float poiseDamage,
            float angleHitPoint,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            if (IsServer)
            {
                NotifyTheServerofCharacterDamageClientClientRpc(damageCharacterID, charcterCausingDamageID, physicalDamage, magicDamage, fireDamage, holyDamage, ligthningDamage, poiseDamage, angleHitPoint, contactPointX, contactPointY, contactPointZ);
            }
        }
        [ClientRpc]
        public void NotifyTheServerofCharacterDamageClientClientRpc(ulong damageCharacterID,
            ulong charcterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float ligthningDamage,
            float poiseDamage,
            float angleHitPoint,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            ProcessCharacterDamageFromServer(damageCharacterID, charcterCausingDamageID, physicalDamage, magicDamage, fireDamage, holyDamage, ligthningDamage, poiseDamage, angleHitPoint, contactPointX, contactPointY, contactPointZ);
        }
        public void ProcessCharacterDamageFromServer(ulong damageCharacterID,
           ulong charcterCausingDamageID,
           float physicalDamage,
           float magicDamage,
           float fireDamage,
           float holyDamage,
           float ligthningDamage,
           float poiseDamage,
           float angleHitPoint,
           float contactPointX,
           float contactPointY,
           float contactPointZ)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[charcterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeDamageEffect);

            damageEffect._physicalDamage = physicalDamage;
            damageEffect._magicDamage = magicDamage;
            damageEffect._fireDamage = fireDamage;
            damageEffect._holyDamage = holyDamage;
            damageEffect._lightnigamage = ligthningDamage;
            damageEffect._poiseDamage = poiseDamage;
            damageEffect._angleHitFrom = angleHitPoint;
            damageEffect._contantPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect._characteCausingDamage = characterCausingDamage;

            damagedCharacter._characterEffectsManager.ProceesInstanceEffect(damageEffect);

        }
    }
}