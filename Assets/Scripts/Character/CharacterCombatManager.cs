using SKD.Items;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager _characterManager;

        [Header("last Attack Animation Perform")]
        public string _lastAttackAnimationPerformed;

        [Header("Attack Character")]
        public CharacterManager _currentTarget;

        [Header("Attack Type")]
        public AttackType _currentAttackType;

        [Header("Lock On Transform")]
        public Transform _lockOnTransform;

        [Header("Attack Flags")]
        public bool _canPerformRollingAttack;
        public bool _canPerformBackstopAttack;
        public bool _canBlock = true;

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
        public void EnableIsInvulnerable()
        {
            if (_characterManager.IsOwner)
                _characterManager._characterNetworkManager._isInvulnerable.Value = true;
        }
        public void DisableIsInvulnerable()
        {
            if (_characterManager.IsOwner)
                _characterManager._characterNetworkManager._isInvulnerable.Value = false;

        }
        public void EnableCanDoRollingAttack()
        {
            _canPerformRollingAttack = true;
        }

        public void DisableCanDoRollingAttack()
        {
            _canPerformRollingAttack = false;
        }

        public void EnableCanDoBackstepAttack()
        {
            _canPerformBackstopAttack = true;
        }

        public void DisableCanDoBackstepAttack()
        {
            _canPerformBackstopAttack = false;
        }
    }
}