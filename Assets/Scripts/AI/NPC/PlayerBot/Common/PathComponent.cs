using System.Collections.Generic;
using UnityEngine;

public class PathComponent : MonoBehaviour
{
    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private int currentNodeIndex;
    #endregion

    #region PROPERTIES
    public int CurrentNodeIndex { get => currentNodeIndex; set => currentNodeIndex = value; }
    public Pathfinding Pathfinding { get => pathfinding; }
    public List<AStarNode> Path { get; set; }
    #endregion

    public void PerformPath(Vector3 startPos, Vector3 targetPos)
    {
        Path = Pathfinding.FindPath(startPos, targetPos);
        CurrentNodeIndex = 0;
    }
}
