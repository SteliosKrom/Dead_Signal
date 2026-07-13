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
        if (Bot.PathComponent.Path == null || Bot.PathComponent.Path.Count == 0) return NodeState.Running;
        if (Bot.PathComponent.CurrentNodeIndex >= Bot.PathComponent.Path.Count) return NodeState.Success;

        Bot.PlayWalkAnimation();

        AStarNode currentNode = Bot.PathComponent.Path[Bot.PathComponent.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);

        if (distanceToNode <= NodeThreshold)
        {
            Bot.PathComponent.CurrentNodeIndex++;
            return Bot.PathComponent.CurrentNodeIndex >= Bot.PathComponent.Path.Count 
                ? NodeState.Success : NodeState.Running;
        }

        Vector3 directionToNode = (currentNode.WorldPosition - Bot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNode);

        Bot.ApplyMovementAndRotation(directionToNode, MoveSpeed, RotationSpeed, targetRotation);

        return NodeState.Running;
    }
}
