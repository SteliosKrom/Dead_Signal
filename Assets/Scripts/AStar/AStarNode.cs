using UnityEngine;

public class AStarNode
{
    public bool IsWalkable { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public AStarNode ParentNode { get; set; }
    public int GridX { get; private set; }
    public int GridY { get; private set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;

    public AStarNode(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.IsWalkable = isWalkable;
        this.WorldPosition = worldPosition;
        this.GridX = gridX;
        this.GridY = gridY;
        this.GCost = int.MaxValue;
    }
}
