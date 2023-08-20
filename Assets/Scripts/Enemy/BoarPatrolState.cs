using UnityEngine;
using Utilities;

namespace Enemy
{
    public class BoarPatrolState : BoarState
    {
        public override void OnEnter(Boar enemy)
        {
            CurrentEnemy = enemy;
            CurrentEnemy.currentSpeed = CurrentEnemy.normalSpeed;
            CurrentEnemy.animator.SetBool("isWalk", true);
        }
        public override void LogicUpdate()
        {
            //发现player切换到chase
            if (CurrentEnemy.FoundPlayer())
            {
                CurrentEnemy.SwitchState(NPCState.Chase);
                return;
            }

            if (!CurrentEnemy.CheckFrontGround() || CurrentEnemy.CheckFrontWall())
            {
                CurrentEnemy.isWait = true;
                CurrentEnemy.rb.velocity = new Vector2(0f, CurrentEnemy.rb.velocity.y);
                CurrentEnemy.animator.SetBool("isWalk", false);
            }
        }
    
        public override void PhysicsUpdate()
        {
            if (!CurrentEnemy.isWait && !CurrentEnemy.isHurt && !CurrentEnemy.isDead)
                CurrentEnemy.Move();
        }
        public override void OnExit()
        {
            CurrentEnemy.animator.SetBool("isWalk", false);
        }
    }
}
