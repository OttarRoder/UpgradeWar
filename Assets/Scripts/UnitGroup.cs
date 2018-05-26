using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    private List<int> IDList;
    private List<Unit> UnitList;
    private List<Vector3> UnitOffset;

    public int Number { set; get; }
    public int Team { set; get; }
    public GameObject prefab { set; get; }
    public Vector3 UnitGroupPosition { set; get; }

    public void Awake()
    {
        UnitList = new List<Unit>();
        UnitOffset = new List<Vector3>();
        UnitGroupPosition = transform.position;
    }
}
