using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    DeckManager deckManagerDrag;
    DropZone dropZoneCurrent;
    DropZone dropZoneAvailable;
    public GameObject countCheck;
    public Transform parentToReturn = null;
    public static Transform dragParent;
    public static bool dragEnd;
    public static int cardclassIndex;
    public static string tokenId;
    private Vector3 screenPoint;
    private Vector3 offset;

    private Image[] cardImagesDrag;
    public static bool hoverDisabled = false;//see CardHover script

    public Texture2D cursorTextureCard;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpotDrag = Vector2.zero;

    Color cardOriginal;

    void Start()
    {
        deckManagerDrag = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        dropZoneCurrent = GameObject.FindGameObjectWithTag("Current").GetComponent<DropZone>();
        dropZoneAvailable = GameObject.FindGameObjectWithTag("Available").GetComponent<DropZone>();
        countCheck = GameObject.FindGameObjectWithTag("ContentCurrent");
    }

    public void OnBeginDrag(PointerEventData eventData) // when user drag the cards
    {
        if (deckManagerDrag.isDragCheckAllowed == true && eventData.button == PointerEventData.InputButton.Left)
        {
            Cursor.SetCursor(cursorTextureCard, hotSpotDrag, cursorMode);

            dropZoneCurrent.selectedCardOriginalParent = gameObject.transform.parent;
            dropZoneAvailable.selectedCardOriginalParent = gameObject.transform.parent;

            dropZoneCurrent.currentCount = countCheck.transform.childCount;
            dropZoneAvailable.currentCount = countCheck.transform.childCount;

            Debug.Log("original parent name " + gameObject.transform.parent);

            hoverDisabled = true;//see CardHover script
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            CardClass cardClass = transform.GetComponent<Card>().cardClass;
            cardclassIndex = (int)Convert.ChangeType(cardClass, cardClass.GetTypeCode());
            tokenId = transform.GetComponent<Card>().ergoTokenId;

            dragParent = transform.parent.parent.parent;

            parentToReturn = transform.parent;
            transform.SetParent(transform.parent.parent.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            cardImagesDrag = gameObject.GetComponentsInChildren<Image>();

            if (dragParent.name.Split(" ")[0] == "Available" || dragParent.name.Split(" ")[0] == "Current")
            {
                if (DeckManager.generalsIndex == cardclassIndex || cardclassIndex == 0)
                {
                    DeckManager.isMatch = true;
                }
                else
                {
                    DeckManager.isMatch = false;
                }

                if (ErgoQuery.instance.tokenIDs.Contains(tokenId) || tokenId == "")
                {
                    DeckManager.hasToken = true;
                }
                else
                {
                    DeckManager.hasToken = false;
                }

                if (DeckManager.hasToken == true && DeckManager.isMatch == true && cardImagesDrag != null)
                {
                    foreach (Image thisImage in cardImagesDrag)

                    {
                        if ((thisImage.ToString() == "Image (UnityEngine.UI.Image)" || thisImage.ToString() == "Frame (UnityEngine.UI.Image)"))
                        {
                            cardOriginal = thisImage.color;
                            thisImage.color = new Vector4(0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                        }
                    }
                }
                else
                {
                    foreach (Image thisImage in cardImagesDrag)

                    {
                        if ((thisImage.ToString() == "Image (UnityEngine.UI.Image)" || thisImage.ToString() == "Frame (UnityEngine.UI.Image)"))
                        {
                            cardOriginal = thisImage.color;
                            thisImage.color = new Vector4(255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
                        }
                    }
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData) // When user hold the card
    {
        if (deckManagerDrag.isDragCheckAllowed == true && eventData.button == PointerEventData.InputButton.Left)
        {
            DeckManager.onDragCardCount = DeckManager.currentCards.Count;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // When user drag remove
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (deckManagerDrag.isDragCheckAllowed == true)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);

                if (dropZoneCurrent.dropCheck == false && dropZoneAvailable.dropCheck == false)
                {
                    Debug.Log(parentToReturn.name);
                    if (parentToReturn.name.Contains("Available"))
                    {
                        transform.SetParent(parentToReturn);
                        ImageReset();
                        deckManagerDrag.AvailableSelectedLoad();
                    }
                    else if (parentToReturn.name.Contains("Current"))
                    {
                        transform.SetParent(parentToReturn);
                        deckManagerDrag.CurrentSelectedLoad();
                    }
                    else
                    {
                        transform.SetParent(parentToReturn);
                        ImageReset();
                        deckManagerDrag.AvailableSelectedLoad();
                        deckManagerDrag.CurrentSelectedLoad();
                    }
                }
                dropZoneCurrent.dropCheck = false;
                dropZoneAvailable.dropCheck = false;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            ImageReset();

            hoverDisabled = false;//see CardHover script
            dragEnd = true;//this is the last thing that happens, used to be in the first IF statement
        }
    }

    public void ImageReset()
    {
        foreach (Image thisImage in cardImagesDrag)
        {
            if ((thisImage.ToString() == "Image (UnityEngine.UI.Image)" || thisImage.ToString() == "Frame (UnityEngine.UI.Image)"))
            {
                if (cardOriginal.ToString().Contains("0.502"))
                {
                    thisImage.color = cardOriginal;
                }
                else
                {
                    thisImage.color = new Vector4(1f, 1f, 1f, 1f);
                }
            }
        }
    }
}

