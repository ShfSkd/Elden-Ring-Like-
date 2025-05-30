﻿using SKD.Character;
using SKD.Effects;
using SKD.WorldManager;
using System.Collections;
using SKD.World_Manager;
using UnityEngine;

namespace SKD.Colliders
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager _characterCausingDamage;// When calculation damage is used to check for attackers damage, modifiers etc

        [Header("Weapon Attack Modifiers")]
        public float _light_Attack_01_Modofier;
        public float _light_Attack_02_Modofier;
        public float _heavy_Attack_01_Modofier;
        public float _heavy_Attack_02_Modofier;
        public float _charge_Attack_01_Modofier;
        public float _charge_Attack_02_Modofier;
        public float _running_Attack_01_Modifier;
        public float _rolling_Attack_01_Modifier;
        public float _backstep_Attack_01_Modifier;
        public float _light_Jump_Attack_01_Modofier;
        public float _heavy_Jump_Attack_01_Modofier;

        protected override void Awake()
        {
            base.Awake();
            if (_damageCollider == null)
                _damageCollider = GetComponent<Collider>();

            _damageCollider.enabled = false;// Melee Weapon colliders should be disabled at start , only enabled when animation allow
        }
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // We do not want to damage ourselves 
                if (damageTarget == _characterCausingDamage)
                    return;

                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(_characterCausingDamage._characterGroup, damageTarget._characterGroup))
                    return;

                CheckForParry(damageTarget);

                CheckForBlock(damageTarget);

                if (!damageTarget._characterNetworkManager._isInvulnerable.Value)
                    DamageTarget(damageTarget);
            }
        }
        protected override void CheckForParry(CharacterManager damageTarget)
        {
            if (_charactersDamagedList.Contains(damageTarget))
                return;

            if (!_characterCausingDamage._characterNetworkManager._isParryable.Value)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (damageTarget._characterNetworkManager._isParrying.Value)
            {
                _charactersDamagedList.Add(damageTarget);
                damageTarget._characterNetworkManager.NotifyTheServerOfParryServerRpc(_characterCausingDamage.NetworkObjectId);
                damageTarget._characterAnimationManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
            }
        }
        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            _directionFromAttackToDamageTarget = _characterCausingDamage.transform.position - damageTarget.transform.position;
            _dotValueFromAttackToDamageTarget = Vector3.Dot(_directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // We don't want to damage the same target more then once in a single attack. So we add them to a list that check before applying damage 
            if (_charactersDamagedList.Contains(damageTarget))
                return;

            _charactersDamagedList.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeDamageEffect);
            damageEffect._physicalDamage = _physicalDamage;
            damageEffect._magicDamage = _magicDamage;
            damageEffect._fireDamage = _fireDamage;
            damageEffect._holyDamage = _holyDamage;
            damageEffect._poiseDamage = _poiseDamage;
            damageEffect._lightingDamage = _lightningDamage;
            damageEffect._constantPoint = _contactPoint;
            damageEffect._angleHitFrom = Vector3.SignedAngle(_characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (_characterCausingDamage._characterCombatManager._currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyDamageModifier(_light_Attack_01_Modofier, damageEffect);
                    break;
                case AttackType.LightAttack02:
                    ApplyDamageModifier(_light_Attack_02_Modofier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyDamageModifier(_heavy_Attack_01_Modofier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApplyDamageModifier(_heavy_Attack_02_Modofier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApplyDamageModifier(_charge_Attack_01_Modofier, damageEffect);
                    break;
                case AttackType.ChargedAttack02:
                    ApplyDamageModifier(_charge_Attack_02_Modofier, damageEffect);
                    break;
                case AttackType.RunningAttack01:
                    ApplyDamageModifier(_running_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.RollingAttack01:
                    ApplyDamageModifier(_rolling_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.BackstepAttack01:
                    ApplyDamageModifier(_backstep_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.LightJumpingAttack01:
                    ApplyDamageModifier(_light_Jump_Attack_01_Modofier, damageEffect);
                    break;
                case AttackType.HeavyJumpAttack01:
                    ApplyDamageModifier(_heavy_Jump_Attack_01_Modofier, damageEffect);
                    break;
                default:
                    break;
            }
            if (_characterCausingDamage.IsOwner)
            {
                // Send a damage request from the server
                damageTarget._characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, _characterCausingDamage.NetworkObjectId,
                    damageEffect._physicalDamage,
                    damageEffect._magicDamage,
                    damageEffect._fireDamage,
                    damageEffect._holyDamage,
                    damageEffect._lightingDamage,
                    damageEffect._poiseDamage,
                    damageEffect._angleHitFrom,
                    damageEffect._constantPoint.x,
                    damageEffect._constantPoint.y,
                    damageEffect._constantPoint.z);
            }

            //damageTarget._characterEffectsManager.ProceesInstanceEffect(damageEffect);
        }
        private void ApplyDamageModifier(float modifier, TakeDamageEffect damage)
        {
            damage._physicalDamage *= modifier;
            damage._magicDamage *= modifier;
            damage._fireDamage *= modifier;
            damage._holyDamage *= modifier;
            damage._lightingDamage *= modifier;
            damage._poiseDamage *= modifier;

            // If attack is fully charged heavy, multiply by a full charge modifier after normal modifier have been calculate 
        }
    }
}