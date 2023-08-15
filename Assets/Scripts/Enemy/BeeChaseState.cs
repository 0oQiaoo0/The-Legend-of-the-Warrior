using UnityEngine;

namespace Enemy
{
    public class BeeChaseState : BeeState
    {
        public override void OnEnter(Bee enemy)
        {
            CurrentEnemy = enemy;
            CurrentEnemy.currentSpeed = CurrentEnemy.chaseSpeed;
            CurrentEnemy.isWait = false;
            CurrentEnemy.waitTimeCounter = CurrentEnemy.waitTime;
            CurrentEnemy.canAttack = false;
            CurrentEnemy.animator.SetBool("isChase", true);
        }
        public override void LogicUpdate()
        {
            CurrentEnemy.ChangeTarget(new Vector3(CurrentEnemy.attacker.position.x, CurrentEnemy.attacker.position.y + 1.5f, 0));

            CurrentEnemy.ChangeDirectionWithJudge();

            if ((CurrentEnemy.target - CurrentEnemy.transform.position).magnitude <= CurrentEnemy.attack.attackRange)
            {
                //攻击
                CurrentEnemy.canAttack = true;
                if (!CurrentEnemy.isHurt && !CurrentEnemy.isDead)
                    CurrentEnemy.rb.velocity = Vector2.zero;
            }
            else
            {
                CurrentEnemy.canAttack = false;
            }
        }

        public override void PhysicsUpdate()
        {
            if (!CurrentEnemy.canAttack && !CurrentEnemy.isWait && !CurrentEnemy.isHurt && !CurrentEnemy.isDead) 
            {
                CurrentEnemy.Move();
            }
        }
        public override void OnExit()
        {
            CurrentEnemy.canAttack = false;
            CurrentEnemy.animator.SetBool("isChase", false);
        }
    }
}
