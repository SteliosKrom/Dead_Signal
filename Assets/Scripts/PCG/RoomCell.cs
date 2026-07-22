using UnityEngine;

public class RoomCell : MonoBehaviour
{
    [SerializeField] private bool visited;

    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    [SerializeField] private RoomType roomType;

    #region OBJECTS
    [Header("WALLS")]
    [SerializeField] private GameObject northWall;
    [SerializeField] private GameObject southWall;
    [SerializeField] private GameObject eastWall;
    [SerializeField] private GameObject westWall;
    #endregion

    #region PROPERTIES
    public bool Visited { get => visited; set => visited = value; }

    public int GridX { get => gridX; set => gridX = value; }
    public int GridY { get => gridY; set => gridY = value; }

    public GameObject NorthWall => northWall;
    public GameObject SouthWall => southWall;
    public GameObject EastWall => eastWall;
    public GameObject WestWall => westWall;
    #endregion

    public int CountOpenConnections()
    {
        int openConnections = 0;

        if (!NorthWall.activeSelf)
            openConnections++;

        if (!SouthWall.activeSelf)
            openConnections++;

        if (!EastWall.activeSelf)
            openConnections++;

        if (!WestWall.activeSelf)
            openConnections++;

        return openConnections;
    }

    public void SetRoomType()
    {
        int openConnections = CountOpenConnections();

        switch (openConnections)
        {
            case 1:
                roomType = RoomType.DeadEnd;
                break;
            case 2:
                if (IsCorridor())
                    roomType = RoomType.Corridor;
                else if (IsCorner())
                    roomType = RoomType.Corner;
                break;
            case 3:
                roomType = RoomType.TJunction;
                break;
            case 4:
                roomType = RoomType.Cross;
                break;
        }
    }

    public bool IsCorridor()
    {
        if (HasCorridorOpenings())
            return true;

        return false;
    }

    public bool IsCorner()
    {
        if (HasCornerOpenings())
            return true;

        return false;
    }

    public bool HasCorridorOpenings()
    {
        return !NorthWall.activeSelf && !SouthWall.activeSelf
            || !EastWall.activeSelf && !WestWall.activeSelf;
    }

    public bool HasCornerOpenings()
    {
        return !NorthWall.activeSelf && !EastWall.activeSelf
            || !NorthWall.activeSelf && !WestWall.activeSelf
            || !SouthWall.activeSelf && !EastWall.activeSelf
            || !SouthWall.activeSelf && !WestWall.activeSelf;
    }
}
