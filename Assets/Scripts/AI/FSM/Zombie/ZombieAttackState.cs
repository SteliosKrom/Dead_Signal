using UnityEngine;

public sealed class ZombieAttackState : ZombieState
{
    public ZombieAttackState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;
        stateController.Timer = 0f;
        stateController.ZombieAnimator.SetInteger("MovementState", 3);
    }

    public override void Update()
    {
        float distanceToPlayer = Vector3.Distance(stateController.transform.position, stateController.Player.position);

        if (stateController.IsAttackingDoor)
        {
            if (stateController.CurrentDoor == null)
            {
                stateController.ChangeState(new ZombieChaseState(stateController));
                return;
            }

            //Vector3 directionToDoor = stateController.CurrentDoor.transform.position - stateController.transform.position;
            //Quaternion doorTargetRotation = Quaternion.LookRotation(directionToDoor);
            //stateController.transform.rotation = doorTargetRotation;
        }
        else
        {
            Vector3 directionToPlayer = (stateController.Player.position - stateController.transform.position).normalized;
            Quaternion playerTargetRotation = Quaternion.LookRotation(directionToPlayer);
            stateController.transform.rotation = playerTargetRotation;

            if (distanceToPlayer >= stateController.AttackRange)
            {
                stateController.ChangeState(new ZombieChaseState(stateController));
                return;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Attack...");
    }
}
