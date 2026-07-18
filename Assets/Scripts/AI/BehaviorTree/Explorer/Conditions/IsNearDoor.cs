using System.Collections.Generic;
using UnityEngine;

public class IsNearDoor : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Door> Doors { get; set; }

    public float DoorStopThreshold { get; set; }

    public IsNearDoor(ExplorerBot bot, List<Door> doors, float doorStopThreshold)
    {
        this.Bot = bot;
        this.Doors = doors;
        this.DoorStopThreshold = doorStopThreshold;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i <= Doors.Count - 1; i++)
        {
            float doorDistance = Vector3.Distance(Bot.transform.position, Doors[i].transform.position);
            bool isNearDoor = doorDistance <= DoorStopThreshold;

            if (isNearDoor)
            {
                Bot.IsGoingToDoor = false;
                Bot.TargetDoor = Doors[i];
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
