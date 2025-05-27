using System;
using System.Collections.Generic;
using SKD.Colliders;
using SKD.Effects;
using SKD.Items;
using SKD.Items.Equipment;
using SKD.Items.Quick_Item_Slot;
using SKD.Items.Weapon_Actions;
using SKD.Items.Weapons;
using SKD.Spells.Items;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        static readonly int IsTwoHandedWeapon = Animator.StringToHash("IsTwoHandedWeapon");
        PlayerManager _player;

        public NetworkVariable<FixedString64Bytes> _characterName = new NetworkVariable<FixedString64Bytes>("Character",
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> _currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentLeftWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentSpellID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _currentQuickSlotItemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Two Handed Weapons")]
        public NetworkVariable<int> _currentWeaponBeingTwoHanded = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isTwoHandingWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isTwoHandingRightWepoen = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isTwoHandingLeftWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Spells")]
        public NetworkVariable<bool> _isChargingRightSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _isChargingLeftSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Armor")]
        public NetworkVariable<bool> _isMale = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _headEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _bodyEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _legEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _handEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Projectiles")]
        public NetworkVariable<int> _mainProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> _secondaryProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // This lets us know if we already have a projectile loaded
        public NetworkVariable<bool> _hasArrowNotched = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // This lets us know if we are holding this projectile so it does not release
        public NetworkVariable<bool> _isHoldingArrow = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // Lets us know if we are "Zoomed" in and using for aiming camera 
        public NetworkVariable<bool> _isAiming = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<PlayerManager>();
        }
        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if (rightHandedAction)
            {
                _isUsingLeftHand.Value = false;
                _isUsingRightHand.Value = true;
            }
            else
            {
                _isUsingRightHand.Value = false;
                _isUsingLeftHand.Value = true;
            }
        }
        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            _maxHealth.Value = _player._playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.Instance._playerUIHUDManager.SetMaxHealthValue(_maxHealth.Value);
            _currentHealth.Value = _maxHealth.Value;
        }
        public void SetNewMaxFocusPointsValue(int oldMind, int newMind)
        {
            _maxFocusPoints.Value = _player._playerStatsManager.CalculateFucosPointsBasedOnMindLevel(newMind);
            PlayerUIManager.Instance._playerUIHUDManager.SetMaxFocusPointsValue(_maxFocusPoints.Value);
            _currentFocusPoints.Value = _maxFocusPoints.Value;
        }
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            _maxStamina.Value = _player._playerStatsManager.CalculateStaminaBasedOnEnduraceLevel(newEndurance);
            PlayerUIManager.Instance._playerUIHUDManager.SetMaxStaminaValue(_maxStamina.Value);
            _currentStamina.Value = _maxStamina.Value;
        }
        public void OnCurrentRightHandWeaponIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _player._playerInventoryManager._currentRightHandWeapon = newWeapon;
            _player._playerEquipmentManager.LoadRightWepon();

            if (_player.IsOwner)
            {
                PlayerUIManager.Instance._playerUIHUDManager.SetRightWeaponQuickSlotIcon(newId);
            }
        }
        public void OnCurrentSpellIDChange(int oldId, int newId)
        {
            SpellItem newSpell = null;

            if (WorldItemDatabase.Instance.GetSpellByID(newId))
                newSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(newId));

            if (newSpell != null)
            {
                _player._playerInventoryManager._currentSpell = newSpell;

                if (_player.IsOwner)
                    PlayerUIManager.Instance._playerUIHUDManager.SetSpellItemQuickSlotIcon(newId);
            }


        }
        
        public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)
        {
            QuickSlotItem newQuickSlotItem = null;

            if (WorldItemDatabase.Instance.GetQuickSlotItemByID(newID))
                newQuickSlotItem = Instantiate(WorldItemDatabase.Instance.GetQuickSlotItemByID(newID));

            if (newQuickSlotItem != null)
            {
                _player._playerInventoryManager._currentQuickSlotItem = newQuickSlotItem;

                if (_player.IsOwner)
                    PlayerUIManager.Instance._playerUIHUDManager.SetQuickSlotItemQuickSlotIcon(newID);
            }
        }
        public void OnMainProjectileIDChange(int oldId, int newId)
        {
            RangedProjectileItem projectileItem = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newId))
                projectileItem = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newId));


            if (projectileItem != null)
                _player._playerInventoryManager._mainProjectile = projectileItem;

        }
        public void OnSecondaryProjectileIDChange(int oldId, int newId)
        {
            RangedProjectileItem projectileItem = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newId))
                projectileItem = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newId));


            if (projectileItem != null)
                _player._playerInventoryManager._seconderyrojectile = projectileItem;

        }
        public void OnIsChargingRightSpellChange(bool oldValue, bool newValue)
        {
            _player._animator.SetBool("IsChargingRightSpell", _isChargingRightSpell.Value);
        }
        public void OnIsHoldingArrowChange(bool oldValue, bool newValue)
        {
            _player._animator.SetBool("IsHoldingArrow", _isHoldingArrow.Value);
        }
        public void OnIsAimingChanged(bool oldValue, bool newValue)
        {
            if (!_isAiming.Value)
            {
                PlayerCamera.Instance._cameraObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.Instance._cameraObject.fieldOfView = 60;
                PlayerCamera.Instance._cameraObject.nearClipPlane = 0.3f;
                PlayerCamera.Instance._cameraPivotTransform.localPosition = new Vector3(0, PlayerCamera.Instance._cameraPivotYPositionOffset, 0);
                PlayerUIManager.Instance._playerUIHUDManager._crosshair.SetActive(false);
            }
            else
            {
                PlayerCamera.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.Instance._cameraPivotTransform.localEulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.Instance._cameraObject.fieldOfView = 40;
                PlayerCamera.Instance._cameraObject.nearClipPlane = 1.3f;
                PlayerCamera.Instance._cameraPivotTransform.localPosition = Vector3.zero;
                PlayerUIManager.Instance._playerUIHUDManager._crosshair.SetActive(true);

            }
        }
        public void OnIsChargingLeftSpellChange(bool oldValue, bool newValue)
        {
            _player._animator.SetBool("IsChargingLeftSpell", _isChargingLeftSpell.Value);
        }
        public void OnCurrentLeftHandWeaponIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _player._playerInventoryManager._currentLeftHandWeapon = newWeapon;
            _player._playerEquipmentManager.LoadLeftWeapon();

            if (_player.IsOwner)
            {
                PlayerUIManager.Instance._playerUIHUDManager.SetLeftWeaponQuickSlotIcon(newId);
            }
        }
        public void OnCurrentWeaponBeingUsedIDChange(int oldId, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            _player._playerCombatManager._currentWeaponBeingUsed = newWeapon;

            if (_player.IsOwner)
                return;

            if (_player._playerCombatManager._currentWeaponBeingUsed != null)
                _player._playerAnimationManager.UpdateAnimatorController(_player._playerCombatManager
                    ._currentWeaponBeingUsed._weaponAnimator);
        }
        public void OnIsTwoHandingWeaponChanged(bool oldValue, bool newValue)
        {
            if (!_isTwoHandingWeapon.Value)
            {
                if (IsOwner)
                {
                    _isTwoHandingLeftWeapon.Value = false;
                    _isTwoHandingRightWepoen.Value = false;
                }
                _player._playerEquipmentManager.UnTwoHandWeapon();
                _player._playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.Instance._twoHandingEffect._staticEffectID);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.Instance._twoHandingEffect);
                _player._playerEffectsManager.AddStaticEffect(twoHandEffect);
            }

            _player._animator.SetBool(IsTwoHandedWeapon, _isTwoHandingWeapon.Value);
        }
        public void OnIsTwoHandingRightWeaponChanged(bool oldValue, bool newValue)
        {
            if (_isTwoHandingRightWepoen.Value)
                return;

            if (IsOwner)
            {
                _currentWeaponBeingTwoHanded.Value = _currentRightHandWeaponID.Value;
                _isTwoHandingWeapon.Value = true;
            }

            _player._playerInventoryManager._currentTwoHandWeapon = _player._playerInventoryManager._currentRightHandWeapon;
            _player._playerEquipmentManager.TwoHandRightWeapon();
        }
        public void OnIsTwoHandingLeftWeaponChanged(bool oldValue, bool newValue)
        {
            if (!_isTwoHandingLeftWeapon.Value)
                return;

            if (IsOwner)
            {
                _currentWeaponBeingTwoHanded.Value = _currentLeftWeaponID.Value;
                _isTwoHandingWeapon.Value = true;
            }
            _player._playerInventoryManager._currentTwoHandWeapon =
                _player._playerInventoryManager._currentLeftHandWeapon;
            _player._playerEquipmentManager.TwoHandLeftWeapon();
        }
        public void OnHeadEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            HeadEquipmentItem equipment = WorldItemDatabase.Instance.GetHeadEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadHeadEquipment(null);
            }
        }
        public void OnBodyEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            BodyEquipmentItem equipment = WorldItemDatabase.Instance.GetBodyEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadBodyEquipment(null);
            }
        }
        public void OnLegEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            LegEquipmentItem equipment = WorldItemDatabase.Instance.GetLegEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadLegEquipment(null);
            }
        }
        public void OnHandEquipmentChanged(int oldValue, int newValue)
        {
            // we already run the logic on the owner side, so there no point in running it again 
            if (!IsOwner) return;

            HandEquipmentItem equipment = WorldItemDatabase.Instance.GetHandEquipmentByID(_headEquipmentID.Value);

            if (equipment != null)
            {
                _player._playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
            }
            else
            {
                _player._playerEquipmentManager.LoadHandEquipment(null);
            }
        }
        public void OnIsMaleChanged(bool oldStatus, bool newStatus)
        {
            _player._playerBodyManager.ToggleBodyType(_isMale.Value);
        }
        public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsBlockingChanged(oldStatus, newStatus);

            if (IsOwner)
            {
                _player._playerStatsManager._blockingPhysicalAbsorption = _player._playerCombatManager
                    ._currentWeaponBeingUsed._physicalBaseDamageAbsorption;
                _player._playerStatsManager._blockingMagicAbsorption = _player._playerCombatManager
                    ._currentWeaponBeingUsed._magicBaseDamageAbsorption;
                _player._playerStatsManager._blockingFireAbsorption =
                    _player._playerCombatManager._currentWeaponBeingUsed._fireBaseDamageAbsorption;
                _player._playerStatsManager._blockingLightningAbsorption = _player._playerCombatManager
                    ._currentWeaponBeingUsed._lightingBaseDamageAbsorption;
                _player._playerStatsManager._blockingHolyAbsorption =
                    _player._playerCombatManager._currentWeaponBeingUsed._holyBaseDamageAbsorption;
                _player._playerStatsManager._blockingStability =
                    _player._playerCombatManager._currentWeaponBeingUsed._stability;
            }
        }

        // Item Actions
        [ServerRpc]
        public void NotifyServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            if (IsServer)
            {
                NotifyServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }

        [ClientRpc]
        private void NotifyServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            // We do not play the action again for the character who called it, because they already played it locally
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.Instance.GetWeponActionItemByID(weaponID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformedAction(_player, WorldItemDatabase.Instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action Is Null, Cannot be performed");
            }
        }
        [ClientRpc]
        protected override void DestroyALlCurrentActionFXClientRpc()
        {
            base.DestroyALlCurrentActionFXClientRpc();

            if (_hasArrowNotched.Value)
            {
                _player._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance._releaseArrowSFX));

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

                if (_player.IsOwner)
                    _hasArrowNotched.Value = false;
            }
        }
        // Draw Projectile
        [ServerRpc]
        public void NotifyServerOfDrawnProjectileServerRpc(int projectileItemItemID)
        {
            if (IsServer)
            {
                NotifyClientOfDrawnProjectileClientRpc(projectileItemItemID);
            }
        }
        [ClientRpc]
        private void NotifyClientOfDrawnProjectileClientRpc(int projectileItemID)
        {
            Animator bowAnimator = null;

            if (_isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = _player._playerEquipmentManager._leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else if (_isTwoHandingRightWepoen.Value)
            {
                bowAnimator = _player._playerEquipmentManager._rightHandWeaponModel.GetComponentInChildren<Animator>();
            }

            // Animate the bow
            if (bowAnimator != null)
            {
                bowAnimator.SetBool("IsDrawn", true);
                bowAnimator.Play("Bow_Drawn_01");
            }

            // Instantiate the bow 
            GameObject arrow = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(
                projectileItemID)._drawProjectileModel, _player._playerEquipmentManager._leftHandWeaponModel.transform);
            _player._playerEffectsManager._activeDrawnProjectileFX = arrow;

            // Play SFX
            _player._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance._notchArrowSFX));

        }

        // Release Projectile
        [ServerRpc]
        public void NotifyServerOfReleasedProjectileServerRpc(ulong playerClientId, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (IsServer)
            {
                NotifyServerOfReleasedProjectileClientRpc(playerClientId, projectileID, xPosition, yPosition, zPosition, yCharacterRotation);
            }
        }
        [ClientRpc]
        public void NotifyServerOfReleasedProjectileClientRpc(ulong playerClientId, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (playerClientId != NetworkManager.Singleton.LocalClientId)
                PerformReleaseProjectileFromRpc(projectileID, xPosition, yPosition, zPosition, yCharacterRotation);
        }
        private void PerformReleaseProjectileFromRpc(int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            RangedProjectileItem projectileItem = null;

            // The projectile we are firing 
            if (WorldItemDatabase.Instance.GetProjectileByID(projectileID) != null)
            {
                projectileItem = WorldItemDatabase.Instance.GetProjectileByID(projectileID);
            }

            if (projectileItem == null)
                return;

            Transform projectileInstantiationLocation = null;
            GameObject projectileGameObject = null;
            Rigidbody projectileRigidbody = null;
            RangedProjectileDamageCollider projectileDamageCollider = null;

            projectileInstantiationLocation = _player._playerCombatManager._lockOnTransform;
            projectileGameObject = Instantiate(projectileItem._releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            projectileDamageCollider._physicalDamage = 100;
            projectileDamageCollider._characterShootingProjectile = _player;

            // Fire an arrow based on 1 of 3 variations 
            // 1. Locked onto target

            // 2. Aiming
            if (_player._playerNetworkManager._isAiming.Value)
            {
                projectileGameObject.transform.LookAt(new Vector3(xPosition, yPosition, zPosition));
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
                // 3. Unlocked and not aiming
                else
                {
                    _player.transform.rotation = Quaternion.Euler(_player.transform.rotation.x, yCharacterRotation, _player.transform.rotation.z);
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
        }

    }
}