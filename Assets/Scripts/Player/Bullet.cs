using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region SERVICES
    private ObjectPoolManager poolManager;
    #endregion

    private Vector3 moveDirection;
    private float bulletSpeed = 25f;

    private void Start()
    {
        poolManager = ServiceManager.GetService<ObjectPoolManager>();
    }

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
        poolManager.ReturnObject("Bullet", this.gameObject);
        Debug.Log("Return bullet!");
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}
