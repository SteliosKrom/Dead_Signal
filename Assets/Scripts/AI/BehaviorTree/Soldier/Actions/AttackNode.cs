using UnityEngine;

public class AttackNode : Node
{ 
    public SoldierBot Bot { get; set; }
    public ZombieStateController Zombie { get; set; }
    public float RotationSpeed { get; set; }

    public AttackNode(SoldierBot bot, ZombieStateController zombie, float rotationSpeed)
    {
        this.Bot = bot;
        this.Zombie = zombie;
        this.RotationSpeed = rotationSpeed;
    }

    public override NodeState Evaluate()
    {
        Vector3 directionToTarget = (Zombie.transform.position - Bot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        Bot.AttackTimer += Time.deltaTime;

        if (Bot.AttackTimer >= Bot.AttackTimeInterval)
        {
            Bot.AttackTimer = 0f;
            Bot.AttackZombie(directionToTarget);
        }

        Bot.transform.rotation = Quaternion.Slerp(Bot.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        return NodeState.Success;
    }
}
