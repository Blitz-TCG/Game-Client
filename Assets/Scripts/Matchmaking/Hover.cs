using Photon.Pun;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
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
    private PlayerController playerController;
    private EnemyController enemyController;
    private GameObject gameBoardParent;
    private TMP_Text error;
    private bool isHovering = false;

    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    #endregion

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        cardUI = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("StoreUI").gameObject;
        goldErrorTooltip = cardUI.transform.GetChild(cardUI.transform.childCount - 1).gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("--- on pointer enter " + transform.GetComponent<BoxCollider2D>().gameObject);
        Debug.Log("transform name " + transform.name);
        isHovering = true;
        Debug.Log("pointer enter ");
        if (!isClicked)
            Invoke("ShowInfoCard", 0.1f);
        //if (!isClicked)
        //    ShowInfoCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer exit ");
        if(transform.GetComponent<Card>() != null && transform.GetComponent<Card>().transform.parent.name != null)
        {
            Debug.Log("transform name " + transform.GetComponent<Card>() + " card parent " + transform.GetComponent<Card>().transform.parent.name);
        }
        isHovering =false;
        isClicked = false;
        Invoke("HideInfoCard", 0.1f);
        //HideInfoCard();
    }

    private void Start()
    {
        gameBoardParent = GameObject.Find("Game Board Parent");
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
        Debug.Log("Show card ");
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
            Debug.Log(gameObject.transform.parent.name.Split(" ")[2] + " name of card postition");
            int index = int.Parse(gameObject.transform.parent.name.Split(" ")[2]);
            Debug.Log(index + " index ");
            GameObject actualParent = cardparent2.transform.GetChild(index - 1).gameObject;
            Debug.Log(actualParent + " actual parent name");

            CardDetails hoveredCard = cardDetails.Find(item => item.id == id);
            Card cardInfo = Instantiate<Card>(infoCard, actualParent.transform);
            cardInfo.SetProperties(hoveredCard.id, hoveredCard.ergoTokenId, hoveredCard.ergoTokenAmount, hoveredCard.cardName, hoveredCard.cardDescription, hoveredCard.attack, hoveredCard.HP, hoveredCard.gold, hoveredCard.XP, hoveredCard.clan, hoveredCard.levelRequired, hoveredCard.cardImage, hoveredCard.cardFrame, hoveredCard.cardClass);
        }
        else if (gameObject.transform.parent.name == "Canvas")
        {
            return;
        }
    }

    private void HideInfoCard()
    {
        Debug.Log("hide card");
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
                if(cardparent2.transform.GetChild(i).childCount > 0)
                    Destroy(cardparent2.transform.GetChild(i).GetChild(0).gameObject);
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
            goldErrorTooltip.transform.Find("Gold Error Text").GetComponent<TMP_Text>().SetText("You can not buy card more than gold you have.");
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
        playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
        enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

        pv = gameObject.transform.GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                if (hand.transform.GetChild(i).childCount == 0)
                {
                    Debug.Log(playerController.totalXP + " player total xp " + enemyController.totalXP + " enemy's XP");
                    CardDetails cardClicked = cardDetails.Find(item => item.id == id);
                    Debug.Log(cardClicked.levelRequired + " level req");
                    int level = int.Parse(cardClicked.levelRequired.Split(" ")[1]);
                    Debug.LogError("level " + level + " total xp " + playerController.totalXP);
                    if (IsRecruit(playerController.totalXP, level))
                    {
                        GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation);
                        miniCard.transform.SetParent(hand.transform.GetChild(i));
                        miniCard.transform.position = hand.transform.GetChild(i).transform.position;
                        miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                        miniCard.GetComponent<DragMiniCards>().enabled = true;
                        //miniCard.AddComponent<DragMiniCards>();
                        //miniCard.AddComponent<PhotonView>();
                        Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
                        card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage);
                        card.name = cardClicked.cardName;
                        card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);

                        if (PhotonNetwork.IsMasterClient)
                        {
                            int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                            int availableGold = getGold - cardClicked.gold;
                            Gold.instance.SetGold(availableGold);
                            properties["masterGold"] = availableGold;
                            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                            //Invoke("UpdatedProperties", 1f);
                        }
                        else if (!PhotonNetwork.IsMasterClient)
                        {
                            int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                            int availableGold = getGold - cardClicked.gold;
                            Gold.instance.SetGold(availableGold);
                            properties["clientGold"] = availableGold;
                            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                            //Debug.LogError("available client " + availableGold);

                            //Invoke("UpdatedProperties", 1f);
                        }
                    }
                    else
                    {
                        Debug.Log(" else called ");
                        Debug.Log(" Gold error tool tip " + goldErrorTooltip);
                        Debug.Log(" gold text name " + goldErrorTooltip.transform.Find("Gold Error Text").name);
                        Debug.Log(" parent name " + goldErrorTooltip.transform.parent.name + " back ground " + goldErrorTooltip.transform.parent.parent.name + " game board " + goldErrorTooltip.transform.parent.parent.parent.name);
                        goldErrorTooltip.SetActive(true);
                        goldErrorTooltip.transform.Find("Gold Error Text").GetComponent<TMP_Text>().SetText("You can not recruit this card!!!");
                        Invoke("DisableGoldErrorTooltip", 1f);
                    }
                    //GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation);
                    //miniCard.transform.SetParent(hand.transform.GetChild(i));
                    //miniCard.transform.position = hand.transform.GetChild(i).transform.position;
                    //miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                    //miniCard.GetComponent<DragMiniCards>().enabled = true;
                    ////miniCard.AddComponent<DragMiniCards>();
                    ////miniCard.AddComponent<PhotonView>();
                    //Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
                    //card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount ,cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage);
                    //card.name = cardClicked.cardName;
                    //card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);

                    //if (PhotonNetwork.IsMasterClient)
                    //{
                    //    int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                    //    int availableGold = getGold - cardClicked.gold;
                    //    Gold.instance.SetGold(availableGold);
                    //    properties["masterGold"] = availableGold;
                    //    PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                    //    //Invoke("UpdatedProperties", 1f);
                    //}
                    //else if (!PhotonNetwork.IsMasterClient)
                    //{
                    //    int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    //    int availableGold = getGold - cardClicked.gold;
                    //    Gold.instance.SetGold(availableGold);
                    //    properties["clientGold"] = availableGold;
                    //    PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                    //    //Debug.LogError("available client " + availableGold);

                    //    //Invoke("UpdatedProperties", 1f);
                    //}

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
                    Debug.Log(playerController.totalXP + " player total xp " + enemyController.totalXP + " enemy's XP");
                    CardDetails cardClicked = cardDetails.Find(item => item.id == id);
                    Debug.Log(cardClicked.levelRequired + " level required");
                    int level = int.Parse(cardClicked.levelRequired.Split(" ")[1]);
                    Debug.LogError("level " + level + " total xp " + playerController.totalXP);
                    if (IsRecruit(playerController.totalXP, level))
                    {
                        GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyHand.transform.GetChild(i).position, enemyHand.transform.GetChild(i).rotation);
                        miniCard.transform.SetParent(enemyHand.transform.GetChild(i));
                        miniCard.transform.position = enemyHand.transform.GetChild(i).transform.position;
                        miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                        miniCard.AddComponent<DragMiniCards>();
                        Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
                        card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage);
                        card.name = cardClicked.cardName;
                        card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
                        miniCard.SetActive(false);
                    }
                    //    GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyHand.transform.GetChild(i).position, enemyHand.transform.GetChild(i).rotation);
                    //miniCard.transform.SetParent(enemyHand.transform.GetChild(i));
                    //miniCard.transform.position = enemyHand.transform.GetChild(i).transform.position;
                    //miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                    //miniCard.AddComponent<DragMiniCards>();
                    //Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
                    //card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId , cardClicked.ergoTokenAmount , cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage);
                    //card.name = cardClicked.cardName;
                    //card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
                    //miniCard.SetActive(false);
                    break;
                }
            }
        }
    }
    #endregion

    //private void UpdatedProperties()
    //{
    //    Debug.LogError("1s photon master custom properties:- " + PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
    //    Debug.LogError("1s Photon client custom properties:- " + PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
    //}

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

    public bool IsRecruit(int playerXP, int level)
    {
        if(playerXP >= 0 && playerXP < 200)
        {
            if(level <= 1)
            {
                Debug.Log(level + " level value");
                return true;
            }
        }
        else if(playerXP >= 0 && playerXP < 400)
        {
            if(level <= 4)
            {
                Debug.Log(level + " level value");
                return true;
            }
        }
        else if (playerXP >= 0 && playerXP < 600)
        {
            if (level <= 8)
            {
                Debug.Log(level + " level value");
                return true;
            }
        }
        else if (playerXP >= 600)
        {
            Debug.Log(level + " level value");
            return true;

        }
        else
        {
            Debug.Log(level + " level value");
            return false;
        }
        return false;
    }

}
