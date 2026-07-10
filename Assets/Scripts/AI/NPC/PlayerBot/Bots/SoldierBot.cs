using System.Collections.Generic;
using UnityEngine;

public sealed class SoldierBot : PlayerBot
{
    #region SERVICES
    private ObjectPoolManager objectPoolManager;
    #endregion

    #region BOT
    [Header("BOT")]
    [SerializeField] private bool isGoingToAmmoBox;

    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;

    [SerializeField] private float bulletSpeed;

    [SerializeField] private Transform shootingPoint;
    #endregion

    #region COROUTINES
    [Header("COROUTINES")]
    [SerializeField] private float idleRandomWaitTime;
    #endregion

    #region TIMERS
    [Header("TIMERS")]
    [SerializeField] private float idleTimer;
    [SerializeField] private float idleTimeInterval;

    [SerializeField] private float attackTimer;
    [SerializeField] private float attackTimeInterval;
    #endregion

    #region PARTICLES
    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem gunFX;
    #endregion

    #region PATHFINDING
    [Header("PATHFINDING")]
    [SerializeField] private int currentNodeIndex;
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Transform[] patrolPoints;
    #endregion

    #region PROPERTIES
    public float IdleTimer { get => idleTimer; set => idleTimer = value; }
    public float IdleTimeInterval { get => idleTimeInterval; }
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float AttackTimeInterval { get => attackTimeInterval; }
    public float IdleRandomWaitTime { get => idleRandomWaitTime; set => idleRandomWaitTime = value; }

    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = Mathf.Clamp(value, 0, 10); }
    public int MaxAmmo { get => maxAmmo; }
    public int CurrentNodeIndex { get => currentNodeIndex; set => currentNodeIndex = value; }

    public bool IsGoingToAmmoBox { get => isGoingToAmmoBox; set => isGoingToAmmoBox = value; }

    public List<AStarNode> Path { get; set; }
    public Pathfinding Pathfinding { get => pathfinding; }
    public Transform CurrentPatrolPoint { get; set; }
    public Transform ShootingPoint { get => shootingPoint; set => shootingPoint = value; }
    #endregion

    protected override void Start()
    {
        base.Start();
        objectPoolManager = ServiceManager.GetService<ObjectPoolManager>();
        SelectNewPatrolPoint();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Soldier;
        PlayIdleAnimation();
    }

    public void SelectNewPatrolPoint()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, patrolPoints.Length);
        }
        while (CurrentPatrolPoint == patrolPoints[randomIndex]);

        CurrentPatrolPoint = patrolPoints[randomIndex];
        Path = pathfinding.FindPath(this.transform.position, CurrentPatrolPoint.position);
        CurrentNodeIndex = 0;
    }

    public void AttackZombie(Vector3 directionToTarget)
    {
        GameObject bullet = objectPoolManager.GetObject("Bullet");

        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(directionToTarget);

        bullet.GetComponent<Bullet>().SetDirection(directionToTarget);
        gunFX.Play();

        CurrentAmmo--;
    }
}
