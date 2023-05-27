using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePartolState : BeeState
{
    public override void OnEnter(Bee enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.GetNewPoint();
        currentEnemy.ChangeDirectionWithJudge();
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
            return;
        }

        if (Mathf.Abs(currentEnemy.target.x - currentEnemy.transform.position.x) < 0.1f
            && Mathf.Abs(currentEnemy.target.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.isWait = true;
            currentEnemy.rb.velocity = Vector2.zero;
            currentEnemy.GetNewPoint();
        }
    }
    public override void PhysicsUpdate()
    {
        if(!currentEnemy.isWait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.Move();
        }
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
}
