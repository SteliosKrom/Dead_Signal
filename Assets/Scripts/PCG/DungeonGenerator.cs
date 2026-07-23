using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private float cellSize;
    [SerializeField] private float doorOffset;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private ObjectsRoomManager objectsRoomManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject roomCellPrefab;
    [SerializeField] private GameObject doorPrefab;
    #endregion

    #region GRID
    private RoomCell[,] grid;
    private RoomCell currentCell;
    private Stack<RoomCell> path;
    #endregion

    private void Start()
    {
        grid = new RoomCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = new Vector3(x * cellSize - 10f, 0f, y * cellSize + 20f);
                GameObject room = Instantiate(roomCellPrefab, worldPosition, Quaternion.identity);
                RoomCell cell = room.GetComponent<RoomCell>();

                cell.GridX = x;
                cell.GridY = y;

                grid[x, y] = cell;
            }
        }
        path = new Stack<RoomCell>();
        RoomCell startCell = grid[0, 0];
        currentCell = startCell;
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
        SetRoomTypes();
        SpawnObjects();
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
        if (nextCell.GridX > currentCell.GridX)
        {
            currentCell.EastWall.SetActive(false);
            nextCell.WestWall.SetActive(false);
            SpawnDoor(currentCell, Direction.East);
        }
        else if (nextCell.GridX < currentCell.GridX)
        {
            currentCell.WestWall.SetActive(false);
            nextCell.EastWall.SetActive(false);
            SpawnDoor(currentCell, Direction.West);
        }
        else if (nextCell.GridY > currentCell.GridY)
        {
            currentCell.NorthWall.SetActive(false);
            nextCell.SouthWall.SetActive(false);
            SpawnDoor(currentCell, Direction.North);
        }
        else if (nextCell.GridY < currentCell.GridY)
        {
            currentCell.SouthWall.SetActive(false);
            nextCell.NorthWall.SetActive(false);
            SpawnDoor(currentCell, Direction.South);
        }
    }

    public void SetRoomTypes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].SetRoomType();
            }
        }
    }

    public void SpawnObjects()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                objectsRoomManager.SpawnObject(grid[x, y]);
            }
        }
    }

    public void SpawnDoor(RoomCell room, Direction direction)
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;

        switch (direction)
        {
            case Direction.North:
                targetRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Direction.South:
                targetRotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case Direction.East:
                targetRotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case Direction.West:
                targetRotation = Quaternion.Euler(0f, -90f, 0f);
                break;
        }
        Instantiate(doorPrefab, spawnPosition, targetRotation);
    }
}
