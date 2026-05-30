public class ZombieState
{
    protected ZombieStateController stateController;

    public ZombieState(ZombieStateController stateController)
    {
        this.stateController = stateController;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
