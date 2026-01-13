using SKD.Character.Player;
using SKD.Utility;
using UnityEngine;
namespace SKD.Spells.Items
{
    [CreateAssetMenu(menuName = "Items/Spells/Fire Ball")]
    public class FireBallSpell : SpellItem
    {
        [Header("Projectile Velocity")]
        [SerializeField] float _upwardVelocity = 3f;
        [SerializeField] float _forwardVelocity = 15f;
        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);

            if (!CanICastThisSpell(player))
                return;

            if (player._playerNetworkManager._isUsingRightHand.Value)
            {
                player._playerAnimationManager.PlayTargetActionAnimation(_mainHandSpellAnimation, true);
            }
            else
            {
                player._playerAnimationManager.PlayTargetActionAnimation(_offHandSpellAnimation, true);

            }
        }
        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);

            // Determine which hand player is using
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiateWarmUpFX = Instantiate(_spellCastWarmUpFX);

            if (player._playerNetworkManager._isUsingRightHand.Value)
            {
                // Instantiate warm up FX on the correct Place(Hand Right)
                spellInstantiationLocation = player._playerEquipmentManager._rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                // Instantiate warm up FX on the correct Place(Hand Left)
                spellInstantiationLocation = player._playerEquipmentManager._leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            instantiateWarmUpFX.transform.parent = spellInstantiationLocation.transform;
            instantiateWarmUpFX.transform.localPosition = Vector3.zero;
            instantiateWarmUpFX.transform.localRotation = Quaternion.identity;

            // "Save" the warm-up fx as a variable so it can be destroyed out of the animation
            player._playerEffectsManager._activeSpellWarmUpFX = instantiateWarmUpFX;

        }
        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            // Destroy any warm up fx remaining from the spell
            if (player.IsOwner)
                player._playerCombatManager.DestroyALlCurrentActionFX();


            // Instantiate the projectile

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiateReleaseSpellFX = Instantiate(_spellCastReleaseFX);

            if (player._playerNetworkManager._isUsingRightHand.Value)
            {
                // Instantiate warm up FX on the correct Place(Hand Right)
                spellInstantiationLocation = player._playerEquipmentManager._rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                // Instantiate warm up FX on the correct Place(Hand Left)
                spellInstantiationLocation = player._playerEquipmentManager._leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiateReleaseSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiateReleaseSpellFX.transform.localPosition = Vector3.zero;
            instantiateReleaseSpellFX.transform.localRotation = Quaternion.identity;
            instantiateReleaseSpellFX.transform.parent = null;

            // Apply damage to projectile damage collider
            FireBallManager fireBallManager = instantiateReleaseSpellFX.GetComponent<FireBallManager>();
            fireBallManager.InitializeFireBall(player);

            // We dont have a damage collider yet, so we don't need to ignore collision yet 
            // Use the list of the colliders from the caster and now apply the ignore physics with the collider from the projectile
            // Other way of doing handle colliders
            /*Physics.IgnoreCollision(characterCollisionCollider,fireBallManager._damageCollider._damageCollider,true);

            foreach (Collider collider in characterColliders)
            {
                Physics.IgnoreCollision(collider,fireBallManager._damageCollider._damageCollider,true);

            }*/
            // Get any colliders from the caster and make it so the spell projectile ignore them
            /*Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            Collider characterCollisionCollider = player.GetComponent<Collider>();*/

            // Set the projectile velocity

            if (player._playerNetworkManager._isLockOn.Value)
            {
                instantiateReleaseSpellFX.transform.LookAt(player._playerCombatManager._currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiateReleaseSpellFX.transform.forward = forwardDirection;
            }
            Rigidbody spellRigidbody = instantiateReleaseSpellFX.GetComponent<Rigidbody>();
            Vector3 upwardVelocity = instantiateReleaseSpellFX.transform.up * _upwardVelocity;
            Vector3 forwardVelocity = instantiateReleaseSpellFX.transform.forward * _forwardVelocity;
            Vector3 totalVelocity = upwardVelocity + forwardVelocity;
            spellRigidbody.linearVelocity = totalVelocity;

        }
        public override void SuccessfullyChargeSpell(PlayerManager player)
        {
            base.SuccessfullyChargeSpell(player);

            if (player.IsOwner)
                player._playerCombatManager.DestroyALlCurrentActionFX();

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiateChargeSpellFX = Instantiate(_spellChargeFX);

            if (player._playerNetworkManager._isUsingRightHand.Value)
            {
                // Instantiate warm up FX on the correct Place(Hand Right)
                spellInstantiationLocation = player._playerEquipmentManager._rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                // Instantiate warm up FX on the correct Place(Hand Left)
                spellInstantiationLocation = player._playerEquipmentManager._leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            // "Save" the charge fx a variable so it can be destroyed if the player it knocks out of the animation
            player._playerEffectsManager._activeSpellWarmUpFX = instantiateChargeSpellFX;

            instantiateChargeSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiateChargeSpellFX.transform.localPosition = Vector3.zero;
            instantiateChargeSpellFX.transform.localRotation = Quaternion.identity;

        }
        public override void SuccessfullyCastSpellFullCharge(PlayerManager player)
        {
            base.SuccessfullyCastSpellFullCharge(player);

            // Destroy any warm up fx remaining from the spell
            if (player.IsOwner)
                player._playerCombatManager.DestroyALlCurrentActionFX();


            // Instantiate the projectile

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiateReleaseSpellFX = Instantiate(_spellCastReleaseFXFullCharge);

            if (player._playerNetworkManager._isUsingRightHand.Value)
            {
                // Instantiate warm up FX on the correct Place(Hand Right)
                spellInstantiationLocation = player._playerEquipmentManager._rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                // Instantiate warm up FX on the correct Place(Hand Left)
                spellInstantiationLocation = player._playerEquipmentManager._leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiateReleaseSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiateReleaseSpellFX.transform.localPosition = Vector3.zero;
            instantiateReleaseSpellFX.transform.localRotation = Quaternion.identity;
            instantiateReleaseSpellFX.transform.parent = null;

            // Apply damage to projectile damage collider
            FireBallManager fireBallManager = instantiateReleaseSpellFX.GetComponent<FireBallManager>();
            fireBallManager._isFullyCharge = true;
            fireBallManager.InitializeFireBall(player);

            // We dont have a damage collider yet, so we don't need to ignore collision yet 
            // Use the list of the colliders from the caster and now apply the ignore physics with the collider from the projectile
            // Other way of doing handle colliders
            /*Physics.IgnoreCollision(characterCollisionCollider,fireBallManager._damageCollider._damageCollider,true);

            foreach (Collider collider in characterColliders)
            {
                Physics.IgnoreCollision(collider,fireBallManager._damageCollider._damageCollider,true);

            }*/
            // Get any colliders from the caster and make it so the spell projectile ignore them
            /*Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            Collider characterCollisionCollider = player.GetComponent<Collider>();*/

            // Set the projectile velocity

            if (player._playerNetworkManager._isLockOn.Value)
            {
                instantiateReleaseSpellFX.transform.LookAt(player._playerCombatManager._currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiateReleaseSpellFX.transform.forward = forwardDirection;
            }
            Rigidbody spellRigidbody = instantiateReleaseSpellFX.GetComponent<Rigidbody>();
            Vector3 upwardVelocity = instantiateReleaseSpellFX.transform.up * _upwardVelocity;
            Vector3 forwardVelocity = instantiateReleaseSpellFX.transform.forward * _forwardVelocity;
            Vector3 totalVelocity = upwardVelocity + forwardVelocity;
            spellRigidbody.linearVelocity = totalVelocity;
        }
    
    }
}