using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private float nodeSize;

    private AStarNode[,] grid;

    private void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new AStarNode[gridWidth, gridHeight];

        float offsetX = (gridWidth * nodeSize) / 2f;
        float offsetY = (gridHeight * nodeSize) / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPosition = new Vector3(x * nodeSize - offsetX, 0f, y * nodeSize - offsetY);
                grid[x, y] = new AStarNode(true, worldPosition, x, y);
            }
        }
    }

    public List<AStarNode> GetNeighbors(AStarNode currentNode)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int neighborX = currentNode.GridX + x;
                int neighborY = currentNode.GridY + y;

                if ((neighborX >= 0 && neighborX < gridWidth) && (neighborY >= 0 && neighborY < gridHeight))
                {
                    neighbors.Add(grid[neighborX, neighborY]);
                }
            }
        }
        return neighbors;
    }

    public AStarNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / (gridWidth * nodeSize);
        float percentY = worldPosition.z / (gridHeight * nodeSize);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridWidth - 1) * percentX);
        int y = Mathf.RoundToInt((gridHeight - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
            return;

        foreach (AStarNode node in grid)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * nodeSize);
        }
    }
}
