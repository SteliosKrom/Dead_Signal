using UnityEngine;

public class GhostPerception : MonoBehaviour
{
    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private Transform player;
    #endregion

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

    private void Update()
    {
        IsOutOfHearingRange();
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
            Debug.Log("Ghost have heard noise");
        }
    }

    public bool CanSeePlayer(Vector3 playerPosition)
    {
        float viewDistance = Vector3.Distance(playerPosition, this.transform.position);
        return viewDistance <= viewRange;
    }

    public bool IsOutOfHearingRange()
    {
        float hearDistance = Vector3.Distance(this.transform.position, player.position);
        return hearDistance >= hearingRange;
    }
}
