using UnityEngine;

public class IsZombieInRange : Node
{
    public SoldierBot Bot { get; set; }
    public ZombieStateController Zombie { get; set; }
    public float ViewRange { get; set; }
    public float DotThreshold { get; set; }
    public float ViewDistance { get; set; }

    public IsZombieInRange(SoldierBot bot, ZombieStateController zombie, float viewDistance, float dotThreshold)
    {
        this.Bot = bot;
        this.Zombie = zombie;
        this.ViewDistance = viewDistance;
        this.DotThreshold = dotThreshold;
    }

    public override NodeState Evaluate()
    {
        Vector3 forward = Bot.transform.forward;
        Vector3 directionToTarget = (Zombie.transform.position - Bot.transform.position).normalized;

        float viewRange = Vector3.Distance(Bot.transform.position, Zombie.transform.position);
        float dot = Vector3.Dot(forward, directionToTarget);

        if (viewRange <= ViewDistance && dot > DotThreshold)
            return NodeState.Success;

        return NodeState.Failure;
    }
}
