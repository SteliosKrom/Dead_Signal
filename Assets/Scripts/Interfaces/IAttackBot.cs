using UnityEngine;

public interface IAttackBot
{
    float AttackTimer { get; set; }
    float AttackTimeInterval { get; }
    void AttackZombie(Vector3 direction);
}
