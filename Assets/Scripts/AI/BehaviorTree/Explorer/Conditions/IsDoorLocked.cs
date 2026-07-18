using System.Collections.Generic;
using UnityEngine;

public class IsDoorLocked : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Door> Doors { get; set; }

    public IsDoorLocked(ExplorerBot bot, List<Door> doors)
    {
        this.Bot = bot;
        this.Doors = doors;
    }

    public override NodeState Evaluate()
    {
        if (Bot.TargetDoor != null && Bot.TargetDoor.IsLocked())
            return NodeState.Success;

        return NodeState.Failure;
    }
}
