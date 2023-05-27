using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;

public class Snail : GroundEnemy
{
    private SnailState currentState;
    private SnailState patrolState;
    private SnailState skillState;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SnailPartolState();
        skillState = new SnailSkillState();
    }
    
    #region ×´Ì¬Ïà¹Ø
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
            NPCState.Skill => skillState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
}
