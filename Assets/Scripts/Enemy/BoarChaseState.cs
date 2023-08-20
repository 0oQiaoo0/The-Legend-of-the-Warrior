namespace Enemy
{
    public class BoarChaseState : BoarState
    {
        public override void OnEnter(Boar enemy)
        {
            CurrentEnemy = enemy;
            //Debug.Log("boar chase enter");
            CurrentEnemy.currentSpeed = CurrentEnemy.chaseSpeed;
            CurrentEnemy.animator.SetBool("isRun", true);

            if (CurrentEnemy.isWait)
            {
                CurrentEnemy.isWait = false;
                CurrentEnemy.waitTimeCounter = CurrentEnemy.waitTime;
            }
        }
        public override void LogicUpdate()
        {
            if (!CurrentEnemy.CheckFrontGround() || CurrentEnemy.CheckFrontWall())
            {
                CurrentEnemy.ChangeDirection(); 
            }
        }

        public override void PhysicsUpdate()
        {
            if (!CurrentEnemy.isWait && !CurrentEnemy.isHurt && !CurrentEnemy.isDead)
                CurrentEnemy.Move();
        }
        public override void OnExit()
        {
            CurrentEnemy.animator.SetBool("isRun", false);
        }
    }
}
