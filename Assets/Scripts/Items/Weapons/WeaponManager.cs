using SKD.Character;
using SKD.Colliders;
using SKD.Items;
using System.Collections;
using System.Collections.Generic;
using SKD.Items.Weapons;
using UnityEngine;

namespace SKD.Weapons.Items
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider _meleeDamageCollider;

        private void Awake()
        {
            _meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }
        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            if (_meleeDamageCollider == null)
                return;

            _meleeDamageCollider._characterCausingDamage = characterWieldingWeapon;
            _meleeDamageCollider._physicalDamage = weapon._physicalDamage;
            _meleeDamageCollider._magicDamage = weapon._magicDamage;
            _meleeDamageCollider._fireDamage = weapon._fireDamage;
            _meleeDamageCollider._lightningDamage = weapon._lightningDamage;
            _meleeDamageCollider._holyDamage = weapon._holyDamage;
            _meleeDamageCollider._poiseDamage = weapon._posieDamage;

            _meleeDamageCollider._light_Attack_01_Modofier = weapon._light_Attack_01_modifier;
            _meleeDamageCollider._light_Attack_02_Modofier = weapon._light_Attack_02_modifier;
            _meleeDamageCollider._heavy_Attack_01_Modofier = weapon._heavy_Attack_01_modifier;
            _meleeDamageCollider._heavy_Attack_02_Modofier = weapon._heavy_Attack_02_modifier;
            _meleeDamageCollider._charge_Attack_01_Modofier = weapon._charge_Attack_01_Modofier;
            _meleeDamageCollider._charge_Attack_02_Modofier = weapon._charge_Attack_02_Modofier;
            _meleeDamageCollider._running_Attack_01_Modifier = weapon._running_Attack_01_Modifier;
            _meleeDamageCollider._rolling_Attack_01_Modifier = weapon._rolling_Attack_01_Modifier;
            _meleeDamageCollider._backstep_Attack_01_Modifier = weapon._backstep_Attack_01_Modifier;
            _meleeDamageCollider._light_Jump_Attack_01_Modofier = weapon._light_Jumping_Attack_01_modifier;
            _meleeDamageCollider._heavy_Jump_Attack_01_Modofier = weapon._heavy_Jumping_Attack_01_modifier;
        }
    }

}