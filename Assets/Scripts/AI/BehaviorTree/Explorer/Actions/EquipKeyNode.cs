using System.Collections.Generic;
using UnityEngine;

public class EquipKeyNode : Node
{
    public ExplorerBot Bot { get; set; }
    public List<Key> Keys { get; set; }

    public EquipKeyNode(ExplorerBot bot, List<Key> keys)
    {
        this.Bot = bot;
        this.Keys = keys;
    }

    public override NodeState Evaluate()
    {
        Bot.HasKey = true;
        Bot.TargetKey.gameObject.SetActive(false);
        Keys.Remove(Bot.TargetKey);

        return NodeState.Success;
    }
}
