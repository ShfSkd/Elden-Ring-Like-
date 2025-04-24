using System.Collections;
using SKD.Character;
using UnityEngine;
namespace SKD
{
    public class FireBallManager : SpellManager
    {
        [Header("Colliders")]
        public FireBallDamageCollider _damageCollider;

        [Header("Instantiate FX")]
        GameObject _InstantiatedDestructionFX;

        private bool _hasCollided;
        public bool _isFullyCharge;
        private Rigidbody _fireBallRigidbody;
        private Coroutine _destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();

            _fireBallRigidbody = GetComponent<Rigidbody>();
        }
        protected override void Update()
        {
            base.Update();

            if (_spellTaregt != null)
                transform.LookAt(_spellTaregt.transform);

            if (_fireBallRigidbody != null)
            {
                Vector3 currentVelocity = _fireBallRigidbody.velocity;
                _fireBallRigidbody.velocity = transform.forward + currentVelocity;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // If we collide with a character, ignore this we will let the damage coliider handle character collision, this is just for impact VFX
            if (collision.gameObject.layer == 6)
                return;

            if (!_hasCollided)
            {
                _hasCollided = true;
                InstantiateSpellDestructionFX();
            }
        }
        public void InitializeFireBall(CharacterManager spellCaster)
        {
            _damageCollider._spellCaster = spellCaster;

            _damageCollider._fireDamage = 150f;

            if (_isFullyCharge)
                _damageCollider._fireDamage *= 1.4f;
        }
        public void InstantiateSpellDestructionFX()
        {
            if (_isFullyCharge)
            {
                _InstantiatedDestructionFX = Instantiate(_impactParticleFullCharge, transform.position, Quaternion.identity);
            }
            else
            {
                _InstantiatedDestructionFX = Instantiate(_impactParticle, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        public void WaitThenInstantiateSpellDestructionFX(float timeToWait)
        {
            if (_destructionFXCoroutine != null)
                StopCoroutine(_destructionFXCoroutine);

            _destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));
            StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }
        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            InstantiateSpellDestructionFX();
        }
    }
}