using System.Collections.Generic;

public class BodyguardBTBuilder
{
    public BodyguardBot BodyguardBot { get; set; }
    public IAttackBot AttackBot { get; set; }

    public ZombieStateController Zombie { get; set; }
    public PlayerController Player { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float ViewDistance { get; set; }
    public float DotThreshold { get; set; }
    public float NodeThreshold { get; set; }
    public float PlayerStopThreshold { get; set; }
    public float ZombieStopThreshold { get; set; }

    public BodyguardBTBuilder SetBot(BodyguardBot baseBot, IAttackBot attackBot)
    {
        this.BodyguardBot = baseBot;
        this.AttackBot = attackBot;
        return this;
    }

    public BodyguardBTBuilder SetPlayer(PlayerController player)
    {
        this.Player = player;
        return this;
    }

    public BodyguardBTBuilder SetZombie(ZombieStateController zombie)
    {
        this.Zombie = zombie;
        return this;
    }

    public BodyguardBTBuilder SetMovement(float moveSpeed)
    {
        this.MoveSpeed = moveSpeed;
        return this;
    }

    public BodyguardBTBuilder SetRotation(float rotationSpeed)
    {
        this.RotationSpeed = rotationSpeed;
        return this;
    }

    public BodyguardBTBuilder SetThresholds(float nodeThreshold, 
        float playerStopThreshold, float zombieStopThreshold)
    {
        this.NodeThreshold = nodeThreshold;
        this.PlayerStopThreshold = playerStopThreshold;
        this.ZombieStopThreshold = zombieStopThreshold;
        return this;
    }

    public BodyguardBTBuilder SetVision(float viewDistance, float dotThreshold)
    {
        this.ViewDistance = viewDistance;
        this.DotThreshold = dotThreshold;
        return this;
    }

    public Node Build()
    {
        // Actions
        Node idle = new IdleNode(BodyguardBot);
        Node attack = new AttackNode(BodyguardBot, AttackBot, Zombie, RotationSpeed);
        Node reload = new ReloadNode(BodyguardBot);
        Node playerReached = new PlayerReachedNode(BodyguardBot);
        Node moveToPlayer = new MoveToPlayerNode(BodyguardBot, Player, MoveSpeed, 
            RotationSpeed, NodeThreshold, PlayerStopThreshold);

        // Conditions
        Node isZombieInRange = new IsZombieInRange(BodyguardBot, Zombie, ViewDistance, DotThreshold);
        Node isOutOfAmmo = new IsOutOfAmmo(BodyguardBot);
        Node isFarFromPlayer = new IsFarFromPlayer(BodyguardBot, Player, PlayerStopThreshold);
        Node isZombieNearPlayer = new IsZombieNearPlayer(Zombie, Player, ZombieStopThreshold);

        // Sequences
        Sequence attackSequence = new Sequence(new List<Node> { isZombieInRange, isZombieNearPlayer, attack });
        Sequence reloadSequence = new Sequence(new List<Node> { isOutOfAmmo, reload });
        Sequence moveToPlayerSequence = new Sequence(new List<Node> { isFarFromPlayer, 
            moveToPlayer, playerReached });

        // Root
        return new Selector(new List<Node> { attackSequence, moveToPlayerSequence, reloadSequence, idle });
    }
}
