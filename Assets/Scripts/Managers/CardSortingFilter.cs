using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSortingFilter : MonoBehaviour
{
    public DeckManager deckManager;

    public GameObject currentABCbuttonObject;
    public GameObject current123buttonObject;
    public GameObject availableABCbuttonObject;
    public GameObject available123buttonObject;

    private GameObject rightClickedObject;

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1) && deckManager.editDeckObject.activeSelf)
        {
            RaycastHit hit;
            //Send a ray from the camera to the mouseposition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Create a raycast from the Camera and output anything it hits
            if (Physics.Raycast(ray, out hit))
            {
                //Check the hit GameObject has a Collider
                if (hit.collider != null)
                {
                    //Click a GameObject to return that GameObject your mouse pointer hit
                    rightClickedObject = hit.collider.gameObject;
                    if (rightClickedObject == currentABCbuttonObject)
                    {
                        Debug.Log("currentABCbuttonObject");
                    }
                    else if(rightClickedObject == current123buttonObject)
                    {
                        Debug.Log("current123buttonObject");
                    }
                    else if (rightClickedObject == availableABCbuttonObject)
                    {
                        Debug.Log("availableABCbuttonObject");
                    }
                    else if (rightClickedObject == available123buttonObject)
                    {
                        Debug.Log("available123buttonObject");
                    }
                }
            }
        }
    }
}
