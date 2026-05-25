using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;

    #region SERVICES
    private UIManager uiManager;
    #endregion

    #region COLLIDERS
    private Collider doorHandleCollider;
    [SerializeField] private Collider doorCollider;
    #endregion

    #region COROUTINES
    private float canInteractDelay = 1f;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator doorAnimator;
    #endregion

    private void Awake()
    {
        doorHandleCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();

        isOpen = false;
    }

    public void Interact()
    {
        if (IsOpen())
        {
            doorAnimator.SetTrigger("Open");
            isOpen = true;
        }
        else
        {
            doorAnimator.SetTrigger("Close");
            isOpen = false;
        }
        StartCoroutine(CanInteractCoroutine());
    }

    public IEnumerator CanInteractCoroutine()
    {
        doorHandleCollider.enabled = false;
        doorCollider.enabled = false;
        uiManager.InteractIcon.SetActive(false);
        yield return new WaitForSeconds(canInteractDelay);
        uiManager.InteractIcon.SetActive(true);
        doorHandleCollider.enabled = true;
        doorCollider.enabled = true;
    }

    public bool IsOpen()
    {
        isOpen = !isOpen;
        return isOpen;
    }
}
