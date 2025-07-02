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
    public enum EquipmentModelType
    {
        FullHelmet,Hat,Hood,HelmetAcessorie,FaceCover,Torso,Back,RightShoulder,
        RightUpperArm,RightElbow,RightLowerArm,RightHand,LeftShoulder,LeftUpperArm,LeftElbow,LeftLowerArm,LeftHand,
        Hips,HipsAttachment,RightLeg,RightKnee,LeftLeg,LeftKnee
    }
    public enum EquipmentType
    {
       RightWeapon01,RightWeapon02,RightWeapon03, // 0-2
       LeftWeapon01,LeftWeapon02,LeftWeapon03, // 3-5
       Head,Body,Legs,Hands, // 6-9 
       MainProjectile,SecondaryProjectile // 10-11
       ,QuickSlot01,QuickSlot02,QuickSlot03 //12-14
    }
    public enum HeadEquipmentType
    {
        FullHelmet,
        Hat,
        Hood,
        FaceCover
    }
    // This is used to calculate damage based on attack type
    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
        HeavyAttack01,
        HeavyAttack02,
        ChargedAttack01,
        ChargedAttack02,
        RunningAttack01,
        RollingAttack01,
        BackstepAttack01,
        LightJumpingAttack01,
        HeavyJumpAttack01,
    }
    public enum DamageIntensity
    {
        Ping,Light,Meduim,Heavy,Colossal 
    }
    public enum WeaponClass
    {
        StraightSword,Spear,MediumShield,Fist,LightShield,Bow
    }
    public enum SpellClass
    {
        Incantation,Sorcery
    }
    public enum ProjectileClass
    {
        Arrow,Bolt
    }
    public enum ProjectileSlot
    {
        Main,Secondary
    }
    public enum ItemPickUpType
    {
        WorldSpawn,CharacterDrop,Drop
    }
}