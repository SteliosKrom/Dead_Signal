using UnityEngine;

public class BTController : MonoBehaviour
{
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
        //
    }
}
