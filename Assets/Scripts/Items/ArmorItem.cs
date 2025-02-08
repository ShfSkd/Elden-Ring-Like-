using UnityEngine;
namespace SKD.Items
{
    public class ArmorItem : EquipmentItem
    {
        [Header("Equipment Absorption Bonus")]
        public float _physicalDamageAbsorption;
        public float _magicDamageAbsorption;
        public float _fireDamageAbsorption;
        public float _ligthningDamageAbsorption;
        public float _holyDamageAbsorption;

        [Header("Equipment Resistance Bonus")]
        public float _immunity;// Resistance to ROT and poision
        public float _robustness;// Resistance to bleed and frost 
        public float _focus;// Resistance to madness and sleep
        public float _vitsality;// Resistance to death course

        [Header("Poise")]
        public float _poise;

        // Armor models 

    }
}