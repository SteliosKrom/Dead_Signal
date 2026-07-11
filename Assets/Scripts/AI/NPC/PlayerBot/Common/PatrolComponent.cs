using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PatrolComponent : MonoBehaviour
{
    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator botAnimator;
    #endregion

    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private int currentNodeIndex;
    #endregion

    #region PROPERTIES
    public int CurrentNodeIndex { get => currentNodeIndex; set => currentNodeIndex = value; }
    public Transform CurrentPatrolPoint { get; set; }
    public Pathfinding Pathfinding { get => pathfinding; }
    public List<AStarNode> Path { get; set; }
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
        Path = pathfinding.FindPath(this.transform.position, CurrentPatrolPoint.position);
        currentNodeIndex = 0;
    }

    public void PlayWalkAnimation() => botAnimator.SetBool("IsWalking", true);
}
