public abstract class ChickenBase
{
    protected ChickBehaviour chick;

    public ChickenBase(ChickBehaviour chick)
    {
        this.chick = chick;
    }

    public abstract void Enter();
    public abstract void Execute(float delta);
    public abstract void Exit();
}