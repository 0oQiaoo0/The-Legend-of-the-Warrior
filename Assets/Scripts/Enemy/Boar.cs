using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : GroundEnemy
{
    private BoarState currentState;
    private BoarState patrolState;
    private BoarState chaseState;
    
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPartolState();
        chaseState = new BoarChaseState();
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
        if (!isWait && !isHurt && !isDead)
            currentState.PhysicsUpdate();
    }
    protected override void OnDisable()
    {
        currentState.OnExit();
    }
    #endregion
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

    
}
