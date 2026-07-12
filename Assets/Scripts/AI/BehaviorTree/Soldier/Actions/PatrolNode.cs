using UnityEngine;

public class PatrolNode : Node
{
    public SoldierBot Bot { get; set; }
    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }

    public PatrolNode(SoldierBot bot, float moveSpeed, float rotationSpeed, float nodeThreshold)
    {
        this.Bot = bot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.NodeThreshold = nodeThreshold;
    }

    public override NodeState Evaluate()
    {
        if (Bot.PatrolComponent.Path == null || Bot.PatrolComponent.Path.Count == 0) return NodeState.Running;
        if (Bot.PatrolComponent.CurrentNodeIndex >= Bot.PatrolComponent.Path.Count) return NodeState.Success;

        Bot.PlayWalkAnimation();

        AStarNode currentNode = Bot.PatrolComponent.Path[Bot.PatrolComponent.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);

        if (distanceToNode <= NodeThreshold)
        {
            Bot.PatrolComponent.CurrentNodeIndex++;

            if (Bot.PatrolComponent.CurrentNodeIndex >= Bot.PatrolComponent.Path.Count)
                return NodeState.Success;

            return NodeState.Running;
        }

        Vector3 directionToNode = (currentNode.WorldPosition - Bot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNode);

        Bot.ApplyMovementAndRotation(directionToNode, MoveSpeed, RotationSpeed, targetRotation);

        return NodeState.Running;
    }
}
