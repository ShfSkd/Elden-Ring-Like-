using SKD.Character;
using UnityEngine;
namespace SKD.Effects
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Critical Damage Effect")]
    public class TakeCriticalDamageEffect : TakeDamageEffect
    {
        public override void ProcessesEffect(CharacterManager character)
        {
            if (character._characterNetworkManager._isInvulnerable.Value)
                return;

            // If the character is dead no need to add more damage animations
            if (character._isDead.Value)
                return;
            
            CalculateDamage(character);

            character._characterCombatManager._pendingCriticalDamage = _finalDamageDealt;
        }
        protected override void CalculateDamage(CharacterManager character)
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
    }
}