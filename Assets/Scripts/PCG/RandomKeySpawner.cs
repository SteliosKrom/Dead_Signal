using System.Collections.Generic;
using UnityEngine;

public class RandomKeySpawner : MonoBehaviour
{
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

    #region PROPERTIES
    public GameObject RedKey => redKey;
    public GameObject GreenKey => greenKey;
    public GameObject BlueKey => blueKey;
    #endregion
    private void Start()
    {
        SpawnRandomKeys(redKeysSpawnPoints, greenKeysSpawnPoints, blueKeysSpawnPoints);
    }

    public void SpawnRandomKeys(GameObject[] redSpawnPoints, GameObject[] greenSpawnPoints,
        GameObject[] blueSpawnPoints)
    {
        int randomRedSpawnPointIndex = Random.Range(0, redSpawnPoints.Length);
        RedKey.transform.position = redKeysSpawnPoints[randomRedSpawnPointIndex].transform.position;

        int randomGreenSpawnPointIndex = Random.Range(0, greenSpawnPoints.Length);
        GreenKey.transform.position = greenKeysSpawnPoints[randomGreenSpawnPointIndex].transform.position;

        int randomBlueSpawnPointIndex = Random.Range(0, blueSpawnPoints.Length);
        BlueKey.transform.position = blueKeysSpawnPoints[randomBlueSpawnPointIndex].transform.position;
    }

    public void EnableKeys()
    {
        RedKey.SetActive(true);
        GreenKey.SetActive(true);
        BlueKey.SetActive(true);
    }
}
