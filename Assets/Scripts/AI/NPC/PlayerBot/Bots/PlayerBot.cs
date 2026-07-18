using System;
using UnityEngine;

public abstract class PlayerBot : MonoBehaviour
{
    #region SERVICES
    protected PlayerBotManager playerBotManager;
    #endregion

    #region ACTIONS
    protected Action onIdleFinished;
    #endregion

    #region TIMERS
    [Header("IDLE TIMER")]
    [SerializeField] protected float idleTimer;
    [SerializeField] protected float idleTimeInterval;
    #endregion

    #region BOT
    [Header("BOT")]
    [SerializeField] protected BotRole currentRole;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATORS")]
    [SerializeField] protected Animator botAnimator;
    #endregion

    #region PROPERTIES
    public Animator BotAnimator { get => botAnimator; set => botAnimator = value; }

    public float IdleTimer { get => idleTimer; set => idleTimer = value; }
    public float IdleTimeInterval { get => idleTimeInterval; }
    #endregion

    protected virtual void Start()
    {
        playerBotManager = ServiceManager.GetService<PlayerBotManager>();
    }

    public virtual void InitializeBot()
    {
        PlayIdleAnimation();
    }

    public void ApplyMovementAndRotation(Vector3 direction, float moveSpeed, 
        float rotationSpeed, Quaternion rotation)
    {
        this.transform.position += direction * moveSpeed * Time.deltaTime;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation,
            rotationSpeed * Time.deltaTime);
    }

    public void OnIdleFinished() => onIdleFinished?.Invoke();

    public void PlayIdleAnimation() => botAnimator.SetBool("IsWalking", false);
    public void PlayWalkAnimation() => botAnimator.SetBool("IsWalking", true);
}