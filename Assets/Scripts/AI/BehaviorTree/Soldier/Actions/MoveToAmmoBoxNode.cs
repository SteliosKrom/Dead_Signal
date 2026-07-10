using UnityEngine;

public class MoveToAmmoBoxNode : Node
{
    public SoldierBot Bot { get; set; }
    public Transform AmmoBox { get; set; }
    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }
    public float AmmoBoxStopThreshold { get; set; }

    public MoveToAmmoBoxNode(SoldierBot bot, Transform ammoBox, float ammoBoxStopThreshold, float nodeThreshold, float moveSpeed, float rotationSpeed)
    {
        this.Bot = bot;
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        this.NodeThreshold = nodeThreshold;
        this.AmmoBoxStopThreshold = ammoBoxStopThreshold;
        this.AmmoBox = ammoBox;
    }

    public override NodeState Evaluate()
    {
        if (!Bot.IsGoingToAmmoBox)
        {
            Bot.IsGoingToAmmoBox = true;
            Bot.Path = Bot.Pathfinding.FindPath(Bot.transform.position, AmmoBox.position);
            Bot.IdleTimer = 0f;
            Bot.CurrentNodeIndex = 0;
        }

        if (Bot.Path == null || Bot.Path.Count == 0) return NodeState.Running;

        Bot.PlayWalkAnimation();

        AStarNode currentNode = Bot.Path[Bot.CurrentNodeIndex];
        float distanceToNode = Vector3.Distance(Bot.transform.position, currentNode.WorldPosition);
        float distanceToAmmoBox = Vector3.Distance(Bot.transform.position, AmmoBox.position);

        if (distanceToAmmoBox <= AmmoBoxStopThreshold)
            return NodeState.Success;

        if (distanceToNode <= NodeThreshold)
        {
            Bot.CurrentNodeIndex++;
            return NodeState.Running;
        }

        Vector3 directionToNode = (currentNode.WorldPosition - Bot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNode);
        Bot.ApplyMovementAndRotation(directionToNode, MoveSpeed, RotationSpeed, targetRotation);

        return NodeState.Running;
    }
}
