using UnityEngine;

public sealed class GuardBot : PlayerBot
{
    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Guard;
    }
}
