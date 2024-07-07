public abstract class BabyChickenBase
{
    protected baby_chick chick;

    public BabyChickenBase(baby_chick chick)
    {
        this.chick = chick;
    }

    public abstract void Enter();
    public abstract void Execute(float delta);
    public abstract void Exit();
}