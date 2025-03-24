using SKD.Items;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager _character;

        [Header("last Attack Animation Perform")]
        public string _lastAttackAnimationPerformed;
        
        [Header("Previous Poise Damage Taken")]
        public float _previousPoiseDamageTaken;

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
            _character = GetComponent<CharacterManager>();
        }
        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (_character.IsOwner)
            {
                if (newTarget != null)
                {
                    _currentTarget = newTarget;
                    _character._characterNetworkManager._currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    _currentTarget = null;
                }
            }
        }
        public void EnableIsInvulnerable()
        {
            if (_character.IsOwner)
                _character._characterNetworkManager._isInvulnerable.Value = true;
        }
        public void DisableIsInvulnerable()
        {
            if (_character.IsOwner)
                _character._characterNetworkManager._isInvulnerable.Value = false;

        }
        public void EnableIsRipostable()
        {
            if(_character.IsOwner)
                _character._characterNetworkManager._isRipostable.Value = true;
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