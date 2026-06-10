using System.IO;
using UnityEngine;

public sealed class ZombieChaseState : ZombieState
{
    public ZombieChaseState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;

        stateController.Path = stateController.Pathfinding.FindPath(stateController.transform.position, stateController.Player.position);

        stateController.ZombieAnimator.SetInteger("MovementState", 2);
    }

    public override void Update()
    {
        if (stateController.CurrentNodeIndex >= stateController.Path.Count)
        {
            Debug.LogError($"INVALID INDEX: {stateController.CurrentNodeIndex} / {stateController.Path.Count}");
            return;
        }

        AStarNode currentNode = stateController.Path[stateController.CurrentNodeIndex];

        Vector3 directionToPlayer = (currentNode.WorldPosition - stateController.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        float distance = Vector3.Distance(stateController.transform.position, stateController.Player.transform.position);
        float nodeDistance = Vector3.Distance(stateController.transform.position, currentNode.WorldPosition);

        if (nodeDistance <= stateController.NodeReachThreshold)
        {
            if (stateController.CurrentNodeIndex >= stateController.Path.Count - 1)
            {
                stateController.Path = stateController.Pathfinding.FindPath(stateController.transform.position, stateController.Player.position);
                stateController.CurrentNodeIndex = 0;
                return;
            }
            stateController.CurrentNodeIndex++;
        }

        if (distance >= stateController.ViewDistance)
            stateController.CanSeePlayer = false;
        else
            stateController.CanSeePlayer = true;

        if (stateController.CanSeePlayer)
        {
            stateController.Timer = 0f;

            ApplyChaseMovement(directionToPlayer, targetRotation);
            MoveToAttackState(distance);
        }
        else
        {
            ExtendChase(directionToPlayer, targetRotation);
            MoveToAttackState(distance);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Chase...");
    }

    public void ApplyChaseMovement(Vector3 directionToPlayer, Quaternion targetRotation)
    {
        stateController.transform.position += directionToPlayer * stateController.MoveSpeed *
            stateController.SpeedMultiplier * Time.deltaTime;

        stateController.transform.rotation = targetRotation;
    }

    public void ExtendChase(Vector3 directionToPlayer, Quaternion targetRotation)
    {
        stateController.Timer += Time.deltaTime;

        if (stateController.Timer >= stateController.NSEC)
        {
            MoveToIdleState();
            return;
        }
        ApplyChaseMovement(directionToPlayer, targetRotation);
    }

    public void MoveToAttackState(float distance)
    {
        if (distance <= stateController.AttackRange)
        {
            stateController.ChangeState(new ZombieAttackState(stateController));
            return;
        }
    }

    public void MoveToIdleState()
    {
        if (!stateController.CanSeePlayer)
        {
            stateController.ChangeState(new ZombieIdleState(stateController));
            return;
        }
    }
}
