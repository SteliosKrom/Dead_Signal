using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool canInteract;

    #region SERVICES
    private GameManager gameManager;
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
    #endregion

    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light gunLight;
    #endregion

    #region PROPERTIES
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
    }

    private void OnDisable()
    {
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        playerControls.Player.Interact.performed -= OnInteract;
        playerControls.Disable();
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        canInteract = true;
    }

    private void Update()
    {
        ApplyMovement();
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

    public void ApplyMovement()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    public IEnumerator CanInteractDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(canInteractDelay);
        canInteract = true;
    }
}
