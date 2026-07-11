using UnityEngine;

public class IdleNode : Node
{
    public PlayerBot Bot { get; set; }

    public IdleNode(PlayerBot bot)
    {
        this.Bot = bot;
    }

    public override NodeState Evaluate()
    {
        Bot.PlayIdleAnimation();
        Bot.IdleTimer += Time.deltaTime;

        if (Bot.IdleTimer >= Bot.IdleTimeInterval)
        {
            Bot.IdleTimer = 0f;
            Bot.OnIdleFinished();
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}
