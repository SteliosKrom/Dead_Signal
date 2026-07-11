using UnityEngine;

public sealed class SoldierBot : PlayerBot, IAttackBot, IPatrolBot
{
    #region TIMERS
    [Header("ATTACK TIMER")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackTimeInterval;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private PatrolComponent patrolComponent;
    #endregion

    #region BOT
    [Header("BOT")]
    [SerializeField] private bool isGoingToAmmoBox;

    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    #endregion

    #region PROPERTIES
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = Mathf.Clamp(value, 0, 10); }
    public int MaxAmmo { get => maxAmmo; }

    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float AttackTimeInterval { get => attackTimeInterval; }

    public bool IsGoingToAmmoBox { get => isGoingToAmmoBox; set => isGoingToAmmoBox = value; }

    public PatrolComponent PatrolComponent => patrolComponent;
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
        currentRole = BotRole.Soldier;
        PlayIdleAnimation();
    }

    public void MoveToPatrolPoint()
    {
        patrolComponent.PerformPatrol();
    }

    public void AttackZombie(Vector3 direction)
    {
        attackComponent.PerformAttack(direction);
        CurrentAmmo--;
    }
}
