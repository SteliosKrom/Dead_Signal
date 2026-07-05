using UnityEngine;
using System.Collections.Generic;

public class SoldierBTController : MonoBehaviour
{
    private Transform currentPatrolPoint;
    [SerializeField] private SoldierBot soldierBot;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float stopThreshold;

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
        Node hasReachedPatrolPoint = new HasReachedPatrolPoint(currentPatrolPoint, soldierBot, stopThreshold);
        Node patrol = new PatrolNode(currentPatrolPoint, soldierBot, moveSpeed, rotationSpeed);

        Sequence patrolSequence = new Sequence(new List<Node> { hasReachedPatrolPoint, patrol });
        rootNode = new Selector(new List<Node> { patrolSequence, patrol });
    }
}
