using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 moveDirection;
    private float bulletSpeed = 1f;

    private void Update()
    {
        this.transform.position += moveDirection * bulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        IDamageable damageable = other.collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("Hit damageable!");
        }

        ObjectPoolManager.Instance.ReturnObject("Bullet", this.gameObject);
        Debug.Log("Return bullet!");
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}
