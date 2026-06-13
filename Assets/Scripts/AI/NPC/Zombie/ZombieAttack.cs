using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<BoxCollider>())
        {
            Debug.Log("Zombie has hit the door!");
        }
    }
}
