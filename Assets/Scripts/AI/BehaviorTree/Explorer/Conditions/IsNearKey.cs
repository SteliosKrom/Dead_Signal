using System.Collections.Generic;
using UnityEngine;

public class IsNearKey : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Key> Keys { get; set; }

    public float KeyStopThreshold { get; set; }

    public IsNearKey(ExplorerBot bot, List<Key> keys, float keyStopThreshold)
    {
        this.Bot = bot;
        this.Keys = keys;
        this.KeyStopThreshold = keyStopThreshold;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i <= Keys.Count - 1; i ++)
        {
            float keyDistance = Vector3.Distance(Bot.transform.position, Keys[i].transform.position);
            bool isNearKey = keyDistance <= KeyStopThreshold;

            if (isNearKey)
            {
                Bot.IsGoingToKey = false;
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
