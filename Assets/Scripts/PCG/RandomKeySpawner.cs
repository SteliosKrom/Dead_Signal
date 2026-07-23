using System.Collections.Generic;
using UnityEngine;

public class RandomKeySpawner : MonoBehaviour
{
    #region SERVICES
    private GameManager gameManager;
    #endregion

    #region SPAWN POINTS
    [Header("SPAWN POINTS")]
    [SerializeField] private GameObject[] redKeysSpawnPoints;
    [SerializeField] private GameObject[] greenKeysSpawnPoints;
    [SerializeField] private GameObject[] blueKeysSpawnPoints;
    #endregion

    #region KEYS
    [Header("KEYS")]
    [SerializeField] private GameObject redKey;
    [SerializeField] private GameObject greenKey;
    [SerializeField] private GameObject blueKey;
    #endregion

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();

        if (gameManager.CurrentGameState != GameState.Playing)
            return;

        SpawnRandomKeys(redKeysSpawnPoints, greenKeysSpawnPoints, blueKeysSpawnPoints);
    }

    public void SpawnRandomKeys(GameObject[] redSpawnPoints, GameObject[] greenSpawnPoints,
        GameObject[] blueSpawnPoints)
    {
        int randomRedSpawnPointIndex = Random.Range(0, redSpawnPoints.Length);
        redKey.transform.position = redKeysSpawnPoints[randomRedSpawnPointIndex].transform.position;

        int randomGreenSpawnPointIndex = Random.Range(0, greenSpawnPoints.Length);
        greenKey.transform.position = greenKeysSpawnPoints[randomGreenSpawnPointIndex].transform.position;

        int randomBlueSpawnPointIndex = Random.Range(0, blueSpawnPoints.Length);
        blueKey.transform.position = blueKeysSpawnPoints[randomBlueSpawnPointIndex].transform.position;
    }
}
