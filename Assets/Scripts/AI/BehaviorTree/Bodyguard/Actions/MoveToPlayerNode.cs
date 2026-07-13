using UnityEngine;

public class MoveToPlayerNode : Node
{
    public BodyguardBot Bot { get; set; }
    public PlayerController Player { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }
    public float PlayerStopThreshold { get; set; }

    public MoveToPlayerNode(BodyguardBot bot, PlayerController player, float moveSpeed,
        float rotationSpeed, float nodeThreshold, float playerStopThreshold)
    {
        this.Bot = bot;
        this.Player = player;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.NodeThreshold = nodeThreshold;
        this.PlayerStopThreshold = playerStopThreshold;
    }

    public override NodeState Evaluate()
    {
        if (Bot.PathComponent.CurrentNodeIndex >= Bot.PathComponent.Path.Count)
        {
            Bot.PathComponent.PerformPath(Bot.transform.position, Player.transform.position);
            return NodeState.Success;
        }

        if (Bot.PathComponent.Path == null || Bot.PathComponent.Path.Count == 0) return NodeState.Running;

        Bot.PlayWalkAnimation();

        AStarNode currentNode = Bot.PathComponent.Path[Bot.PathComponent.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);
        float distanceToPlayer = Vector3.Distance(Bot.transform.position, Player.transform.position);

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
