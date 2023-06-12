using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour//, IDeselectHandler //IPointerClickHandler, IDeselectHandler
{
}
/*    public bool IsSelected { get; private set; } = false;
    public GameObject m_MyGameObject;
    void Update()
    {
        //Check if there is a mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //Send a ray from the camera to the mouseposition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Create a raycast from the Camera and output anything it hits
            if (Physics.Raycast(ray, out hit))
                //Check the hit GameObject has a Collider
                if (hit.collider != null)
                {
                    //Click a GameObject to return that GameObject your mouse pointer hit
                    m_MyGameObject = hit.collider.gameObject;
                    //Set this GameObject you clicked as the currently selected in the EventSystem
                    EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                    //Output the current selected GameObject's name to the console
                    Debug.Log("Current selected GameObject : " + EventSystem.current.currentSelectedGameObject);
                    if (IsSelected == false)
                    {
                        EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                        IsSelected = true;
                        Debug.Log(IsSelected);
                    }
                    else if (IsSelected == true)
                    {
                        IsSelected = false;
                        DeckManager.deckId = -1;
                        EventSystem.current.SetSelectedGameObject(null);
                        //DeckManager.deckIdCheck = 0;
                        Debug.Log(IsSelected);
                    }
                }
        }
    }
}*/
/*    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log(IsSelected);
        if (IsSelected == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            IsSelected = false;
            Debug.Log(IsSelected);

           // DeckManager.deckId = -1;//test

        }
*//*        else if (IsSelected == false)
        {
            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
            IsSelected = true;
            Debug.Log(IsSelected);
        }*//*

    }*/

/*    public void OnDeselect(BaseEventData data)
    {

        if (DeckManager.deckIdCheck == 1) //checks to see if a deck is selected, if it is and then it's deselected, reset deckID back to -1 and deckID back to 0
        {
            DeckManager.deckId = -1;
            DeckManager.deckIdCheck = 0;
            *//*Debug.Log(DeckManager.deckIdCheck);
            Debug.Log(DeckManager.deckId);*//*
        }

        IsSelected = false;
        //DeckManager.deckProfile.sprite = DeckManager.tootDeckPreview.sprite; test
        Debug.Log(IsSelected);
    }*/
/*}*/
