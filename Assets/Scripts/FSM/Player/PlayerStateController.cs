using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerState currentState;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerController playerController;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator playerAnimator;
    #endregion

    #region PROPERTIES
    public Animator PlayerAnimator => playerAnimator;
    public PlayerController PlayerController => playerController;
    #endregion
    private void Start()
    {
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
