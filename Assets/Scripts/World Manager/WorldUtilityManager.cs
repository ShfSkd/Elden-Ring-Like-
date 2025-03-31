using SKD.Character;
using System;
using System.Collections;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldUtilityManager : MonoBehaviour
    {
        private static WorldUtilityManager instance;
        public static WorldUtilityManager Instance { get { return instance; } }

        [Header("Layers")]
        [SerializeField] LayerMask _characterLayers;
        [SerializeField] LayerMask _enviroLayers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(gameObject);
            }

        }

        public LayerMask GetCharacterLayers()
        {
            return _characterLayers;
        }
        public LayerMask GetEnviroLayers()
        {
            return _enviroLayers;
        }
        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01:
                        return false;
                    case CharacterGroup.Team02:
                        return true;
                    default:
                        break;

                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01:
                        return true;  
                    case CharacterGroup.Team02:
                        return false;
                    default:
                        break;

                }
            }
            return false;
        }

        public float GetAngleOfTarget(Transform characterTransform , Vector3 targetDirection)
        {
           targetDirection.y = 0f;
            float vieableAngle = Vector3.Angle(characterTransform.forward, targetDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);

            if (cross.y < 0)
                vieableAngle = -vieableAngle;

            return vieableAngle; 
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            if (poiseDamage >= 10)
                damageIntensity = DamageIntensity.Light;

            if (poiseDamage >= 30)
                damageIntensity = DamageIntensity.Meduim;

            if (poiseDamage >= 70)
                damageIntensity = DamageIntensity.Heavy;

            if (poiseDamage >= 120)
                damageIntensity = DamageIntensity.Colossal;


            return damageIntensity;
        }

        public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.32f, 0f, 0.74f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword:// change position here 
                    break;
                case WeaponClass.Spear:// change position here 
                    break;
                case WeaponClass.MediumShield:// change position here 
                    break;
                case WeaponClass.Fist:// change position here 
                    break;
                default:
                    break;
            }
            return position;
        }
        public Vector3 GetBackstapPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.12f, 0f, 0.74f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword:// change position here 
                    break;
                case WeaponClass.Spear:// change position here 
                    break;
                case WeaponClass.MediumShield:// change position here 
                    break;
                case WeaponClass.Fist:// change position here 
                    break;
                default:
                    break;
            }
            return position;
        }
    }
}