using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private Transform shootPoint;
    private float currentAmmo;
    private float currentReserveAmmo;
    private float shootNoiseStrength = 20f;

    #region SERVICES
    private AudioManager audioManager;
    private UIManager uiManager;
    private GameManager gameManager;
    private ObjectPoolManager poolManager;
    private FreeRoamController freeRoamController;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private GhostPerception ghostPerception;
    #endregion

    #region INPUT
    private PlayerControls playerControls;
    #endregion

    #region PARTICLES
    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem gunFX;
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
        playerControls = new PlayerControls();
        shootPoint = GameObject.Find("ShootPoint").GetComponent<Transform>();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Shoot.performed += OnShoot;
        playerControls.Player.Shoot.canceled += OnShoot;
    }

    private void OnDisable()
    {
        playerControls.Player.Shoot.performed -= OnShoot;
        playerControls.Player.Shoot.canceled -= OnShoot;

        playerControls.Disable();
    }

    private void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();
        uiManager = ServiceManager.GetService<UIManager>();
        gameManager = ServiceManager.GetService<GameManager>();
        poolManager = ServiceManager.GetService<ObjectPoolManager>();
        freeRoamController = ServiceManager.GetService<FreeRoamController>();

        CurrentAmmo = 24;
        currentReserveAmmo = 120;
    }

    public void OnShoot(InputAction.CallbackContext cxt)
    {
        if (gameManager.CurrentGameState != GameState.Playing) return;
        if (gameManager.IsBotMenuPanelOpen) return;
        if (freeRoamController.IsFreeRoam) return;
        if (CurrentAmmo <= 0)
        {
            if (!audioManager.IsPlaying(SoundType.OutOfAmmo))
                audioManager.PlaySFX(SoundType.OutOfAmmo);
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {


            CurrentAmmo--;
            uiManager.UpdateCurrentAmmoUI();
            GameObject obj = poolManager.GetObject("Bullet");
            obj.transform.position = shootPoint.position;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetDirection(shootPoint.forward);
            gunFX.Play();
            audioManager.PlaySFX(SoundType.Shoot);
        }
        ghostPerception.HearNoise(this.transform.position, shootNoiseStrength);
    }

    public void ResetCurrentAmmo()
    {
        CurrentAmmo = 24;
    }
}
