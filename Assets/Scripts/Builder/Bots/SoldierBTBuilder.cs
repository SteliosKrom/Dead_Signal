using System.Collections.Generic;
using UnityEngine;

public class SoldierBTBuilder
{
    public SoldierBot SoldierBot { get; set; }
    public IAttackBot AttackBot { get; set; }
    public ZombieStateController Zombie { get; set; }
    public Transform AmmoBox { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float AmmoBoxStopThreshold { get; set; }
    public float NodeThreshold { get; set; }
    public float ViewDistance { get; set; }
    public float DotThreshold { get; set; }

    public SoldierBTBuilder SetBot(SoldierBot baseBot, IAttackBot attackBot)
    {
        this.SoldierBot = baseBot;
        this.AttackBot = attackBot;
        return this;
    }

    public SoldierBTBuilder SetZombie(ZombieStateController zombie)
    {
        this.Zombie = zombie;
        return this;
    }

    public SoldierBTBuilder SetAmmoBox(Transform ammoBox)
    {
        this.AmmoBox = ammoBox;
        return this;
    }

    public SoldierBTBuilder SetMovement(float moveSpeed)
    {
        this.MoveSpeed = moveSpeed;
        return this;
    }

    public SoldierBTBuilder SetRotation(float rotationSpeed)
    {
        this.RotationSpeed = rotationSpeed;
        return this;
    }

    public SoldierBTBuilder SetThresholds(float nodeThreshold, float ammoBoxStopThreshold)
    {
        this.NodeThreshold = nodeThreshold;
        this.AmmoBoxStopThreshold = ammoBoxStopThreshold;
        return this;
    }

    public SoldierBTBuilder SetVision(float viewDistance, float dotThreshold)
    {
        this.ViewDistance = viewDistance;
        this.DotThreshold = dotThreshold;
        return this;
    }

    public Node Build()
    {
        // Actions
        Node idle = new IdleNode(SoldierBot);
        Node patrol = new PatrolNode(SoldierBot, MoveSpeed, RotationSpeed, NodeThreshold);
        Node attack = new AttackNode(SoldierBot, AttackBot, Zombie, RotationSpeed);
        Node reload = new ReloadNode(SoldierBot);
        Node moveToAmmoBox = new MoveToAmmoBoxNode(SoldierBot, AmmoBox, AmmoBoxStopThreshold, 
            NodeThreshold, MoveSpeed, RotationSpeed);
        Node pickupAmmo = new PickupAmmoNode(SoldierBot);

        // Conditions
        Node hasReachedPatrolPoint = new HasReachedPatrolPoint(SoldierBot);
        Node isZombieInRange = new IsZombieInRange(SoldierBot, Zombie, ViewDistance, DotThreshold);
        Node isOutOfAmmo = new IsOutOfAmmo(SoldierBot);

        // Sequences
        Sequence idleSequence = new Sequence(new List<Node> { hasReachedPatrolPoint, idle });
        Sequence attackSequence = new Sequence(new List<Node> { isZombieInRange, attack });
        Sequence moveToAmmoBoxSequence = new Sequence(new List<Node> { isOutOfAmmo, moveToAmmoBox, pickupAmmo, reload });

        // Root
        return new Selector(new List<Node> { moveToAmmoBoxSequence, attackSequence, idleSequence, patrol });
    }
}
