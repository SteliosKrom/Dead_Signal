using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    #region STRUCTS
    private Vector3 initialPos;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera menuCamera;
    #endregion

    #region DATA STRUCTURES
    private float movementSpeed = 1;
    private float movementRange = 0.1f;
    #endregion

    private void Start()
    {
        initialPos = this.transform.position;
    }

    private void Update()
    {
        if (menuCamera.enabled)
        {
            float offsetX = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetY = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetZ = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            menuCamera.transform.position = initialPos + new Vector3(offsetX, offsetY, offsetZ);
        }
    }
}
