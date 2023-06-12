using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    public DeckManager deckManager;

    public GameObject toolTip;
    public static bool isDraggable = false;

    GameObject selectedCardDropped;
    public Transform selectedCardOriginalParent; //gets set on drop
    public bool dropCheck; //checks to make sure a card was dropped in the right area

    public GameObject currentCardsSortDropZone;
    public Transform currentCardsSortTransformDropZone;

    public GameObject availableCardsSortDropZone;
    public Transform availableCardsSortTransformDropZone;

    public int currentCount;

    void Start()
    {
        dropCheck = false;
        currentCardsSortDropZone = GameObject.FindGameObjectWithTag("ContentCurrent");
        availableCardsSortDropZone = GameObject.FindGameObjectWithTag("ContentAvailable");

        selectedCardOriginalParent = gameObject.transform; //set initial value
        currentCount = 0; //set initial value
    }
    public void OnDrop(PointerEventData eventData) // When user drop the card on drop zone
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            currentCardsSortTransformDropZone = currentCardsSortDropZone.transform;

            availableCardsSortTransformDropZone = availableCardsSortDropZone.transform;

            selectedCardDropped = eventData.pointerDrag.gameObject;

            if (selectedCardDropped != null && selectedCardDropped.transform.GetChild(0).name == "ID")
            {
                dropCheck = true;
                if (DeckManager.isMatch && DeckManager.hasToken)
                {
                    if (transform.ToString().Contains("Available") || transform.ToString().Contains("Current")) //this is if you land on viewports
                    {
                        DragCards();
                    }
                }
                else
                {
                    Debug.Log("not a match");
                    ResetCard();
                }
            }
            else
            {
                Debug.Log("null or not a card");
                DeckManager.isMatch = false;
                DeckManager.hasToken = false;
            }
        }
    }

    public void ResetCard()
    {
        if (selectedCardOriginalParent.name.Contains("Current"))
        {
            selectedCardDropped.transform.SetParent(currentCardsSortTransformDropZone);
            deckManager.CurrentSelectedLoad();
        }
        else if (selectedCardOriginalParent.name.Contains("Available"))
        {
            selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
;
            Image[] cardImages = selectedCardDropped.transform.GetComponentsInChildren<Image>();
            foreach (Image thisImage in cardImages)
            {
                Debug.Log(thisImage.color.ToString());
                if ((thisImage.ToString() == "Image (UnityEngine.UI.Image)" || thisImage.ToString() == "Frame (UnityEngine.UI.Image)"))
                {
                    if (thisImage.color.ToString() == "RGBA(1.000, 0.000, 0.000, 1.000)")
                    {
                        thisImage.color = new Vector4(200f / 255f, 200f / 255f, 200f / 255f, 128f / 255f);
                    }
                }
            }

            deckManager.AvailableSelectedLoad();
        }

        DeckManager.isMatch = false;
        DeckManager.hasToken = false;
    }
    public void DragCards()
    {
        if (currentCount > 24)
        {
            if (transform.name.Contains("Current") && selectedCardOriginalParent.name.Contains("Current"))
            {
                Debug.Log("25 current to current move");
                selectedCardDropped.transform.SetParent(currentCardsSortTransformDropZone);
                deckManager.CurrentSelectedLoad();
            }
            else if (transform.name.Contains("Available") && selectedCardOriginalParent.name.Contains("Available"))
            {
                Debug.Log("25 available to available move");
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
            else if (transform.name.Contains("Current") && selectedCardOriginalParent.name.Contains("Available"))
            {
                Debug.Log("25 available to current move");
                deckManager.hitCardLimit = true;
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
            else if (transform.name.Contains("Available") && selectedCardOriginalParent.name.Contains("Current"))
            {
                Debug.Log("25 current to available move");
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
            else if (selectedCardOriginalParent.name.Contains("Current"))
            {
                Debug.Log("25 did not drop on view - Current");
                selectedCardDropped.transform.SetParent(currentCardsSortTransformDropZone);
                deckManager.CurrentSelectedLoad();
            }
            else if (selectedCardOriginalParent.name.Contains("Available"))
            {
                Debug.Log("25 did not drop on view - Available");
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
        }
        else if (currentCount <= 24)
        {
            if (transform.name.Contains("Available") && selectedCardOriginalParent.name.Contains("Available"))
            {
                Debug.Log("available to available move");
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
            else if (transform.name.Contains("Available") && selectedCardOriginalParent.name.Contains("Current"))
            {
                Debug.Log("current to available move");
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
            else if (transform.name.Contains("Current") && selectedCardOriginalParent.name.Contains("Available"))
            {
                Debug.Log("available to current move");
                selectedCardDropped.transform.SetParent(currentCardsSortTransformDropZone);
                deckManager.CurrentSelectedLoad();

            }
            else if (transform.name.Contains("Current") && selectedCardOriginalParent.name.Contains("Current"))
            {
                Debug.Log("current to current move");
                selectedCardDropped.transform.SetParent(currentCardsSortTransformDropZone);
                deckManager.CurrentSelectedLoad();
            }
            else if (selectedCardOriginalParent.name.Contains("Current"))
            {
                selectedCardDropped.transform.SetParent(currentCardsSortTransformDropZone);
                deckManager.CurrentSelectedLoad();
            }
            else if (selectedCardOriginalParent.name.Contains("Available"))
            {
                selectedCardDropped.transform.SetParent(availableCardsSortTransformDropZone);
                deckManager.AvailableSelectedLoad();
            }
        }

        DeckManager.isMatch = false;
        DeckManager.hasToken = false;
    }
}
