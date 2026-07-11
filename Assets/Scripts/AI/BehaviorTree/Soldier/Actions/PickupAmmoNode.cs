using UnityEngine;

public class PickupAmmoNode : Node
{
    public SoldierBot Bot { get; set; }

    public PickupAmmoNode(SoldierBot bot)
    {
        this.Bot = bot;
    }

    public override NodeState Evaluate()
    {
        Bot.IsGoingToAmmoBox = false;
        Bot.CurrentAmmo = Bot.MaxAmmo;
        Bot.PatrolComponent.CurrentNodeIndex = 0;
        return NodeState.Success;
    }
}
