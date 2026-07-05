using UnityEngine;

public class HasReachedPatrolPoint : Node
{
    public SoldierBot Bot { get; set; }
    public float StopThreshold { get; set; }

    public HasReachedPatrolPoint(SoldierBot bot, float stopThreshold)
    {
        this.Bot = bot;
        this.StopThreshold = stopThreshold;
    }

    public override NodeState Evaluate()
    {
        float distanceToPoint = Vector3.Distance(Bot.transform.position, Bot.CurrentPatrolPoint.position);

        if (distanceToPoint <= StopThreshold)
            return NodeState.Success;

        return NodeState.Failure;
    }
}
