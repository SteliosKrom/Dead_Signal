using UnityEngine;

public sealed class ZombieChaseState : ZombieState
{
    public ZombieChaseState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        Debug.Log("Enter Chase...");
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {

    }
}
