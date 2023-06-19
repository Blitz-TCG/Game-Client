using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMiniCards : MonoBehaviourPunCallbacks, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables
    public string previousParent;
    public string previousSubParent;
    public static string endParent;
    public static string endSubParent;
    
    [HideInInspector] public Transform parentAfterDrag;
    private Canvas canvas;
    private Vector3 screenPoint;
    private Vector3 offset;
    public static bool dragEnd;
    public static GameObject obj;
    public static string parent;
    private List<CardDetails> cardDetails;
    private bool previousVal;
    private bool currVal;
    public bool isDragging = false;
    #endregion
    
    private void Awake()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        cardDetails = CardDataBase.instance.cardDetails;
        previousVal = GameBoardManager.player1Turn;
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Hand")
        {
            return;
        }
        else if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Field")
        {
            return;
        }
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient))
        {
            isDragging = true;
            previousParent = transform.parent.parent.name;
            previousSubParent = transform.parent.name;
            
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            obj = transform.gameObject;
            endParent = previousParent;
            endSubParent = previousSubParent;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient))
        {
            isDragging = true;
            previousParent = transform.parent.parent.name;
            previousSubParent = transform.parent.name;
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            obj = transform.gameObject;
            endParent = previousParent;
            endSubParent = previousSubParent;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Hand")
        {
            return;
        }
        else if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Field")
        {
            return;
        }
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient))
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);//+ offset;
            transform.position = curPosition;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient))
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);//+ offset;
            transform.position = curPosition;
        }
        
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Hand")
        {
            return;
        }
        else if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Field")
        {
            return;
        }
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient))
        {
            isDragging = false;
            transform.SetParent(parentAfterDrag);
            transform.position = parentAfterDrag.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            obj = transform.gameObject;
            endParent = previousParent;
            endSubParent = previousSubParent;
            dragEnd = true;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient))
        {
            isDragging = false;
            transform.SetParent(parentAfterDrag); transform.position = parentAfterDrag.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            obj = transform.gameObject;
            endParent = previousParent;
            endSubParent = previousSubParent;
            dragEnd = true;
        }
        
    }
    
    private void Update()
    {
        currVal = GameBoardManager.player1Turn;
        if (dragEnd)
        {
            dragEnd = false;
            CardDrag();
        }
        else if (isDragging && !dragEnd && previousVal != currVal)
        {
            EndForceTurn();
            isDragging = false;
        }
        previousVal = currVal;
    }
    
    public void CardDrag()
    {
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient))
        {
            Card card = obj.transform.GetComponentInChildren<Card>();
            Transform currrParent = obj.transform.parent;
            
            int cardId = card.id;
            int len = obj.transform.parent.name.Split(" ").Length;
            int pos = int.Parse
            (obj.transform.parent.name.Split(" ")[len - 1]);
            
            PhotonView pv = obj.transform.GetComponent<PhotonView>();
            
            if (obj.transform.parent.parent.name == "Player Field" && endParent == "Player Hand" && DropMiniCard.dropOnField)
            {
                Destroy(obj.GetComponent<DragMiniCards>());
                obj.AddComponent<DragFieldCard>();
                pv.RPC("DragCards", RpcTarget.Others, cardId, pos, endSubParent);
            }
            
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient))
        {
            Card card = obj.transform.GetComponentInChildren<Card>();
            Transform currrParent = obj.transform.parent;
            
            int cardId = card.id;
            int len = obj.transform.parent.name.Split(" ").Length;
            int pos = int.Parse
            (obj.transform.parent.name.Split(" ")[len - 1]);
            
            PhotonView pv = obj.transform.GetComponent<PhotonView>();
            
            if (obj.transform.parent.parent.name == "Player Field" && endParent == "Player Hand" && DropMiniCard.dropOnField)
            {
                Destroy(obj.GetComponent<DragMiniCards>());
                obj.AddComponent<DragFieldCard>();
                pv.RPC("DragCards", RpcTarget.Others, cardId, pos, endSubParent);
            }
        }
        obj = null;
    }
    
    public void EndForceTurn()
    {
        Card card = obj.transform.GetComponentInChildren<Card>();
        
        int cardParentPos = int.Parse(endSubParent.Split(" ")[2]) - 1;
        GameObject playerHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Hand").gameObject;
        obj.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject cardPosition = playerHand.transform.GetChild(cardParentPos).gameObject;
        obj.transform.SetParent(cardPosition.transform);
        obj.transform.localScale = cardPosition.transform.localScale;
        obj.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        obj.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        obj = null;
    }
    
    #region RPC Method
    [PunRPC]
    public void DragCards(int id, int pos, string parent)
    {
        CardDetails clickedCard = cardDetails.Find(card => card.id == id);
        
        int actualCardPos = int.Parse(parent.Split(" ")[2]);
        
        GameObject selectedCardParent = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Field").Find("Enemy Parent " + pos).gameObject;
        GameObject enemyHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        
        GameObject selectedcard = enemyHand.transform.GetChild(actualCardPos - 1).GetChild(0).gameObject;
        selectedcard.transform.SetParent(selectedCardParent.transform);
        selectedcard.transform.localPosition = Vector3.zero;
        selectedcard.AddComponent<DropFieldCard>();
        
        if (selectedCardParent.tag == "Front Line Enemy")
        {
            selectedcard.SetActive(true);
        }
        else if (selectedCardParent.tag == "Back Line Enemy")
        {
            if (GameBoardManager.isWallDestroyed)
            selectedcard.SetActive(true);
            else
            selectedcard.SetActive(false);
            
        }
    }
    #endregion
}
