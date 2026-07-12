using UnityEngine;

public class IsOutOfAmmo : Node
{
    public IReloadBot ReloadBot { get; set; }

    public IsOutOfAmmo(IReloadBot bot)
    {
        this.ReloadBot = bot;
    }

    public override NodeState Evaluate()
    {
        return ReloadBot.CurrentAmmo <= 0 ? NodeState.Success : NodeState.Failure;
    }
}
