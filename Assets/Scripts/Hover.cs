using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hover : MonoBehaviourPunCallbacks, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Variables
    [SerializeField] private Card infoCard;
    [SerializeField] private GameObject miniCardParent;

    private GameObject cardUI;
    private GameObject goldErrorTooltip;
    private GameObject gameboard;
    private GameObject cardparent1;
    private GameObject cardparent2;
    private PhotonView pv;
    private List<CardDetails> cardDetails;
    private GameObject canvas;
    private GameObject hand;
    private GameObject enemyHand;
    public static bool isClicked = false;
    public static int clickCounter = 0;
    public static GameObject cardParent;
    #endregion

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        cardUI = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Cards UI").gameObject;
        goldErrorTooltip = cardUI.transform.GetChild(cardUI.transform.childCount - 1).gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
            ShowInfoCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideInfoCard();
        isClicked = false;
    }

    private void Start()
    {
        cardDetails = CardDataBase.instance.cardDetails;
        GetParent();
    }

    public void GetParent()
    {
        GameObject gameboard = canvas.transform.Find("Game Board Parent").Find("GameBoard").gameObject;
        GameObject background = gameboard.transform.GetChild(0).gameObject;

        cardparent1 = background.transform.Find("Card Parent 1").gameObject;
        cardparent2 = background.transform.Find("Card Parent 2").gameObject;
        hand = background.transform.Find("Player Hand").gameObject;
        enemyHand = background.transform.Find("Enemy Hand").gameObject;
    }


    private void ShowInfoCard()
    {
        if (gameObject.transform.parent.name == "Content")
        {
            gameObject.transform.GetChild(0).GetChild(gameObject.transform.GetChild(0).childCount - 1).gameObject.SetActive(true);

            int id = gameObject.transform.GetChild(0).GetComponent<Card>().id;

            CardDetails hoveredCard = cardDetails.Find(item => item.id == id);
            Card cardInfo = Instantiate<Card>(infoCard, cardparent1.transform);
            cardInfo.SetProperties(hoveredCard.id,hoveredCard.ergoTokenId,hoveredCard.ergoTokenAmount, hoveredCard.cardName, hoveredCard.cardDescription, hoveredCard.attack, hoveredCard.HP, hoveredCard.gold, hoveredCard.XP, hoveredCard.clan , hoveredCard.levelRequired, hoveredCard.cardImage, hoveredCard.cardFrame, hoveredCard.cardClass);
        }
        else if (gameObject.transform.parent.parent.name == "Player Hand")
        {
            int id = gameObject.transform.GetChild(0).GetComponent<Card>().id;

            CardDetails hoveredCard = cardDetails.Find(item => item.id == id);
            Card cardInfo = Instantiate<Card>(infoCard, cardparent2.transform);
            cardInfo.SetProperties(hoveredCard.id, hoveredCard.ergoTokenId, hoveredCard.ergoTokenAmount, hoveredCard.cardName, hoveredCard.cardDescription, hoveredCard.attack, hoveredCard.HP, hoveredCard.gold, hoveredCard.XP, hoveredCard.clan, hoveredCard.levelRequired, hoveredCard.cardImage, hoveredCard.cardFrame, hoveredCard.cardClass);
        }
        else if (gameObject.transform.parent.name == "Canvas")
        {
            return;
        }
    }

    private void HideInfoCard()
    {
        gameObject.transform.GetChild(0).GetChild(gameObject.transform.GetChild(0).childCount - 1).gameObject.SetActive(false);
        if (cardparent1.transform.childCount > 0)
        {
            for (int i = 0; i < cardparent1.transform.childCount; i++)
            {
                Destroy(cardparent1.transform.GetChild(i).gameObject);
            }
        }
        if (cardparent2.transform.childCount > 0 && !isClicked)
        {
            for (int i = 0; i < cardparent2.transform.childCount; i++)
            {
                Destroy(cardparent2.transform.GetChild(i).gameObject);
            }
        }
    }

    public void RecruitCard()
    {
        ResetAnimation();
        CardDetails clickedCard = cardDetails.Find(item => item.id == gameObject.transform.GetChild(0).GetComponent<Card>().id);

        int clickedCardid = clickedCard.id;

        if (Gold.instance.GetGold() >= clickedCard.gold)
        {
            pv = gameObject.transform.GetComponent<PhotonView>();
            pv.RPC("Recruit", RpcTarget.All, clickedCardid);
        }
        else
        {
            goldErrorTooltip.SetActive(true);
            Invoke("DisableGoldErrorTooltip", 1f);
        }
    }

    private void DisableGoldErrorTooltip()
    {
        goldErrorTooltip.SetActive(false);
    }

    #region RPC Method
    [PunRPC]
    public void Recruit(int id)
    {
        pv = gameObject.transform.GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                if (hand.transform.GetChild(i).childCount == 0)
                {
                    CardDetails cardClicked = cardDetails.Find(item => item.id == id);
                    GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation);
                    miniCard.transform.SetParent(hand.transform.GetChild(i));
                    miniCard.transform.position = hand.transform.GetChild(i).transform.position;
                    miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                    miniCard.GetComponent<DragMiniCards>().enabled = true;
                    //miniCard.AddComponent<DragMiniCards>();
                    //miniCard.AddComponent<PhotonView>();
                    Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
                    card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount ,cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage);
                    card.name = cardClicked.cardName;
                    card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);

                    int availableGold = Gold.instance.GetGold() - cardClicked.gold;
                    Gold.instance.SetGold(availableGold);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                if (enemyHand.transform.GetChild(i).childCount == 0)
                {
                    CardDetails cardClicked = cardDetails.Find(item => item.id == id);
                    GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyHand.transform.GetChild(i).position, enemyHand.transform.GetChild(i).rotation);
                    miniCard.transform.SetParent(enemyHand.transform.GetChild(i));
                    miniCard.transform.position = enemyHand.transform.GetChild(i).transform.position;
                    miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                    miniCard.AddComponent<DragMiniCards>();
                    Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
                    card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId , cardClicked.ergoTokenAmount , cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage);
                    card.name = cardClicked.cardName;
                    card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
                    miniCard.SetActive(false);
                    break;
                }
            }
        }
    }
    #endregion

    public void ResetAnimation()
    {
        GameObject gameObj = hand;

        for (int i = 0; i < gameObj.transform.childCount; i++)
        {
            if (gameObj.transform.GetChild(i).childCount == 1)
            {
                gameObj.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCounter = 0;
        ResetAnimation();

        if (GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient)
        {
            DisplayAnimation();
        }
        else if (!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient)
        {
            DisplayAnimation();
        }
    }

    public void DisplayAnimation()
    {
        if (gameObject.transform.parent.parent.name == "Player Hand")
        {
            HideInfoCard();
            isClicked = true;
            HideInfoCard();
            cardParent = gameObject;
            clickCounter += 1;
            Button button = gameObject.transform.GetComponent<Button>();
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetBool("Scale", true);
        }
    }
}
