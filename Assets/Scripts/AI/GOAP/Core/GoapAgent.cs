using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoapAgent : MonoBehaviour
{
    private bool isWaiting;
    private bool insideCandleRoom;

    [SerializeField] private LayerMask unreachableLayer;
    [SerializeField] private float collisionCheckRadius = 0.3f;

    [SerializeField] private float ghostVisibleTimer;
    [SerializeField] private float ghostVisibleTimeInterval;

    [SerializeField] private float flickerTimer;
    [SerializeField] private float flickerTimeInterval;

    [SerializeField] private float ghostSoundTimer;
    [SerializeField] private float ghostSoundTimeInterval;

    [SerializeField] private float teleportTimer;
    [SerializeField] private float teleportTimeInterval;

    #region GOAP
    private WorldState world;
    private GoapPlanner planner;

    private GoapGoal patrolGoal;
    private GoapGoal chaseGoal;
    private GoapGoal currentGoal;

    private List<GoapAction> actions;
    private List<GoapAction> currentPlan;

    private int currentActionIndex;
    #endregion

    #region AGENT
    [SerializeField] private GhostPerception ghostPerception;
    [SerializeField] private SkinnedMeshRenderer ghostBody;
    [SerializeField] private MeshRenderer ghostHat;
    [SerializeField] private BoxCollider ghostCollider;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotationSpeed = 10f;
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
    [SerializeField] private float stopThreshold = 0.1f;
    private int randomTargetIndex;
    private Transform currentDoorTarget;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] doorTargets;
    #endregion

    #region PROPERTIES
    public WorldState World { get => world; set => world = value; }
    #endregion
    private void Awake()
    {
        // Setup
        world = new WorldState();
        planner = new GoapPlanner();
        actions = new List<GoapAction>();

        //Actions
        GoapAction patrol = new GoapAction();
        patrol.ActionType = GoapActionType.Patrol;
        patrol.Effects.Add(GoapKeys.REACHED_PATROL_POINT, true);
        actions.Add(patrol);

        GoapAction chase = new GoapAction();
        chase.ActionType = GoapActionType.Chase;
        chase.Preconditions.Add(GoapKeys.PLAYER_DETECTED, true);
        chase.Effects.Add(GoapKeys.PLAYER_REACHED, true);
        actions.Add(chase);

        // Goals
        patrolGoal = new GoapGoal(GoapKeys.REACHED_PATROL_POINT, true);
        chaseGoal = new GoapGoal(GoapKeys.PLAYER_REACHED, true);
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
    }

    private void Update()
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        UpdateWorldState();
        UpdateGhostBehaviourFromSanity();

        if (playerSanity.Sanity > 80f)
            return;

        UpdateGoal();
        ExecuteCurrentPlan();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CandleRoom"))
        {
            insideCandleRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CandleRoom"))
        {
            insideCandleRoom = false;
        }
    }

    // GOAP Agent Implementation...
    public void UpdateWorldState()
    {
        world.SetState(GoapKeys.PLAYER_DETECTED, ghostPerception.CanSeePlayer(player.position));
        world.SetState(GoapKeys.REACHED_PATROL_POINT, DistanceToTarget() <= stopThreshold);
    }

    private void ChangeGoal(GoapGoal newGoal)
    {
        if (currentGoal == newGoal)
            return;

        currentGoal = newGoal;
        CreateNewPlan();
    }

    private void UpdateGoal()
    {
        if (playerSanity.Sanity <= 40f && world.GetState(GoapKeys.PLAYER_DETECTED))
        {
            ChangeGoal(chaseGoal);
        }
        else
        {
            ChangeGoal(patrolGoal);
        }
    }

    private void CreateNewPlan()
    {
        currentActionIndex = 0;
        currentPlan = planner.CreatePlan(currentGoal, actions);
    }

    private void ExecuteCurrentPlan()
    {
        if (currentPlan.Count == 0)
            return;

        if (currentActionIndex >= currentPlan.Count)
        {
            CreateNewPlan();
            return;
        }

        GoapAction currentAction = currentPlan[currentActionIndex];

        if (!currentAction.CanExecute(world))
            return;

        switch (currentAction.ActionType)
        {
            case GoapActionType.Patrol:
                GoToTarget();
                currentAction.ApplyEffects(world);
                currentActionIndex++;
                break;
            case GoapActionType.Chase:
                ChasePlayer();
                currentAction.ApplyEffects(world);
                currentActionIndex++;
                break;
        }
    }

    // Rest Implementation...
    public void UpdateGhostBehaviourFromSanity()
    {
        float sanity = playerSanity.Sanity;

        switch (sanity)
        {
            case > 80f:
                DisableGhost();
                break;
            case > 60f:
                ghostCollider.enabled = false;
                ResetGhostTimers();

                if (DistanceToTarget() <= stopThreshold && !isWaiting)
                {
                    // Add door knock sound effect...
                    StartCoroutine(GhostWaitTimeCoroutine());
                }
                break;
            case > 40f:
                ghostCollider.enabled = false;
                TeleportToRandomTargetPoint();
                PatrolBehaviour();
                GenerateAudioVisualCues();
                break;
            case > 20f:
                ghostCollider.enabled = true;
                TeleportToRandomTargetPoint();
                PatrolBehaviour();
                GenerateAudioVisualCues();
                break;
            case >= 0:
                ghostCollider.enabled = true;
                EnableGhost();
                TeleportToRandomTargetPoint();
                PatrolBehaviour();
                GenerateAudioVisualCues();
                break;
        }
    }

    public void EnableGhost()
    {
        ghostHat.enabled = true;
        ghostBody.enabled = true;
    }

    public void DisableGhost()
    {
        ghostCollider.enabled = false;
        ghostBody.enabled = false;
        ghostHat.enabled = false;
        ResetGhostTimers();
    }

    public void PatrolBehaviour()
    { 
        if (DistanceToTarget() <= stopThreshold && !isWaiting)
            StartCoroutine(GhostWaitTimeCoroutine());
    }

    public void GenerateAudioVisualCues()
    {
        ghostVisibleTimer += Time.deltaTime;
        ghostSoundTimer += Time.deltaTime;
        flickerTimer += Time.deltaTime;

        if (ghostSoundTimer >= ghostSoundTimeInterval)
        {
            audioManager.PlaySFX(SoundType.GhostSound);
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

    public void ApplyGhostChaseMovementAndRotation()
    {
        Vector3 directionToPlayer = (player.position - this.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        this.transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
    }

    public void ChasePlayer()
    {
        if (insideCandleRoom)
            return;

        ApplyGhostChaseMovementAndRotation();
    }

    public void TeleportToRandomTargetPoint()
    {
        if (ghostPerception.IsOutOfHearingRange())
        {
            teleportTimer += Time.deltaTime;

            if (teleportTimer >= teleportTimeInterval)
            {
                SelectNewTargetPoint();
                this.transform.position = currentDoorTarget.position;
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
        if (isWaiting)
            return;

        Vector3 directionToDoor = currentDoorTarget.position - this.transform.position;
        Vector3 directionToTarget = (currentDoorTarget.position - this.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToDoor);

        if (DistanceToTarget() > stopThreshold)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            this.transform.position += directionToTarget * moveSpeed * Time.deltaTime;
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

    private void ResetGhostTimers()
    {
        teleportTimer = 0f;
        ghostSoundTimer = 0f;
        ghostVisibleTimer = 0f;
        flickerTimer = 0f;
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(this.transform.position, currentDoorTarget.position);
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
        isWaiting = true;
        ghostWaitTime = Random.Range(2f, 5f);

        yield return new WaitForSeconds(ghostWaitTime);

        SelectNewTargetPoint();

        isWaiting = false;
    }
}
