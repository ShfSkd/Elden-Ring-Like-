using UnityEngine;
using UnityEngine.AI;

namespace SKD.Character.AI_Character
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterNetworkManager _aICharacterNetworkManager;
        [HideInInspector] public AICharcterCombatManager _aICharcterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager _aICharacterLocomotionManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent _navMeshAgent;

        [Header("Current State")]
        [SerializeField] AIState _currentState;

        [Header("States")]
        public IdleState _idle;
        public PursueTargetState _pursueTarget;
        protected override void Awake()
        {
            base.Awake();
            _aICharcterCombatManager = GetComponent<AICharcterCombatManager>();
            _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            _aICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            _aICharacterLocomotionManager =GetComponent<AICharacterLocomotionManager>(); 

            _idle = Instantiate(_idle);
            _pursueTarget = Instantiate(_pursueTarget);

            _currentState = _idle;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ProcessStateMachine();
        }
        private void ProcessStateMachine()
        {
            AIState nextStat = _currentState?.Tick(this);

            if (nextStat != null)
            {
                _currentState = nextStat;
            }

            // The position/rotation should be reset only after the state machine was processed its tick
            _navMeshAgent.transform.localPosition = Vector3.zero;
            _navMeshAgent.transform.localRotation = Quaternion.identity;

            if (_navMeshAgent.enabled)
            {
                Vector3 agentDestination = _navMeshAgent.destination;
                float remainigDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainigDistance > _navMeshAgent.stoppingDistance)
                {
                    _aICharacterNetworkManager._isMoving.Value = true;
                }
                else
                {
                    _aICharacterNetworkManager._isMoving.Value = false;
                }
            }
            else
            {
                _aICharacterNetworkManager._isMoving.Value = false;
            }
        }
    }
}