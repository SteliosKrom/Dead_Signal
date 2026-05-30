using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        Debug.Log("Enter Idle");
    }

    public override void Update()
    {
        float moveAmount = stateController.PlayerController.MoveInput.magnitude;

        stateController.PlayerAnimator.SetFloat("MoveX", stateController.PlayerController.MoveInput.x);
        stateController.PlayerAnimator.SetFloat("MoveY", stateController.PlayerController.MoveInput.y);

        if (moveAmount > 0.1f)
        {
            stateController.ChangeState(new MoveState(stateController));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle");
    }
}