using UnityEngine;

public sealed class ZombieIdleState : ZombieState
{
    public ZombieIdleState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        Debug.Log("Enter Idle...");
    }

    public override void Update()
    {
        // 
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle...");
    }
}
