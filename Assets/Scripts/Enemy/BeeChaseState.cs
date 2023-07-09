using UnityEngine;

public class BeeChaseState : BeeState
{
    public override void OnEnter(Bee enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.isWait = false;
        currentEnemy.waitTimeCounter = currentEnemy.waitTime;
        currentEnemy.canAttack = false;
        currentEnemy.animator.SetBool("isChase", true);
    }
    public override void LogicUpdate()
    {
        currentEnemy.ChangeTarget(new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0));

        currentEnemy.ChangeDirectionWithJudge();

        if ((currentEnemy.target - currentEnemy.transform.position).magnitude <= currentEnemy.attack.attackRange)
        {
            //攻击
            currentEnemy.canAttack = true;
            if (!currentEnemy.isHurt && !currentEnemy.isDead)
                currentEnemy.rb.velocity = Vector2.zero;
        }
        else
        {
            currentEnemy.canAttack = false;
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.canAttack && !currentEnemy.isWait && !currentEnemy.isHurt && !currentEnemy.isDead) 
        {
            currentEnemy.Move();
        }
    }
    public override void OnExit()
    {
        currentEnemy.canAttack = false;
        currentEnemy.animator.SetBool("isChase", false);
    }
}
