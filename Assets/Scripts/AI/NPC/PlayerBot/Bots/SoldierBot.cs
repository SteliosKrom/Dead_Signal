using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SoldierBot : PlayerBot
{
    [SerializeField] private float idleRandomWaitTime;
    [SerializeField] private int currentNodeIndex;

    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Transform[] patrolPoints;
    #endregion

    #region PROPERTIES
    public int CurrentNodeIndex { get => currentNodeIndex; set => currentNodeIndex = value; }
    public List<AStarNode> Path { get; set; }
    public Transform CurrentPatrolPoint { get; set; }
    #endregion

    private void Start()
    {
        SelectNewPatrolPoint();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Soldier;
    }

    public void BeginPatrolWait()
    {
        StartCoroutine(SelectNewPatrolPointCoroutine());
    }

    public void SelectNewPatrolPoint()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, patrolPoints.Length);
        }
        while (CurrentPatrolPoint == patrolPoints[randomIndex]);

        CurrentPatrolPoint = patrolPoints[randomIndex];
        Path = pathfinding.FindPath(this.transform.position, CurrentPatrolPoint.position);
        CurrentNodeIndex = 0;
    }

    public IEnumerator SelectNewPatrolPointCoroutine()
    {
        yield return new WaitForSeconds(idleRandomWaitTime);
        SelectNewPatrolPoint();
    }
}
