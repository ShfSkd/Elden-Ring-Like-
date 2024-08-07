using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;

namespace SKD.Items
{
    [CreateAssetMenu(menuName ="Items/Weapons/Melee Weapons")]
    public class WeaponItem : Item
    {
        // Animator controller overdrive (Change attack animations based on weapon you are currently using)

        [Header("Weapon Model")]
        public GameObject _weaponModel;

        [Header("Weapon Requirements")]
        public int _weaponRequirement = 0;
        public int _dexRequirement = 0; // Dexterity- skill in performing tasks, especially with the hands. 
        public int _intelligentRequirement = 0;
        public int _faitRequirement = 0;

        [Header("Weapon Base Damage")]
        public int _physicalDamage = 0;
        public int _magicDamage = 0;
        public int _fireDamage = 0;
        public int _holyDamage = 0;
        public int _lightningDamage = 0;

        // Weapon guard absorptions(Blocking power)

        [Header("Weapon Base Poise Damage")]
        public float _posieDamage = 10f;
        // Offensive Poise bonus when attacking 

        // Weapon Modifier
        [Header("Attack Modifiers")]
        public float _light_Attack_01_modifier  = 1.1f;
        // Light Attack Modifier
        // Heavy Attack Modifier
        // Critical Damage Modifier etc..

        [Header("Stamina Cost Modifiers")]
        public int _baseStaminaCost = 20;
        public float _lightAttackStaminaCostMultiplier = 0.9f;
        // Running stamina cost modifier 
        // Light stamina cost modifier 
        // Heavy stamina cost modifier ETC

        // Item base Action(RB,RT,LB,LT)
        [Header("Actions")]
        public WeaponItemAction _keyboard_RB_Action;// One hand right bumper action


        // Ash of war 

        // Blocking sounds 

    }
}