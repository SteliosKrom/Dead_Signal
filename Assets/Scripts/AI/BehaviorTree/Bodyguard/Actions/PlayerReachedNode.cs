using UnityEngine;

public class PlayerReachedNode : Node
{
    public BodyguardBot Bot { get; set; }

    public PlayerReachedNode(BodyguardBot bot)
    {
        this.Bot = bot;
    }

    public override NodeState Evaluate()
    {
        Bot.PathComponent.CurrentNodeIndex = 0;
        return NodeState.Success;
    }
}
