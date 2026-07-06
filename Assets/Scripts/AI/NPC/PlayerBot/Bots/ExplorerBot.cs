public sealed class ExplorerBot : PlayerBot
{
    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Explorer;
    }
}
