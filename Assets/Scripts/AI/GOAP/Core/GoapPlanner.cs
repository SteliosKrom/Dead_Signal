using System.Collections.Generic;

public class GoapPlanner
{
    public List<GoapAction> CreatePlan(GoapGoal goal, List<GoapAction> actions)
    {
        List<GoapAction> plan = new List<GoapAction>();

        while (true)
        {
            GoapAction action = FindAction(goal.StateKey, actions);
            plan.Add(action);

            if (action.Preconditions.Count == 0)
                break;

            foreach (KeyValuePair<string, bool> precondition in action.Preconditions)
            {
                goal.StateKey = precondition.Key;
                break;
            }
        }
        plan.Reverse();
        return plan;
    }

    public GoapAction FindAction(string stateKey, List<GoapAction> actions)
    {
        foreach (GoapAction action in actions)
        {
            if (action.Effects.ContainsKey(stateKey))
            {
                return action;
            }
        }
        return null;
    }
}
