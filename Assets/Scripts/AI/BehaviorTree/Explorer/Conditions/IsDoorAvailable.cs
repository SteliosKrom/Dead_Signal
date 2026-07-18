using System.Collections.Generic;
using UnityEngine;

public class IsDoorAvailable : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Door> Doors { get; set; }

    public IsDoorAvailable(ExplorerBot bot, List<Door> doors)
    {
        this.Bot = bot;
        this.Doors = doors;
    }

    public override NodeState Evaluate()
    {
        if (Doors.Count == 0)
            return NodeState.Failure;

        Bot.TargetDoor = Doors[0];
        return NodeState.Success;
    }
}
