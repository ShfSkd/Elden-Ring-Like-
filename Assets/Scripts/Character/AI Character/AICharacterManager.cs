using SKD.Character.AI_Character.States;
using SKD.World_Manager;
using UnityEngine;
using UnityEngine.AI;

namespace SKD.Character.AI_Character
{
    public class AICharacterManager : CharacterManager
    {
        [Header("Character Name")]
        public string _characterName = "";

        [HideInInspector] public AICharacterNetworkManager _aICharacterNetworkManager;
        [HideInInspector] public AICharterCombatManager _aICharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager _aICharacterLocomotionManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent _navMeshAgent;

        [Header("Current State")]
        [SerializeField] protected AIState _currentState;

        [Header("States")]
        public IdleState _idle;
        public PursueTargetState _pursueTarget;
        public CombatStanceState _combatStance;
        public AttackState _attack;

        protected override void Awake()
        {
            base.Awake();

            _aICharacterCombatManager = GetComponent<AICharterCombatManager>();
            _aICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            _aICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            _navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        }
        protected override void OnEnable()
        {
            base.OnEnable();

            if(_characterUIManager._hasFloatingHPBar)
            _characterNetworkManager._currentHealth.OnValueChanged += _characterUIManager.OnHPChanged;
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            if(_characterUIManager._hasFloatingHPBar)
            _characterNetworkManager._currentHealth.OnValueChanged -= _characterUIManager.OnHPChanged;
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                _idle = Instantiate(_idle);
                _pursueTarget = Instantiate(_pursueTarget);
                _combatStance = Instantiate(_combatStance);
                _attack = Instantiate(_attack);

                _currentState = _idle;
            }
            _aICharacterNetworkManager._currentHealth.OnValueChanged += _aICharacterNetworkManager.CheckHP;

        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _aICharacterNetworkManager._currentHealth.OnValueChanged -= _aICharacterNetworkManager.CheckHP;
        }
        protected override void Update()
        {
            base.Update();
            _aICharacterCombatManager.HandleActionRecovery(this);
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsOwner)
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

            if (_aICharacterCombatManager._currentTarget != null)
            {
                _aICharacterCombatManager._targetDirection = _aICharacterCombatManager._currentTarget.transform.position - transform.position;
                _aICharacterCombatManager._viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, _aICharacterCombatManager._targetDirection);
                _aICharacterCombatManager._distanceFromTarget = Vector3.Distance(transform.position, _aICharacterCombatManager._currentTarget.transform.position);
            }

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