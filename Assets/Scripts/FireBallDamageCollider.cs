using SKD.Character;
using SKD.Effects;
using SKD.World_Manager;
using SKD.WorldManager;
using UnityEngine;
namespace SKD
{
    public class FireBallDamageCollider : SpellProjectileDamageCollider
    {
        private FireBallManager _fireBallManager;
        protected override void Awake()
        {
            base.Awake();

            _fireBallManager = GetComponentInParent<FireBallManager>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // We do not want to damage ourselves 
                if (damageTarget == _spellCaster)
                    return;

                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(_spellCaster._characterGroup, damageTarget._characterGroup))
                    return;

                CheckForParry(damageTarget);

                CheckForBlock(damageTarget);

                if (!damageTarget._characterNetworkManager._isInvulnerable.Value)
                    DamageTarget(damageTarget);
                
                _fireBallManager.WaitThenInstantiateSpellDestructionFX(0.4f);
            }
        }
        protected override void CheckForParry(CharacterManager damageTarget)
        {

        }
        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            _directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
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
            damageEffect._angleHitFrom = Vector3.SignedAngle(_spellCaster.transform.forward, damageTarget.transform.forward, Vector3.up);


            if (_spellCaster.IsOwner)
            {
                // Send a damage request from the server
                damageTarget._characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, _spellCaster.NetworkObjectId,
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
    }
}