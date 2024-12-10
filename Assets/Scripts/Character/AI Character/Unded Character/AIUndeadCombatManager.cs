using SKD.Colliders;
using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character.UndeadCharacter
{
    public class AIUndeadCombatManager : AICharterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] UndeadHandDamageCollider _rightHandDamageCollider;
        [SerializeField] UndeadHandDamageCollider _leftHandDamageCollider;

        [Header("Damage")]
        [SerializeField] int _baseDamage = 25;
        [SerializeField] int _poiseBaseDamage = 25;
        [SerializeField] float _attack01DamageModifier = 1f;
        [SerializeField] float _attack02DamageModifier = 1.4f;
 
        public void SetAttack01Damage()
        {
            _rightHandDamageCollider._physicalDamage = _baseDamage * _attack01DamageModifier;
            _leftHandDamageCollider._physicalDamage = _baseDamage * _attack01DamageModifier;
            
            _rightHandDamageCollider._poiseDamage = _poiseBaseDamage * _attack01DamageModifier;
            _leftHandDamageCollider._poiseDamage = _poiseBaseDamage * _attack01DamageModifier;
        }
        public void SetAttack02Damage()
        {
            _rightHandDamageCollider._physicalDamage = _baseDamage * _attack02DamageModifier;
            _leftHandDamageCollider._physicalDamage = _baseDamage * _attack02DamageModifier;
            
            _rightHandDamageCollider._poiseDamage = _poiseBaseDamage * _attack02DamageModifier;
            _leftHandDamageCollider._poiseDamage = _poiseBaseDamage * _attack02DamageModifier;
        }
        public void OpenRightHandCollider()
        {
            _aICharacterManager._characterSoundFXManager.PlayAttackGrunt();
            _rightHandDamageCollider.EnableDamageCollider();
        }
        public void CloseRightHandCollider()
        {
            _rightHandDamageCollider.DisableDamageCollider();
        }
        public void OpenLeftHandCollider()
        {
            _aICharacterManager._characterSoundFXManager.PlayAttackGrunt();
            _leftHandDamageCollider.EnableDamageCollider();
        }
        public void CloseLeftHandCollider()
        {
            _leftHandDamageCollider.DisableDamageCollider();
        }
    }
}