using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    private List<Unit> unitList;
    private List<Vector3> unitOffset;

    public int Num { set; get; }
    public int Width { set; get; }
    public float Spacing { set; get; }
    public Vector3 UnitGroupPosition { set; get; }
    public Quaternion UnitGroupRotation { set; get; }

    public int Team { set; get; }
    private bool activeEngage;
    private UnitGroup targetGroup;

    //Temp
    public Material enemy;

    private void Awake()
    {
        unitList = new List<Unit>();
        unitOffset = new List<Vector3>();
    }

    private void Update()
    {
        if(unitList.Count == 0)
        {
            UnitManager.instance.KillUnitGroup(this);
        }
    }

    private void OnEnable()
    {
        SpawnGroup();
        InvokeRepeating("FixPosition", 5, 0.5f);
    }

    private void OnDisable()
    {
        if (unitList.Count > 0)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                unitList[i].gameObject.SetActive(false);
                unitList.Clear();
                unitOffset.Clear();
                Num = 0;
            }
        }
    }

    private void SpawnGroup()
    {
        Vector3 vector = UnitGroupPosition;
        int counterX = 0;
        int counterY = 0;
        for (int i = 0; i < Num; i++)
        {
            if (counterX >= Width)
            {
                counterX = 0;
                counterY++;
            }
            Vector3 temp = (Vector3.left * Width / 2) +
                           (Vector3.right * (counterX * Spacing)) +
                           (Vector3.back * (counterY * Spacing));
            unitOffset.Add(temp);

            //Set up unit
            Unit u = UnitManager.instance.GetUnit(vector + temp, transform.rotation);
            unitList.Add(u);
            u.Team = Team;
            u.group = this;

            //Increment Placing
            counterX++;
        }
    }

    private void FixPosition()
    {
        if (!activeEngage)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                unitList[i].MoveUnit(UnitGroupPosition + unitOffset[i]);
            }
        }
        else
        {
            if (!(targetGroup.isActiveAndEnabled))
            {
                targetGroup = null;
                activeEngage = false;
                return;
            }
            else
            {
                AttackGroup(targetGroup);
            }
        }
    }

    private Vector3 ClosestUnit(Vector3 pos)
    {
        int index = 0;
        float dist = Vector3.Distance(pos, unitList[0].gameObject.transform.position);
        float temp = 0;
        for (int i = 0; i < unitList.Count; i++)
        {
            temp = Vector3.Distance(pos, unitList[i].gameObject.transform.position);
            if(temp < dist)
            {
                dist = temp;
                index = i;
            }
        }
        return unitList[index].gameObject.transform.position;
    }


    // Public Unit Group Functions
    public void AttackGroup(UnitGroup target)
    {
        activeEngage = true;
        targetGroup = target;
        UnitGroupPosition = target.UnitGroupPosition;
        for(int i = 0; i < unitList.Count; i++)
        {
            unitList[i].MoveUnit(target.ClosestUnit(unitList[i].gameObject.transform.position));
        }
    }

    public void MoveGroup(Vector3 pos)
    {
        activeEngage = false;
        targetGroup = null;
        UnitGroupPosition = pos;
        for (int i = 0; i < unitList.Count; i++)
        {
            unitList[i].MoveUnit(UnitGroupPosition + unitOffset[i]);
        }
    }

    public void RemoveUnit(Unit u)
    {
        unitList.Remove(u);
    }

    //Temporary Testing Functions
    public void ChangeTeam()
    {
        Team++;
        for(int i = 0; i < unitList.Count; i++)
        {
            unitList[i].Team++;
            Renderer rend = unitList[i].GetComponent<Renderer>();
            rend.material = enemy;
        }
    }
}
