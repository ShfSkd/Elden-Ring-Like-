﻿using System.Collections;
using SKD.Items;
using SKD.Items.Weapons;
using UnityEngine;

namespace SKD.Items.Weapons
{
    public class MeleeWeaponItem : WeaponItem
    {
        [Header("Attack Modifiers")]
        public float _riposte_Attack_01_Modifier = 3.3f;
        public float _backstab_Attack_01_Modifier;
        // Weapon "Deflection" (If the weapon will bounce off another weapon when it is being guarded against) 
        // Can be buffed


    }
}