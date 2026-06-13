using UnityEngine;

public class ZombieInteractor : MonoBehaviour
{
    [SerializeField] private bool doorDetected;

    #region INTERFACES
    private DoorDetectable doorDetectable;
    #endregion

    #region DETECTION
    [SerializeField] private float detectionRadius;
    [SerializeField] private Transform raySource;
    #endregion

    #region PROPERTIES
    public DoorDetectable DoorDetectable { get => doorDetectable; set => doorDetectable = value; }
    public bool DoorDetected { get => doorDetected; set => doorDetected = value; }
    #endregion
    private void Update()
    {
        DetectInteractable();
    }

    public void DetectInteractable()
    {
        doorDetectable = null;
        doorDetected = false;

        Vector3 forward = transform.forward;
        Ray ray = new Ray(raySource.position, forward);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRadius))
        {
            doorDetectable = hit.collider.GetComponent<DoorDetectable>();

            if (doorDetectable != null)
            {
                doorDetected = true;
            }
        }
        Debug.DrawRay(raySource.position, forward * detectionRadius, Color.red);
    }
}
