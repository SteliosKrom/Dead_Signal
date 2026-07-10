using UnityEngine;

public class IsOutOfAmmo : Node
{
    public SoldierBot Bot { get; set; }

    public IsOutOfAmmo(SoldierBot bot)
    {
        this.Bot = bot;
    }

    public override NodeState Evaluate()
    {
        return Bot.CurrentAmmo <= 0 ? NodeState.Success : NodeState.Failure;
    }
}
