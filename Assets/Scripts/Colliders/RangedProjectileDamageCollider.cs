using System;
using SKD.Character;
using SKD.Effects;
using SKD.World_Manager;
using SKD.WorldManager;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
namespace SKD.Colliders
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        [Header("Marksmen")]
        public CharacterManager _characterShootingProjectile;

        [Header("Collision")]
        private bool _hasPenetrateSurface;
        public Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;

        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
        }
        private void FixedUpdate()
        {
            if (_rigidbody.velocity != Vector3.zero)
            {
                _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            CreatePenetrationIntoObject(collision);
            CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>();

            if (_characterShootingProjectile == null)
                return;

            Collider contactCollider = collision.gameObject.GetComponent<Collider>();

            if (contactCollider != null)
                _contactPoint = contactCollider.ClosestPointOnBounds(transform.position);

            if (potentialTarget == null)
                return;

            if (WorldUtilityManager.Instance.CanIDamageThisTarget(_characterShootingProjectile._characterGroup, potentialTarget._characterGroup))
            {
                DamageTarget(potentialTarget);
            }
            {
                CheckForBlock(potentialTarget);
                DamageTarget(potentialTarget);
            }

        }
        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            if (_charactersDamagedList.Contains(damageTarget))
                return;

            float angle = Vector3.Angle(damageTarget.transform.forward, transform.forward);

            if (damageTarget._characterNetworkManager._isBlocking.Value && angle > 145)
            {
                _charactersDamagedList.Add(damageTarget);
                TakeBlockedDamageEffect blockDamageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeBlockedDamageEffect);

                if (_characterShootingProjectile != null)
                    blockDamageEffect._characteCausingDamage = _characterShootingProjectile;

                blockDamageEffect._physicalDamage = _physicalDamage;
                blockDamageEffect._magicDamage = _magicDamage;
                blockDamageEffect._fireDamage = _fireDamage;
                blockDamageEffect._holyDamage = _holyDamage;
                blockDamageEffect._poiseDamage = _poiseDamage;
                blockDamageEffect._staminaDamage = _poiseDamage;
                blockDamageEffect._constantPoint = _contactPoint;

                damageTarget._characterEffectsManager.ProceesInstanceEffect(blockDamageEffect);
            }
        }
        private void CreatePenetrationIntoObject(Collision hit)
        {
            if (!_hasPenetrateSurface)
            {
                _hasPenetrateSurface = true;

                gameObject.transform.position = hit.GetContact(0).point;

                // Stop our arrow from scaling in size with a scaled up or down object
                var emptyObject = new GameObject
                {
                    transform =
                    {
                        parent = hit.collider.transform
                    }
                };
                gameObject.transform.SetParent(emptyObject.transform, true);

                transform.position += transform.forward * (UnityEngine.Random.Range(0.1f, 0.3f));

                _rigidbody.isKinematic = true;
                _capsuleCollider.enabled = false;

                Destroy(GetComponent<RangedProjectileDamageCollider>());
                Destroy(gameObject, 20);
            }
        }
    }
}