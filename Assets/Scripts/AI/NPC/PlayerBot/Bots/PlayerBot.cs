using UnityEngine;

public class PlayerBot : MonoBehaviour
{
    #region BOT
    [Header("BOT")]
    [SerializeField] protected BotRole currentRole;
    #endregion

    public virtual void InitializeBot()
    {
        // Shared logic for bot initialization...
    }
}