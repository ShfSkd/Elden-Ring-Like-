using SKD.World_Manager;
using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AICharcterCombatManager : CharacterCombatManager
    {
        [Header("Detection")]
        [SerializeField] float _detectionRaduis = 15f;
        [SerializeField] float _minimumDetectaionAngle = -35f;
        [SerializeField] float _maxuimumDetectaionAngle = 35f;
        public void FindATargetViaLineOfSight(AICharacterManager aICharacter)
        {
            if (_currentTarget != null)
                return;

            Collider[] colliders = Physics.OverlapSphere(aICharacter.transform.position, _detectionRaduis, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                    continue;

                if (targetCharacter == aICharacter)
                    continue;

                if (targetCharacter._isDead.Value)
                    continue;

                // Can i even this character, is so, make them my target 
                if (WorldUtilityManager.Instance.CanIDamageThisTarget(aICharacter._characterGruop, targetCharacter._characterGruop))
                {
                    // If a potential target is found, it has to be in front of us 
                    Vector3 targetDirection = targetCharacter.transform.position - aICharacter.transform.position;
                    float viewAbleAngle = Vector3.Angle(targetDirection, aICharacter.transform.forward);

                    if (viewAbleAngle > _minimumDetectaionAngle && viewAbleAngle < _maxuimumDetectaionAngle)
                    {
                        //Lastly,we check for enviro blocks
                        if (Physics.Linecast(aICharacter._characterCombatManager._lockOnTransform.position, targetCharacter._characterCombatManager._lockOnTransform.position, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aICharacter._characterCombatManager._lockOnTransform.position, targetCharacter._characterCombatManager._lockOnTransform.position);
                            Debug.Log("Blocked");
                        }
                        else
                        {
                            aICharacter._characterCombatManager.SetTarget(targetCharacter);
                        }
                    }
                }
            }
        }
    }
}    