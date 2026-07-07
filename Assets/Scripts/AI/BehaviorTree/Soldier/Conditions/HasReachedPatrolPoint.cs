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
        if (Bot.CurrentNodeIndex >= Bot.Path.Count)
            return NodeState.Success;

        return NodeState.Failure;
    }
}
