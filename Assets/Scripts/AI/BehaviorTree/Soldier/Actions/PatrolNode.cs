using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : Node
{
    public SoldierBot Bot { get; set; }
    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float StopThreshold { get; set; }

    public PatrolNode(SoldierBot bot, float moveSpeed, float rotationSpeed, float stopThreshold)
    {
        this.Bot = bot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.StopThreshold = stopThreshold;
    }

    public override NodeState Evaluate()
    {
        AStarNode currentNode = Bot.Path[Bot.CurrentNodeIndex];

        Vector3 directionToNode = (currentNode.WorldPosition - Bot.transform.position).normalized;
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);

        this.Bot.transform.position += directionToNode * MoveSpeed * Time.deltaTime;

        if (distanceToNode <= StopThreshold)
        {
            Bot.CurrentNodeIndex++;

            if (Bot.CurrentNodeIndex >= Bot.Path.Count)
            {
                Bot.BeginPatrolWait();
                return NodeState.Success;
            }
        }
        return NodeState.Running;
    }
}
