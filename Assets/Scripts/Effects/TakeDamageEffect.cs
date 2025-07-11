﻿using SKD.Character;
using SKD.World_Manager;
using System.Collections;
using SKD.Character.AI_Character;
using UnityEngine;

namespace SKD.Effects
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effect/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager _characteCausingDamage;// If the damage is caused by another characters attacks it will be store here 

        [Header("Damage")]
        public float _physicalDamage;// In the future will split into "Standard", "Strike", "Slash" end Pierce

        public float _magicDamage;
        public float _fireDamage;
        public float _lightingDamage;
        public float _holyDamage;

        [Header("Final Damage")]
        protected int _finalDamageDealt;// The damage the character takes after all calculation have been set

        [Header("Animation")]
        public bool _playDamageAnimation = true;
        public bool _manualSelectDamageAnimation;
        public string _damageAnimation;

        [Header("Posie")]
        public float _poiseDamage;
        public bool _poiseIsBroken;// If the character poise is broken,  they will "Stunned" and play damage animation

        [Header("Sound FX")]
        public bool _willPlayDamageSFX = true;

        public AudioClip _elementalDamageSoundSfx;// Used on top of regular SFX there is elemental damage present(Magic/Fire/Lightning/Holy)

        [Header("Direction Damage Taken From")]
        public float _angleHitFrom;// Used to determine what damage animation to play (Move backwards, to the left/right.etc

        public Vector3 _constantPoint;// Used to determine where the blood FX instantiate)

        public override void ProcessesEffect(CharacterManager character)
        {
            if (character._characterNetworkManager._isInvulnerable.Value)
                return;

            base.ProcessesEffect(character);

            // If the character is dead no need to add more damage animations
            if (character._isDead.Value)
                return;

            // Check for "Invulnerably"
            CalculateDamage(character);
            PlayDirectionBasedDamageAnimation(character);
            // Check which direction damage came from
            // Play Damage Animation

            // Check for builds ups (Poison,Bleed etc)
            // Play Damage SFX
            PlayDamageSFX(character);
            // Play Damage VFX (Blood)
            PlayDamageVFX(character);
            
            // Run this after all other function that would attempt to play an animation upon being damaged & after poise/stance damage is calculated
            CalculateStanceDamage(character);

            // If character is A.I , check for new target if character causing damage is present
        }
        protected void CalculateStanceDamage(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;

            int stanceDamage = Mathf.RoundToInt(_poiseDamage);

            if (aiCharacter != null)
            {
                aiCharacter._aICharacterCombatManager.DamageStance(stanceDamage);
            }
        }
        protected virtual void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            if (_characteCausingDamage != null)
            {
                // Check for damage modifiers and modify base damage (Physical/Elemental,Damage Buff)
            }

            // Check character for flat defenses and subtract them from the damage 

            // Check for armor absorption, and subtract the percentage from the damage

            // Add all of the damage types together, and apply final damage
            _finalDamageDealt =
                Mathf.RoundToInt(_holyDamage + _fireDamage + _lightingDamage + _physicalDamage + _holyDamage);

            if (_finalDamageDealt <= 0)
            {
                _finalDamageDealt = 1;
            }

            character._characterNetworkManager._currentHealth.Value -= _finalDamageDealt;

            // Calculate Poise damage to determine if the character will be stunned
            character._characterCombatManager._previousPoiseDamageTaken = _poiseDamage;
            // We subject poise damage from the character total 
            character._characterStatsManager._totalPoiseDamage -= _poiseDamage;
            // We store the previous poise damage taken from the character total 
            character._characterCombatManager._previousPoiseDamageTaken = _poiseDamage;


            float remainingPoise = character._characterStatsManager._basePoiseDefence +
                                   character._characterStatsManager._offensivePoiseDamage +
                                   character._characterStatsManager._totalPoiseDamage;

            if (remainingPoise <= 0)
                _poiseIsBroken = true;

            //  Since the character has been hit reset the poise timer
            character._characterStatsManager._poiseResetTimer = character._characterStatsManager._defualtPoiseRestTimer;
        }

        protected void PlayDamageVFX(CharacterManager character)
        {
            // If we have fire/lightning/magic etc.. damage, play fire/lightning/magic etc
            character._characterEffectsManager.PlayBloodSplatterVFX(_constantPoint);
        }

        protected void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX =
                WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance._physicalDamageSFX);

            character._characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character._characterSoundFXManager.PlayDamageGrunts();
        }

        protected void PlayDirectionBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character._isDead.Value)
                return;

            if (_poiseIsBroken)
            {
                if (_angleHitFrom >= 145 && _angleHitFrom <= 180)
                {
                    // Play front animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._forward_Medium_Damage_List);
                }
                else if (_angleHitFrom <= -145 && _angleHitFrom >= -180)
                {
                    // Play front animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._forward_Medium_Damage_List);
                }
                else if (_angleHitFrom >= -45 && _angleHitFrom <= 45)
                {
                    // Play back animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._backeard_Medium_damage_List);
                }
                else if (_angleHitFrom >= -144 && _angleHitFrom <= -45)
                {
                    // Play left animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._left_Medium_damage_List);
                }
                else if (_angleHitFrom >= 45 && _angleHitFrom <= 144)
                {
                    // Play right animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager
                            ._right_Medium_damage_List);
                }
            }
            else
            {
                if (_angleHitFrom >= 145 && _angleHitFrom <= 180)
                {
                    // Play front animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._forward_Oing_Damage_List);
                }
                else if (_angleHitFrom <= -145 && _angleHitFrom >= -180)
                {
                    // Play front animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._forward_Ping_Damage_List);
                }
                else if (_angleHitFrom >= -45 && _angleHitFrom <= 45)
                {
                    // Play back animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._backeard_Ping_damage_List);
                }
                else if (_angleHitFrom >= -144 && _angleHitFrom <= -45)
                {
                    // Play left animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager._left_Ping_damage_List);
                }
                else if (_angleHitFrom >= 45 && _angleHitFrom <= 144)
                {
                    // Play right animation
                    _damageAnimation =
                        character._characterAnimationManager.GetRandomAnimationFromList(character
                            ._characterAnimationManager
                            ._right_Ping_damage_List);
                }
            }

            character._characterAnimationManager._lastDamageAnimationPlayed = _damageAnimation;

            if (_poiseIsBroken)
            {
                character._characterAnimationManager.PlayTargetActionAnimation(_damageAnimation, true);
                character._characterCombatManager.DestroyALlCurrentActionFX();
            }
            else
            {
                character._characterAnimationManager.PlayTargetActionAnimation(_damageAnimation, false, false, true, true);
            }
        }
    }
}