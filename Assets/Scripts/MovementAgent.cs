using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    //Current unit position and rotation
    private Vector3 pos;
    private Quaternion rot;

    //Unit states
    private bool WaitingForPath;
    private bool ReachedGoal;
    private bool IncrementWaypoint;

    //Waypoint List
    private Vector3 CurrentWaypoint;
    private Queue<Vector3> Waypoints;

    //Unit Move Attributes
    private float MoveSpeed;
    private Vector3 MoveVector;

    private void Start()
    {
        pos = transform.position;
        rot = transform.rotation;
        MoveVector = Vector3.forward * MoveSpeed;
    }

    private void FixedUpdate()
    {
        if (IncrementWaypoint)
        {
            GetNextWaypoint();
            if (Waypoints.Count == 0)
            {
                SetState("ReachedGoal");
            }
            else
            {
                SetState("WaitingForPath");
            }
        }
        if(ReachedGoal)
        {
            //Actions such as ending animations should be here
            return;
        }
        if(WaitingForPath)
        {
            //Find Path and Save it
            //if(no path)
            // {
            //     Exit Function
            // }

            Quaternion facing = FindDirection(CurrentWaypoint);
            Vector3 futurePosition = pos + (facing * MoveVector);

            // if(Collision)
            // {
            //     SetState("WaitingForPath")
            //     Return to top of loop
            // }

            if(Vector3.Distance(CurrentWaypoint, pos) < Vector3.Distance(CurrentWaypoint, futurePosition))
            {
                SetState("IncrementWaypoint");
                // Return to top of loop
            }
            if(Vector3.Distance(CurrentWaypoint, pos) < MoveVector.magnitude)
            {
                SetState("IncrementWaypoint");
                return;
            }
        }
        //Set Accelerations
        //Commit Move
        //Update Predicted Positions
    }

    //Removes the current waypoint and gets the next from the list
    private void GetNextWaypoint()
    {
        if (Waypoints.Count > 0)
        {
            CurrentWaypoint = Waypoints.Dequeue();
        }
    }

    // Simple state selector, for the exclusive states "ReachedGoal"
    // "WaitingForPath" and "IncrementWaypoint"
    private void SetState(string state)
    {
        if(state == "ReachedGoal")
        {
            ReachedGoal = true;
            WaitingForPath = false;
            IncrementWaypoint = false;
        }
        else if(state == "WaitingForPath")
        {
            WaitingForPath = true;
            ReachedGoal = false;
            IncrementWaypoint = false;
        }
        else if(state == "IncrementWaypoint")
        {
            IncrementWaypoint = true;
            WaitingForPath = false;
            ReachedGoal = false;
        }
        else
        {
            Debug.LogError("Not a valid State",this);
        }
    }

    // Returns the quaternion for facing towards the current waypoint
    private Quaternion FindDirection(Vector3 target)
    {
        return Quaternion.LookRotation(target - pos);
    }
}
