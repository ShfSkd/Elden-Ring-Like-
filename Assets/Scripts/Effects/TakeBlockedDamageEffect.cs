using SKD.Character;
using SKD.World_Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.Effects
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effect/Take Blocked Damage")]
    public class TakeBlockedDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")] public CharacterManager
            _characteCausingDamage; // If the damage is caused by another characters attacks it will be store here 

        [Header("Damage")]
        public float _physicalDamage; // In the future will split into "Standard", "Strike", "Slash" end Pierce

        public float _magicDamage;
        public float _fireDamage;
        public float _lightingDamage;
        public float _holyDamage;

        [Header("Final Damage")]
        private int _finalDamageDealt; // The damage the character takes after all calculation have been set

        [Header("Animation")] public bool _playDamageAnimation = true;
        public bool _manualSelectDamageAnimation;
        public string _damageAnimation;

        [Header("Posie")] public float _poiseDamage;
        public bool _poiseIsBroken; // If the character poise is broken,  they will "Stunned" and play damage animation

        [FormerlySerializedAs("_baseStaminaDamage")] [Header("Stamina")]
        public float _staminaDamage;

        public float _finalStaminaDamage;

        [Header("Sound FX")] public bool _willPlayDamageSFX = true;

        public AudioClip
            _elementalDamageSoundSfx; // Used on top of regular SFX there is elemental damage present(Magic/Fire/Lightning/Holy)

        [Header("Direction Damage Taken From")]
        public float
            _angleHitFrom; // Used to determine what damage animation to play (Move backwards, to the left/right.etc

        public Vector3 _constantPoint; // Used to determine where the blood FX instantiate)

        public override void ProcessesEffect(CharacterManager character)
        {
            if (character._characterNetworkManager._isInvulnerable.Value)
                return;

            base.ProcessesEffect(character);

            Debug.Log("Hit Was Blocked ");
            // If the character is dead no need to add more damage animations
            if (character._isDead.Value)
                return;

            // Check for "Invulnerably"
            PlayDirectionBasedBlockingAnimation(character);
            CalculateDamage(character);
            CalculateStaminaDamage(character);
            // Check which direction damage came from
            // Play Damage Animation

            // Check for builds ups (Poison,Bleed etc)
            // Play Damage SFX
            PlayDamageSFX(character);
            // Play Damage VFX (Blood)
            PlayDamageVFX(character);

            // If character is A.I , check for new target if character causing damage is present

            CheckForGuardBreak(character);
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
            Debug.Log("Original physical damage " + _physicalDamage);

            _physicalDamage -= (_physicalDamage *
                                (characterManager._characterStatsManager._blockingPhysicalAbsorption / 100));
            _magicDamage -= _magicDamage * (characterManager._characterStatsManager._blockingMagicAbsorption / 100);
            _fireDamage -= _fireDamage * (characterManager._characterStatsManager._blockingFireAbsorption / 100);
            _lightingDamage -= _lightingDamage *
                               (characterManager._characterStatsManager._blockingLightningAbsorption / 100);
            _holyDamage -= _holyDamage * (characterManager._characterStatsManager._blockingHolyAbsorption / 100);

            _finalDamageDealt =
                Mathf.RoundToInt(_holyDamage + _fireDamage + _lightingDamage + _physicalDamage + _holyDamage);

            if (_finalDamageDealt <= 0)
            {
                _finalDamageDealt = 1;
            }

            Debug.Log("Final physical damage " + _physicalDamage);
            characterManager._characterNetworkManager._currentHealth.Value -= _finalDamageDealt;

            // Calculate Pose damage to determine if the character will be stunned
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            _finalStaminaDamage = _staminaDamage;

            float staminaDamageAbsorption =
                _finalStaminaDamage * (character._characterStatsManager._blockingStability / 100f);
            float staminaDamageAfterAbsorption = _finalStaminaDamage - staminaDamageAbsorption;

            character._characterNetworkManager._currentStamina.Value -= staminaDamageAfterAbsorption;
        }

        private void CheckForGuardBreak(CharacterManager character)
        {
            // Play SFX
            if (!character.IsOwner)
                return;

            if (character._characterNetworkManager._currentStamina.Value <= 0)
            {
                character._characterAnimationManager.PlayTargetActionAnimation("Guard_Break_01", true);
                character._characterNetworkManager._isBlocking.Value = false;
            }
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            // If we have fire/lightning/magic etc.. damage, play fire/lightning/magic etc
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            character._characterSoundFXManager.PlayBlockingSFX();
        }

        private void PlayDirectionBasedBlockingAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character._isDead.Value)
                return;

            DamageIntensity damageIntensity =
                WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(_poiseDamage);

            switch (damageIntensity)
            {
                case DamageIntensity.Ping:
                    _damageAnimation = "Block_Ping_01";
                    break;
                case DamageIntensity.Light:
                    _damageAnimation = "Block_Light_01";
                    break;
                case DamageIntensity.Meduim:
                    _damageAnimation = "Block_Meduim_01";
                    break;
                case DamageIntensity.Heavy:
                    _damageAnimation = "Block_Heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    _damageAnimation = "Block_Colossal_01";
                    break;
                default:
                    break;
            }

            if (_poiseIsBroken)
            {
                character._characterAnimationManager._lastDamageAnimationPlayed = _damageAnimation;
                character._characterAnimationManager.PlayTargetActionAnimation(_damageAnimation, true);
            }
        }
    }
}