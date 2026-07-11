using UnityEngine;

public class AttackNode : Node
{
    public PlayerBot BaseBot { get; set; }
    public IAttackBot AttackBot { get; set; }
    public ZombieStateController Zombie { get; set; }
    public float RotationSpeed { get; set; }

    public AttackNode(PlayerBot baseBot, IAttackBot attackBot, ZombieStateController zombie, float rotationSpeed)
    {
        this.BaseBot = baseBot;
        this.AttackBot = attackBot;
        this.Zombie = zombie;
        this.RotationSpeed = rotationSpeed;
    }

    public override NodeState Evaluate()
    {
        BaseBot.PlayIdleAnimation();

        Vector3 directionToTarget = (Zombie.transform.position - BaseBot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        AttackBot.AttackTimer += Time.deltaTime;

        if (AttackBot.AttackTimer >= AttackBot.AttackTimeInterval)
        {
            AttackBot.AttackTimer = 0f;
            AttackBot.AttackZombie(directionToTarget);
        }

        BaseBot.transform.rotation = Quaternion.Slerp(BaseBot.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        return NodeState.Success;
    }
}
