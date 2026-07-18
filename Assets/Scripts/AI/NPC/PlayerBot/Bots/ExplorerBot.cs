using UnityEngine;

public sealed class ExplorerBot : PlayerBot, IFollowBot, IPatrolBot
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PatrolComponent patrolComponent;
    [SerializeField] private PathComponent pathComponent;
    #endregion

    #region AGENT
    [Header("AGENT")]
    [SerializeField] private bool isGoingToKey;
    [SerializeField] private bool isGoingToDoor;
    [SerializeField] private bool hasKey;
    #endregion

    #region PROPERTIES
    public PatrolComponent PatrolComponent => patrolComponent;
    public PathComponent PathComponent => pathComponent;

    public bool IsGoingToKey { get => isGoingToKey; set => isGoingToKey = value; }
    public bool IsGoingToDoor { get => isGoingToDoor; set => isGoingToDoor = value; }
    public bool HasKey { get => hasKey; set => hasKey = value; }

    public Key TargetKey { get; set; }
    public Door TargetDoor { get; set; }
    #endregion
    private void OnEnable()
    {
        onIdleFinished += MoveToPatrolPoint;
    }

    private void OnDisable()
    {
        onIdleFinished -= MoveToPatrolPoint;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Explorer;
    }

    public void MoveToPatrolPoint()
    {
        PatrolComponent.PerformPatrol();
    }

    public void FollowPath()
    {
        // Future implementation
    }
}
