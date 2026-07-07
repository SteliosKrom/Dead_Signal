using UnityEngine;
using System.Collections.Generic;

public class SoldierBTController : MonoBehaviour
{
    [SerializeField] private SoldierBot soldierBot;
    [SerializeField] private ZombieStateController zombie;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float stopThreshold;
    [SerializeField] private float viewDistance;
    [SerializeField] private float dotThreshold;

    private Node rootNode;

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
        Node hasReachedPatrolPoint = new HasReachedPatrolPoint(soldierBot, stopThreshold);
        Node isZombieInRange = new IsZombieInRange(soldierBot, zombie, viewDistance, dotThreshold);

        Node idle = new IdleNode(soldierBot);
        Node patrol = new PatrolNode(soldierBot, moveSpeed, rotationSpeed, stopThreshold);
        Node attack = new AttackNode(soldierBot, zombie, rotationSpeed);

        Sequence idleSequence = new Sequence(new List<Node> { hasReachedPatrolPoint, idle });
        Sequence attackSequence = new Sequence(new List<Node> { isZombieInRange, attack });

        rootNode = new Selector(new List<Node> { attackSequence, idleSequence, patrol });
    }
}
