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

    //Agent statisitscs
    private float MovementSpeed;
    private float AngularSpeed;
    private float Acceleration;
    private float StoppingDistance;

    //Body statistics
    private float Mass;

    //Unit Display
    public GameObject unitCardPrefab;
    private GameObject unitCard;

    //Team of Unit
    public int Team;

    //Unit Currently being targeted
    private Unit Target;

    //UnitGroup this unit belongs too
    public UnitGroup group;

    private void Awake()
    {
        unitID = globalID;
        globalID++;

        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        attackCollider = transform.GetChild(1).GetComponent<Collider>();

        Team = 0;
        AttackMax = 8;
        AttackMin = 6;
        AttackSpeed = 1;
        HealthMax = 50;
        HealthCurrent = HealthMax;
        CreateUnitCard();
    }

    private void Update()
    {
        unitCard.transform.position = Camera.main.WorldToScreenPoint((Vector3.up * 2f) + transform.position);
        unitCard.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (HealthCurrent / HealthMax);
        if(HealthCurrent <=0 )
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Target = other.gameObject.GetComponent<Unit>();
        if(Target != null)
        {
            if (Target.Team != Team)
            {
                InvokeRepeating("AttackUnit", 0, AttackSpeed);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.CancelInvoke();
        Target = null;
    }

    private void AttackUnit()
    {
        Target.HealthCurrent -= Random.Range(AttackMin, AttackMax);
    }

    private void CreateUnitCard()
    {
        GameObject go = Instantiate(unitCardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(UIController.instance.getCanvas().transform , false);
        unitCard = go;

    }

    public int getID()
    {
        return unitID;
    }

    public void Kill()
    {
        unitCard.SetActive(false);
        gameObject.SetActive(false);
    }

    public void MoveUnit(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
}
