using System.Collections.Generic;

public class ExplorerBTBuilder
{
    public ExplorerBot BaseBot { get; set; }
    public List<Key> Keys { get; set; }
    public List<Door> Doors { get; set; }
    public IFollowBot FollowBot { get; set; }

    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float NodeThreshold { get; set; }
    public float KeyStopThreshold { get; set; }
    public float DoorStopThreshold { get; set; }
    public float DotThreshold { get; set; }
    public float ViewDistance { get; set; }

    public ExplorerBTBuilder SetBot(ExplorerBot bot, IFollowBot followBot)
    {
        this.BaseBot = bot;
        this.FollowBot = followBot;
        return this;
    }

    public ExplorerBTBuilder SetSpeed(float moveSpeed, float rotationSpeed)
    {
        this.MoveSpeed = moveSpeed;
        this.RotationSpeed = rotationSpeed;
        return this;
    }

    public ExplorerBTBuilder SetKeys(List<Key> keys)
    {
        this.Keys = keys;
        return this;
    }

    public ExplorerBTBuilder SetDoors(List<Door> doors)
    {
        this.Doors = doors;
        return this;
    }

    public ExplorerBTBuilder SetThresholds(float nodeThreshold, float keyStopThreshold, 
        float doorStopThreshold)
    {
        this.NodeThreshold = nodeThreshold;
        this.KeyStopThreshold = keyStopThreshold;
        this.DoorStopThreshold = doorStopThreshold;
        return this;
    }

    public ExplorerBTBuilder SetVision(float viewDistance, float dotThreshold)
    {
        this.ViewDistance = viewDistance;
        this.DotThreshold = dotThreshold;
        return this;
    }

    public Node Build()
    {
        // Actions
        Node idle = new IdleNode(BaseBot);
        Node patrol = new PatrolNode(BaseBot, FollowBot, MoveSpeed, RotationSpeed, NodeThreshold);
        Node equipKey = new EquipKeyNode(BaseBot, Keys);
        Node unlockDoor = new UnlockDoorNode(BaseBot, Doors);

        Node moveToDoor = new MoveToDoorNode(BaseBot, MoveSpeed, RotationSpeed,
            NodeThreshold, DoorStopThreshold);

        Node moveToKey = new MoveToKeyNode(BaseBot, MoveSpeed, 
            RotationSpeed, NodeThreshold, KeyStopThreshold);

        // Conditions
        Node hasEquippedKey = new HasEquippedKey(BaseBot);
        Node hasReachedPatrolPoint = new HasReachedPatrolPoint(BaseBot, FollowBot);
        Node areKeysInRange = new AreKeysInRange(BaseBot, Keys, ViewDistance, DotThreshold);
        Node isDoorLocked = new IsDoorLocked(BaseBot, Doors);
        Node isDoorAvailable = new IsDoorAvailable(BaseBot, Doors);

        // Sequences
        Sequence idleSequence = new Sequence(new List<Node> { hasReachedPatrolPoint, idle });

        Sequence moveToKeySequence = new Sequence(new List<Node> { areKeysInRange, moveToKey, 
            idle, equipKey });

        Sequence moveToDoorSequence = new Sequence(new List<Node> { hasEquippedKey, isDoorAvailable, 
            moveToDoor, idle, isDoorLocked, unlockDoor });

        // Root
        return new Selector(new List<Node> { moveToKeySequence, moveToDoorSequence, idleSequence, patrol});
    }
}
