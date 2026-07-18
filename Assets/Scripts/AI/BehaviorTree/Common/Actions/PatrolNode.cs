using UnityEngine;

public class PatrolNode : Node
{
    public PlayerBot BaseBot { get; set; }
    public IFollowBot FollowBot { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }

    public PatrolNode(PlayerBot bot, IFollowBot followBot, float moveSpeed,
        float rotationSpeed, float nodeThreshold)
    {
        this.BaseBot = bot;
        this.FollowBot = followBot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.NodeThreshold = nodeThreshold;
    }

    public override NodeState Evaluate()
    {
        if (FollowBot.PathComponent.Path == null || FollowBot.PathComponent.Path.Count == 0) 
            return NodeState.Running;

        if (FollowBot.PathComponent.CurrentNodeIndex >= FollowBot.PathComponent.Path.Count) 
            return NodeState.Success;

        BaseBot.PlayWalkAnimation();

        AStarNode currentNode = FollowBot.PathComponent.Path[FollowBot.PathComponent.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(BaseBot.transform.position, currentNode.WorldPosition);

        if (distanceToNode <= NodeThreshold)
        {
            FollowBot.PathComponent.CurrentNodeIndex++;
            return FollowBot.PathComponent.CurrentNodeIndex >= FollowBot.PathComponent.Path.Count 
                ? NodeState.Success : NodeState.Running;
        }

        Vector3 directionToNode = (currentNode.WorldPosition - BaseBot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNode);

        BaseBot.ApplyMovementAndRotation(directionToNode, MoveSpeed, RotationSpeed, targetRotation);

        return NodeState.Running;
    }
}
