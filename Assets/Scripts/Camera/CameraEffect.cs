using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] private Camera mainMenuCamera;

    private Vector3 initialPos;

    private float movementSpeed = 1;
    private float movementRange = 0.1f; 

    private void Start()
    {
        initialPos = this.transform.position;
    }

    private void Update()
    {
        if (mainMenuCamera.enabled)
        {
            float offsetX = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetY = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetZ = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            mainMenuCamera.transform.position = initialPos + new Vector3(offsetX, offsetY, offsetZ);
        }
    }
}
