public abstract class AdultChickenBase
{
    protected adult_chick chicken;

    public AdultChickenBase(adult_chick chicken)
    {
        this.chicken = chicken;
    }

    public abstract void Enter();
    public abstract void Execute(float delta);
    public abstract void Exit();
}