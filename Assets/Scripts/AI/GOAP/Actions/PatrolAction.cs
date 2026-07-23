using UnityEngine;

public class PatrolAction : GoapAction
{
    public override void Execute(GoapAgent agent, WorldState world)
    {
        agent.GoToTarget();
        ApplyEffects(world);
    }
}
