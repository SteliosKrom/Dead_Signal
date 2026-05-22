using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;

    #region DATA
    private float mouseSensitivity = 1f;
    private float xRotation;
    private float yRotation;
    private float baseYRotation;
    #endregion

    #region INPUT
    private PlayerControls playerControls;
    private Vector2 lookInput;
    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Look.performed += OnLook;
        playerControls.Player.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        playerControls.Player.Look.performed -= OnLook;
        playerControls.Player.Look.canceled -= OnLook;
        playerControls.Disable();
    }

    private void Start()
    {
        baseYRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        ApplyRotation();
    }

    public void OnLook(InputAction.CallbackContext cxt)
    {
        lookInput = cxt.ReadValue<Vector2>();
    }

    public void ApplyRotation()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        this.transform.localRotation = Quaternion.Euler(0f, baseYRotation + yRotation, 0f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
