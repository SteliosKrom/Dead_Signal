using System.Collections.Generic;

public class GoapPlanner
{
    public List<GoapAction> CreatePlan(GoapGoal goal, List<GoapAction> actions)
    {
        List<GoapAction> plan = new List<GoapAction>();
        string currentState = goal.StateKey;

        while (true)
        {
            GoapAction action = FindAction(currentState, actions);

            if (action == null)
                break;

            plan.Add(action);

            if (action.Preconditions.Count == 0)
                break;

            foreach (KeyValuePair<string, bool> precondition in action.Preconditions)
            {
                currentState = precondition.Key;
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
                return action;
        }
        return null;
    }
}
