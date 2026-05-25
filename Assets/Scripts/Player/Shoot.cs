using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Transform shootPoint;

    private void Awake()
    {
        shootPoint = GameObject.Find("ShootPoint").GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject("Bullet");
            obj.transform.position = shootPoint.position;
            Bullet bullet = GetComponent<Bullet>();
            bullet.SetDirection(transform.forward);
        }
    }
}
