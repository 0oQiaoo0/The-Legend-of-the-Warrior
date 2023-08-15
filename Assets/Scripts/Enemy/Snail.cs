using Utilities;

namespace Enemy
{
    public class Snail : GroundEnemy
    {
        private SnailState _currentState;
        private SnailState _patrolState;
        private SnailState _skillState;
        protected override void Awake()
        {
            base.Awake();
            _patrolState = new SnailPatrolState();
            _skillState = new SnailSkillState();
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
                NPCState.Skill => _skillState,
                _ => null
            };

            _currentState.OnExit();
            _currentState = newState;
            _currentState?.OnEnter(this);
        }
    }
}
