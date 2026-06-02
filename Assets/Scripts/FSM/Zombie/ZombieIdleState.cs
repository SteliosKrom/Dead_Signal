using System.Threading;
using UnityEngine;

public sealed class ZombieIdleState : ZombieState
{
    public ZombieIdleState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.InitializePathfinding();
        stateController.ZombieAnimator.SetBool("IsWalking", false);
    }

    public override void Update()
    {
        stateController.Timer += Time.deltaTime;

        float viewDistance = Vector3.Distance(stateController.transform.position, stateController.Player.position);

        if (viewDistance <= stateController.ViewDistance)
        {
            stateController.CanSeePlayer = true;
            stateController.ChangeState(new ZombieChaseState(stateController));
            return;
        }

        if (stateController.Timer >= stateController.RandomWaitTime)
        {
            stateController.Timer = 0f;
            stateController.ChangeState(new ZombiePatrolState(stateController));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle...");
    }
}
