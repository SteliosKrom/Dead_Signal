using UnityEngine;

public sealed class ZombieAttackState : ZombieState
{
    public ZombieAttackState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;
        stateController.ZombieAnimator.SetInteger("MovementState", 3);
    }

    public override void Update()
    {
        Vector3 directionToPlayer = (stateController.Player.position - stateController.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        float distance = Vector3.Distance(stateController.transform.position, stateController.Player.position);
        stateController.transform.rotation = targetRotation;

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
