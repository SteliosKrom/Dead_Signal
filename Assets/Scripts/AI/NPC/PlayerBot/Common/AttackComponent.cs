using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private ParticleSystem gunFX;

    #region SERVICES
    private ObjectPoolManager objectPoolManager;
    #endregion

    #region PROPERTIES
    public Transform ShootingPoint { get => shootingPoint; set => shootingPoint = value; }
    #endregion
    private void Start()
    {
        objectPoolManager = ServiceManager.GetService<ObjectPoolManager>();
    }

    public void PerformAttack(Vector3 direction)
    {
        GameObject bullet = objectPoolManager.GetObject("Bullet");

        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        bullet.GetComponent<Bullet>().SetDirection(direction);
        gunFX.Play();
    }
}
