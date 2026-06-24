using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool canInteract;
    private bool isSprinting;

    #region SERVICES
    private GameManager gameManager;
    private UIManager uiManager;
    private GhostPerception ghostPerception;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Shoot shoot;
    #endregion

    #region INPUT
    private PlayerControls playerControls;
    private Vector2 moveInput;
    #endregion

    #region COROUTINES
    private float canInteractDelay = 1f;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] CharacterController characterController;
    private float moveSpeed = 2.5f;
    private float sprintSpeed = 4f;
    private float sprintNoiseStrength = 15f;
    #endregion

    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light gunLight;
    #endregion

    #region PROPERTIES
    public Light GunLight { get => gunLight; set => gunLight = value; }
    public Vector2 MoveInput => moveInput;
    #endregion
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;

        playerControls.Player.Interact.performed += OnInteract;
        playerControls.Player.Interact.performed += OnReload;

        playerControls.Player.Sprint.performed += OnSprint;
        playerControls.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;

        playerControls.Player.Interact.performed -= OnInteract;
        playerControls.Player.Interact.performed -= OnReload;

        playerControls.Player.Sprint.performed -= OnSprint;
        playerControls.Player.Sprint.canceled -= OnSprint;

        playerControls.Disable();
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        uiManager = ServiceManager.GetService<UIManager>();
        ghostPerception = ServiceManager.GetService<GhostPerception>();

        canInteract = true;
    }

    private void Update()
    {
        if (isSprinting)
            ghostPerception.HearNoise(this.transform.position, sprintNoiseStrength);

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
        ApplyMovement(currentSpeed);
    }

    private void OnMove(InputAction.CallbackContext cxt)
    {
        moveInput = cxt.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext cxt)
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        if (canInteract)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                if (gunLight.enabled)
                    gunLight.enabled = false;
                else
                    gunLight.enabled = true;
            }
            StartCoroutine(CanInteractDelay());
        }
    }

    private void OnReload(InputAction.CallbackContext cxt)
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (shoot.CurrentReserveAmmo <= 0)
                return;

            float missingAmmo = 24 - shoot.CurrentAmmo;

            if (shoot.CurrentReserveAmmo >= missingAmmo)
            {
                shoot.CurrentReserveAmmo -= missingAmmo;
                shoot.ResetCurrentAmmo();
            }
            else
            {
                shoot.CurrentAmmo += shoot.CurrentReserveAmmo;
                shoot.CurrentReserveAmmo = 0f;
            }
                uiManager.UpdateFullAmmoCapacityUI();
        }
    }

    private void OnSprint(InputAction.CallbackContext cxt)
    {
        isSprinting = cxt.ReadValueAsButton();
    }

    public void ApplyMovement(float currentSpeed)
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
    }

    public IEnumerator CanInteractDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(canInteractDelay);
        canInteract = true;
    }
}
