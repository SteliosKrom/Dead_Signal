using System.Threading;
using UnityEngine;

public sealed class ZombieIdleState : ZombieState
{
    public ZombieIdleState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        ChangePatrolPoint();
        stateController.CurrentNodeIndex = 0;
        stateController.ZombieAnimator.SetInteger("MovementState", 0);
    }

    public override void Update()
    {
        stateController.Timer += Time.deltaTime;
        Vector3 directionToPlayer = (stateController.Player.position - stateController.transform.position).normalized;
        Vector3 forward = stateController.transform.forward;
        float viewDistance = Vector3.Distance(stateController.transform.position, stateController.Player.position);
        float dot = Vector3.Dot(forward, directionToPlayer);

        MoveToChaseState(dot, viewDistance);
        MoveToPatrolState();
    }

    public override void Exit()
    {
        stateController.Timer = 0f;
    }

    public void ChangePatrolPoint()
    {
        stateController.RandomWaitTime = Random.Range(stateController.XSEC, stateController.YSEC);
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, stateController.PatrolPoints.Length);
        }
        while (stateController.CurrentPatrolPoint == stateController.PatrolPoints[randomIndex]);

        stateController.CurrentPatrolPoint = stateController.PatrolPoints[randomIndex];
    }

    public void MoveToPatrolState()
    {
        if (stateController.Timer >= stateController.RandomWaitTime)
        {
            stateController.ChangeState(new ZombiePatrolState(stateController));
            return;
        }
    }

    public void MoveToChaseState(float dot, float viewDistance)
    {
        if (viewDistance <= stateController.ViewDistance && dot > stateController.DotThreshold)
        {
            stateController.CanSeePlayer = true;
            stateController.ChangeState(new ZombieChaseState(stateController));
            return;
        }
    }
}
