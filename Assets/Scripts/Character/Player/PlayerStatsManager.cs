using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager _player;
        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<PlayerManager>();
        }
        protected override void Start()
        {
            base.Start();

            // Why calculate it here? When we make a character creation menu, and set the stats depending on the class , this will be calculate there. Until then however, stats are never calculate, so we do it here on start. If a save file exist they will be over written when loading into a scene 
            CalculateHealthBasedOnVitalityLevel(_player._playerNetworkManager._vitality.Value);
            CalculateStaminaBasedOnEnduraceLevel(_player._playerNetworkManager._endurance.Value);
            CalculateFucosPointsBasedOnMindLevel(_player._playerNetworkManager._mind.Value);

        }
        public void CalculateTotalArmorAbsorption()
        {
            // Reset all values to 0 
            _armorPhysicalDamageAbsorption = 0;
            _armorMagicDamageAbsorption = 0;
            _armorFireDamageAbsorption = 0;
            _armorHolyDamageAbsorption = 0;
            _armorLigthningDamageAbsorption = 0;

            _armorRobustness = 0;
            _armorVitsality = 0;
            _armorImmunity = 0;
            _armorFocus = 0;

            _basePoiseDefence = 0;

            if (_player._playerInventoryManager._headEquipment != null)
            {
                // Damage Resistance
                _armorPhysicalDamageAbsorption += _player._playerInventoryManager._headEquipment._physicalDamageAbsorption;
                _armorMagicDamageAbsorption += _player._playerInventoryManager._headEquipment._magicDamageAbsorption;
                _armorFireDamageAbsorption += _player._playerInventoryManager._headEquipment._fireDamageAbsorption;
                _armorLigthningDamageAbsorption += _player._playerInventoryManager._headEquipment._ligthningDamageAbsorption;
                _armorHolyDamageAbsorption += _player._playerInventoryManager._headEquipment._holyDamageAbsorption;
                
                // Status effect resistance
                _armorRobustness += _player._playerInventoryManager._headEquipment._robustness;
                _armorVitsality+= _player._playerInventoryManager._headEquipment._vitsality;
                _armorImmunity += _player._playerInventoryManager._headEquipment._immunity;
                _armorFocus += _player._playerInventoryManager._headEquipment._focus;
                
                // Poise
                _basePoiseDefence += _player._playerInventoryManager._headEquipment._poise;
            }
            if (_player._playerInventoryManager._bodyEquipment != null)
            {
                // Damage Resistance
                _armorPhysicalDamageAbsorption += _player._playerInventoryManager._bodyEquipment._physicalDamageAbsorption;
                _armorMagicDamageAbsorption += _player._playerInventoryManager._bodyEquipment._magicDamageAbsorption;
                _armorFireDamageAbsorption += _player._playerInventoryManager._bodyEquipment._fireDamageAbsorption;
                _armorLigthningDamageAbsorption += _player._playerInventoryManager._bodyEquipment._ligthningDamageAbsorption;
                _armorHolyDamageAbsorption += _player._playerInventoryManager._bodyEquipment._holyDamageAbsorption;
                
                // Status effect resistance
                _armorRobustness += _player._playerInventoryManager._bodyEquipment._robustness;
                _armorVitsality+= _player._playerInventoryManager._bodyEquipment._vitsality;
                _armorImmunity += _player._playerInventoryManager._bodyEquipment._immunity;
                _armorFocus += _player._playerInventoryManager._bodyEquipment._focus;
                
                // Poise
                _basePoiseDefence += _player._playerInventoryManager._bodyEquipment._poise;
            }
            if (_player._playerInventoryManager._legEquipment != null)
            {
                // Damage Resistance
                _armorPhysicalDamageAbsorption += _player._playerInventoryManager._legEquipment._physicalDamageAbsorption;
                _armorMagicDamageAbsorption += _player._playerInventoryManager._legEquipment._magicDamageAbsorption;
                _armorFireDamageAbsorption += _player._playerInventoryManager._legEquipment._fireDamageAbsorption;
                _armorLigthningDamageAbsorption += _player._playerInventoryManager._legEquipment._ligthningDamageAbsorption;
                _armorHolyDamageAbsorption += _player._playerInventoryManager._legEquipment._holyDamageAbsorption;
                
                // Status effect resistance
                _armorRobustness += _player._playerInventoryManager._legEquipment._robustness;
                _armorVitsality+= _player._playerInventoryManager._legEquipment._vitsality;
                _armorImmunity += _player._playerInventoryManager._legEquipment._immunity;
                _armorFocus += _player._playerInventoryManager._legEquipment._focus;
                
                // Poise
                _basePoiseDefence += _player._playerInventoryManager._legEquipment._poise;
            }
            if (_player._playerInventoryManager._handEquipment != null)
            {
                // Damage Resistance
                _armorPhysicalDamageAbsorption += _player._playerInventoryManager._handEquipment._physicalDamageAbsorption;
                _armorMagicDamageAbsorption += _player._playerInventoryManager._handEquipment._magicDamageAbsorption;
                _armorFireDamageAbsorption += _player._playerInventoryManager._handEquipment._fireDamageAbsorption;
                _armorLigthningDamageAbsorption += _player._playerInventoryManager._handEquipment._ligthningDamageAbsorption;
                _armorHolyDamageAbsorption += _player._playerInventoryManager._handEquipment._holyDamageAbsorption;
                
                // Status effect resistance
                _armorRobustness += _player._playerInventoryManager._handEquipment._robustness;
                _armorVitsality+= _player._playerInventoryManager._handEquipment._vitsality;
                _armorImmunity += _player._playerInventoryManager._handEquipment._immunity;
                _armorFocus += _player._playerInventoryManager._handEquipment._focus;
                
                // Poise
                _basePoiseDefence += _player._playerInventoryManager._handEquipment._poise;
            }

        }
    }
}