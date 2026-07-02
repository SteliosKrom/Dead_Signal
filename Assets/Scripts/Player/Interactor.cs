using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private IInteractable currentInteractable;
    private RaycastHit hit;

    #region SERVICES
    private UIManager uiManager;
    private GameManager gameManager;
    #endregion

    #region INPUT
    private PlayerControls playerControls;
    #endregion

    #region DATA
    private float rayDistance = 1.5f;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private CameraController cameraController;
    #endregion

    #region INTERACTION SOURCE
    [SerializeField] private Transform raySource;
    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        playerControls.Player.Interact.performed -= OnInteract;
        playerControls.Disable();
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
        gameManager = ServiceManager.GetService<GameManager>();
    }

    private void Update()
    {
        if (gameManager.CurrentGameState != GameState.Playing) return;
        if (gameManager.IsBotMenuPanelOpen) return;

        DetectInteractable();
    }

    public void OnInteract(InputAction.CallbackContext cxt)
    {
        if (gameManager.IsBotMenuPanelOpen) return;
        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent(out currentInteractable))
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
                currentInteractable.Interact();
        }
    }

    public void DetectInteractable()
    {
        Ray ray = new Ray(cameraController.CameraHolder.position, cameraController.CameraHolder.forward);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out currentInteractable))
            {
                uiManager.ShowObject(uiManager.InteractIcon);
                uiManager.HideObject(uiManager.CrossHair);
            }
            else
            {
                uiManager.ShowObject(uiManager.CrossHair);
                uiManager.HideObject(uiManager.InteractIcon);
            }
        }
        else
        {
            uiManager.ShowObject(uiManager.CrossHair);
            uiManager.HideObject(uiManager.InteractIcon);
            currentInteractable = null;
        }
        Debug.DrawRay(raySource.transform.position, cameraController.CameraHolder.forward * rayDistance, Color.red);
    } 
}
