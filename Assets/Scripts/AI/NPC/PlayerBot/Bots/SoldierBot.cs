using System.Collections;
using UnityEngine;

public sealed class SoldierBot : PlayerBot
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void InitializeBot()
    {
        base.InitializeBot();
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
    }

    public IEnumerator SelectNewPatrolPointCoroutine()
    {
        yield return new WaitForSeconds(InitialRandomWaitTime);
        SelectNewPatrolPoint();
    }
}
