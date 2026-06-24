using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;

    private float doorNoiseStrength = 10f;

    #region SERVICES
    private GhostPerception ghostPerception;
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
        ghostPerception = ServiceManager.GetService<GhostPerception>();

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
        ghostPerception.HearNoise(this.transform.position, doorNoiseStrength);
        StartCoroutine(CanInteractCoroutine());
    }

    public IEnumerator CanInteractCoroutine()
    {
        doorHandleCollider.enabled = false;
        doorCollider.enabled = false;

        yield return new WaitForSeconds(canInteractDelay);

        doorHandleCollider.enabled = true;
        doorCollider.enabled = true;
    }

    public bool IsOpen()
    {
        isOpen = !isOpen;
        return isOpen;
    }
}
