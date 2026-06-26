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
        stateController.ZombieTimer += Time.deltaTime;
        Vector3 directionToPlayer = (stateController.Player.position - stateController.transform.position).normalized;
        Vector3 forward = stateController.transform.forward;
        float viewDistance = Vector3.Distance(stateController.transform.position, stateController.Player.position);
        float senseDistance = Vector3.Distance(stateController.transform.position, stateController.Player.position);
        float dot = Vector3.Dot(forward, directionToPlayer);

        MoveToChaseState(dot, viewDistance, senseDistance);
        MoveToPatrolState();
    }

    public override void Exit()
    {
        stateController.ZombieTimer = 0f;
    }

    public void ChangePatrolPoint()
    {
        stateController.ZombieRandomWaitTime = Random.Range(stateController.IdleXSec, stateController.IdleYSec);
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
        if (stateController.ZombieTimer >= stateController.ZombieRandomWaitTime)
        {
            stateController.ChangeState(new ZombiePatrolState(stateController));
            return;
        }
    }

    public void MoveToChaseState(float dot, float viewDistance, float senseDistance)
    {
        if (viewDistance <= stateController.ViewDistance && dot > stateController.DotThreshold)
        {
            stateController.CanSeePlayer = true;
            stateController.ChangeState(new ZombieChaseState(stateController));
            return;
        }

        if (senseDistance <= stateController.SenseDistance && dot < stateController.DotThreshold)
        {
            stateController.CanSensePlayer = true;
            stateController.ChangeState(new ZombieChaseState(stateController));
            return;
        }
    }
}
