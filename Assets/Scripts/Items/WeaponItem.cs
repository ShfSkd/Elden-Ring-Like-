using SKD.Items.WeaponItems;
using System.Collections;
using UnityEngine;

namespace SKD.Items
{
    [CreateAssetMenu(menuName = "Items/Weapons/Melee Weapons")]
    public class WeaponItem : Item
    {
        // Animator controller overdrive (Change attack animations based on weapon you are currently using)
        [Header("Animations")]
        public AnimatorOverrideController _weaponAnimator;

        [Header("Model Instantiation")]
        public WeaponModelType _weaponModelType;

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
        // Light Attack Modifier
        public float _light_Attack_01_modifier  = 1.0f;
        public float _light_Attack_02_modifier = 1.2f;
        // Heavy Attack Modifier
        public float _heavy_Attack_01_modifier = 1.4f;
        public float _heavy_Attack_02_modifier = 1.6f;
        public float _charge_Attack_01_Modofier = 2.0f;
        public float _charge_Attack_02_Modofier = 2.2f;
        public float _running_Attack_01_Modifier = 1.1f;
        public float _rolling_Attack_01_Modifier = 1.1f;
        public float _backstep_Attack_01_Modifier = 1.1f;
        public float _heavyAttackStaminaCostMultiplier = 1.3f;
        public float _chargedAttackStaminaCostMultiplier = 1.5f;
        public float _runningAttackStaminaCostMultiplier = 1.1f;
        public float _rollingAttackStaminaCostMultiplier = 1.1f;
        public float _backstepAttackStaminaCostMultiplier = 1.1f;
        // Critical Damage Modifier etc..

        [Header("Stamina Cost Modifiers")]
        public int _baseStaminaCost = 20;
        public float _lightAttackStaminaCostMultiplier = 0.9f;
        // Running stamina cost modifier 
        // Light stamina cost modifier 
        // Heavy stamina cost modifier ETC

        [Header("Weapon Blocking Absorption")]
        public float _physicalBaseDamageAbsorption = 50f;
        public float _magicBaseDamageAbsorption = 50f;
        public float _fireBaseDamageAbsorption = 50f;
        public float _holyBaseDamageAbsorption = 50f;
        public float _lightingBaseDamageAbsorption = 50f;
        public float _stability = 50f; // Reduce Stamina lost from block

        // Item base Action(RB,RT,LB,LT)
        [Header("Actions")]
        public WeaponItemAction _keyboard_RB_Action;// One hand right bumper action
        public WeaponItemAction _keyboard_RT_Action;// One hand right trigger action
        public WeaponItemAction _keyboard_LB_Action;// One hand left bumper  action

        [Header("Whooshes")]
        public AudioClip[] _whooshes;


        // Ash of war 

        // Blocking sounds 

    }
}