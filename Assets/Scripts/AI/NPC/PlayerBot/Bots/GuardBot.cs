using UnityEngine;

public sealed class GuardBot : PlayerBot
{
    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Guard;
    }
}
