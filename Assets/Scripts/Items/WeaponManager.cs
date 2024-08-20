using SKD.Character;
using SKD.Colliders;
using SKD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Items
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider _meleeDamageCollider;

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
            _meleeDamageCollider._lightningDamage = weapon._lightningDamage;
            _meleeDamageCollider._holyDamage = weapon._holyDamage;

            _meleeDamageCollider._light_Attack_01_Modofier = weapon._light_Attack_01_modifier;
            _meleeDamageCollider._light_Attack_02_Modofier = weapon._light_Attack_02_modifier;
            _meleeDamageCollider._heavy_Attack_01_Modofier = weapon._heavy_Attack_01_modifier;
            _meleeDamageCollider._heavy_Attack_02_Modofier = weapon._heavy_Attack_02_modifier;
            _meleeDamageCollider._charge_Attack_01_Modofier = weapon._charge_Attack_01_Modofier;
            _meleeDamageCollider._charge_Attack_02_Modofier = weapon._charge_Attack_02_Modofier;
        }
    }

}
