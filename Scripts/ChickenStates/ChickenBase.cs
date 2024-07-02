public abstract class ChickenBase
{
    protected baby_chick chick;

    public ChickenBase(baby_chick chick)
    {
        this.chick = chick;
    }

    public abstract void Enter();
    public abstract void Execute(float delta);
    public abstract void Exit();
}