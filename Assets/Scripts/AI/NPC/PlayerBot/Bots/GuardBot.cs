using UnityEngine;

public sealed class GuardBot : PlayerBot, IAttackBot, IReloadBot
{
    #region TIMERS
    [Header("ATTACK TIMER")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackTimeInterval; 
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private AttackComponent attackComponent;
    #endregion

    #region AGENT
    [Header("AGENT")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    #endregion

    #region PROPERTIES
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = Mathf.Clamp(value, 0, 10); }
    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float AttackTimeInterval => attackTimeInterval;
    #endregion
    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeBot()
    {
        base.InitializeBot();
        currentRole = BotRole.Guard;
    }

    public void AttackZombie(Vector3 direction)
    {
        attackComponent.PerformAttack(direction);
        CurrentAmmo--;
    }

    public void Reload()
    {
        PlayReloadAnimation();
        CurrentAmmo = MaxAmmo;
    }

    public void PlayReloadAnimation() => botAnimator.SetTrigger("Reload");
}
