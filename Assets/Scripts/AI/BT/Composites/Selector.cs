using System.Collections.Generic;

public class Selector : Node
{
    private List<Node> children = new List<Node>();

    public Selector(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (Node child in children)
        {
            NodeState state = child.Evaluate();

            if (state == NodeState.Success)
            {
                return NodeState.Success;
            }    

            if (state == NodeState.Running)
            {
                return NodeState.Running;
            }
        }
        return NodeState.Failure;
    }
}
