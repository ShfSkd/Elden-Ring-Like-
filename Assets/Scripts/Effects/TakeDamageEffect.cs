using SKD.Character;
using System.Collections;
using UnityEngine;

namespace SKD.Effects
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effect/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager _characteCausingDamage;// If the damage is caused by another characters attacks it will be store here 

        [Header("Damage")]
        public float _physicalDamage; // In the future will split into "Standard", "Strike", "Slash" end Pierce
        public float _magicDamage;
        public float _fireDamage;
        public float _lightnigamage;
        public float _holyDamage;

        [Header("Final Damage")]
        private int _finalDamageDealt; // The damage the character takes after all calculation have been set

        [Header("Animation")]
        public bool _playDamageAnimation = true;
        public bool _manualySelectDamageAnimation;
        public string _damageAnimation;

        [Header("Posie")]
        public float _poiseDamage;
        public bool _poiseIsBroken;// If the character poise is broken,  they will "Stunned" and play damage animation

        [Header("Sound FX")]
        public bool _willPlayDamageSFX = true;
        public AudioClip _elementalDamageSoundSfx; // Used on top of regular SFX there is elemental damage present(Magic/Fire/Lightning/Holy)
        [Header("Direction Damage Taken From")]
        public float _angleHitFrom; // Used to determine what damage animation to play (Move backwards, to the left/right.etc
        public Vector3 _contantPoint; // Used to determine where the blood FX instantiate)

        public override void ProccesEffect(CharacterManager characterManager)
        {
            base.ProccesEffect(characterManager);

            // If the character is dead no need to add more damage animations
            if (characterManager._isDead.Value)
                return;

            // Check for "Invulnerably"

            CalculateDamage(characterManager);
            // Check which direction damage came from
            // Play Damage Animation
            // Check for builds ups (Poison,Bleed etc)
            // Play Damage SFX
            // Play Damage VFX (Blood)

            // If character is A.I , check for new target if character causing damage is present

        }
        private void CalculateDamage(CharacterManager characterManager)
        {
            if (!characterManager.IsOwner)
                return;

            if (_characteCausingDamage != null)
            {
                // Check for damage modifiers and modify base damage (Physical/Elemental,Damage Buff)
            }

            // Check character for flat defenses and subtract them from the damage 

            // Check for armor absorption, and subtract the percentage from the damage

            // Add all of the damage types together, and apply final damage
            _finalDamageDealt = Mathf.RoundToInt(_holyDamage + _fireDamage + _lightnigamage + _physicalDamage + _holyDamage);

            if (_finalDamageDealt <= 0)
            {
                _finalDamageDealt = 1;
            }

            Debug.Log("Final Damage Given: "+_finalDamageDealt);
            characterManager._characterNetworkManager._currentHealth.Value -= _finalDamageDealt;

            // Calculate Pose damage to determine if the character will be stunned
        }
    }
}