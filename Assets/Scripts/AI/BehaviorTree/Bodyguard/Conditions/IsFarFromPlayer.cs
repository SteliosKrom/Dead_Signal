using UnityEngine;

public class IsFarFromPlayer : Node
{
    public BodyguardBot Bot { get; set; }
    public PlayerController Player { get; set; }

    public float PlayerStopThreshold { get; set; }

    public IsFarFromPlayer(BodyguardBot bot, PlayerController player, float playerStopThreshold)
    {
        this.Bot = bot;
        this.Player = player;
        this.PlayerStopThreshold = playerStopThreshold;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(Bot.transform.position, Player.transform.position);
        return distanceToPlayer > PlayerStopThreshold ? NodeState.Success : NodeState.Failure;
    }
}
