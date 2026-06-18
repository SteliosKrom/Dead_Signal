using UnityEngine;
using System.Collections.Generic;

public class GoapAgent : MonoBehaviour
{
    private Planner planner;
    private Goal currentGoal;
    private List<GoapAction> actions;

    private void Awake()
    {
        // Setup
        planner = new Planner();
        actions = new List<GoapAction>();

        //Actions
        GoapAction Patrol = new GoapAction();
        Patrol.Name = "Patrol";
        Patrol.Effects.Add(GoapKeys.REACHED_PATROL_POINT, true);
        actions.Add(Patrol);

        GoapAction Chase = new GoapAction();
        Chase.Name = "Chase";
        Chase.Preconditions.Add(GoapKeys.PLAYER_DETECTED, true);
        Chase.Effects.Add(GoapKeys.NEAR_PLAYER, true);
        actions.Add(Chase);
    }

    private void Start()
    {
        List<GoapAction> plan = planner.CreatePlan(currentGoal, actions);

        foreach (GoapAction action in plan)
        {
            Debug.Log(action.Name);
        }
    }
}
