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
        public bool CanIDamageThisTarget(CharacterGruop attackingCharacter, CharacterGruop targetCharacter)
        {
            if (attackingCharacter == CharacterGruop.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGruop.Team01:
                        return false;
                    case CharacterGruop.Team02:
                        return true;
                    default:
                        break;

                }
            }
            else if (attackingCharacter == CharacterGruop.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGruop.Team01:
                        return true;  
                    case CharacterGruop.Team02:
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
    }
}