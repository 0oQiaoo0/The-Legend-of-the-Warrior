using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPartolState : SnailState
{
    public override void OnEnter(Snail enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("isWalk", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);
            return;
        }

        if (!currentEnemy.CheckFrontGround() || currentEnemy.CheckFrontWall())
        {
            //Debug.Log(currentEnemy.gameObject.name + " wait");
            currentEnemy.isWait = true;
            currentEnemy.rb.velocity = new Vector2(0f, currentEnemy.rb.velocity.y);
            currentEnemy.animator.SetBool("isWalk", false);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isWait && !currentEnemy.isHurt && !currentEnemy.isDead)
            if (!currentEnemy.animator.GetCurrentAnimatorStateInfo(0).IsName("snailPreMove") &&
            !currentEnemy.animator.GetCurrentAnimatorStateInfo(0).IsName("snailRecover"))
                currentEnemy.Move();
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isWalk", false);
    }
}
