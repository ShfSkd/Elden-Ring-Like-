﻿using System.Collections;
using System.Globalization;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager _characterManager;
        [Header("Stamina Regeneration")]
        [SerializeField] float _staminaRegenAmount = 2f;
        private float _staminaRegeneartionTimer = 0f;
        private float _staminaTickTimer = 0f;
        [SerializeField] float _staminaRegenarationDelay = 2f;

        [Header("Blocking Absorptions")]
        public float _blockingPhysicalAbsorption;
        public float _blockingFireAbsorption;
        public float _blockingMagicAbsorption;
        public float _blockingLightningAbsorption;
        public float _blockingHolyAbsorption;
        public float _stabiltyRating;
        public float _blockingStability;
        
        [Header("Armor Absorption")]
        [Header("Equipment Absorption Bonus")]
        public float _armorPhysicalDamageAbsorption;
        public float _armorMagicDamageAbsorption;
        public float _armorFireDamageAbsorption;
        public float _armorLigthningDamageAbsorption;
        public float _armorHolyDamageAbsorption;

        [Header("Armor Resistances")]
        public float _armorImmunity;// Resistance to ROT and poision
        public float _armorRobustness;// Resistance to bleed and frost 
        public float _armorFocus;// Resistance to madness and sleep
        public float _armorVitsality;// Resistance to death course
        
        [Header("Poise")]
        public float _totalPoiseDamage; // how much poise damage we have taken
        public float _offensivePoiseDamage; // The posie bonus gained from using weapons (heavy weapon or much large bonus)
        public float _basePoiseDefence;    // The poise bonus gained from armor/talisman etc..
        public float _defualtPoiseRestTimer = 8f; // The time it takes for poise to reset (must not be hit in time or will reset)
        public float _poiseResetTimer;


        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {
            HandlePoiseResetTimer();    
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }
        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = vitality * 15;
            return Mathf.RoundToInt(health);
        }   
        public int CalculateStaminaBasedOnEnduraceLevel(int endurance)
        {
            float stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }
        public virtual void RegenerateStamina()
        {
            // Only owners can edit their network variables 
            if (!_characterManager.IsOwner)
                return;

            // We do not want to regenerate stamina while sprinting
            if (_characterManager._characterNetworkManager._isSprinting.Value)
                return;

            if (_characterManager._isPerformingAction)
                return;

            _staminaRegeneartionTimer += Time.deltaTime;

            if (_staminaRegeneartionTimer >= _staminaRegenarationDelay)
            {
                if (_characterManager._characterNetworkManager._currentStamina.Value < _characterManager._characterNetworkManager._maxStamina.Value)
                {
                    _staminaTickTimer += Time.deltaTime;

                    if (_staminaTickTimer >= 0.1f)
                    {
                        _staminaTickTimer = 0;
                        _characterManager._characterNetworkManager._currentStamina.Value += _staminaRegenAmount;
                    }
                }
            }
        }
        public virtual void ResetStaminaReganTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // We only want to reset the regeneration if the action used stamina
            // We don't want to reset the regeneration if we are already regenerating stamina
            if (currentStaminaAmount < previousStaminaAmount)
                _staminaRegeneartionTimer = 0;
        }

        protected virtual void HandlePoiseResetTimer()
        {
            if (_poiseResetTimer > 0)
                _poiseResetTimer -= Time.deltaTime;
            else
                _totalPoiseDamage = 0;
        }
    }
}