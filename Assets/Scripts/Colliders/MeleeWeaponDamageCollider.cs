using SKD.Character;
using SKD.Effects;
using SKD.WorldManager;
using System.Collections;
using UnityEngine;

namespace SKD.Colliders
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager _characterCausingDamage; // When calculation damage is used to check for attackers damage, modifiers etc

        [Header("Weapon Attack Modifiers")]
        public float _light_Attack_01_Modofier;
        protected override void Awake()
        {
            base.Awake();
            if (_damageCollider == null)
                _damageCollider = GetComponent<Collider>() ;

            _damageCollider.enabled = false; // Melee Weapon colliders should be disables at start , only enabled when animation allow
        }
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();


            if (damageTarget != null)
            {
                // We do not want to damage ourselves 
                if (damageTarget == _characterCausingDamage)
                    return;

                _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            }
            DamageTarget(damageTarget);
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
            damageEffect._lightnigamage = _lightnigamage;
            damageEffect._contantPoint = _contactPoint;

            switch (_characterCausingDamage._characterCombatManager._currentAttacktype)
            {
                case AttackType.LigthAttack01:
                    ApplyDamageModifier(_light_Attack_01_Modofier, damageEffect);
                    break;
                default:
                    break;
            }
            if (_characterCausingDamage.IsOwner)
            {
                // Send a damage request from the server
                damageTarget._characterNetworkManager.NotifyTheServerofCharacterdamageServerRpc(damageTarget.NetworkObjectId, _characterCausingDamage.NetworkObjectId,
                    damageEffect._physicalDamage,
                    damageEffect._magicDamage,
                    damageEffect._fireDamage,
                    damageEffect._holyDamage,
                    damageEffect._lightnigamage,
                    damageEffect._poiseDamage,
                    damageEffect._angleHitFrom,
                    damageEffect._contantPoint.x,
                    damageEffect._contantPoint.y,
                    damageEffect._contantPoint.z);
            }

            //damageTarget._characterEffectsManager.ProceesInstanceEffect(damageEffect);
        }
        private void ApplyDamageModifier(float modifier, TakeDamageEffect damage)
        {
            damage._physicalDamage *= modifier;
            damage._magicDamage *= modifier;
            damage._fireDamage *= modifier;
            damage._holyDamage *= modifier;
            damage._lightnigamage *= modifier;
            damage._poiseDamage *= modifier;

            // If attack is fully charged heavy, multiply by a full charge modifier after normal modifier have been calculate 
        }
    }
}
