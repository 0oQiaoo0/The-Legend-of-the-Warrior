namespace Enemy
{
    public class BoarChaseState : BoarState
    {
        public override void OnEnter(Boar enemy)
        {
            currentEnemy = enemy;
            //Debug.Log("boar chase enter");
            currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
            currentEnemy.animator.SetBool("isRun", true);

            if (currentEnemy.isWait)
            {
                currentEnemy.isWait = false;
                currentEnemy.waitTimeCounter = currentEnemy.waitTime;
            }
        }
        public override void LogicUpdate()
        {
            if (!currentEnemy.CheckFrontGround() || currentEnemy.CheckFrontWall())
            {
                currentEnemy.ChangeDirection(); 
            }
        }

        public override void PhysicsUpdate()
        {
            if (!currentEnemy.isWait && !currentEnemy.isHurt && !currentEnemy.isDead)
                currentEnemy.Move();
        }
        public override void OnExit()
        {
            currentEnemy.animator.SetBool("isRun", false);
        }
    }
}
