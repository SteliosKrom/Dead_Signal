using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomCellPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private float cellSize;

    private RoomCell[,] grid;
    private RoomCell currentCell;
    private Stack<RoomCell> path;

    private void Start()
    {
        grid = new RoomCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = new Vector3(x * cellSize, 0f, y * cellSize);
                GameObject room = Instantiate(roomCellPrefab, worldPosition, Quaternion.identity);

                RoomCell cell = room.GetComponent<RoomCell>();

                cell.GridX = x;
                cell.GridY = y;

                grid[x, y] = cell;
            }
        }
        path = new Stack<RoomCell>();
        currentCell = grid[0, 0];
        currentCell.Visited = true;
        path.Push(currentCell);

        while (path.Count > 0)
        {
            List<RoomCell> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count > 0)
            {
                int randomIndex = Random.Range(0, neighbors.Count);
                RoomCell nextCell = neighbors[randomIndex];

                RemoveWall(currentCell, nextCell);
                nextCell.Visited = true;

                path.Push(nextCell);
                currentCell = nextCell;
            }
            else
            {
                path.Pop();

                if (path.Count > 0)
                    currentCell = path.Peek();
            }
        }
    }

    public List<RoomCell> GetUnvisitedNeighbors(RoomCell currentCell)
    {
        List<RoomCell> neighbors = new List<RoomCell>();

        if (currentCell.GridX > 0)
        {
            RoomCell left = grid[currentCell.GridX - 1, currentCell.GridY];

            if (!left.Visited)
                neighbors.Add(left);
        }

        if (currentCell.GridX < width - 1)
        {
            RoomCell right = grid[currentCell.GridX + 1, currentCell.GridY];

            if (!right.Visited)
                neighbors.Add(right);
        }

        if (currentCell.GridY > 0)
        {
            RoomCell down = grid[currentCell.GridX, currentCell.GridY - 1];

            if (!down.Visited)
                neighbors.Add(down);
        }

        if (currentCell.GridY < height - 1)
        {
            RoomCell up = grid[currentCell.GridX, currentCell.GridY + 1];

            if (!up.Visited)
                neighbors.Add(up);
        }
        return neighbors;
    }

    public void RemoveWall(RoomCell currentCell, RoomCell nextCell)
    {
        if (nextCell.GridX < currentCell.GridX)
        {
            currentCell.WestWall.SetActive(false);
            nextCell.EastWall.SetActive(false);
        }
        else if (nextCell.GridX > currentCell.GridX)
        {
            currentCell.EastWall.SetActive(false);
            nextCell.WestWall.SetActive(false);
        }
        else if (nextCell.GridY < currentCell.GridY)
        {
            currentCell.SouthWall.SetActive(false);
            nextCell.NorthWall.SetActive(false);
        }
        else if (nextCell.GridY > currentCell.GridY)
        {
            currentCell.NorthWall.SetActive(false);
            nextCell.SouthWall.SetActive(false);
        }
    }
}
