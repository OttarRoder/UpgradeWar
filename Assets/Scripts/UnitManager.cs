using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //Global Reference
    public static UnitManager instance;

    private List<UnitGroup> activeUnitGroups;
    private List<Unit> activeUnits;

    private void Awake()
    {
        instance = this;

        activeUnitGroups = new List<UnitGroup>();
        activeUnits = new List<Unit>();
    }


    //Unit Group Commands
    public UnitGroup GetUnitGroup(Vector3 position, Quaternion rotation, int n, int w, float s)
    {
        UnitGroup group = ObjectPooler.instance.GetPooledObject(2).GetComponent<UnitGroup>();
        group.Num = n;
        group.Width = w;
        group.Spacing = s;
        group.UnitGroupPosition = position;
        group.UnitGroupRotation = rotation;

        group.gameObject.SetActive(true);
        activeUnitGroups.Add(group);
        return group;
    }

    public void KillUnitGroup(UnitGroup group)
    {
        activeUnitGroups.Remove(group);
        group.gameObject.SetActive(false);
    }

    //Unit Commands
    public Unit GetUnit(Vector3 position, Quaternion rotation)
    {
        Unit u = ObjectPooler.instance.GetPooledObject(1).GetComponent<Unit>();
        u.gameObject.transform.position = position;
        u.gameObject.transform.rotation = rotation;
        u.gameObject.SetActive(true);

        activeUnits.Add(u);
        return u;
    }

    public void KillUnit(Unit u)
    {
        activeUnits.Remove(u);
        u.group.RemoveUnit(u);
        u.transform.position = Vector3.zero;
        u.gameObject.SetActive(false);
    }
}
