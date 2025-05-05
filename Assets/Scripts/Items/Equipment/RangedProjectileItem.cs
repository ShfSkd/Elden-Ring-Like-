using SKD.Character.Player;
using UnityEngine;
namespace SKD.Items.Equipment
{
    [CreateAssetMenu(menuName = "Items/Ranged Projectile")]
    public class RangedProjectileItem : Item
    {
        public ProjectileClass _projectileClass;

        [Header("Velocity")]
        public float _forwardVelocity = 2200f;
        public float _upwardVelocity = 0f;
        public float _ammoMass = 0.01f;

        [Header("Capacity")]
        public int _maxAmmoAmount = 30;
        public int _currentAmmoAmount = 30;
        
        [Header("Damage")]
        public int _physicalDamage = 0;
        public int _magicDamage = 0;
        public int _fireDamage = 0;
        public int _holyDamage = 0;
        public int _lightningDamage = 0;

        [Header("Model")]
        public GameObject _drawProjectileModel;
        public GameObject _releaseProjectileModel;
        
        public virtual void AttemptTooFireProjectile(PlayerManager player)
        {
            
        }

    }
}