using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateController stateController;

    public PlayerState(PlayerStateController stateController)
    {
        this.stateController = stateController;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
