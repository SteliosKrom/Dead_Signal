using UnityEngine;

public class ZombieInteractor : MonoBehaviour
{
    #region SCRIPT REFERENCES
    private DoorDetectable doorDetectable;
    [SerializeField] private ZombieStateController stateController;
    #endregion

    #region DETECTION
    [SerializeField] private float detectionRadius;
    [SerializeField] private Transform raySource;
    #endregion

    #region PROPERTIES
    public DoorDetectable DoorDetectable { get => doorDetectable; set => doorDetectable = value; }
    #endregion
    private void Update()
    {
        DetectInteractable();
    }

    public void DetectInteractable()
    {
        doorDetectable = null;

        Vector3 forward = transform.forward;
        Ray ray = new Ray(raySource.position, forward);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRadius))
        {
            doorDetectable = hit.collider.GetComponent<DoorDetectable>();
        }
        Debug.DrawRay(raySource.position, forward * detectionRadius, Color.red);
    }
}
