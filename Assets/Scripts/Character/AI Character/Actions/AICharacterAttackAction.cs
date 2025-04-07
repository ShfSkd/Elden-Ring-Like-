using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Attack Action")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField] string _attackAnimation;
        [SerializeField] bool _isParryable = true;

        [Header("Combo Action")]
        public AICharacterAttackAction _comboAction;// The combo action of this attack action 

        [Header("Action Values")]
        public int _attackWeight = 50;
        [SerializeField] AttackType _attackType;
        public float _actionRecoveryTime = 1.5f;// The time before the character can make another attack after performing this one 
        public float _minimumAttackAngle = -35f;
        public float _maximumAttackAngle = 35f;
        public float _minimumAttackDistance = 0f;
        public float _maximumAttackDistance = 2f;

        public void AttempToPerformAction(AICharacterManager aICharacter)
        {
            aICharacter._characterAnimationManager.PlayTargetActionAnimation(_attackAnimation, true);
          
            aICharacter._aICharacterNetworkManager._isParryable.Value = _isParryable;
        }
    }
}