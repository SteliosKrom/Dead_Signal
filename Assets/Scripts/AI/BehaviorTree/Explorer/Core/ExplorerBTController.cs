using System.Collections.Generic;
using UnityEngine;

public class ExplorerBTController : MonoBehaviour
{
    private Node rootNode;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private ExplorerBot explorerBot;
    [SerializeField] private List<Key> keys;
    [SerializeField] private List<Door> doors;
    #endregion

    #region AGENT
    [Header("AGENT")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float nodeThreshold;
    [SerializeField] private float doorStopThreshold;
    [SerializeField] private float keyStopThreshold;
    [SerializeField] private float viewDistance;
    [SerializeField] private float dotThreshold;
    #endregion

    private void Start()
    {
        rootNode = new ExplorerBTBuilder()
            .SetBot(explorerBot, explorerBot)
            .SetSpeed(moveSpeed, rotationSpeed)
            .SetKeys(keys)
            .SetDoors(doors)
            .SetThresholds(nodeThreshold, keyStopThreshold, doorStopThreshold)
            .SetVision(viewDistance, dotThreshold)
            .Build();
    }

    private void Update()
    {
        rootNode?.Evaluate();
    }
}
