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
    public enum WeaponModelSlot
    {
        RightHand, LeftHand
    }
    // This is used to calculate damage based on attack type
    public enum AttackType
    {
        LigthAttack01,
        LigthAttack02,
        HeavyAttack01,
        HeavyAttack02,
        ChargedAttack01,
        ChargedAttack02
    }
}