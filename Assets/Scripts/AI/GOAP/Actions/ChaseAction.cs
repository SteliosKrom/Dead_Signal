using UnityEngine;

public class ChaseAction : GoapAction
{
    public override void Execute(GoapAgent agent, WorldState world)
    {
        agent.ChasePlayer();
        ApplyEffects(world);
    }
}
