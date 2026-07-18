using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorNode : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Door> Doors { get; set; }

    public UnlockDoorNode(ExplorerBot bot, List<Door> doors)
    {
        this.Bot = bot;
        this.Doors = doors;
    }

    public override NodeState Evaluate()
    {
        Bot.HasKey = false;
        Bot.TargetDoor.Unlock();
        Doors.Remove(Bot.TargetDoor);
        return NodeState.Success;
    }
}
