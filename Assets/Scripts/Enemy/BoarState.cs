public abstract class BoarState : EnemyState
{
    public Boar currentEnemy;

    public abstract void OnEnter(Boar enemy);
}