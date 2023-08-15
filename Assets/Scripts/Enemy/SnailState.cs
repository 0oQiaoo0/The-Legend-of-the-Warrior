namespace Enemy
{
    public abstract class SnailState : EnemyState
    {
        protected Snail CurrentEnemy;

        public abstract void OnEnter(Snail enemy);
    }
}