using System.Collections.Generic;
using UnityEngine;

public class ObjectsRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;

    [SerializeField] private int minObjectsPerRoom = 1;
    [SerializeField] private int maxObjectsPerRoom = 3;

    [SerializeField] private int minGhostsPerRoom = 1;
    [SerializeField] private int maxGhostsPerRoom = 3;

    public void SpawnObject(RoomCell room)
    {
        ObjectsPoint[] points = room.GetComponentsInChildren<ObjectsPoint>();
        List<ObjectsPoint> availablePoints = new List<ObjectsPoint>(points);
        int objectsCount = Random.Range(minObjectsPerRoom, maxObjectsPerRoom + 1);

        for (int i = 0; i < objectsCount; i++)
        {
            if (availablePoints.Count == 0)
                break;

            int randomIndex = Random.Range(0, availablePoints.Count);
            ObjectsPoint randomPoint = availablePoints[randomIndex];

            int randomObjectIndex = Random.Range(0, objectPrefabs.Length);
            GameObject randomObject = objectPrefabs[randomObjectIndex];

            Instantiate(randomObject, randomPoint.transform.position, randomPoint.transform.rotation);
            availablePoints.Remove(randomPoint);
        }
    }
}
