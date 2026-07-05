using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : Node
{
    public List<AStarNode> Path { get; set; }
    public Transform CurrentPatrolPoint { get; set; }
    public SoldierBot Bot { get; set; }
    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public int CurrentNodeIndex { get; set; }

    public PatrolNode(Transform currentPatrolPoint, SoldierBot bot, float moveSpeed, float rotationSpeed)
    {
        this.CurrentPatrolPoint = currentPatrolPoint;
        this.Bot = bot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
    }

    public override NodeState Evaluate()
    {
        // TODO: Connect A* pathfinding for the bot to find path...
        Vector3 directionToTarget = (this.Bot.transform.position - CurrentPatrolPoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        this.Bot.transform.position += directionToTarget * MoveSpeed * Time.deltaTime;
        this.Bot.transform.rotation = Quaternion.Slerp(this.Bot.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        return NodeState.Running;
    }
}
