using Unity.VisualScripting;
using UnityEngine;

public sealed class ZombieChaseState : ZombieState
{
    public ZombieChaseState(ZombieStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.ZombieAnimator.SetInteger("MovementState", 2);
    }

    public override void Update()
    {
        Vector3 futurePlayerPosition = stateController.Player.position + stateController.PlayerVelocity * stateController.PredictionTime;

        Vector3 directionToPlayer = (futurePlayerPosition - stateController.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        float distance = Vector3.Distance(stateController.transform.position, stateController.Player.transform.position);

        if (distance >= stateController.ViewDistance)
            stateController.CanSeePlayer = false;
        else
            stateController.CanSeePlayer = true;

        if (stateController.CanSeePlayer)
        {
            stateController.Timer = 0f;

            ApplyChaseMovement(directionToPlayer, targetRotation);
            MoveToAttackState(distance);
        }
        else
        {
            ExtendChase(directionToPlayer, targetRotation);
            MoveToAttackState(distance);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Chase...");
    }

    public void ApplyChaseMovement(Vector3 directionToPlayer, Quaternion targetRotation)
    {
        stateController.transform.position += directionToPlayer * stateController.MoveSpeed *
            stateController.SpeedMultiplier * Time.deltaTime;

        stateController.transform.rotation = targetRotation;
    }

    public void ExtendChase(Vector3 directionToPlayer, Quaternion targetRotation)
    {
        stateController.Timer += Time.deltaTime;

        if (stateController.Timer >= stateController.NSEC)
        {
            MoveToIdleState();
            return;
        }
        ApplyChaseMovement(directionToPlayer, targetRotation);
    }

    public void MoveToAttackState(float distance)
    {
        if (distance <= stateController.AttackRange)
        {
            stateController.ChangeState(new ZombieAttackState(stateController));
            return;
        }
    }

    public void MoveToIdleState()
    {
        if (!stateController.CanSeePlayer)
        {
            stateController.ChangeState(new ZombieIdleState(stateController));
            return;
        }
    }
}
