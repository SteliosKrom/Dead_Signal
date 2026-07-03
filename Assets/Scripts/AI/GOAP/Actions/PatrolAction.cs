using UnityEngine;
using UnityEngine.LightTransport;

public class PatrolAction : GoapAction
{
    public override void Execute(GoapAgent agent, WorldState world)
    {
        agent.GoToTarget();
        ApplyEffects(world);
    }
}
