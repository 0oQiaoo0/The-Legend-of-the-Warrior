using UnityEngine;
using Utilities;

namespace Enemy
{
    public class SnailPatrolState : SnailState
    {
        public override void OnEnter(Snail enemy)
        {
            CurrentEnemy = enemy;
            CurrentEnemy.currentSpeed = CurrentEnemy.normalSpeed;
            CurrentEnemy.animator.SetBool("isWalk", true);
        }
        public override void LogicUpdate()
        {
            if (CurrentEnemy.FoundPlayer())
            {
                CurrentEnemy.SwitchState(NPCState.Skill);
                return;
            }

            if (!CurrentEnemy.CheckFrontGround() || CurrentEnemy.CheckFrontWall())
            {
                //Debug.Log(currentEnemy.gameObject.name + " wait");
                CurrentEnemy.isWait = true;
                CurrentEnemy.rb.velocity = new Vector2(0f, CurrentEnemy.rb.velocity.y);
                CurrentEnemy.animator.SetBool("isWalk", false);
            }
        }

        public override void PhysicsUpdate()
        {
            if (!CurrentEnemy.isWait && !CurrentEnemy.isHurt && !CurrentEnemy.isDead)
                if (!CurrentEnemy.animator.GetCurrentAnimatorStateInfo(0).IsName("snailPreMove") &&
                    !CurrentEnemy.animator.GetCurrentAnimatorStateInfo(0).IsName("snailRecover"))
                    CurrentEnemy.Move();
        }
        public override void OnExit()
        {
            CurrentEnemy.animator.SetBool("isWalk", false);
        }
    }
}
