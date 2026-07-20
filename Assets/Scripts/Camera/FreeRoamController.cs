using UnityEngine;
using UnityEngine.InputSystem;

public class FreeRoamController : MonoBehaviour
{
    private PlayerControls playerControls;

    #region SERVICES
    private GameManager gameManager;
    private UIManager uiManager;
    #endregion

    [SerializeField] private PlayerController playerController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private SkinnedMeshRenderer[] soldierRenderers;

    [SerializeField] private bool isFreeRoam;
    private bool isSprinting;

    private Vector3 savePlayerPosition;
    private Quaternion savePlayerRotation;

    #region PROPERTIES
    public bool IsFreeRoam { get => isFreeRoam; set => isFreeRoam = value; }
    #endregion
    private void Awake()
    {
        ServiceManager.RegisterService<FreeRoamController>(this);
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.FreeRoam.performed += OnFreeRoamModeInput;

        playerControls.Player.Move.performed += OnMoveInput;
        playerControls.Player.Move.canceled += OnMoveInput;

        playerControls.Player.Sprint.performed += OnSprintInput;
        playerControls.Player.Sprint.canceled += OnSprintInput;
    }

    private void OnDisable()
    {
        playerControls.Player.FreeRoam.performed -= OnFreeRoamModeInput;

        playerControls.Player.Move.performed -= OnMoveInput;
        playerControls.Player.Move.canceled -= OnMoveInput;

        playerControls.Player.Sprint.performed -= OnSprintInput;
        playerControls.Player.Sprint.canceled -= OnSprintInput;

        playerControls.Disable();
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        uiManager = ServiceManager.GetService<UIManager>();

        isFreeRoam = false;
    }

    private void Update()
    {
        if (!isFreeRoam)
            return;

        Move();
    }

    public void OnFreeRoamModeInput(InputAction.CallbackContext cxt)
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        if (isFreeRoam)
            EnableNormalMode();
        else
            EnableFreeRoamMode();
    }

    public void OnMoveInput(InputAction.CallbackContext cxt)
    {
        playerController.MoveInput = cxt.ReadValue<Vector2>();
    }

    public void OnSprintInput(InputAction.CallbackContext cxt)
    {
        isSprinting = cxt.ReadValueAsButton();
    }

    public void Move()
    {
        float speed = isSprinting ? playerController.SprintSpeed : playerController.MoveSpeed;

        Vector3 direction = transform.forward * playerController.MoveInput.y +
            transform.right * playerController.MoveInput.x;

        transform.position += direction * speed * Time.deltaTime;
    }

    public void EnableNormalMode()
    {
        isFreeRoam = false;

        this.transform.position = savePlayerPosition;
        this.transform.rotation = savePlayerRotation;

        playerController.MoveInput = Vector2.zero;
        isSprinting = false;

        playerController.enabled = true;
        characterController.enabled = true;

        uiManager.ShowObject(uiManager.HUDMenu);
        playerController.GunLight.enabled = true;

        foreach (SkinnedMeshRenderer renderer in soldierRenderers)
            renderer.enabled = true;
    }

    public void EnableFreeRoamMode()
    {
        isFreeRoam = true;

        savePlayerPosition = this.transform.position;
        savePlayerRotation = this.transform.rotation;

        playerController.enabled = false;
        characterController.enabled = false;

        uiManager.HideObject(uiManager.HUDMenu);
        playerController.GunLight.enabled = false;

        foreach (SkinnedMeshRenderer renderer in soldierRenderers)
            renderer.enabled = false;
    }
}
