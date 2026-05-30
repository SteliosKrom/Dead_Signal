using UnityEngine;

public class ZombieStateController : MonoBehaviour
{
    private ZombieState currentState;

    private void Start()
    {
        ChangeState(new ZombieIdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(ZombieState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
