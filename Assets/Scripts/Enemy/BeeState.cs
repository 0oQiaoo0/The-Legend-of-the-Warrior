public abstract class BeeState : EnemyState
{
    public Bee currentEnemy;
    public abstract void OnEnter(Bee enemy);
}
