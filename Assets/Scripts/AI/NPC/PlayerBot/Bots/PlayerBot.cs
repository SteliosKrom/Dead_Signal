using UnityEngine;

public class PlayerBot : MonoBehaviour
{
    [SerializeField] private float initialRandomWaitTime;

    #region BOT
    [Header("BOT")]
    [SerializeField] protected BotRole currentRole;
    #endregion

    #region TIMERS
    [Header("TIMERS")]
    [SerializeField] protected float idleTimer;
    protected float idleTimeInterval;
    #endregion

    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] protected Transform[] patrolPoints;
    #endregion

    #region PROPERTIES
    public float IdleTimer { get => idleTimer; set => idleTimer = value; }
    public float IdleTimeInterval { get => idleTimeInterval; set => idleTimeInterval = value; }
    public float InitialRandomWaitTime { get => initialRandomWaitTime; set => initialRandomWaitTime = value; }
    public Transform CurrentPatrolPoint { get; set; }
    public BotRole CurrentRole { get => currentRole; set => currentRole = value; }
    #endregion
    protected virtual void Start()
    {
        currentRole = BotRole.None;
    }

    protected virtual void InitializeBot()
    {
        // Shared logic for bot initialization...
    }
}