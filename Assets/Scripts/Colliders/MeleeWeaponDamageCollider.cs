using SKD.Character;
using System.Collections;
using UnityEngine;

namespace SKD.Colliders
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager _characterCausingDamage; // When calculation damage is used to check for attackers damage, modifiers etc
    }
}