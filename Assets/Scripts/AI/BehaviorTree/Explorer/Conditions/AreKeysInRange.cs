using System.Collections.Generic;
using UnityEngine;

public class AreKeysInRange : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Key> Keys { get; set; }

    public float ViewDistance { get; set; }
    public float DotThreshold { get; set; }

    public AreKeysInRange(ExplorerBot bot, List<Key> keys, float viewDistance, float dotThreshold)
    {
        this.Bot = bot;
        this.Keys = keys;
        this.ViewDistance = viewDistance;
        this.DotThreshold = dotThreshold;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i <= Keys.Count - 1; i++)
        {
            if (!Keys[i].gameObject.activeSelf)
                continue;

            Vector3 forward = Bot.transform.forward;
            Vector3 directionToTarget = (Keys[i].transform.position - Bot.transform.position).normalized;

            float viewRange = Vector3.Distance(Bot.transform.position, Keys[i].transform.position);
            float dot = Vector3.Dot(forward, directionToTarget);

            bool canSeeKey = viewRange <= ViewDistance && dot > DotThreshold;

            if (canSeeKey)
            {
                Bot.TargetKey = Keys[i];
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
