using SKD.Items.Weapon_Actions;
using System.Collections.Generic;
using SKD.Colliders;
using SKD.Effects;
using SKD.Items.Weapons;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using UnityEngine;
using RangedProjectileItem = SKD.Items.Equipment.RangedProjectileItem;

namespace SKD.Character.Player
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager _player;

        public WeaponItem _currentWeaponBeingUsed;
        public ProjectileSlot _currentProjectileBeingUsed;

        [Header("Projectile")]
        private Vector3 _projectileAimeDirection;

        [Header("Flags")]
        public bool _canComboWithMainHandWeapon;
        /*  public bool _canPerformRollingAttack;
          public bool _canPerformBackstopAttack;*/
        public bool _isUsingItem;

        // public bool _canComboWithOffHandWeapon;
        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<PlayerManager>();
        }
        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (_player.IsOwner)
            {
                // perform the action
                weaponAction.AttemptToPerformedAction(_player, weaponPerformingAction);

            }

        }
        protected override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            _player._playerEquipmentManager._rightWeaponManager._meleeDamageCollider.DisableDamageCollider();
            _player._playerEquipmentManager._leftWeaponManager._meleeDamageCollider.DisableDamageCollider();
        }
        public override void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            // If some how since the initial check the character can no longer be riposted, return
            if (!targetCharacter._characterNetworkManager._isRipostable.Value)
                return;

            // If somebody else is already performing a critical strike on the character (or we already are) , return
            if (targetCharacter._characterNetworkManager._isBeingCrititcalDamged.Value)
                return;

            // You can inly riposte with a melee weapon item 
            MeleeWeaponItem riposteWeapon;
            MeleeWeaponDamageCollider riposteCollider;

            // Check if we are two handing weapon right/left (this will change the riposte weapon

            if (_player._playerNetworkManager._isTwoHandingWeapon.Value)
            {
                riposteWeapon = _player._playerInventoryManager._currentLeftHandWeapon as MeleeWeaponItem;
                riposteCollider = _player._playerEquipmentManager._leftWeaponManager._meleeDamageCollider;

            }
            else
            {
                riposteWeapon = _player._playerInventoryManager._currentRightHandWeapon as MeleeWeaponItem;
                riposteCollider = _player._playerEquipmentManager._rightWeaponManager._meleeDamageCollider;
            }

            // The riposte animation will change depending on the weapon's animator controller, so the animation can be chosen there, the name will always be the same 
            _character._characterAnimationManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

            // While performing a critical strike, you cannot be damged 
            if (_character.IsOwner)
                _character._characterNetworkManager._isInvulnerable.Value = true;

            // 1. Create a new damage effect for this type of damage  
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeCriticalDamageEffect);

            // 2. Apply all of th e damage states from the collider to the damage effect
            damageEffect._physicalDamage = riposteCollider._physicalDamage;
            damageEffect._holyDamage = riposteCollider._holyDamage;
            damageEffect._fireDamage = riposteCollider._fireDamage;
            damageEffect._lightingDamage = riposteCollider._lightningDamage;
            damageEffect._magicDamage = riposteCollider._magicDamage;
            damageEffect._poiseDamage = riposteCollider._poiseDamage;

            // 3. Multiply damage by weapon riposte modifier
            if (riposteWeapon != null)
            {
                damageEffect._physicalDamage *= riposteWeapon._riposte_Attack_01_Modifier;
                damageEffect._holyDamage *= riposteWeapon._riposte_Attack_01_Modifier;
                damageEffect._fireDamage *= riposteWeapon._riposte_Attack_01_Modifier;
                damageEffect._lightingDamage *= riposteWeapon._riposte_Attack_01_Modifier;
                damageEffect._magicDamage *= riposteWeapon._riposte_Attack_01_Modifier;
                damageEffect._poiseDamage *= riposteWeapon._riposte_Attack_01_Modifier;

                // 4. Using a server RPC send the riposte to the target, where the will play the proper animation on their end, and take the damage
                targetCharacter._characterNetworkManager.NotifyTheServerOfRiposteServerRpc(targetCharacter.NetworkObjectId, _character.NetworkObjectId,
                    "Riposted_01", riposteWeapon._itemID, damageEffect._physicalDamage,
                    damageEffect._magicDamage, damageEffect._fireDamage, damageEffect._holyDamage, damageEffect._lightingDamage, damageEffect._poiseDamage);
            }

        }
        public override void AttemptBackstab(RaycastHit hit)
        {
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            // If some how since the initial check the character can no longer be riposted, return
            if (!targetCharacter._characterCombatManager._canBackstabbed)
                return;

            // If somebody else is already performing a critical strike on the character (or we already are) , return
            if (targetCharacter._characterNetworkManager._isBeingCrititcalDamged.Value)
                return;

            // You can inly riposte with a melee weapon item 
            MeleeWeaponItem backstabWeapon;
            MeleeWeaponDamageCollider backstabCollider;

            // Check if we are two handing weapon right/left (this will change the riposte weapon
            if (_player._playerNetworkManager._isTwoHandingWeapon.Value)
            {
                backstabWeapon = _player._playerInventoryManager._currentLeftHandWeapon as MeleeWeaponItem;
                backstabCollider = _player._playerEquipmentManager._leftWeaponManager._meleeDamageCollider;

            }
            else
            {
                backstabWeapon = _player._playerInventoryManager._currentRightHandWeapon as MeleeWeaponItem;
                backstabCollider = _player._playerEquipmentManager._rightWeaponManager._meleeDamageCollider;
            }


            // The riposte animation will change depending on the weapon's animator controller, so the animation can be chosen there, the name will always be the same 
            _character._characterAnimationManager.PlayTargetActionAnimationInstantly("Backstab_01", true);

            // While performing a critical strike, you cannot be damged 
            if (_character.IsOwner)
                _character._characterNetworkManager._isInvulnerable.Value = true;

            // 1. Create a new damage effect for this type of damage  
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeCriticalDamageEffect);

            // 2. Apply all of th e damage states from the collider to the damage effect
            damageEffect._physicalDamage = backstabCollider._physicalDamage;
            damageEffect._holyDamage = backstabCollider._holyDamage;
            damageEffect._fireDamage = backstabCollider._fireDamage;
            damageEffect._lightingDamage = backstabCollider._lightningDamage;
            damageEffect._magicDamage = backstabCollider._magicDamage;
            damageEffect._poiseDamage = backstabCollider._poiseDamage;

            // 3. Multiply damage by weapon riposte modifier
            if (backstabWeapon != null)
            {
                damageEffect._physicalDamage *= backstabWeapon._backstab_Attack_01_Modifier;
                damageEffect._holyDamage *= backstabWeapon._backstab_Attack_01_Modifier;
                damageEffect._fireDamage *= backstabWeapon._backstab_Attack_01_Modifier;
                damageEffect._lightingDamage *= backstabWeapon._backstab_Attack_01_Modifier;
                damageEffect._magicDamage *= backstabWeapon._backstab_Attack_01_Modifier;
                damageEffect._poiseDamage *= backstabWeapon._backstab_Attack_01_Modifier;

                // 4. Using a server RPC send the riposte to the target, where the will play the proper animation on their end, and take the damage
                targetCharacter._characterNetworkManager.NotifyTheServerOfBackstabServerRpc(targetCharacter.NetworkObjectId, _character.NetworkObjectId,
                    "Backstabed_01", backstabWeapon._itemID, damageEffect._physicalDamage,
                    damageEffect._magicDamage, damageEffect._fireDamage, damageEffect._holyDamage, damageEffect._lightingDamage, damageEffect._poiseDamage);
            }

        }
        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!_player.IsOwner)
                return;
            if (_currentWeaponBeingUsed == null)
                return;

            float staminaDetucted = 0f;

            switch (_currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack02:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._runningAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._rollingAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._backstepAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightJumpingAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._lightJumpAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyJumpAttack01:
                    staminaDetucted = _currentWeaponBeingUsed._baseStaminaCost * _currentWeaponBeingUsed._heavyJumpAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }
            _player._playerNetworkManager._currentStamina.Value -= Mathf.RoundToInt(staminaDetucted);
        }
        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if (_player.IsOwner)
            {
                PlayerCamera.Instance.SetLockOnCameraHeight();
            }
        }
        // Projectile
        public void ReleaseArrow()
        {
            if (_player.IsOwner)
                _player._playerNetworkManager._hasArrowNotched.Value = false;

            // Destroy The warm-up Projectile
            if (_player._playerEffectsManager._activeDrawnProjectileFX != null)
                Destroy(_player._playerEffectsManager._activeDrawnProjectileFX);

            _player._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance._releaseArrowSFX));

            // Animate the bow
            Animator bowAnimator;

            if (_player._playerNetworkManager._isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = _player._playerEquipmentManager._leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = _player._playerEquipmentManager._rightHandWeaponModel.GetComponentInChildren<Animator>();
            }
            bowAnimator.SetBool("IsDrawn", false);
            bowAnimator.Play("Bow_Fire_01");

            if (!_player.IsOwner)
                return;

            // The projectile we are firing 
            RangedProjectileItem projectileItem = null;

            switch (_currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    projectileItem = _player._playerInventoryManager._mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = _player._playerInventoryManager._secondaryProjectile;
                    break;
                default:
                    break;
            }

            if (projectileItem == null)
                return;

            if (projectileItem._currentAmmoAmount <= 0)
                return;

            Transform projectileInstantiationLocation = null;
            GameObject projectileGameObject = null;
            Rigidbody projectileRigidbody = null;
            RangedProjectileDamageCollider projectileDamageCollider = null;

            // Subtract Ammo
            projectileItem._currentAmmoAmount -= 1;
            
            // Update ammo count in the UI
            switch (_currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    PlayerUIManager.Instance._playerUIHUDManager.SetMainProjectileQuickSlotIcon(projectileItem);
                    break;
                case ProjectileSlot.Secondary:
                    PlayerUIManager.Instance._playerUIHUDManager.SetSecondaryProjectileQuickSlotIcon(projectileItem);
                    break;
                default:
                    break;
            }
            
            projectileInstantiationLocation = _player._playerCombatManager._lockOnTransform;
            projectileGameObject = Instantiate(projectileItem._releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            projectileDamageCollider._physicalDamage = 100;
            projectileDamageCollider._characterShootingProjectile = _player;

            // Fire an arrow based on 1 of 3 variations 
            // 1. Locked onto target
            float yRotationDuringFire = _player.transform.localEulerAngles.y;
            // 2. Aiming
            if (_player._playerNetworkManager._isAiming.Value)
            {
                Ray newRay = new Ray(_lockOnTransform.position, PlayerCamera.Instance._aimDirection);
                _projectileAimeDirection = newRay.GetPoint(5000);
                projectileGameObject.transform.LookAt(_projectileAimeDirection);
            }
            else
            {
                // 3. Locked and not aiming

                if (_player._playerCombatManager._currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(_player._playerCombatManager._currentTarget._characterCombatManager._lockOnTransform.position -
                                                                       projectileGameObject.transform.position);

                    projectileGameObject.transform.rotation = arrowRotation;
                }
                // 4. Unlocked and not aiming
                else
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(_player.transform.forward);

                    projectileGameObject.transform.rotation = arrowRotation;
                }

            }


            // Get all character colliders and ignore self
            Collider[] characterColliders = _player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgnored = new List<Collider>();

            foreach (var item in characterColliders)
                collidersArrowWillIgnored.Add(item);

            foreach (var hitBox in collidersArrowWillIgnored)
                Physics.IgnoreCollision(projectileDamageCollider._damageCollider, hitBox, true);

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem._forwardVelocity);
            projectileGameObject.transform.parent = null;

            // Sync arrow fire with serverRpc
            _player._playerNetworkManager.NotifyServerOfReleasedProjectileServerRpc(_player.OwnerClientId, projectileItem._itemID,
                _projectileAimeDirection.x, _projectileAimeDirection.y, _projectileAimeDirection.z, yRotationDuringFire);

        }
        public void InstantiateSpellWarmUpFX()
        {
            if (_player._playerInventoryManager._currentSpell == null)
                return;

            _player._playerInventoryManager._currentSpell.InstantiateWarmUpSpellFX(_player);
        }

        // Spell
        public void SuccessfullyCastSpell()
        {
            if (_player._playerInventoryManager._currentSpell == null)
                return;

            _player._playerInventoryManager._currentSpell.SuccessfullyChargeSpell(_player);
        }
        public void SuccessfullyCastSpellFullCharge()
        {

            if (_player._playerInventoryManager._currentSpell == null)
                return;

            _player._playerInventoryManager._currentSpell.SuccessfullyCastSpellFullCharge(_player);
        }

        // Quick Slot
        public void SuccessfullyQuickSlotItem()
        {
            if (_player._playerInventoryManager._currentQuickSlotItem != null)
                _player._playerInventoryManager._currentQuickSlotItem.SuccessfullyUseItem(_player);
        }

        // Ashes of war
        public WeaponItem SelectWeaponToPerformAshOfWar()
        {
            WeaponItem selectedWeapon = _player._playerInventoryManager._currentLeftHandWeapon;
            _player._playerNetworkManager.SetCharacterActionHand(false);
            _player._playerNetworkManager._currentWeaponBeingUsed.Value = selectedWeapon._itemID;

            return selectedWeapon;
        }
    }
}