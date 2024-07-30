using System.Collections;
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

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
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

            if (_characterManager._isPerfomingAction)
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
    }
}