using UnityEngine;


public sealed class ZombiePatrolState : ZombieState
{
    public ZombiePatrolState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;

        stateController.Path = stateController.Pathfinding.FindPath(stateController.transform.position, 
            stateController.CurrentPatrolPoint.transform.position);

        stateController.ZombieAnimator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        AStarNode currentNode = stateController.Path[stateController.CurrentNodeIndex];

        Vector3 directionToTarget = (currentNode.WorldPosition - stateController.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        stateController.transform.position += directionToTarget * stateController.MoveSpeed * Time.deltaTime;
        stateController.transform.rotation = targetRotation;

        float distance = Vector3.Distance(stateController.transform.position, currentNode.WorldPosition);
        float viewDistance = Vector3.Distance(stateController.transform.position, stateController.Player.position);

        if (viewDistance <= stateController.ViewDistance)
        {
            stateController.ChangeState(new ZombieChaseState(stateController));
            return;
        }

        if (distance <= stateController.StopThreshold)
        {
            if (stateController.CurrentNodeIndex >= stateController.Path.Count - 1)
            {
                stateController.ChangeState(new ZombieIdleState(stateController));
                return;
            }
            stateController.CurrentNodeIndex++;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Patrol...");
    }
}
