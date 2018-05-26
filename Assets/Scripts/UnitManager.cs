using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    //Pool of Units
    private int pooledUnits = 500;
    private List<GameObject> unitPool;

    //Pool of Unit Groups
    private int pooledGroups = 50;
    private List<GameObject> unitGroupPool;

    public GameObject unitPrefab;
    public GameObject unitGroupPrefab;

    private void Start()
    {
        //Global reference
        instance = this;

        //Start up unit Pool
        unitPool = new List<GameObject>();
        for(int i = 0; i < pooledUnits; i++)
        {
            GameObject go = (GameObject)Instantiate(unitPrefab);
            go.SetActive(false);
            unitPool.Add(go);
        }

        //Start up unitGroup Pool
        unitGroupPool = new List<GameObject>();
        for(int i = 0; i < pooledGroups; i++)
        {
            GameObject go = (GameObject)Instantiate(unitPrefab);
            go.SetActive(false);
            unitPool.Add(go);
        }
    }


    //Unit Group Commands
    public void SpawnUnitGroup(Vector3 position, Quaternion rotation, GameObject prefab, int n)
    {
    }

    public void MoveUnitGroup(UnitGroup group)
    {
    }

    public void KillUnitGroup(UnitGroup group)
    {
    }

    public void KillUnit(Unit u)
    {
    }
}
