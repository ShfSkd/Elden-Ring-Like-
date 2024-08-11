using SKD.Items;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        private CharacterManager _characterManager;

        [Header("Attack Character")]
        public CharacterManager _currentTarget;

        [Header("Attack Type")]
        public AttackType _currentAttacktype;

        [Header("Lock On Transform")]
        public Transform _lockOnTransform;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (_characterManager.IsOwner)
            {
                if (newTarget != null)
                {
                    _currentTarget = newTarget;
                    _characterManager._characterNetworkManager._currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    _currentTarget = null;
                }
            }
        }
    }
}