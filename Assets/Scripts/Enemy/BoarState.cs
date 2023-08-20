namespace Enemy
{
    public abstract class BoarState : EnemyState
    {
        protected Boar CurrentEnemy;

        public abstract void OnEnter(Boar enemy);
    }
}