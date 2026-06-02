using UnityEngine;

public sealed class ZombieChaseState : ZombieState
{
    public ZombieChaseState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.ZombieAnimator.SetBool("IsRunning", true);
    }

    public override void Update()
    {
        Vector3 directionToPlayer = (stateController.Player.position - stateController.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        float distance = Vector3.Distance(stateController.transform.position, stateController.Player.transform.position);

        if (distance >= stateController.ViewDistance)
            stateController.CanSeePlayer = false;

        if (stateController.CanSeePlayer)
        {
            stateController.transform.position += directionToPlayer * stateController.MoveSpeed * 1 * Time.deltaTime;
            stateController.transform.rotation = targetRotation;

            if (distance <= stateController.StopThreshold)
            {
                stateController.ChangeState(new ZombieAttackState(stateController));
                return;
            }
        }
        else
        {
            stateController.Timer += Time.deltaTime;

            if (stateController.Timer <= stateController.NSEC)
            {
                stateController.transform.position += directionToPlayer * stateController.MoveSpeed * 1 * Time.deltaTime;
                stateController.transform.rotation = targetRotation;

                if (distance <= stateController.StopThreshold)
                {
                    stateController.ChangeState(new ZombieAttackState(stateController));
                    stateController.Timer = 0f;
                    return;
                }

                if (!stateController.CanSeePlayer)
                {
                    stateController.ChangeState(new ZombieIdleState(stateController));
                    stateController.Timer = 0f;
                    return;
                }
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Chase...");
    }
}
