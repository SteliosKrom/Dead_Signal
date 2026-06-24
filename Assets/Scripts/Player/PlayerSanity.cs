using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float sanity = 100f;

    #region SERVICES
    private GhostPerception ghostPerception;
    private UIManager uiManager;
    private GameManager gameManager;
    #endregion

    #region SCRIPT REFERENCES
    [SerializeField] private PlayerController playerController;
    #endregion

    #region PROPERTIES
    public float Sanity
    {
        get => sanity;
        set => sanity = Mathf.Clamp(value, 0f, 100f);
    }
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<PlayerSanity>(this);
    }

    private void Start()
    {
        ghostPerception = ServiceManager.GetService<GhostPerception>();
        uiManager = ServiceManager.GetService<UIManager>();
        gameManager = ServiceManager.GetService<GameManager>();
    }

    private void Update()
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        DecreaseSanityWithinHearingRange();
        DecreaseSanityWithinViewRange();
        DecreaseSanityInDarkness();
    }

    private void OnTriggerStay(Collider other)
    {
        other = GameObject.Find("CandleRoomCollider").GetComponent<BoxCollider>();

        if (!other.gameObject.CompareTag("CandleRoom"))
            return;

        Sanity += 10 * Time.deltaTime;
        uiManager.UpdateSanityCounterUI();
    }

    public void DecreaseSanityWithinHearingRange()
    {
        float distanceFromGhost = Vector3.Distance(this.transform.position, ghostPerception.transform.position);

        if (distanceFromGhost > ghostPerception.HearingRange)
            return;

        Sanity -= Time.deltaTime;
        uiManager.UpdateSanityCounterUI();
    }

    public void DecreaseSanityWithinViewRange()
    {
        if (!ghostPerception.CanSeePlayer(this.transform.position))
            return;

        Sanity -= Time.deltaTime;
        uiManager.UpdateSanityCounterUI();
    }

    public void DecreaseSanityInDarkness()
    {
        if (playerController.GunLight.enabled)
            return;

        Sanity -= Time.deltaTime;
        uiManager.UpdateSanityCounterUI();
    }
}
