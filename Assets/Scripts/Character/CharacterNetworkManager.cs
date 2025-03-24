using SKD.Effects;
using SKD.WorldManager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager _characterManager;

        [Header("Active")]
        public NetworkVariable<bool> _isActive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Position")]
        public NetworkVariable<Vector3> _networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<Quaternion> _networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        public Vector3 _networkPositionVelocity;
        public float _networkPositionSmoothTime = 0.1f;
        public float _networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<bool> _isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> _horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> _verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> _moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Target")]
        public NetworkVariable<ulong> _currentTargetNetworkObjectID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> _isBlocking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isAttacking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isInvulnerable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isLockOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isChargingAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isRipostable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Recurses")]
        public NetworkVariable<int> _currentHealth = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _maxHealth = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> _currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _maxStamina = new NetworkVariable<int>(200, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> _vitality = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _strength = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Stats Modifiers")]
        public NetworkVariable<int> _strengthModifier = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        public virtual void CheckHP(int oldValue, int newValue)
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

        public void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            if (!IsOwner)
                _characterManager._characterCombatManager._currentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();
        }
        public void OnIsLockOnChanged(bool old, bool isLockOn)
        {
            if (!isLockOn)
            {
                _characterManager._characterCombatManager._currentTarget = null;
            }
        }
        public void OnIsCharagingAttackChanged(bool oldStatus, bool newStatus)
        {
            _characterManager._animator.SetBool("IsChargingAttack", _isChargingAttack.Value);
        }
        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            _characterManager._animator.SetBool("IsMoving", _isMoving.Value);
        }
        public virtual void OnIsActiveChange(bool oldStatus, bool newStatus)
        {
            gameObject.SetActive(_isActive.Value);
        }
        public virtual void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            _characterManager._animator.SetBool("IsBlocking", _isBlocking.Value);
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
            _characterManager._characterAnimationManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(animationID, 0.2f);
        }
        // Attack Animations
        [ServerRpc]
        public void NotifyTheServerOfActionAttackAnimationServerRpc(ulong clientId, string animationID, bool applyRootMotion)
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
            _characterManager._characterAnimationManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(animationID, 0.2f);
        }
        // Attack Animations
        [ServerRpc]
        public void NotifyTheServerOfInstantActionAttackAnimationServerRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            // If this client is the host/server , then activate the client RPC
            if (IsServer)
            {
                PlayInstantActionAttackAnimationForAllClientsClientRpc(clientId, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayInstantActionAttackAnimationForAllClientsClientRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it (so we don't play the animation twice)
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformInstantActionAttackAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformInstantActionAttackAnimationFromServer(string animationID, bool applyRootMotion)
        {
            _characterManager._characterAnimationManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.Play(animationID);
        }
        // Damage
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfCharacterDamageServerRpc(ulong damageCharacterID,
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
                NotifyTheServerOfCharacterDamageClientClientRpc(damageCharacterID, charcterCausingDamageID, physicalDamage, magicDamage, fireDamage, holyDamage, ligthningDamage, poiseDamage, angleHitPoint, contactPointX, contactPointY, contactPointZ);
            }
        }
        [ClientRpc]
        public void NotifyTheServerOfCharacterDamageClientClientRpc(ulong damageCharacterID,
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
           ulong characterCausingDamageID,
           float physicalDamage,
           float magicDamage,
           float fireDamage,
           float holyDamage,
           float lightningDamage,
           float poiseDamage,
           float angleHitPoint,
           float contactPointX,
           float contactPointY,
           float contactPointZ)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeDamageEffect);

            damageEffect._physicalDamage = physicalDamage;
            damageEffect._magicDamage = magicDamage;
            damageEffect._fireDamage = fireDamage;
            damageEffect._holyDamage = holyDamage;
            damageEffect._lightingDamage = lightningDamage;
            damageEffect._poiseDamage = poiseDamage;
            damageEffect._angleHitFrom = angleHitPoint;
            damageEffect._constantPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect._characteCausingDamage = characterCausingDamage;

            damagedCharacter._characterEffectsManager.ProceesInstanceEffect(damageEffect);

        }
    
    }
}