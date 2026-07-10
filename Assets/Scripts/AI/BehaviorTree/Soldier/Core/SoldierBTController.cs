using UnityEngine;
using System.Collections.Generic;

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

    #region PROPERTIES
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    #endregion
    private void Start()
    {
        SetupTree();
    }

    private void Update()
    {
        rootNode?.Evaluate();
    }

    public void SetupTree()
    {
        // Actions
        Node idle = new IdleNode(soldierBot);
        Node patrol = new PatrolNode(soldierBot, moveSpeed, rotationSpeed, nodeThreshold);
        Node attack = new AttackNode(soldierBot, zombie, rotationSpeed);
        Node moveToAmmoBox = new MoveToAmmoBoxNode(soldierBot, ammoBox, ammoBoxStopThreshold, nodeThreshold, moveSpeed, rotationSpeed);
        Node pickupAmmo = new PickupAmmoNode(soldierBot);

        // Conditions
        Node hasReachedPatrolPoint = new HasReachedPatrolPoint(soldierBot);
        Node isZombieInRange = new IsZombieInRange(soldierBot, zombie, viewDistance, dotThreshold);
        Node isOutOfAmmo = new IsOutOfAmmo(soldierBot);

        // Sequences
        Sequence idleSequence = new Sequence(new List<Node> { hasReachedPatrolPoint, idle });
        Sequence attackSequence = new Sequence(new List<Node> { isZombieInRange, attack });
        Sequence moveToAmmoBoxSequence = new Sequence(new List<Node> { isOutOfAmmo, moveToAmmoBox, pickupAmmo });

        // Root
        rootNode = new Selector(new List<Node> { moveToAmmoBoxSequence, attackSequence, idleSequence, patrol });
    }
}
