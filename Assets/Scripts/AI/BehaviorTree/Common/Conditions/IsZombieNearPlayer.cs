using UnityEngine;

public class IsZombieNearPlayer : Node
{
    public ZombieStateController Zombie { get; set; }
    public PlayerController Player { get; set; }

    public float ZombieStopThreshold { get; set; }

    public IsZombieNearPlayer(ZombieStateController zombie, PlayerController player, 
        float zombieStopThreshold)
    {
        this.Zombie = zombie;
        this.Player = player;
        this.ZombieStopThreshold = zombieStopThreshold;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(Player.transform.position, Zombie.transform.position);
        return distanceToPlayer <= ZombieStopThreshold ? NodeState.Success : NodeState.Failure;
    }
}
