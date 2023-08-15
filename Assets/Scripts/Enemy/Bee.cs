using General;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Enemy
{
    public class Bee : Enemy
    {
        [HideInInspector] public Attack attack;

        private BeeState _currentState;
        private BeeState _patrolState;
        private BeeState _chaseState;
        [Header("移动范围")]
        [SerializeField] private Vector3 spawnPoint;
        [FormerlySerializedAs("partolRadius")] [SerializeField] private float patrolRadius;

        [HideInInspector] public Vector3 target;
        [HideInInspector] public Vector3 moveDir;
        [Header("状态")]
        public bool canAttack;
        private float _attackRateCounter;

        protected override void Awake()
        {
            base.Awake();
            attack = GetComponent<Attack>();
            _patrolState = new BeePartolState(); 
            _chaseState = new BeeChaseState();
            spawnPoint = transform.position;
            _attackRateCounter = 0;
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
            _currentState.PhysicsUpdate();
        }
        protected override void OnDisable()
        {
            _currentState.OnExit();
        }
        #endregion
        public override void Move()
        {
            rb.velocity = moveDir * currentSpeed * Time.deltaTime;
        }
        public override bool FoundPlayer()
        {
            var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
            if (obj)
            {
                attacker = obj.transform;
            }
            return obj;
        }
        public override void ChangeDirectionWithJudge()
        {
            if (faceDir == 1 && moveDir.x < 0 || faceDir == -1 && moveDir.x > 0) 
            {
                ChangeDirection();
            }
        }
        protected override void AfterWaitSetAnimator()
        {
        }
        protected override void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, checkDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint, patrolRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target, 1f);
            Gizmos.DrawWireSphere(transform.position + moveDir, 0.2f);
        }

        public void GetNewPoint()
        {
            ChangeTarget(spawnPoint + (Vector3)Random.insideUnitCircle * patrolRadius);
        }

        public void ChangeTarget(Vector3 tar)
        {
            target = tar;
            moveDir = (target - transform.position).normalized;
        }

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

        protected override void TimeCounter()
        {
            base.TimeCounter();
            //attackRateCounter
            if (_attackRateCounter > -1)
                _attackRateCounter -= Time.deltaTime;
            if (canAttack && _attackRateCounter <= 0) 
            {
                _attackRateCounter = attack.attackRate;
                animator.SetTrigger("attack");
            }
        }
    }
}
