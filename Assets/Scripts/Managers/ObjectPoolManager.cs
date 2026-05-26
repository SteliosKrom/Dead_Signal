using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Serializable]
    public class PoolItems
    {
        public Transform parent;
        public string type;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<PoolItems> pools;
    private Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        poolDict = new Dictionary<string, Queue<GameObject>>();
    }

    private void Start()
    {
        foreach (PoolItems item in pools)
        {
            Queue<GameObject> objects = new Queue<GameObject>();

            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = Instantiate(item.prefab, item.parent);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }
            poolDict.Add(item.type, objects);
        }
    }

    public GameObject GetObject(string type)
    {
        if (!poolDict.ContainsKey(type))
        {
            Debug.Log("Key not found!");
            return null;
        }

        GameObject obj = poolDict[type].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(string type, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}
