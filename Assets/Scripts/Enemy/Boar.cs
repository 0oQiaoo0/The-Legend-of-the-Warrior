using Utilities;

namespace Enemy
{
    public class Boar : GroundEnemy
    {
        private BoarState _currentState;
        private BoarState _patrolState;
        private BoarState _chaseState;
    
        protected override void Awake()
        {
            base.Awake();
            _patrolState = new BoarPatrolState();
            _chaseState = new BoarChaseState();
        }
        #region 状态相关
        protected override void OnEnable()
        {
            _currentState = _patrolState;
            _currentState.OnEnter(this);
        }
        protected override void Update()
        {
            _currentState.LogicUpdate();
            TimeCounter();
        }
        protected override void FixedUpdate()
        {
            if (!isWait && !isHurt && !isDead)
                _currentState.PhysicsUpdate();
        }
        protected override void OnDisable()
        {
            _currentState.OnExit();
        }
        #endregion
        public override void SwitchState(NPCState state)
        {
            var newState = state switch
            {
                NPCState.Patrol => _patrolState,
                NPCState.Chase => _chaseState,
                _ => null
            };

            _currentState.OnExit();
            _currentState = newState;
            _currentState?.OnEnter(this);
        }

    
    }
}
