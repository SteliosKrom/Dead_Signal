using UnityEngine;

public sealed class BodyguardBot : PlayerBot
{
    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Bodyguard;
    }
}
