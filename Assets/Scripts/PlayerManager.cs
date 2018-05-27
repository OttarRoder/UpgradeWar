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
    private UnitGroup SelectedUnit;
    private bool attackMove = false;

    private void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            if (attackMove)
                attackMove = false;
            attackMove = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Selection();
        }
        if(Input.GetMouseButtonDown(1) && SelectedUnit != null)
        {
            Vector3 targetPosition = GetPoint();
            if(targetPosition != new Vector3(-1, -1, -1))
            {
                Debug.Log("Moving UnitGroup to positon " + targetPosition.ToString());
                if (attackMove)
                    SelectedUnit.AttackGroup(targetPosition);
                SelectedUnit.MoveGroup(targetPosition);
            }
        }
        if(Input.GetKeyDown("e"))
        {
            UnitManager.instance.GetUnitGroup(GetPoint(), Quaternion.identity, 50, 10, 1.5f);
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
            SelectedUnit = hit.collider.GetComponent<Unit>().group;
            Debug.Log("Selected Unit " + (hit.collider.GetComponent<Unit>().getID()).ToString());
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
