using System.Collections.Generic;
using UnityEngine;

public class GuardBTBuilder
{ 
    public GuardBot GuardBot { get; set; }
    public IAttackBot AttackBot { get; set; }
    public ZombieStateController Zombie { get; set; }

    public float RotationSpeed { get; set; }
    public float DotThreshold { get; set; }
    public float ViewDistance { get; set; }

    public GuardBTBuilder SetBot(GuardBot baseBot, IAttackBot attackBot)
    {
        this.GuardBot = baseBot;
        this.AttackBot = attackBot;
        return this;
    }

    public GuardBTBuilder SetZombie(ZombieStateController zombie)
    {
        this.Zombie = zombie;
        return this;
    }

    public GuardBTBuilder SetRotation(float rotationSpeed)
    {
        this.RotationSpeed = rotationSpeed;
        return this;
    }

    public GuardBTBuilder SetVision(float viewDistance, float dotThreshold)
    {
        this.ViewDistance = viewDistance;
        this.DotThreshold = dotThreshold;
        return this;
    }

    public Node Build()
    {
        // Actions...
        Node idle = new IdleNode(GuardBot);
        Node attack = new AttackNode(GuardBot, AttackBot, Zombie, RotationSpeed);
        Node reload = new ReloadNode(GuardBot);

        // Conditions...
        Node isZombieInRange = new IsZombieInRange(GuardBot, Zombie, ViewDistance, DotThreshold);
        Node isOutOfAmmo = new IsOutOfAmmo(GuardBot);

        // Sequences...
        Sequence attackSequence = new Sequence(new List<Node> { isZombieInRange, attack });
        Sequence reloadSequence = new Sequence(new List<Node> { isOutOfAmmo, reload });

        return new Selector(new List<Node> { reloadSequence, attackSequence, idle});
    }
}
