using UnityEngine;

public class ZombieHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int zombieCurrentLives;
    [SerializeField] private ZombieStateController zombieStateController;

    #region PROPERTIES
    public int ZombieCurrentLives { get => zombieCurrentLives; set => zombieCurrentLives = Mathf.Clamp(value, 0, 3); }
    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        Debug.Log("HAs taken damage!");
        ZombieCurrentLives--;

        if (ZombieCurrentLives == 0)
            zombieStateController.ZombieAnimator.SetTrigger("Died");
    }
}
