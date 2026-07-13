using UnityEngine;

public class BodyguardBTController : MonoBehaviour
{
    private Node rootNode;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private BodyguardBot bodyguardBot;
    [SerializeField] private ZombieStateController zombie;
    [SerializeField] private PlayerController player;
    #endregion

    #region AGENT
    [Header("AGENT")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float nodeThreshold;
    [SerializeField] private float playerStopThreshold;
    [SerializeField] private float zombieStopThreshold;
    [SerializeField] private float viewDistance;
    [SerializeField] private float dotThreshold;
    #endregion

    private void Start()
    {
        rootNode = new BodyguardBTBuilder()
            .SetBot(bodyguardBot, bodyguardBot)
            .SetPlayer(player)
            .SetZombie(zombie)
            .SetMovement(moveSpeed)
            .SetRotation(rotationSpeed)
            .SetThresholds(nodeThreshold, playerStopThreshold, zombieStopThreshold)
            .SetVision(viewDistance, dotThreshold)
            .Build();
    }

    private void Update()
    {
        rootNode?.Evaluate();
    }
}
