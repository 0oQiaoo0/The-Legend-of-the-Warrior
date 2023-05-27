using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPartolState : BoarState
{
    public override void OnEnter(Boar enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("isWalk", true);
    }
    public override void LogicUpdate()
    {
        //∑¢œ÷player«–ªªµΩchase
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
            return;
        }

        if (!currentEnemy.CheckFrontGround() || currentEnemy.CheckFrontWall())
        {
            currentEnemy.isWait = true;
            currentEnemy.rb.velocity = new Vector2(0f, currentEnemy.rb.velocity.y);
            currentEnemy.animator.SetBool("isWalk", false);
        }
    }
    
    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isWait && !currentEnemy.isHurt && !currentEnemy.isDead)
            currentEnemy.Move();
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isWalk", false);
    }
}
