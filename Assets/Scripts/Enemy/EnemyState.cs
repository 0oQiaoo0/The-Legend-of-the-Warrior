namespace Enemy
{
    public abstract class EnemyState
    {
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void OnExit();
    }
}