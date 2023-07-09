using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bee : Enemy
{
    [HideInInspector] public Attack attack;

    private BeeState currentState;
    private BeeState patrolState;
    private BeeState chaseState;
    [Header("移动范围")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float partolRadius;

    [HideInInspector] public Vector3 target;
    [HideInInspector] public Vector3 moveDir;
    [Header("状态")]
    public bool canAttack;
    private float attackRateCounter;

    protected override void Awake()
    {
        base.Awake();
        attack = GetComponent<Attack>();
        patrolState = new BeePartolState(); 
        chaseState = new BeeChaseState();
        spawnPoint = transform.position;
        attackRateCounter = 0;
    }
    #region 状态相关
    protected override void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    protected override void Update()
    {
        currentState.LogicUpdate();
        TimeCounter();
    }
    protected override void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
    protected override void OnDisable()
    {
        currentState.OnExit();
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
        Gizmos.DrawWireSphere(spawnPoint, partolRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target, 1f);
        Gizmos.DrawWireSphere(transform.position + moveDir, 0.2f);
    }

    public void GetNewPoint()
    {
        ChangeTarget(spawnPoint + (Vector3)Random.insideUnitCircle * partolRadius);
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
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    protected override void TimeCounter()
    {
        base.TimeCounter();
        //attackRateCounter
        if (attackRateCounter > -1)
            attackRateCounter -= Time.deltaTime;
        if (canAttack && attackRateCounter <= 0) 
        {
            attackRateCounter = attack.attackRate;
            animator.SetTrigger("attack");
        }
    }
}
