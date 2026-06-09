using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    public List<AStarNode> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        AStarNode startNode = gridManager.NodeFromWorldPoint(startPos);
        AStarNode targetNode = gridManager.NodeFromWorldPoint(targetPos);

        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedList = new HashSet<AStarNode>();

        openList.Add(startNode);
        startNode.GCost = 0;

        while (openList.Count > 0)
        {
            AStarNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost ||
                    openList[i].FCost == currentNode.FCost &&
                    openList[i].HCost < currentNode.HCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.Log("START == TARGET");
                List<AStarNode> path = RetracePath(startNode, targetNode);
                return path;
            }

            foreach (AStarNode neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                    continue;

                int newMovementCost = currentNode.GCost + 1;

                if (newMovementCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newMovementCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.ParentNode = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    public int GetDistance(AStarNode neighborNode, AStarNode targetNode)
    {
        int distanceX = Mathf.Abs(neighborNode.GridX - targetNode.GridX);
        int distanceY = Mathf.Abs(neighborNode.GridY - targetNode.GridY);

        return distanceX + distanceY;
    }

    public List<AStarNode> RetracePath(AStarNode startNode, AStarNode targetNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }
        path.Reverse();
        return path;
    }
}