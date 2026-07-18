using UnityEngine;

public class MoveToKeyNode : Node
{
    public ExplorerBot Bot { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }
    public float KeyStopThreshold { get; set; }

    public MoveToKeyNode(ExplorerBot bot, float moveSpeed, float rotationSpeed,
        float nodeThreshold, float keyStopThreshold)
    {
        this.Bot = bot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.NodeThreshold = nodeThreshold;
        this.KeyStopThreshold = keyStopThreshold;
    }

    public override NodeState Evaluate()
    {
        if (!Bot.IsGoingToKey)
        {
            Bot.IsGoingToKey = true;
            Bot.PathComponent.PerformPath(Bot.transform.position, Bot.TargetKey.transform.position);
        }

        if (Bot.PathComponent.Path == null || Bot.PathComponent.Path.Count == 0)
            return NodeState.Running;

        if (Bot.PathComponent.CurrentNodeIndex >= Bot.PathComponent.Path.Count)
            return NodeState.Success;

        Bot.PlayWalkAnimation();

        AStarNode currentNode = Bot.PathComponent.Path[Bot.PathComponent.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);
        float distanceToKey = Vector3.Distance(Bot.transform.position, Bot.TargetKey.transform.position);

        if (distanceToKey <= KeyStopThreshold)
        {
            Bot.IsGoingToKey = false;
            return NodeState.Success;
        }

        if (distanceToNode <= NodeThreshold)
        {
            Bot.PathComponent.CurrentNodeIndex++;
            return NodeState.Running;
        }

        Vector3 directionToNode = (currentNode.WorldPosition - Bot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNode);

        Bot.ApplyMovementAndRotation(directionToNode, MoveSpeed, RotationSpeed, targetRotation);
        return NodeState.Running;
    }
}
