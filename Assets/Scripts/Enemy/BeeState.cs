namespace Enemy
{
    public abstract class BeeState : EnemyState
    {
        protected Bee CurrentEnemy;
        public abstract void OnEnter(Bee enemy);
    }
}
