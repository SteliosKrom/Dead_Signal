using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region INPUT
    private PlayerControls playerControls;
    private Vector2 moveInput;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] CharacterController characterController;
    private float moveSpeed = 2.5f;
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
    }

    private void OnDisable()
    {
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        playerControls.Disable();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void OnMove(InputAction.CallbackContext cxt)
    {
        moveInput = cxt.ReadValue<Vector2>();
    }

    public void ApplyMovement()
    {
        Vector2 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }
}
