using UnityEngine;
using Utilities;

namespace Enemy
{
    public class BeePatrolState : BeeState
    {
        public override void OnEnter(Bee enemy)
        {
            CurrentEnemy = enemy;
            CurrentEnemy.currentSpeed = CurrentEnemy.normalSpeed;
            CurrentEnemy.GetNewPoint();
            CurrentEnemy.ChangeDirectionWithJudge();
        }
        public override void LogicUpdate()
        {
            if (CurrentEnemy.FoundPlayer())
            {
                CurrentEnemy.SwitchState(NPCState.Chase);
                return;
            }

            if (Mathf.Abs(CurrentEnemy.target.x - CurrentEnemy.transform.position.x) < 0.1f
                && Mathf.Abs(CurrentEnemy.target.y - CurrentEnemy.transform.position.y) < 0.1f)
            {
                CurrentEnemy.isWait = true;
                CurrentEnemy.rb.velocity = Vector2.zero;
                CurrentEnemy.GetNewPoint();
            }
        }
        public override void PhysicsUpdate()
        {
            if(!CurrentEnemy.isWait && !CurrentEnemy.isHurt && !CurrentEnemy.isDead)
            {
                CurrentEnemy.Move();
            }
        }
        public override void OnExit()
        {
            //throw new System.NotImplementedException();
        }
    }
}
