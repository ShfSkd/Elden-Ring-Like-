using SKD.Character;
using SKD.Colliders;
using SKD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] MeleeWeaponDamageCollider _meleeDamageCollider;

        private void Awake()
        {
            _meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        } 
        public void SetWeaponDamage(CharacterManager characterWieldingWepaon, WeaponItem weapon)
        {
            _meleeDamageCollider._characterCausingDamage = characterWieldingWepaon;
            _meleeDamageCollider._physicalDamage = weapon._physicalDamage;
            _meleeDamageCollider._magicDamage = weapon._magicDamage;
            _meleeDamageCollider._fireDamage = weapon._fireDamage;
            _meleeDamageCollider._lightnigamage = weapon._lightningDamage;
            _meleeDamageCollider._holyDamage = weapon._holyDamage;
        }
    }

}
