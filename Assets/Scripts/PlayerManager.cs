using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    /*
     * The PlayerManager will handle all player actions that would be specific to a single player
     * for instance selecting a unit, player team information ect.
     */
    public Camera playerCamera;

    public int Team { set; get; }

    private float hitRange = 1000f;
    private Unit SelectedUnit;


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Selection();
        }
        if(Input.GetMouseButtonDown(1) && SelectedUnit != null)
        {
            Vector3 targetPosition = GetPoint();
            if(targetPosition != new Vector3(-1, -1, -1))
            {
                SelectedUnit.MoveUnit(targetPosition);
            }
        }
    }

    private void DeselectUnit()
    {
        SelectedUnit = null;
    }

    //Sets SelectedUnit to the unit under the mous curser by using a raycast
    //from the player camera
    private void Selection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out hit, hitRange, LayerMask.GetMask("Units")))
        {
            SelectedUnit = hit.collider.GetComponent<Unit>();
            Debug.Log("selected" + (SelectedUnit.getID()).ToString());
        }
    }

    //Returns the position of a mouse click on terrain
    private Vector3 GetPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out hit, hitRange, LayerMask.GetMask("Terrain")))
        {
            return hit.point;
        }
        return new Vector3(-1, -1, -1);
    }
}
