using UnityEngine;

public sealed class ZombieAttackState : ZombieState
{
    public ZombieAttackState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.CurrentNodeIndex = 0;
        stateController.ZombieTimer = 0f;
        stateController.ZombieAnimator.SetInteger("MovementState", 3);
    }

    public override void Update()
    {
        float distanceToPlayer = Vector3.Distance(stateController.transform.position, 
            stateController.Player.position);

        if (stateController.IsAttackingDoor)
        {
            if (stateController.CurrentDoor != stateController.ZombieInteractor.DoorDetectable)
            {
                stateController.CurrentDoor = null;
                stateController.IsAttackingDoor = false;

                stateController.ChangeState(new ZombieChaseState(stateController));
                return;
            }
        }
        else
        {
            Vector3 directionToPlayer = (stateController.Player.position - 
                stateController.transform.position);

            directionToPlayer.y = 0;
            directionToPlayer.Normalize();
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
