using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private bool hasHit;

    private void OnCollisionEnter(Collision other)
    {
        if (hasHit)
            return;

        DoorDetectable door = other.collider.GetComponent<DoorDetectable>();

        if (door != null)
        {
            door.TakeHit();
            hasHit = true;
        }
    }

    public void ResetHit()
    {
        hasHit = false;
    }
}
