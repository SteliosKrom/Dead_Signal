using UnityEngine;

public class MoveToDoorNode : Node
{
    public ExplorerBot Bot { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }
    public float DoorStopThreshold { get; set; }

    public MoveToDoorNode(ExplorerBot bot, float moveSpeed, float rotationSpeed,
        float nodeThreshold, float doorStopThreshold)
    {
        this.Bot = bot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.NodeThreshold = nodeThreshold;
        this.DoorStopThreshold = doorStopThreshold;
    }

    public override NodeState Evaluate()
    {
        if (!Bot.IsGoingToDoor)
        {
            Bot.IsGoingToDoor = true;
            Bot.PathComponent.PerformPath(Bot.transform.position, Bot.TargetDoor.transform.position);
        }

        if (Bot.PathComponent.Path == null || Bot.PathComponent.Path.Count == 0)
            return NodeState.Running;

        if (Bot.PathComponent.CurrentNodeIndex >= Bot.PathComponent.Path.Count)
            return NodeState.Success;

        Bot.PlayWalkAnimation();

        AStarNode currentNode = Bot.PathComponent.Path[Bot.PathComponent.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);
        float distanceToDoor = Vector3.Distance(Bot.transform.position, Bot.TargetDoor.transform.position);

        if (distanceToDoor <= DoorStopThreshold)
        {
            Bot.IsGoingToDoor = false;
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
