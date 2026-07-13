using UnityEngine;

public sealed class BodyguardBot : PlayerBot, IAttackBot, IReloadBot
{
    [SerializeField] private bool isGoingToPlayer;

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private Transform player;
    #endregion

    #region TIMERS
    [Header("ATTACK TIMER")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackTimeInterval;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private PathComponent pathComponent;
    #endregion

    #region AGENT 
    [Header("AGENT")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    #endregion

    #region PROPERTIES
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float AttackTimeInterval { get => attackTimeInterval; set => attackTimeInterval = value; }

    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = Mathf.Clamp(value, 0, 10); }
    public int MaxAmmo { get => maxAmmo; }

    public PathComponent PathComponent => pathComponent;
    #endregion
    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Bodyguard;
        FollowPath();
    }

    public void AttackZombie(Vector3 direction)
    {
        attackComponent.PerformAttack(direction);
    }

    public void FollowPath()
    {
        PathComponent.PerformPath(this.transform.position, player.position);
    }

    public void Reload()
    {
        PlayReloadAnimation();
        CurrentAmmo = MaxAmmo;
    }

    public void PlayReloadAnimation() => botAnimator.SetTrigger("Shoot");
}