 using SKD.Character;
using SKD.Effects;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Colliders
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider _damageCollider;
        [Header("Damage")]
        public float _physicalDamage;
        public float _magicDamage;
        public float _fireDamage;
        public float _lightningDamage;
        public float _holyDamage;

        [Header("Contact Point")]
        protected Vector3 _contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> _charactersDamagedList = new List<CharacterManager>();

        protected virtual void Awake()
        {

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damagetarget = other.GetComponentInParent<CharacterManager>();

         /*   // If you want to search on both the damageable character colliders & the character collider just check for null here and do the following
            *//* if (damagetarget == null)
             {
                 damagetarget = other.GetComponent<CharacterManager>();
             }*/
            if (damagetarget != null)
            {
                _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damagetarget);
            }
        }
        protected virtual void DamageTarget(CharacterManager damageTarget)
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
            damageEffect._constantPoint = _contactPoint;

            damageTarget._characterEffectsManager.ProceesInstanceEffect(damageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            _damageCollider.enabled = true;
        }
        public virtual void DisableDamageCollider()
        {
            _damageCollider.enabled = false;
            _charactersDamagedList.Clear(); // We rests the characters that have been hit when we reset the collider, so they may be hit again
        }

    }
}
