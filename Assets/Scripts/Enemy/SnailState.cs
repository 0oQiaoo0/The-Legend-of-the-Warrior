public abstract class SnailState : EnemyState
{
    public Snail currentEnemy;

    public abstract void OnEnter(Snail enemy);
}