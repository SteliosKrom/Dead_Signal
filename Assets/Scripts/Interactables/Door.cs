using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    #region SERVICES
    private UIManager uiManager;
    #endregion

    #region COLLIDERS
    private Collider doorCollider;
    #endregion

    #region COROUTINES
    private float canInteractDelay = 2f;
    #endregion

    private void Awake()
    {
        doorCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
    }

    public void Interact()
    {
        StartCoroutine(CanInteractCoroutine());
    }

    public IEnumerator CanInteractCoroutine()
    {
        doorCollider.enabled = false;
        uiManager.InteractIcon.SetActive(false);
        yield return new WaitForSeconds(canInteractDelay);
        uiManager.InteractIcon.SetActive(true);
        doorCollider.enabled = true;
    }
}
