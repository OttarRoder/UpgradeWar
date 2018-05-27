using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    //Unique unit id
    private static int globalID = 0;
    private int unitID;

    //Unit components
    private NavMeshAgent agent;
    private Rigidbody body;
    private Collider attackCollider;

    //Unit statistics
    private float HealthCurrent;
    private float HealthMax;
    private float AttackMax;
    private float AttackMin;
    private float AttackSpeed;
    private float AttackRange;

    //Agent statisitscs
    private float MovementSpeed;
    private float AngularSpeed;
    private float Acceleration;
    private float StoppingDistance;

    //Body statistics
    private float Mass;

    //Unit Display
    private GameObject unitCard;

    //Groups that the Unit belongs to
    public int Team;
    public UnitGroup group;

    //Combat Variables
    private List<Unit> Targets;
    private bool Combat;


    //Private Methods
    private void Awake()
    {
        unitID = globalID;
        globalID++;

        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Targets = new List<Unit>();
        attackCollider = transform.GetChild(1).GetComponent<Collider>();

        AttackMax = 8;
        AttackMin = 6;
        AttackSpeed = 1.5f;
        AttackRange = 3;
        HealthMax = 25;
        HealthCurrent = HealthMax;
    }

    private void Update()
    {
        unitCard.transform.position = Camera.main.WorldToScreenPoint((Vector3.up * 2f) + transform.position);
        unitCard.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (HealthCurrent / HealthMax);

        // If this unit has 0 health or less, kill it
        if (HealthCurrent <= 0)
        {
            UnitManager.instance.KillUnit(this);
        }
        // Remove out of range or dead targets
        RemoveTargets();

        //If there is targets and not in combat, initiate combat, if not targets end combat
        if(Targets.Count > 0 && Combat == false)
        {
            Combat = true;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<NavMeshObstacle>().enabled = true;
            InvokeRepeating("Attack", 0, AttackSpeed);
        }
        if(Targets.Count == 0 && Combat == true)
        {
            Combat = false;
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            CancelInvoke();
        }
    }

    private void OnDisable()
    {
        if (unitCard != null)
        {
            unitCard.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameObject go = ObjectPooler.instance.GetPooledObject(0);
        go.transform.SetParent(UIController.instance.getCanvas().transform , false);
        unitCard = go;
        unitCard.SetActive(true);

        HealthCurrent = HealthMax;
        Team = 0;
        Combat = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Unit")
        {
            Unit u = other.gameObject.GetComponent<Unit>();
            if(u.Team != Team)
            {
                Targets.Add(u);
            }
        }
    }

    private void RemoveTargets()
    {
        if(Targets.Count > 0)
        {
            if (!(Targets[0].isActiveAndEnabled))
            {
                Targets.RemoveAt(0);
                RemoveTargets();
            }
            else if (Vector3.Distance(transform.position, Targets[0].transform.position) > AttackRange)
            {
                Targets.RemoveAt(0);
                RemoveTargets();
            }
        }
    }

    private Vector3 ClosestTarget()
    {
        float dist = 0;
        float lowestDist = AttackRange;
        int t = 0;
        for (int i = 0; i < Targets.Count; i++)
        {
            dist = Vector3.Distance(transform.position, Targets[i].gameObject.transform.position);
            if (dist < lowestDist)
            {
                lowestDist = dist;
                t = i;
            }
        }
        return Targets[t].transform.position;
    }

    //Public Methods
    public int getID()
    {
        return unitID;
    }

    public void Attack()
    {
        Targets[0].HealthCurrent -= Random.Range(AttackMin, AttackMax);
    }

    public void MoveUnit(Vector3 targetPosition)
    {
        if (!Combat)
        {
            agent.SetDestination(targetPosition);
        }
    }
}
