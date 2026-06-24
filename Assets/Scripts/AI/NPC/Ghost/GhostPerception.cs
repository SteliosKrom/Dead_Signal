using UnityEngine;

public class GhostPerception : MonoBehaviour
{
    #region GHOST
    [Header("GHOST")]
    [SerializeField] private float viewRange;
    [SerializeField] private float hearingRange;
    [SerializeField] private float hearingThreshold;
    [SerializeField] private float dotThreshold;
    #endregion

    #region PROPERTIES
    public float ViewRange => viewRange;
    public float HearingRange => hearingRange;
    public bool HeardNoise { get; private set; }
    public Vector3 LastNoisePosition { get; private set; }
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<GhostPerception>(this);
    }

    public bool CanSeePlayer(Vector3 playerPosition)
    {
        Vector3 forward = transform.forward;
        Vector3 directionToPlayer = playerPosition - this.transform.position;
        float viewDistance = Vector3.Distance(playerPosition, this.transform.position);
        float dot = Vector3.Dot(forward, directionToPlayer);

        if (viewDistance <= viewRange && dot > dotThreshold)
            return true;

        return false;
    }

    public void HearNoise(Vector3 noisePosition, float noiseStrength)
    {
        float distanceFromNoise = Vector3.Distance(this.transform.position, noisePosition);

        if (distanceFromNoise > hearingRange)
            return;

        float attenuation = 1f - (distanceFromNoise / hearingRange);
        float perceivedNoise = noiseStrength * attenuation;

        if (perceivedNoise >= hearingThreshold)
        {
            HeardNoise = true;
            LastNoisePosition = noisePosition;
            Debug.Log("Ghost heard noise");
        }
    }
}
