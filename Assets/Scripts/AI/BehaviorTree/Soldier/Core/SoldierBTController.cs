using UnityEngine;

public class SoldierBTController : MonoBehaviour
{
    private Node rootNode;

    [SerializeField] private Transform ammoBox;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SoldierBot soldierBot;
    [SerializeField] private ZombieStateController zombie;
    #endregion

    #region AGENT
    [Header("AGENT")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float ammoBoxStopThreshold;
    [SerializeField] private float nodeThreshold;
    [SerializeField] private float dotThreshold;
    [SerializeField] private float viewDistance;
    #endregion

    private void Start()
    {
        rootNode = new SoldierBTBuilder()
            .SetBot(soldierBot, soldierBot, soldierBot)
            .SetZombie(zombie)
            .SetAmmoBox(ammoBox)
            .SetMovement(moveSpeed)
            .SetRotation(rotationSpeed)
            .SetThresholds(nodeThreshold, ammoBoxStopThreshold)
            .SetVision(viewDistance, dotThreshold)
            .Build();
    }

    private void Update()
    {
        rootNode?.Evaluate();
    }
}
