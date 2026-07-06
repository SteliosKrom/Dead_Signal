using UnityEngine;

public class PlayerBot : MonoBehaviour
{
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
    #endregion
    protected virtual void Start()
    {
        // Shared logic for bot initialization, when the game starts...
    }

    public virtual void InitializeBot()
    {
        // Shared logic for bot initialization, when the player choses a bot from the bot menu...
    }
}