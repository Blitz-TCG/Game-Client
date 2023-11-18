using Photon.Pun;
using System;
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
        Debug.Log(GameBoardManager.player1Turn + " GameBoardManager.player1Turn " + PhotonNetwork.IsMasterClient + " PhotonNetwork.IsMasterClient " + photonView.IsMine + " photonView.IsMine");
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            Debug.Log("inside player 1");
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
            obj = gameObject;
            endParent = previousParent;
            endSubParent = previousSubParent;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            Debug.Log("inside player 2");
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
            obj = gameObject;
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
        //Debug.Log(GameBoardManager.player1Turn + " GameBoardManager.player1Turn " + PhotonNetwork.IsMasterClient + " PhotonNetwork.IsMasterClient " + photonView.IsMine + " photonView.IsMine");
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            //Debug.Log("inside player 1");
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);//+ offset;
            transform.position = curPosition;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            //Debug.Log("inside player 2");
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
        Debug.Log(GameBoardManager.player1Turn + " GameBoardManager.player1Turn " + PhotonNetwork.IsMasterClient + " PhotonNetwork.IsMasterClient " + photonView.IsMine + " photonView.IsMine");
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            Debug.Log("inside player 1");
            isDragging = false;
            transform.SetParent(parentAfterDrag);
            transform.position = parentAfterDrag.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            obj = transform.gameObject;
            endParent = previousParent;
            endSubParent = previousSubParent;
            dragEnd = true;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            Debug.Log("inside player 2");
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

            GameObject playerHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Hand").gameObject;
            GameObject playerField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;

            if (obj.transform.parent.parent.name == "Player Field" && endParent == "Player Hand" && DropMiniCard.dropOnField)
            {
                Destroy(obj.GetComponent<DragMiniCards>());
                obj.AddComponent<DragFieldCard>();
                Tuple<int, int> result = GameBoardManager.GetTotalCardsCount(playerHand, playerField);
                int handCount = result.Item1;
                int fieldCount = result.Item2;
                pv.RPC("DragCards", RpcTarget.Others, cardId, pos, endSubParent, handCount, fieldCount);
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

            GameObject playerHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Hand").gameObject;
            GameObject playerField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;

            if (obj.transform.parent.parent.name == "Player Field" && endParent == "Player Hand" && DropMiniCard.dropOnField)
            {
                Destroy(obj.GetComponent<DragMiniCards>());
                obj.AddComponent<DragFieldCard>();
                Tuple<int, int> result = GameBoardManager.GetTotalCardsCount(playerHand, playerField);
                int handCount = result.Item1;
                int fieldCount = result.Item2;

                pv.RPC("DragCards", RpcTarget.Others, cardId, pos, endSubParent, handCount, fieldCount);
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
    public void DragCards(int id, int pos, string parent, int hCount, int fCount)
    {
        CardDetails clickedCard = cardDetails.Find(card => card.id == id);
        
        int actualCardPos = int.Parse(parent.Split(" ")[2]);
        
        GameObject selectedCardParent = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Field").Find("Enemy Parent " + pos).gameObject;
        GameObject enemyHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        GameObject enemyField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        GameObject selectedcard = enemyHand.transform.GetChild(actualCardPos - 1).GetChild(0).gameObject;
        selectedcard.transform.SetParent(selectedCardParent.transform);
        selectedcard.transform.localPosition = Vector3.zero;
        selectedcard.AddComponent<DropFieldCard>();

        Debug.Log(selectedcard.name + " selected card name ");
        if(selectedCardParent.transform.childCount == 1)
        {
            Debug.Log(" child count 1" + selectedCardParent.name + " selected card parent " + selectedcard + " selected card ");
            
            Debug.Log(" selectedcard.transform.GetChild(0).GetComponent<Card>().id  " + selectedcard.transform.GetChild(0).GetComponent<Card>().id + " click card id " + clickedCard.id);
            if(selectedcard.transform.GetChild(0).GetComponent<Card>().id != clickedCard.id)
            {
                Debug.Log("not matched");
                Destroy(selectedCardParent.transform.GetChild(0).gameObject);
                GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", selectedCardParent.transform.position, selectedCardParent.transform.rotation);
                miniCardParent.transform.SetParent(selectedCardParent.transform);
                miniCardParent.transform.localScale = selectedCardParent.transform.localScale;
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                Debug.Log(" completed card level " + clickedCard.levelRequired);
                int level = (int)(clickedCard.levelRequired);
                miniCard.SetMiniCard(clickedCard.id, clickedCard.ergoTokenId, clickedCard.ergoTokenAmount, clickedCard.cardName, clickedCard.attack, clickedCard.HP, clickedCard.gold, clickedCard.XP, clickedCard.cardImage);
                miniCard.name = clickedCard.cardName;
                miniCardParent.name = clickedCard.cardName;
            }
        }

        if(selectedCardParent.transform.childCount == 0)
        {
            GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", selectedCardParent.transform.position, selectedCardParent.transform.rotation);
            miniCardParent.transform.SetParent(selectedCardParent.transform);
            miniCardParent.transform.localScale = selectedCardParent.transform.localScale;
            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
            Debug.Log(" completed card level " + clickedCard.levelRequired);
            int level = (int)(clickedCard.levelRequired);
            miniCard.SetMiniCard(clickedCard.id, clickedCard.ergoTokenId, clickedCard.ergoTokenAmount, clickedCard.cardName, clickedCard.attack, clickedCard.HP, clickedCard.gold, clickedCard.XP, clickedCard.cardImage);
            miniCard.name = clickedCard.cardName;
            miniCardParent.name = clickedCard.cardName;
        }
        //Tuple<int, int> result = GameBoardManager.GetTotalCardsCount(enemyHand, enemyField);
        //int handCount = result.Item1;
        //int fieldCount = result.Item2;

        //Debug.LogError(hCount + " h count " + fCount + " f count " + handCount + " player count " + fieldCount + " field count ");

        //if(handCount != hCount)
        //{
        //    Debug.LogError( " Not both same " + hCount + " h count " +  handCount + " player count ");
        //}

        //if(fieldCount != fCount)
        //{
        //    Debug.LogError(" not both  same " + fieldCount + " field count " + fCount + " fcount ");
        //}

        //if(handCount == hCount)
        //{
        //    Debug.LogError(" Both same " + handCount + " hand count " + hCount + " hcount ");
        //}

        //if (fieldCount == fCount)
        //{
        //    Debug.LogError(" Both same " + fieldCount + " field count " + fCount + " fcount ");
        //}

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
