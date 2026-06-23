using UnityEngine;

public class GhostHearing : MonoBehaviour
{
    [SerializeField] private float hearingRange;
    [SerializeField] private float hearingThreshold;

    #region PROPERTIES
    public bool HeardNoise { get; private set; }
    public Vector3 LastNoisePosition { get; private set; }
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<GhostHearing>(this);
    }

    public void HearNoise(Vector3 noisePosition, float noiseStrength)
    {
        float distanceFromNoise = Vector3.Distance(this.transform.position, noisePosition);
        float attenuation = 1f - (distanceFromNoise / hearingRange);
        float perceivedNoise = noiseStrength * attenuation;

        if (distanceFromNoise > hearingRange)
            return;

        if (perceivedNoise >= hearingThreshold)
        {
            HeardNoise = true;
            LastNoisePosition = noisePosition;
            Debug.Log("Ghost heard noise");
        }
    }
}
