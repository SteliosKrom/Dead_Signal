using System.IO;
using UnityEngine;

public sealed class ZombieChaseState : ZombieState
{
    public ZombieChaseState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;
        stateController.Timer = 0f;

        stateController.Path = stateController.Pathfinding.FindPath(stateController.transform.position,
            stateController.Player.position);

        stateController.ZombieAnimator.SetInteger("MovementState", 2);
    }

    public override void Update()
    {
        AStarNode currentNode = stateController.Path[stateController.CurrentNodeIndex];

        Vector3 directionToPlayer = (currentNode.WorldPosition - stateController.transform.position).normalized;
        Quaternion playerTargetRotation = Quaternion.LookRotation(directionToPlayer);

        float distanceToPlayer = Vector3.Distance(stateController.transform.position, stateController.Player.transform.position);
        float distanceToNode = Vector3.Distance(stateController.transform.position, currentNode.WorldPosition);

        if (distanceToNode <= stateController.NodeReachThreshold)
        {
            if (stateController.CurrentNodeIndex >= stateController.Path.Count - 1)
            {
                stateController.Path = stateController.Pathfinding.FindPath(stateController.transform.position,
                    stateController.Player.position);

                stateController.CurrentNodeIndex = 0;
                return;
            }
            stateController.CurrentNodeIndex++;
        }

        if (distanceToPlayer >= stateController.ViewDistance)
            stateController.CanSeePlayer = false;
        else 
            stateController.CanSeePlayer = true;

        if (stateController.ZombieInteractor.DoorDetectable != null)
        {
            MoveToAttackDoorState();
            return;
        }

        if (stateController.CanSeePlayer)
        {
            ApplyChaseMovement(directionToPlayer, playerTargetRotation);
            MoveToAttackState(distanceToPlayer);
            return;
        }
        else
        {
            ExtendChase(directionToPlayer, playerTargetRotation);
            MoveToAttackState(distanceToPlayer);
            return;
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
            stateController.IsAttackingDoor = false;

            stateController.ChangeState(new ZombieAttackState(stateController));
            return;
        }
    }

    public void MoveToAttackDoorState()
    {
        stateController.CurrentDoor = stateController.ZombieInteractor.DoorDetectable;

        stateController.IsAttackingDoor = true;

        stateController.ChangeState(new ZombieAttackState(stateController));
        return;
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
