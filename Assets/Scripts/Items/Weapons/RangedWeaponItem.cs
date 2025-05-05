using SKD.Items.Weapons;
using UnityEngine;
namespace Items.Weapons
{
    [CreateAssetMenu(menuName = "Items/Weapons/Ranged Weapon")]
    public class RangedWeaponItem : WeaponItem
    {
        [Header("Ranged SFX")]
        public AudioClip[] _drawSounds;
        public AudioClip[] _releaseSounds;
    }
}