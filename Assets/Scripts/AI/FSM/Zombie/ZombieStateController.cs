using UnityEngine;
using System.Collections.Generic;

public class ZombieStateController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int randomWaitTime;
    [SerializeField] private int XSec;
    [SerializeField] private int YSec;
    [SerializeField] private int NSec;
    [SerializeField] private int currentNodeIndex;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float nodeReachThreshold;
    [SerializeField] private float attackRange;
    [SerializeField] private float dotThreshold;
    [SerializeField] private float viewDistance;
    [SerializeField] private float timer;

    [SerializeField] private bool canSeePlayer;
    [SerializeField] private bool canSensePlayer;

    private float repathTimer;
    private float repathInterval = 1f;

    #region SERVICES
    private GameManager gameManager;
    #endregion

    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Transform[] patrolPoints;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator zombieAnimator;
    #endregion

    #region PROPERTIES
    public List<AStarNode> Path { get; set; }
    public Pathfinding Pathfinding => pathfinding;
    public Transform[] PatrolPoints => patrolPoints;
    public Transform Player => player;
    public Transform CurrentPatrolPoint { get; set; }
    public Animator ZombieAnimator => zombieAnimator;
    public ZombieState CurrentState { get; set; }

    public float Timer { get => timer; set => timer = value; }
    public float RepathTimer { get => repathTimer; set => repathTimer = value; }
    public float RepathInterval => repathInterval;
    public float SpeedMultiplier => speedMultiplier;
    public float MoveSpeed => moveSpeed;
    public float NodeReachThreshold => nodeReachThreshold;
    public float DotThreshold => dotThreshold;
    public float ViewDistance => viewDistance;
    public float AttackRange => attackRange;

    public int XSEC => XSec;
    public int YSEC => YSec;
    public int NSEC => NSec;
    public int CurrentNodeIndex { get => currentNodeIndex; set => currentNodeIndex = value; }
    public int RandomWaitTime { get => randomWaitTime; set => randomWaitTime = value; }

    public bool CanSeePlayer { get => canSeePlayer; set => canSeePlayer = value; }
    public bool CanSensePlayer { get => canSensePlayer; set => canSensePlayer = value; }
    #endregion

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();

        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        ChangeState(new ZombieIdleState(this));
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    public void ChangeState(ZombieState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
