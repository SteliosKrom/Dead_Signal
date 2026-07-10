using UnityEngine;

public class HasReachedPatrolPoint : Node
{
    public SoldierBot Bot { get; set; }

    public HasReachedPatrolPoint(SoldierBot bot)
    {
        this.Bot = bot;
    }

    public override NodeState Evaluate()
    {
        return Bot.CurrentNodeIndex >= Bot.Path.Count ? NodeState.Success : NodeState.Failure;
    }
}
