using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public List<GameObject> objectPrefabs;
    public List<int> pooledAmounts;

    private List<List<GameObject>> pooledObjects;

    private void Awake()
    {
        instance = this;
        if(objectPrefabs.Count != pooledAmounts.Count)
        {
            Debug.LogError("Size of objectPrefabs must equal size of pooledAmounts");
        }
    }

    void Start ()
    {
        pooledObjects = new List<List<GameObject>>();
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            pooledObjects.Add(new List<GameObject>());
            for (int j = 0; j < pooledAmounts[i]; j++)
            {
                GameObject go = (GameObject)Instantiate(objectPrefabs[i]);
                go.SetActive(false);
                pooledObjects[i].Add(go);
            }
        }
	}

    public GameObject GetPooledObject(int n)
    {
        if(n >= objectPrefabs.Count)
        {
            return null;
        }

        for(int i = 0; i < pooledObjects[n].Count; i++)
        {
            if(!pooledObjects[n][i].activeInHierarchy)
            {
                return pooledObjects[n][i];
            }
        }

        GameObject go = (GameObject)Instantiate(objectPrefabs[n]);
        pooledObjects[n].Add(go);
        return go;
    }
}
