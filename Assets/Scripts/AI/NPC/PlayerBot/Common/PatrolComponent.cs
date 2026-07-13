using System.Collections.Generic;
using UnityEngine;

public class PatrolComponent : MonoBehaviour
{
    [SerializeField] private PathComponent pathComponent;

    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] private Transform[] patrolPoints;
    #endregion

    #region PROPERTIES
    public Transform CurrentPatrolPoint { get; set; }
    #endregion

    private void Start()
    {
        PerformPatrol();
    }

    public void PerformPatrol()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, patrolPoints.Length);
        }
        while (CurrentPatrolPoint == patrolPoints[randomIndex]);

        CurrentPatrolPoint = patrolPoints[randomIndex];
        pathComponent.PerformPath(this.transform.position, CurrentPatrolPoint.position);
    }
}
