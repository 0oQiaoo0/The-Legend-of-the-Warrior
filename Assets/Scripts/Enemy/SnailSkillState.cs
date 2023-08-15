using General;

namespace Enemy
{
    public class SnailSkillState : SnailState
    {
        public override void OnEnter(Snail enemy)
        {
            CurrentEnemy = enemy;
            CurrentEnemy.currentSpeed = CurrentEnemy.chaseSpeed;
            CurrentEnemy.animator.SetBool("isHide", true);
            CurrentEnemy.animator.SetTrigger("skill");
            CurrentEnemy.GetComponent<Character>().invulnerable = true;
            CurrentEnemy.GetComponent<Character>().invulnerableCounter = CurrentEnemy.lostTime;
        }
        public override void LogicUpdate()
        {
            CurrentEnemy.GetComponent<Character>().invulnerableCounter = CurrentEnemy.lostTimeCounter;
        }
        public override void PhysicsUpdate()
        {
        
        }
        public override void OnExit()
        {
            CurrentEnemy.animator.SetBool("isHide", false);
            CurrentEnemy.GetComponent<Character>().invulnerable = false;
        }
    }
}
