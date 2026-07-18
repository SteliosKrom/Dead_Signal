using UnityEngine;

public class HasEquippedKey : Node
{
    public ExplorerBot Bot { get; set; }

    public HasEquippedKey(ExplorerBot bot)
    {
        this.Bot = bot;
    }

    public override NodeState Evaluate()
    {
        return Bot.HasKey ? NodeState.Success : NodeState.Failure;
    }
}
