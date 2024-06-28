using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
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
    public static bool turnEnd;
    public static GameObject obj;
    public static string parent;
    private List<CardDetails> cardDetails;
    private GameBoardManager gameboardManager;
    private GameObject gameBoardParent;
    private GameObject cardError;
    private bool previousVal;
    private bool currVal;
    public bool isDragging = false;
    #endregion
    
    private void Awake()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        cardDetails = CardDataBase.instance.cardDetails;
        previousVal = GameBoardManager.player1Turn;
        gameBoardParent = GameObject.Find("Game Board Parent");
        gameboardManager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
        cardError = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Announcements Area").gameObject;
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag called");
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Hand")
        {
            return;
        }
        else if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.name == "Enemy Field")
        {
            return;
        }
        
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
            Debug.Log(" --== gameobject master "+ obj.name);
            endParent = previousParent;
            endSubParent = previousSubParent;
            Debug.Log(endParent + " end parent " + endSubParent + " end sub parent");
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
            Debug.Log(" --== gameobject client " + obj.name);
            endParent = previousParent;
            endSubParent = previousSubParent;
            Debug.Log(endParent + " end parent " + endSubParent + " end sub parent");
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag called ");
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
            Debug.Log("inside player 1");
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);//+ offset;
            transform.position = curPosition;
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            Debug.Log("inside player 2");
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);//+ offset;
            transform.position = curPosition;
        }
        
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag called");
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
            Debug.Log("inside player 1 " + parentAfterDrag + " obj " + obj);
            isDragging = false;

            Card card = obj.transform.GetComponentInChildren<Card>();
            Debug.Log(card + " card " + card.id);
            GameObject playerField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;
            GameObject playerToBePositioned = parentAfterDrag.gameObject;
            Debug.Log(playerToBePositioned + " player to be positioned");

            bool isSatisfy = gameboardManager.IsSatisfyRequirements(card, playerField, playerToBePositioned, true);
            if (!isSatisfy)
            {
                Debug.Log("!satisfy card drag not called ");
                Debug.Log("!isSatisfy ");
                cardError.transform.GetChild(0).gameObject.SetActive(true);
                cardError.GetComponentInChildren<TMP_Text>().SetText("The card can not satisfy the requirement to put the field.");
                EndForceTurn();
                Invoke("RemoveErrorObject", 2f);
                return;

            }

            Debug.Log(parentAfterDrag + " parentAfterDrag " + gameObject + " gameObject " + previousParent + " previousParent " + previousSubParent );
            isDragging = false;
            transform.SetParent(parentAfterDrag);
            transform.position = parentAfterDrag.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            obj = gameObject;
            Debug.Log(" --== gameobject master  end " + obj.name);
            endParent = previousParent;
            endSubParent = previousSubParent;
            //GameObject playerHand
            dragEnd = true;
            Debug.Log("Card setted");
            gameboardManager.OnSetCard(card, PhotonNetwork.IsMasterClient, parentAfterDrag);
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            Debug.Log("inside player 2 " + parentAfterDrag + " obj " + obj);
            isDragging = false;
            
            Card card = obj.transform.GetComponentInChildren<Card>();
            Debug.Log(card + " card " + card.id);
            GameObject playerField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;
            GameObject playerToBePositioned = parentAfterDrag.gameObject;
            Debug.Log(playerToBePositioned + " player to be positioned");

            bool isSatisfy = gameboardManager.IsSatisfyRequirements(card, playerField, playerToBePositioned, true);
            if (!isSatisfy)
            {
                Debug.Log("!satisfy card drag not called ");
                Debug.Log("!isSatisfy ");
                cardError.transform.GetChild(0).gameObject.SetActive(true);
                cardError.GetComponentInChildren<TMP_Text>().SetText("The card can not satisfy the requirement to put the field.");
                EndForceTurn();
                Invoke("RemoveErrorObject", 2f);
                return;

            }

            Debug.Log(parentAfterDrag + " parentAfterDrag " + gameObject + " gameObject " + previousParent + " previousParent " + previousSubParent);
            isDragging = false;
            transform.SetParent(parentAfterDrag); transform.position = parentAfterDrag.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            obj = gameObject;
            Debug.Log(" --== game object client end "+obj.name);
            endParent = previousParent;
            endSubParent = previousSubParent;
            dragEnd = true;
            Debug.Log("Card setted ");
            gameboardManager.OnSetCard(card, PhotonNetwork.IsMasterClient, parentAfterDrag);
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
            turnEnd = true;
            EndForceTurn();
            isDragging = false;
        }
        previousVal = currVal;
        gameboardManager.HidePanel(cardError);
    }
    
    public void CardDrag()
    {
        Debug.Log("card drag called ");
        if ((GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient))
        {
            Debug.Log("====Master start ==== ");
            Card card = obj.transform.GetComponentInChildren<Card>();
            Debug.Log(card + " card " + card.id);

            GameObject playerHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Hand").gameObject;
            GameObject playerField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;
            Debug.Log(playerHand + " player hand " + playerField + " player field");

            Debug.Log(" obj.transform.parent.parent.name " + obj.transform.parent.parent.name + " endParent " + endParent + " DropMiniCard.dropOnField " + DropMiniCard.dropOnField);

            Transform currrParent = obj.transform.parent;
            Debug.Log("object parent " + obj.transform.parent + " current parent " + currrParent);
            
            int cardId = card.id;
            int len = obj.transform.parent.name.Split(" ").Length;
            int pos = int.Parse
            (obj.transform.parent.name.Split(" ")[len - 1]);
            Debug.Log(len + " length " + pos + " pos ");
            
            PhotonView pv = obj.transform.GetComponent<PhotonView>();
            Debug.Log(pv + " photon view ");

            if (obj.transform.parent.parent.name == "Player Field" && endParent == "Player Hand" && DropMiniCard.dropOnField)
            {
                Debug.Log("inside obj.transform.parent.parent.name == \"Player Field\" && endParent == \"Player Hand\" && DropMiniCard.dropOnField ");
               
                Destroy(obj.GetComponent<DragMiniCards>());
                obj.AddComponent<DragFieldCard>();
                int id = int.Parse(endSubParent.Split(" ")[2]);
                Debug.Log(id + " #id value ");
                playerHand.transform.GetChild(id - 1).gameObject.SetActive(false);
                Tuple<int, int> result = GameBoardManager.GetTotalCardsCount(playerHand, playerField);
                int handCount = result.Item1;
                int fieldCount = result.Item2;
                Debug.Log("Drag card called not in others " + handCount + " hand count " + fieldCount);
                pv.RPC("DragCards", RpcTarget.Others, cardId, pos, endSubParent, handCount, fieldCount);
            }
            Debug.Log("====Master End====");
            
        }
        else if ((!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient))
        {
            Debug.Log("====Client start====");
            Card card = obj.transform.GetComponentInChildren<Card>();
            Debug.Log(card + " card " + card.id);

            GameObject playerHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Hand").gameObject;
            GameObject playerField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;
            Debug.Log(playerHand + " player hand " + playerField + " player field");

            Debug.Log(" obj.transform.parent.parent.name " + obj.transform.parent.parent.name + " endParent " + endParent + " DropMiniCard.dropOnField " + DropMiniCard.dropOnField);
           
            Transform currrParent = obj.transform.parent;
            Debug.Log("object parent " + obj.transform.parent + " current parent " + currrParent);

            int cardId = card.id;
            int len = obj.transform.parent.name.Split(" ").Length;
            int pos = int.Parse
            (obj.transform.parent.name.Split(" ")[len - 1]);
            Debug.Log(len + " length " + pos + " pos ");

            PhotonView pv = obj.transform.GetComponent<PhotonView>();
            Debug.Log(pv + " photon view ");

            if (obj.transform.parent.parent.name == "Player Field" && endParent == "Player Hand" && DropMiniCard.dropOnField)
            {
                Debug.Log("inside obj.transform.parent.parent.name == \"Player Field\" && endParent == \"Player Hand\" && DropMiniCard.dropOnField ");

                Destroy(obj.GetComponent<DragMiniCards>());
                obj.AddComponent<DragFieldCard>();
                int id = int.Parse(endSubParent.Split(" ")[2]);
                Debug.Log(id + " #id value ");
                playerHand.transform.GetChild(id - 1).gameObject.SetActive(false);
                Tuple<int, int> result = GameBoardManager.GetTotalCardsCount(playerHand, playerField);
                int handCount = result.Item1;
                int fieldCount = result.Item2;
                Debug.Log("Drag card called not in others " + handCount + " hand count " + fieldCount);
                pv.RPC("DragCards", RpcTarget.Others, cardId, pos, endSubParent, handCount, fieldCount);
            }
            Debug.Log("====client end");
        }
        obj = null;
    }
    
    public void EndForceTurn()
    {
        Debug.Log(obj + " OBJECT");
        if (obj == null) return;
        Card card = obj?.transform.GetComponentInChildren<Card>();
        Debug.Log(card?.name + " card name");
        
        int cardParentPos = int.Parse(endSubParent.Split(" ")[2]) - 1;
        Debug.Log(cardParentPos + " card parent pos");
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
        Debug.Log(" drag cards in others " + PhotonNetwork.IsMasterClient + " master " + PhotonNetwork.LocalPlayer.NickName);
        CardDetails clickedCard = cardDetails.Find(card => card.id == id);
        Debug.Log(clickedCard.id + " clickedCard");
        Debug.Log(hCount + " h count " + fCount + " f count ");
        
        int actualCardPos = int.Parse(parent.Split(" ")[2]);
        Debug.Log(actualCardPos + " actualCardPos " + parent + " parent");
        
        GameObject selectedCardParent = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Field").Find("Enemy Parent " + pos).gameObject;
        GameObject enemyHand = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        GameObject enemyField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        Debug.Log(selectedCardParent + " selected parent");
        Debug.Log(enemyHand + " enemyHand " + enemyField + " enemyField");

        GameObject selectedcard = enemyHand.transform.GetChild(actualCardPos - 1).GetChild(0).gameObject;
        Debug.Log(selectedcard + " selectedcard ");
        selectedcard.transform.SetParent(selectedCardParent.transform);
        selectedcard.transform.localPosition = Vector3.zero;
        int enemyId = int.Parse(parent.Split(" ")[2]);
        Debug.Log(enemyId + " #enemyId value ");
        enemyHand.transform.GetChild(enemyId - 1).gameObject.SetActive(false);
        selectedcard.AddComponent<DropFieldCard>();

        Debug.Log(selectedcard.name + " selected card name ");
        if(selectedCardParent.transform.childCount == 1)
        {
            Debug.Log(" child count 1" + selectedCardParent.name + " selected card parent " + selectedcard + " selected card ");
            
            Debug.Log(" selectedcard.transform.GetChild(0).GetComponent<Card>().id  " + selectedcard.transform.GetChild(0).GetComponent<Card>().id + " click card id " + clickedCard.id);
            //Destroy(selectedCardParent.transform.GetChild(0).gameObject);
            Debug.Log("Destroyed if already present " + selectedCardParent.transform.GetChild(0));
            if (selectedcard.transform.GetChild(0).GetComponent<Card>().id != clickedCard.id)
            {
                Debug.Log("not matched");
                //Destroy(selectedCardParent.transform.GetChild(0).gameObject);
                GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", selectedCardParent.transform.position, selectedCardParent.transform.rotation);
                miniCardParent.transform.SetParent(selectedCardParent.transform);
                miniCardParent.transform.localScale = selectedCardParent.transform.localScale;
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                Debug.Log(" completed card level " + clickedCard.levelRequired);
                int level = (int)(clickedCard.levelRequired);
                miniCard.SetMiniCard(clickedCard.id, clickedCard.ergoTokenId, clickedCard.ergoTokenAmount, clickedCard.cardName, clickedCard.attack, clickedCard.HP, clickedCard.gold, clickedCard.XP, clickedCard.cardImage, clickedCard.ability
                    //, clickedCard.requirements, clickedCard.abilityLevel
                    );
                miniCard.name = clickedCard.cardName;
                miniCardParent.name = clickedCard.cardName;
            }
        }

        if (selectedCardParent.transform.childCount == 0)
        {
            Debug.Log("selectedCardParent.transform.childCount == 0");
            GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", selectedCardParent.transform.position, selectedCardParent.transform.rotation);
            miniCardParent.transform.SetParent(selectedCardParent.transform);
            miniCardParent.transform.localScale = selectedCardParent.transform.localScale;
            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
            Debug.Log(" completed card level " + clickedCard.levelRequired);
            int level = (int)(clickedCard.levelRequired);
            miniCard.SetMiniCard(clickedCard.id, clickedCard.ergoTokenId, clickedCard.ergoTokenAmount, clickedCard.cardName, clickedCard.attack, clickedCard.HP, clickedCard.gold, clickedCard.XP, clickedCard.cardImage, clickedCard.ability
                //, clickedCard.requirements, clickedCard.abilityLevel
                );
            miniCard.name = clickedCard.cardName;
            miniCardParent.name = clickedCard.cardName;
        }
        Tuple<int, int> result = GameBoardManager.GetTotalCardsCount(enemyHand, enemyField);
        int handCount = result.Item1;
        int fieldCount = result.Item2;

        Debug.LogError(hCount + " h count " + fCount + " f count " + handCount + " player count " + fieldCount + " field count ");
        Debug.Log("Drag card called in others " + handCount + " hand count " + fieldCount);

        if (handCount != hCount)
        {
            cardError.transform.GetChild(0).gameObject.SetActive(true);
            cardError.GetComponentInChildren<TMP_Text>().SetText("The hand count and h count not same");
            Debug.Log("The hand count and h count not same");
            Invoke("RemoveErrorObject", 2f);
        }

        if (fieldCount != fCount)
        {
            cardError.transform.GetChild(0).gameObject.SetActive(true);
            cardError.GetComponentInChildren<TMP_Text>().SetText("The fieldCount and f count not same");
            Debug.Log("The fieldCount and f count not same");
            Invoke("RemoveErrorObject", 2f);
        }

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
            Debug.Log("(selectedCardParent.tag " + selectedCardParent.tag);
            selectedcard.SetActive(true);
        }
        else if (selectedCardParent.tag == "Back Line Enemy")
        {
            Debug.Log("(selectedCardParent.tag " + selectedCardParent.tag);
            if (GameBoardManager.isWallDestroyed)
                selectedcard.SetActive(true);
            else
                selectedcard.SetActive(false);
            
        }
        HideBackLineCards();
    }

    [PunRPC]
    private void DostroyCardOnOthers(int id)
    {
        Debug.Log("DostroyCardOnOthers");
        GameObject CardParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        Debug.Log(CardParent + " card parent");
        if (CardParent.transform.GetChild(id - 1) != null)
        {
            Debug.Log(CardParent.transform.GetChild(id - 1).name + " name of parent");
            CardParent.transform.GetChild(id - 1).gameObject.SetActive(false);
        }
        if (CardParent.transform.GetChild(id - 1).GetChild(0) != null)
        {
            Debug.Log(CardParent.transform.GetChild(id - 1).GetChild(0).name + " name of card");
            Destroy(CardParent.transform.GetChild(id - 1).GetChild(0).gameObject);
        }
    }

    #endregion

    private void RemoveErrorObject()
    {
        cardError.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void HideBackLineCards()
    {
        GameObject enemyField = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        Debug.Log("HideBackLineCards() called " + enemyField + " field " + enemyField.transform.childCount);
        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            if(enemyField.transform.GetChild(i).tag == "Back Line Enemy")
            {
                if(enemyField.transform.GetChild(i).childCount == 1)
                {
                    enemyField.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                
            }
        }
    }
}
