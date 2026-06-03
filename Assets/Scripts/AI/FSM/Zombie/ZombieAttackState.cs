using UnityEngine;

public sealed class ZombieAttackState : ZombieState
{
    public ZombieAttackState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.ZombieAnimator.SetInteger("MovementState", 3);
    }

    public override void Update()
    {
        float distance = Vector3.Distance(stateController.transform.position, stateController.Player.position);

        if (distance >= stateController.AttackRange)
        {
            stateController.ChangeState(new ZombieChaseState(stateController));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Attack...");
    }
}
