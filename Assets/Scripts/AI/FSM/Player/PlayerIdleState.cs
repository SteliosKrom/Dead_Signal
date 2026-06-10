using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        //
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
        //
    }
}