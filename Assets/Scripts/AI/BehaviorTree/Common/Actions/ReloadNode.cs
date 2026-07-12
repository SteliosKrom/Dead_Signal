using UnityEngine;

public class ReloadNode : Node
{
    public IReloadBot ReloadBot { get; set; }

    public ReloadNode(IReloadBot bot)
    {
        this.ReloadBot = bot;
    }

    public override NodeState Evaluate()
    {
        ReloadBot.Reload();
        return NodeState.Success;
    }
}
