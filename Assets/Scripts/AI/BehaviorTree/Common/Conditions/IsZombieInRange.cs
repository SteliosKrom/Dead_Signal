using UnityEngine;

public class IsZombieInRange : Node
{
    public PlayerBot Bot { get; set; }
    public ZombieStateController Zombie { get; set; }
    public float DotThreshold { get; set; }
    public float ViewDistance { get; set; }

    public IsZombieInRange(PlayerBot bot, ZombieStateController zombie, float viewDistance, float dotThreshold)
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

        bool canSee = viewRange <= ViewDistance && dot > DotThreshold;
        bool canSense = viewRange <= ViewDistance;

        return (canSee || canSense) ? NodeState.Success : NodeState.Failure;
    }
}
