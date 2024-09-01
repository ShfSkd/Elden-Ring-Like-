using SKD.Character.Player;
using SKD.Colliders;
using SKD.Effects;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Character.AI_Character.Durk
{
    public class AIDurkCombatManager : AICharterCombatManager
    {
        AIDurkCharacterManager _durkCharacterManager;

        [Header("Damage Collider")]
        [SerializeField] DurkClubDamageCollider _clubDamageCollider;
        [SerializeField] DurkStompCollider _durkStompCollider;
        public float _stompAttackAOERadius = 1.5f;

        [Header("Damage")]
        [SerializeField] int _baseDamage = 25;
        [SerializeField] float _attack01DamageModifier = 1f;
        [SerializeField] float _attack02DamageModifier = 1.4f;
        [SerializeField] float _attack03DamageModifier = 1.6f;
        [SerializeField] float _stompDamage = 25f;

        [Header("VFX")]
        public GameObject _durkImpactVFX;

        protected override void Awake()
        {
            base.Awake();

            _durkCharacterManager = GetComponent<AIDurkCharacterManager>();
        }
        public void SetAttack01Damage()
        {
            _aICharacterManager._characterSoundFXManager.PlayAttackGrunt();
            _clubDamageCollider._physicalDamage = _baseDamage * _attack01DamageModifier;
        }
        public void SetAttack02Damage()
        {
            _aICharacterManager._characterSoundFXManager.PlayAttackGrunt();
            _clubDamageCollider._physicalDamage = _baseDamage * _attack02DamageModifier;
        }
        public void SetAttack03Damage()
        {
            _aICharacterManager._characterSoundFXManager.PlayAttackGrunt();
            _clubDamageCollider._physicalDamage = _baseDamage * _attack03DamageModifier;
        }
        public void OpenClubDamageCollider()
        {
            _clubDamageCollider.EnableDamageCollider();
            _durkCharacterManager._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_durkCharacterManager._aIDurkSoundFXManager._clubWhooshes));

        }
        public void CloseClubDamageCollider()
        {
            _clubDamageCollider.DisableDamageCollider();
        }
        public void ActivateDurkStomp()
        {
            _durkStompCollider.StompAttack();
        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            // Play A pivot animation depending on viewable angle of target
            if (aiCharacter._isPerformingAction)
                return;

            if (_viewableAngle >= 61 && _viewableAngle <= 110)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Right Turn 90", true);

            else if (_viewableAngle <= -61 && _viewableAngle >= -110)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Left Turn 90", true);

            else if (_viewableAngle >= 146 && _viewableAngle <= 180)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Right Turn 180", true);

            else if (_viewableAngle <= -146 && _viewableAngle >= -180)
                aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Left Turn 180", true);
        }
        public float GetStompDamage()
        {
            return _stompDamage;
        }
    }
}