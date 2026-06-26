using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoapAgent : MonoBehaviour
{
    private bool isWaiting;

    [SerializeField] private float ghostVisibleTimer;
    [SerializeField] private float ghostVisibleTimeInterval;

    [SerializeField] private float flickerTimer;
    [SerializeField] private float flickerTimeInterval;

    [SerializeField] private float ghostSoundTimer;
    [SerializeField] private float ghostSoundTimeInterval;

    [SerializeField] private float teleportTimer;
    [SerializeField] private float teleportTimeInterval;

    #region GOAP
    private GoapPlanner planner;
    private GoapGoal patrolGoal;
    private GoapGoal currentGoal;
    private List<GoapAction> actions;
    #endregion

    #region AGENT
    [SerializeField] private GhostPerception ghostPerception;
    [SerializeField] private CharacterController ghostCharacterController;
    [SerializeField] private SkinnedMeshRenderer ghostBody;
    [SerializeField] private MeshRenderer ghostHat;
    private float moveSpeed = 5f;
    private float rotationSpeed = 10f;
    #endregion

    #region COROUTINES
    private float ghostWaitTime;
    #endregion

    #region EVENTS
    [SerializeField] private GameEvents gameEvents;
    #endregion

    #region SERVICES
    private GameManager gameManager;
    private AudioManager audioManager;
    private PlayerSanity playerSanity;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerController playerController;
    #endregion

    #region TARGETS
    private float stopThreshold = 0.1f;
    private int randomTargetIndex;
    private Transform currentDoorTarget;
    [SerializeField] private Transform[] doorTargets;
    #endregion

    private void Awake()
    {
        // Setup
        planner = new GoapPlanner();
        actions = new List<GoapAction>();

        //Actions
        GoapAction Patrol = new GoapAction();
        Patrol.Name = "Patrol";
        Patrol.Effects.Add(GoapKeys.REACHED_PATROL_POINT, true);
        actions.Add(Patrol);

        GoapAction Chase = new GoapAction();
        Chase.Name = "Chase";
        Chase.Preconditions.Add(GoapKeys.PLAYER_DETECTED, true);
        Chase.Effects.Add(GoapKeys.NEAR_PLAYER, true);
        actions.Add(Chase);

        // Goals
        patrolGoal = new GoapGoal(GoapKeys.REACHED_PATROL_POINT, true);
        currentGoal = patrolGoal;
    }

    private void OnEnable()
    {
        gameEvents.OnGameplayStarted += InitializeGhost;
    }

    private void OnDisable()
    {
        gameEvents.OnGameplayStarted -= InitializeGhost;
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        audioManager = ServiceManager.GetService<AudioManager>();
        playerSanity = ServiceManager.GetService<PlayerSanity>();

        List<GoapAction> plan = planner.CreatePlan(currentGoal, actions);

        foreach (GoapAction action in plan)
        {
            Debug.Log(action.Name);
        }
    }

    private void Update()
    {
        UpdateGhostBehaviourFromSanity();
    }

    public void UpdateGhostBehaviourFromSanity()
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        float sanity = playerSanity.Sanity;

        if (sanity > 80f)
        {
            ghostBody.enabled = false;
            ghostHat.enabled = false;
        }
        else if (sanity > 60f)
        {
            GoToTarget();

            float distanceToDoor = Vector3.Distance(this.transform.position, currentDoorTarget.position);

            if (distanceToDoor <= stopThreshold && !isWaiting)
            {
                StartCoroutine(GhostWaitTimeCoroutine());
            }
        }
        else if (sanity > 40f)
        {
            TeleportToRandomTargetPoint();
            GoToTarget();
            GenerateAudioVisualCues();
        }
        else if (sanity > 20f)
        {
            // 
        }
    }

    public void GenerateAudioVisualCues()
    {
        ghostVisibleTimer += Time.deltaTime;
        ghostSoundTimer += Time.deltaTime;
        flickerTimer += Time.deltaTime;

        if (ghostSoundTimer >= ghostSoundTimeInterval)
        {
            audioManager.PlaySFX(SoundType.GhostSound);
            ghostBody.enabled = true;
            ghostSoundTimer = 0f;
        }

        if (ghostVisibleTimer >= ghostVisibleTimeInterval)
        {
            StartCoroutine(ShowGhost());
            ghostVisibleTimer = 0f;
        }

        if (flickerTimer >= flickerTimeInterval)
        {
            playerController.GunLight.enabled = !playerController.GunLight.enabled;
            flickerTimer = 0f;
        }
    }

    public void TeleportToRandomTargetPoint()
    {
        if (ghostPerception.IsOutOfHearingRange())
        {
            teleportTimer += Time.deltaTime;

            if (teleportTimer >= teleportTimeInterval)
            {
                TeleportToRandomTargetPoint();
                teleportTimer = 0f;
            }
        }
        else
        {
            teleportTimer = 0f;
        }
    }

    public void GoToTarget()
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        if (isWaiting)
            return;

        Vector3 directionToTarget = (currentDoorTarget.position - this.transform.position).normalized;
        Vector3 directionToDoor = currentDoorTarget.position - this.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(directionToDoor);

        float distanceToDoor = Vector3.Distance(this.transform.position, currentDoorTarget.position);

        if (distanceToDoor > stopThreshold)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            ghostCharacterController.Move(directionToTarget * moveSpeed * Time.deltaTime);
        }
    }

    public void SelectNewTargetPoint()
    {
        int newIndex = Random.Range(0, doorTargets.Length);

        while (newIndex == randomTargetIndex)
        {
            newIndex = Random.Range(0, doorTargets.Length);
        }

        randomTargetIndex = newIndex;
        currentDoorTarget = doorTargets[randomTargetIndex];
    }

    public void InitializeGhost()
    {
        SelectNewTargetPoint();
    }

    public IEnumerator ShowGhost()
    {
        ghostBody.enabled = true;
        ghostHat.enabled = true;

        yield return new WaitForSeconds(ghostVisibleTimeInterval);

        ghostBody.enabled = false;
        ghostHat.enabled = false;
    }

    public IEnumerator GhostWaitTimeCoroutine()
    {
        Debug.Log("Knock knock!");
        isWaiting = true;
        ghostWaitTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(ghostWaitTime);
        SelectNewTargetPoint();
        isWaiting = false;
    }
}
