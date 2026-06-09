using UnityEngine;


public sealed class ZombiePatrolState : ZombieState
{
    public ZombiePatrolState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;

        stateController.Path = stateController.Pathfinding.FindPath(stateController.transform.position,
            stateController.CurrentPatrolPoint.transform.position);

        stateController.ZombieAnimator.SetInteger("MovementState", 1);
    }

    public override void Update()
    {
        AStarNode currentNode = stateController.Path[stateController.CurrentNodeIndex];

        Vector3 forward = stateController.transform.forward;
        Vector3 directionToPlayer = (stateController.Player.position - stateController.transform.position).normalized;
        Vector3 directionToTarget = (currentNode.WorldPosition - stateController.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        float distance = Vector3.Distance(stateController.transform.position, currentNode.WorldPosition);
        float viewDistance = Vector3.Distance(stateController.transform.position, stateController.Player.position);
        float dot = Vector3.Dot(forward, directionToPlayer);

        ApplyWalkMovement(directionToTarget, targetRotation);
        MoveToChaseState(dot, viewDistance);
        MoveToIdleState(distance);
    }

    public override void Exit()
    {
        Debug.Log("Exit Patrol...");
    }

    public void ApplyWalkMovement(Vector3 directionToTarget, Quaternion targetRotation)
    {
        stateController.transform.position += directionToTarget * stateController.MoveSpeed * Time.deltaTime;
        stateController.transform.rotation = targetRotation;
    }

    public void MoveToIdleState(float distance)
    {
        if (distance <= stateController.NodeReachThreshold)
        {
            if (stateController.CurrentNodeIndex >= stateController.Path.Count - 1)
            {
                stateController.ChangeState(new ZombieIdleState(stateController));
                return;
            }
            stateController.CurrentNodeIndex++;
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
