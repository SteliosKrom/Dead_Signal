using UnityEngine;

public class ZombieAttackEvents : MonoBehaviour
{
    [SerializeField] private ZombieAttack zombieAttack;

    public void OnResetHit()
    {
        zombieAttack.ResetHit();
    }
}
