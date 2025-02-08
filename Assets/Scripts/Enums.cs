using System.Collections;
using UnityEngine;

namespace SKD
{
    public class Enums : MonoBehaviour
    {
    
    }
    public enum CharacterSlot
    {
        CharacterSlot_01, CharacterSlot_02, CharacterSlot_03, CharacterSlot_04, CharacterSlot_05, CharacterSlot_06, CharacterSlot_07, CharacterSlot_08, CharacterSlot_09, CharacterSlot_10,No_Slot
    }
    public enum CharacterGroup
    {
        Team01,
        Team02
    }
    public enum WeaponModelSlot
    {
        RightHandWeaponSlot, LeftHandWeaponSlot,LeftHandShieldSlot,
        BackSlot
    }
    public enum WeaponModelType
    {
        Weapon,Shield
    }
    // This is used to calculate damage based on attack type
    public enum AttackType
    {
        LigthAttack01,
        LigthAttack02,
        HeavyAttack01,
        HeavyAttack02,
        ChargedAttack01,
        ChargedAttack02,
        RunningAttack01,
        RollingAttack01,
        BackstepAttack01
    }
    public enum DamageIntensity
    {
        Ping,Light,Meduim,Heavy,Colossal 
    }

    public enum WeaponClass
    {
        StraightSword,Spear,MediumShield,Fist
    }
}