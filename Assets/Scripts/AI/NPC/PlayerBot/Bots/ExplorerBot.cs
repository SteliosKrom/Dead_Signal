using UnityEngine;

public sealed class ExplorerBot : PlayerBot
{
    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Explorer;
    }
}
