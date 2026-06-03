using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build;

public class ZombieStateController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int randomWaitTime;
    [SerializeField] private int XSec;
    [SerializeField] private int YSec;
    [SerializeField] private int NSec;
    [SerializeField] private int currentNodeIndex;

    private float moveSpeed = 1f;
    private float rotationSpeed = 100f;
    private float stopThreshold = 0.2f;
    [SerializeField] private float viewDistance = 5f;
    [SerializeField] private float timer = 0f;

    [SerializeField] private bool canSeePlayer;
    [SerializeField] private bool canSensePlayer;

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
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float StopThreshold => stopThreshold;
    public float ViewDistance => viewDistance;

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

    public void InitializePathfinding()
    {
        RandomWaitTime = Random.Range(XSec, YSec);
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, patrolPoints.Length);
        } 
        while (CurrentPatrolPoint == PatrolPoints[randomIndex]);

        CurrentPatrolPoint = patrolPoints[randomIndex];
    }
}
