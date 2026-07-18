using UnityEngine;

public class HasReachedPatrolPoint : Node
{
    public PlayerBot BaseBot { get; set; }
    public IFollowBot FollowBot { get; set; }

    public HasReachedPatrolPoint(PlayerBot bot, IFollowBot followBot)
    {
        this.BaseBot = bot;
        this.FollowBot = followBot;
    }

    public override NodeState Evaluate()
    {
        return FollowBot.PathComponent.CurrentNodeIndex >= FollowBot.PathComponent.Path.Count 
            ? NodeState.Success : NodeState.Failure;
    }
}
