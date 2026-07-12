using UnityEngine;

public class GuardBTController : MonoBehaviour
{
    private Node rootNode;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private GuardBot guardBot;
    [SerializeField] private ZombieStateController zombie;
    #endregion

    #region AGENT
    [Header("AGENT")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float dotThreshold;
    [SerializeField] private float viewDistance;
    #endregion

    private void Start()
    {
        rootNode = new GuardBTBuilder()
            .SetBot(guardBot, guardBot)
            .SetRotation(rotationSpeed)
            .SetZombie(zombie)
            .SetVision(viewDistance, dotThreshold)
            .Build();
    }

    private void Update()
    {
        rootNode?.Evaluate();
    }
}
