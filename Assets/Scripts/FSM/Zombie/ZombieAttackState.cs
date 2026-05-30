using UnityEngine;

public sealed class ZombieAttackState : ZombieState
{
    public ZombieAttackState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        Debug.Log("Enter Attack...");
    }

    public override void Update()
    {
        // 
    }

    public override void Exit()
    {
        Debug.Log("Exit Attack...");
    }
}
