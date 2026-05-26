using TMPro;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Transform shootPoint;
    private float currentAmmo;
    private float currentReserveAmmo;

    #region SERVICES
    private UIManager uiManager;
    private GameManager gameManager;
    #endregion

    public float CurrentAmmo 
    {
        get => currentAmmo;
        set
        {
            if (value <= 0)
                currentAmmo = 0;
            else 
                currentAmmo = value;
        }
    }
    public float CurrentReserveAmmo { get => currentReserveAmmo; set => currentReserveAmmo = value; }
    private void Awake()
    {
        shootPoint = GameObject.Find("ShootPoint").GetComponent<Transform>();
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
        gameManager = ServiceManager.GetService<GameManager>();

        CurrentAmmo = 24;
        currentReserveAmmo = 120;
    }

    private void Update()
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;



        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CurrentAmmo <= 0)
                return;

            CurrentAmmo--;
            uiManager.UpdateCurrentAmmoUI();
            GameObject obj = ObjectPoolManager.Instance.GetObject("Bullet");
            obj.transform.position = shootPoint.position;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetDirection(shootPoint.forward);
        }
    }

    public void ResetCurrentAmmo()
    {
        CurrentAmmo = 24;
    }
}
