using UnityEngine;

public class IdleNode : Node
{
    public SoldierBot Bot { get; set; }

    public IdleNode(SoldierBot bot)
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
            Bot.SelectNewPatrolPoint();
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}
