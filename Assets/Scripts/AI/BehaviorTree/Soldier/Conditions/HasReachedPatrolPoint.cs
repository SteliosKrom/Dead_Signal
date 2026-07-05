using UnityEngine;

public class HasReachedPatrolPoint : Node
{
    private Transform CurrentPatrolPoint { get; set; }
    private SoldierBot Bot { get; set; }
    private float StopThreshold { get; set; }

    public HasReachedPatrolPoint(Transform currentPatrolPoint, SoldierBot bot, float stopThreshold)
    {
        this.CurrentPatrolPoint = currentPatrolPoint;
        this.Bot = bot;
        this.StopThreshold = stopThreshold;
    }

    public override NodeState Evaluate()
    {
        float stopDistance = Vector3.Distance(CurrentPatrolPoint.position, Bot.transform.position);

        if (stopDistance <= StopThreshold)
        {
            Bot.SelectNewPatrolPointCoroutine();
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}
