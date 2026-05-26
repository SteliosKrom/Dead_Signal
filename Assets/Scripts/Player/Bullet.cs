using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 moveDirection;
    private float bulletSpeed = 25f;

    private void Update()
    {
        this.transform.position += moveDirection * bulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out IDamageable damageable))
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
