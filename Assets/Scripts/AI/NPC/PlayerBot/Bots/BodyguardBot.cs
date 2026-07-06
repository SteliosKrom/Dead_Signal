using UnityEngine;

public sealed class BodyguardBot : PlayerBot
{
    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Bodyguard;
    }
}
