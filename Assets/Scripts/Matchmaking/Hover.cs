using Photon.Pun;
using System.Collections.Generic;
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
    private GameBoardManager gameboardManager;
    private TMP_Text error;
    private bool isHovering = false;
    private TMP_Text errorText;

    private Queue<int> goldUpdateQueue = new Queue<int>();
    private bool isProcessingGoldUpdate = false;
    private List<int> goldList = new();

    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    #endregion

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        cardUI = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("StoreUI").gameObject;
        goldErrorTooltip = cardUI.transform.GetChild(cardUI.transform.childCount - 1).gameObject;
        errorText = goldErrorTooltip.transform.Find("Gold Error Text").GetComponent<TMP_Text>();
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
        if (transform.GetComponent<Card>() != null && transform.GetComponent<Card>().transform.parent.name != null)
        {
            Debug.Log("transform name " + transform.GetComponent<Card>() + " card parent " + transform.GetComponent<Card>().transform.parent.name);
        }
        isHovering = false;
        isClicked = false;
        Invoke("HideInfoCard", 0.1f);
        //HideInfoCard();
    }

    private void Start()
    {
        gameBoardParent = GameObject.Find("Game Board Parent");
        gameboardManager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
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
        if (gameObject?.transform?.parent?.name == "Content")
        {
            gameObject.transform.GetChild(0).GetChild(gameObject.transform.GetChild(0).childCount - 1).gameObject.SetActive(true);

            int id = gameObject.transform.GetChild(0).GetComponent<Card>().id;

            CardDetails hoveredCard = cardDetails.Find(item => item.id == id);
            Card cardInfo = Instantiate<Card>(infoCard, cardparent1.transform);
            cardInfo.SetProperties(hoveredCard.id, hoveredCard.ergoTokenId, hoveredCard.ergoTokenAmount, hoveredCard.cardName, hoveredCard.cardDescription, hoveredCard.attack, hoveredCard.HP, hoveredCard.gold, hoveredCard.XP, hoveredCard.fieldLimit, hoveredCard.fieldPosition, hoveredCard.clan, hoveredCard.levelRequired, hoveredCard.cardImage, hoveredCard.cardFrame, hoveredCard.cardClass, hoveredCard.ability
                //, hoveredCard.requirements, hoveredCard.abilityLevel
                );
            cardInfo.transform.Find("Amount Image").gameObject.GetComponent<Image>().enabled = false;
            cardInfo.transform.Find("Ergo Token Amount").gameObject.GetComponent<TMP_Text>().enabled = false;
            cardInfo.transform.localScale = new Vector3(1 / 2.31f, 1 / 2.34f, 1);
        }
        else if (gameObject?.transform?.parent?.parent?.name == "Player Hand")
        {
            int id = gameObject.transform.GetChild(0).GetComponent<Card>().id;
            Debug.Log(gameObject.transform.parent.name.Split(" ")[2] + " name of card postition");
            int index = int.Parse(gameObject.transform.parent.name.Split(" ")[2]);
            Debug.Log(index + " index ");
            GameObject actualParent = cardparent2.transform.GetChild(0).gameObject;
            Debug.Log(actualParent + " actual parent name");

            CardDetails hoveredCard = cardDetails.Find(item => item.id == id);
            Card cardInfo = Instantiate<Card>(infoCard, actualParent.transform);
            cardInfo.SetProperties(hoveredCard.id, hoveredCard.ergoTokenId, hoveredCard.ergoTokenAmount, hoveredCard.cardName, hoveredCard.cardDescription, hoveredCard.attack, hoveredCard.HP, hoveredCard.gold, hoveredCard.XP, hoveredCard.fieldLimit, hoveredCard.fieldPosition, hoveredCard.clan, hoveredCard.levelRequired, hoveredCard.cardImage, hoveredCard.cardFrame, hoveredCard.cardClass, hoveredCard.ability
                //, hoveredCard.requirements, hoveredCard.abilityLevel
                );
            Debug.Log(transform.position + " transform pos " + cardInfo.transform.position + " card info pos " + transform.name);
            Debug.Log(transform.localPosition + " transform local pos " + cardInfo.transform.localPosition + " card info local pos " + transform.parent.name);
            Debug.Log(transform.parent.localPosition + " transform local pos " + transform.position + " card info local pos " + transform.parent.name);
            Transform cardPos = cardInfo.transform;
            //cardPos.localPosition = 
            cardInfo.transform.localPosition = new Vector3(transform.parent.localPosition.x + 350f, transform.parent.localPosition.y - 500f, 0);
            cardInfo.transform.Find("Amount Image").gameObject.GetComponent<Image>().enabled = false;
            cardInfo.transform.Find("Ergo Token Amount").gameObject.GetComponent<TMP_Text>().enabled = false;
            cardInfo.transform.localScale = new Vector3(1 / 2.31f, 1 / 2.34f, 1);
        }
        else if (gameObject.transform.parent.name == "Canvas")
        {
            return;
        }
    }

    private void HideInfoCard()
    {
        Debug.Log("hide card");
        Debug.Log(gameObject.name + " game object name");
        if (transform.childCount == 1)
        {
            transform?.GetChild(0)?.GetChild(gameObject.transform.GetChild(0).childCount - 1)?.gameObject?.SetActive(false);
        }

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
                if (cardparent2.transform.GetChild(i).childCount > 0)
                    Destroy(cardparent2.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }

    //public void RecruitCard()
    //{
    //    ResetAnimation();
    //    CardDetails clickedCard = cardDetails.Find(item => item.id == gameObject.transform.GetChild(0).GetComponent<Card>().id);
    //    Debug.Log("clicked card " + clickedCard);
    //    int clickedCardid = clickedCard.id;

    //    if (Gold.instance.GetGold() >= clickedCard.gold)
    //    {
    //        pv = gameObject.transform.GetComponent<PhotonView>();
    //        Debug.Log(" gold called");
    //        pv.RPC("Recruit", RpcTarget.All, clickedCardid);
    //    }
    //    else
    //    {
    //        errorText.gameObject.SetActive(true);
    //        errorText.SetText("You can not buy card more than gold you have.");
    //        Invoke("DisableGoldErrorTooltip", 1f);
    //    }
    //}

    public void RecruitCard()
    {
        ResetAnimation();
        CardDetails clickedCard = cardDetails.Find(item => item.id == gameObject.transform.GetChild(0).GetComponent<Card>().id);
        Debug.Log("clicked card " + clickedCard);
        int clickedCardid = clickedCard.id;

        if (Gold.instance.GetGold() >= clickedCard.gold)
        {
            PhotonView pv = gameObject.transform.GetComponent<PhotonView>();
            Debug.Log(" gold called");
            playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
            enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

            for (int i = 0; i < hand.transform.childCount; i++)
            {
                if (hand.transform.GetChild(i).childCount == 0)
                {
                    Debug.Log("%%%%%$$$ inside instantiate with the i :- " + i + " parent value " + hand.transform.parent.parent.parent);
                    Debug.Log(hand.transform.GetChild(i).childCount + " %%%%%$$$ child count " + i);
                    Debug.Log(playerController.totalXP + " %%%%%$$$ player total xp " + enemyController.totalXP + " enemy's XP");
                    CardDetails cardClicked = cardDetails.Find(item => item.id == clickedCardid);
                    Debug.Log(cardClicked.levelRequired + " level req");
                    int level = (int)(cardClicked.levelRequired);
                    Debug.LogError("level " + level + " total xp " + playerController.totalXP);
                    if (IsRecruit(playerController.totalXP, level))
                    {
                        Debug.Log("%%%%%$$$ inside recruit i:- " + i + " parent value " + hand.transform.parent.parent.parent);
                        GameObject miniCardParent;
                        string prefabPath = cardClicked.cardName;
                        GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);
                        if (cardPrefab != null)
                        {
                            Debug.LogError("%%%%%$$$ Prefab found at path: " + prefabPath + " i val " + i);
                            hand.transform.GetChild(i).gameObject.SetActive(true);
                            miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation); miniCardParent.transform.SetParent(hand.transform.GetChild(i));
                            miniCardParent.transform.position = hand.transform.GetChild(i).transform.position;
                            miniCardParent.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                            miniCardParent.GetComponent<DragMiniCards>().enabled = true;
                            miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;
                            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                            miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
                                //, cardClicked.requirements, cardClicked.abilityLevel
                                );
                            miniCard.name = cardClicked.cardName;
                            miniCardParent.name = cardClicked.cardName;

                        }
                        else
                        {
                            Debug.LogError("%%%%%$$$ Prefab not found at path: " + prefabPath + " i val " + i);
                            hand.transform.GetChild(i).gameObject.SetActive(true);
                            miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation);
                            miniCardParent.transform.SetParent(hand.transform.GetChild(i));
                            miniCardParent.transform.SetParent(hand.transform.GetChild(i));
                            miniCardParent.transform.position = hand.transform.GetChild(i).transform.position;
                            miniCardParent.transform.localScale = hand.transform.GetChild(i).transform.localScale;
                            miniCardParent.GetComponent<DragMiniCards>().enabled = true;
                            miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;
                            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                            miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
                                //, cardClicked.requirements, cardClicked.abilityLevel
                                );
                            CardDataBase.instance.UpdateSkill(cardClicked.ability, miniCard);
                            miniCard.name = cardClicked.cardName;
                            miniCardParent.name = cardClicked.cardName;
                        }

                        if (PhotonNetwork.IsMasterClient)
                        {
                            Debug.Log("master client " + PhotonNetwork.IsMasterClient);
                            //int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                            int getGold = Gold.instance.GetGold();
                            //(int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                            Debug.Log(getGold + " get gold ");
                            int availableGold = getGold - cardClicked.gold;
                            //goldList.Add(cardClicked.gold);
                            Debug.Log(availableGold + " available gold "); Debug.Log(getGold + " get gold " + availableGold + " parent og gold " + Gold.instance.gameObject.transform.parent.parent.name);
                            //UpdateGoldLocally(availableGold, PhotonNetwork.IsMasterClient, goldList);
                            //UpdateGold(availableGold, PhotonNetwork.IsMasterClient);
                            Gold.instance.SetGold(availableGold);
                            properties["masterGold"] = availableGold;
                            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                        }
                        else if (!PhotonNetwork.IsMasterClient)
                        {
                            Debug.Log("!master client " + PhotonNetwork.IsMasterClient);
                            //int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                            int getGold = Gold.instance.GetGold();
                            Debug.Log(getGold + " get gold ");
                            int availableGold = getGold - cardClicked.gold;
                            Debug.Log(availableGold + " available gold ");
                            Debug.Log(availableGold + " available gold "); Debug.Log(getGold + " get gold " + availableGold + " parent og gold " + Gold.instance.gameObject.transform.parent.parent.name);
                            //goldList.Add(cardClicked.gold);
                            //UpdateGoldLocally(availableGold, PhotonNetwork.IsMasterClient, goldList);
                            //UpdateGold(availableGold, PhotonNetwork.IsMasterClient);
                            Gold.instance.SetGold(availableGold);
                            properties["clientGold"] = availableGold;
                            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                        }

                        pv.RPC("RecruitCardOnOthers", RpcTarget.Others, clickedCardid, i);
                    }
                    else
                    {
                        Debug.Log(" else called ");
                        Debug.Log(" Gold error tool tip " + goldErrorTooltip);
                        errorText.gameObject.SetActive(true);
                        errorText.SetText("You can not recruit this card!!!");
                        Invoke("DisableGoldErrorTooltip", 1f);
                    }
                    break;
                }
            }


            //pv.RPC("Recruit", RpcTarget.All, clickedCardid);
        }
        else
        {
            ShowError("You can not buy card more than gold you have.");
        }
    }

    private void ShowError(string message)
    {
        errorText.gameObject.SetActive(true);
        errorText.SetText(message);
        Invoke("DisableGoldErrorTooltip", 1f);
    }


    private void DisableGoldErrorTooltip()
    {
        errorText.gameObject.SetActive(false);
    }

    #region RPC Method
    [PunRPC]
    public void RecruitCardOnOthers(int id, int pos)
    {

        Debug.Log("%%%%%$$$ inside instantiate with the i :- " + pos + " parent value " + enemyHand.transform.parent.parent.parent);
        Debug.Log(enemyHand.transform.GetChild(pos).childCount + " %%%%%$$$ child count " + id);
        CardDetails cardClicked = cardDetails.Find(item => item.id == id);

        GameObject miniCardParent;
        string prefabPath = cardClicked.cardName;
        GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);
        Debug.Log(enemyHand.transform.GetChild(pos).childCount + " enemyHand.transform.GetChild(pos).childCount");
        for(int  i = 0; i < enemyHand.transform.GetChild(pos).childCount; i++)
        {
            Destroy(enemyHand.transform.GetChild(pos).GetChild(i).gameObject);
        }

        if (cardPrefab != null)
        {
            Debug.LogError("%%%%%$$$ Prefab found at path: " + prefabPath + " i val " + pos);
            enemyHand.transform.GetChild(pos).gameObject.SetActive(true);
            miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, enemyHand.transform.GetChild(pos).position, enemyHand.transform.GetChild(pos).rotation); miniCardParent.transform.SetParent(enemyHand.transform.GetChild(pos));
            miniCardParent.transform.position = enemyHand.transform.GetChild(pos).transform.position;
            miniCardParent.transform.localScale = enemyHand.transform.GetChild(pos).transform.localScale;
            miniCardParent.GetComponent<DragMiniCards>().enabled = true;
            miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;
            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
            miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
                //, cardClicked.requirements, cardClicked.abilityLevel
                );
            miniCard.name = cardClicked.cardName;
            miniCardParent.name = cardClicked.cardName;
            miniCardParent.SetActive(false);
            miniCardParent.GetComponent<CanvasGroup>().blocksRaycasts = false;

        }
        else
        {
            Debug.LogError("%%%%%$$$ Prefab not found at path: " + prefabPath + " i val " + pos);
            enemyHand.transform.GetChild(pos).gameObject.SetActive(true);
            miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyHand.transform.GetChild(pos).position, enemyHand.transform.GetChild(pos).rotation);
            miniCardParent.transform.SetParent(enemyHand.transform.GetChild(pos));
            miniCardParent.transform.SetParent(enemyHand.transform.GetChild(pos));
            miniCardParent.transform.position = enemyHand.transform.GetChild(pos).transform.position;
            miniCardParent.transform.localScale = enemyHand.transform.GetChild(pos).transform.localScale;
            miniCardParent.GetComponent<DragMiniCards>().enabled = true;
            miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;
            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
            miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
                //, cardClicked.requirements, cardClicked.abilityLevel
                );
            CardDataBase.instance.UpdateSkill(cardClicked.ability, miniCard);
            miniCard.name = cardClicked.cardName;
            miniCardParent.name = cardClicked.cardName;
            miniCardParent.SetActive(false);
            miniCardParent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }

    //[PunRPC]
    //public void Recruit(int id)
    //{
    //    Debug.Log("%%%%%$$$ Recruit called " + id);
    //    playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
    //    enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

    //    pv = gameObject.transform.GetComponent<PhotonView>();
    //    Debug.Log(pv + " photon view " + pv.IsMine);
    //    if (pv.IsMine)
    //    {
    //        Debug.Log("%%%%%$$$ is mine called ");
    //        for (int i = 0; i < hand.transform.childCount; i++)
    //        {
    //            if (hand.transform.GetChild(i).childCount == 0)
    //            {
    //                Debug.Log("%%%%%$$$ inside instantiate with the i :- " + i + " parent value " + hand.transform.parent.parent.parent);
    //                Debug.Log(hand.transform.GetChild(i).childCount + " %%%%%$$$ child count " + i);
    //                Debug.Log(playerController.totalXP + " %%%%%$$$ player total xp " + enemyController.totalXP + " enemy's XP");
    //                CardDetails cardClicked = cardDetails.Find(item => item.id == id);
    //                Debug.Log(cardClicked.levelRequired + " level req");
    //                int level = (int)(cardClicked.levelRequired);
    //                Debug.LogError("level " + level + " total xp " + playerController.totalXP);
    //                if (IsRecruit(playerController.totalXP, level))
    //                {
    //                    Debug.Log("%%%%%$$$ inside recruit i:- " + i + " parent value " + hand.transform.parent.parent.parent);
    //                    GameObject miniCardParent;
    //                    string prefabPath = cardClicked.cardName;
    //                    GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);
    //                    if (cardPrefab != null)
    //                    {
    //                        Debug.LogError("%%%%%$$$ Prefab found at path: " + prefabPath +  " i val "+ i );
    //                        hand.transform.GetChild(i).gameObject.SetActive(true);
    //                        miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation); miniCardParent.transform.SetParent(hand.transform.GetChild(i));
    //                        miniCardParent.transform.position = hand.transform.GetChild(i).transform.position;
    //                        miniCardParent.transform.localScale = hand.transform.GetChild(i).transform.localScale;
    //                        miniCardParent.GetComponent<DragMiniCards>().enabled = true;
    //                        miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;
    //                        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
    //                        miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
    //                            //, cardClicked.requirements, cardClicked.abilityLevel
    //                            );
    //                        miniCard.name = cardClicked.cardName;
    //                        miniCardParent.name = cardClicked.cardName;

    //                    }
    //                    else
    //                    {
    //                        Debug.LogError("%%%%%$$$ Prefab not found at path: " + prefabPath + " i val " + i);
    //                        hand.transform.GetChild(i).gameObject.SetActive(true);
    //                        miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation);
    //                        miniCardParent.transform.SetParent(hand.transform.GetChild(i));
    //                        miniCardParent.transform.SetParent(hand.transform.GetChild(i));
    //                        miniCardParent.transform.position = hand.transform.GetChild(i).transform.position;
    //                        miniCardParent.transform.localScale = hand.transform.GetChild(i).transform.localScale;
    //                        miniCardParent.GetComponent<DragMiniCards>().enabled = true;
    //                        miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;
    //                        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
    //                        miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
    //                            //, cardClicked.requirements, cardClicked.abilityLevel
    //                            );
    //                        gameboardManager.UpdateSkill(cardClicked.ability, miniCard);
    //                        miniCard.name = cardClicked.cardName;
    //                        miniCardParent.name = cardClicked.cardName;
    //                    }

    //                    //GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", hand.transform.GetChild(i).position, hand.transform.GetChild(i).rotation);
    //                    //miniCard.transform.SetParent(hand.transform.GetChild(i));
    //                    //miniCard.transform.position = hand.transform.GetChild(i).transform.position;
    //                    //miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
    //                    //miniCard.GetComponent<DragMiniCards>().enabled = true;
    //                    //Debug.Log(miniCard.name + " mini card ");
    //                    ////miniCard.AddComponent<DragMiniCards>();
    //                    ////miniCard.AddComponent<PhotonView>();
    //                    //Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
    //                    //card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
    //                    //    //, cardClicked.requirements, cardClicked.abilityLevel
    //                    //    );
    //                    //card.name = cardClicked.cardName;
    //                    //card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
    //                    //gameboardManager.UpdateSkill(cardClicked.ability, card);
    //                    //Debug.Log(card + " card value");

    //                    if (PhotonNetwork.IsMasterClient)
    //                    {
    //                        Debug.Log("master client " + PhotonNetwork.IsMasterClient);
    //                        //int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
    //                        int getGold = Gold.instance.GetGold();
    //                            //(int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
    //                        Debug.Log(getGold + " get gold ");
    //                        int availableGold = getGold - cardClicked.gold;
    //                        //goldList.Add(cardClicked.gold);
    //                        Debug.Log(availableGold + " available gold "); Debug.Log(getGold + " get gold " +  availableGold + " parent og gold " + Gold.instance.gameObject.transform.parent.parent.name);
    //                        //UpdateGoldLocally(availableGold, PhotonNetwork.IsMasterClient, goldList);
    //                        //UpdateGold(availableGold, PhotonNetwork.IsMasterClient);
    //                        Gold.instance.SetGold(availableGold);
    //                        properties["masterGold"] = availableGold;
    //                        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //                    }
    //                    else if (!PhotonNetwork.IsMasterClient)
    //                    {
    //                        Debug.Log("!master client " + PhotonNetwork.IsMasterClient);
    //                        //int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
    //                        int getGold = Gold.instance.GetGold(); 
    //                        Debug.Log(getGold + " get gold ");
    //                        int availableGold = getGold - cardClicked.gold;
    //                        Debug.Log(availableGold + " available gold ");
    //                        Debug.Log(availableGold + " available gold "); Debug.Log(getGold + " get gold " + availableGold + " parent og gold " + Gold.instance.gameObject.transform.parent.parent.name);
    //                        //goldList.Add(cardClicked.gold);
    //                        //UpdateGoldLocally(availableGold, PhotonNetwork.IsMasterClient, goldList);
    //                        //UpdateGold(availableGold, PhotonNetwork.IsMasterClient);
    //                        Gold.instance.SetGold(availableGold);
    //                        properties["clientGold"] = availableGold;
    //                        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //                    }
    //                }
    //                else
    //                {
    //                    Debug.Log(" else called ");
    //                    Debug.Log(" Gold error tool tip " + goldErrorTooltip);
    //                    errorText.gameObject.SetActive(true);
    //                    errorText.SetText("You can not recruit this card!!!");
    //                    Invoke("DisableGoldErrorTooltip", 1f);
    //                }
    //                break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("%%%%%$$$ not mine " + pv.IsMine);
    //        for (int i = 0; i < hand.transform.childCount; i++)
    //        {
    //            if (enemyHand.transform.GetChild(i).childCount == 0)
    //            {
    //                Debug.Log(enemyHand.transform.GetChild(i).childCount + " %%%%%$$$ enemyHand.transform.GetChild(i).childCount " + enemyHand.transform.parent.parent.parent + " parent of enemy hand with I: " + i);
    //                Debug.Log(playerController.totalXP + " %%%%%$$$ player total xp " + enemyController.totalXP + " enemy's XP");
    //                CardDetails cardClicked = cardDetails.Find(item => item.id == id);
    //                Debug.Log(cardClicked.levelRequired + " %%%%%$$$ level required");
    //                int level = (int)(cardClicked.levelRequired);
    //                Debug.LogError("%%%%%$$$ level " + level + " total xp " + playerController.totalXP);
    //                if (IsRecruit(playerController.totalXP, level))
    //                {
    //                    Debug.Log("%%%%%$$$ inside recruit ");
    //                    GameObject miniCardParent;
    //                    string prefabPath =  cardClicked.cardName;
    //                    GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);
    //                    if (cardPrefab != null)
    //                    {
    //                        Debug.LogError("%%%%%$$$ Prefab found at path: " + prefabPath);
    //                        enemyHand.transform.GetChild(i).gameObject.SetActive(true);
    //                        miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, enemyHand.transform.GetChild(i).position, enemyHand.transform.GetChild(i).rotation); 
    //                        miniCardParent.transform.SetParent(enemyHand.transform.GetChild(i));
    //                        miniCardParent.transform.position = enemyHand.transform.GetChild(i).transform.position;
    //                        miniCardParent.transform.localScale = enemyHand.transform.GetChild(i).transform.localScale;
    //                        miniCardParent.AddComponent<DragMiniCards>(); ;
    //                        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
    //                        miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
    //                            //, cardClicked.requirements, cardClicked.abilityLevel
    //                            );
    //                        miniCard.name = cardClicked.cardName;
    //                        miniCard.transform.GetChild(miniCard.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
    //                        miniCardParent.name = cardClicked.cardName;
    //                        miniCardParent.SetActive(false);
    //                        miniCardParent.GetComponent<CanvasGroup>().blocksRaycasts = false;

    //                    }
    //                    else
    //                    {
    //                        Debug.LogError("%%%%%$$$ Prefab not found at path: " + prefabPath);
    //                        enemyHand.transform.GetChild(i).gameObject.SetActive(true);
    //                        miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyHand.transform.GetChild(i).position, enemyHand.transform.GetChild(i).rotation);
    //                        miniCardParent.transform.SetParent(enemyHand.transform.GetChild(i));
    //                        miniCardParent.transform.position = enemyHand.transform.GetChild(i).transform.position;
    //                        miniCardParent.transform.localScale = enemyHand.transform.GetChild(i).transform.localScale;
    //                        miniCardParent.AddComponent<DragMiniCards>();
    //                        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
    //                        miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
    //                            //, cardClicked.requirements, cardClicked.abilityLevel
    //                            );
    //                        gameboardManager.UpdateSkill(cardClicked.ability, miniCard);
    //                        miniCard.name = cardClicked.cardName;
    //                        miniCardParent.name = cardClicked.cardName;
    //                        miniCardParent.SetActive(false);
    //                        miniCardParent.GetComponent<CanvasGroup>().blocksRaycasts = false;
    //                    }

    //                    //GameObject miniCard = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyHand.transform.GetChild(i).position, enemyHand.transform.GetChild(i).rotation);
    //                    //miniCard.transform.SetParent(enemyHand.transform.GetChild(i));
    //                    //miniCard.transform.position = enemyHand.transform.GetChild(i).transform.position;
    //                    //miniCard.transform.localScale = hand.transform.GetChild(i).transform.localScale;
    //                    //miniCard.AddComponent<DragMiniCards>();
    //                    //Card card = miniCard.transform.GetChild(0).GetComponent<Card>();
    //                    //card.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability
    //                    //    //, cardClicked.requirements, cardClicked.abilityLevel
    //                    //    );
    //                    //Debug.Log(card + " card");
    //                    //card.name = cardClicked.cardName;
    //                    //card.transform.GetChild(card.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
    //                    //gameboardManager.UpdateSkill(cardClicked.ability, card);
    //                    //miniCard.SetActive(false);
    //                }
    //                break;
    //            }
    //        }
    //    }
    //}

    //[PunRPC]
    //public void Recruit(int id)
    //{
    //    Debug.Log("Recruit called with id: " + id);

    //    PhotonView pv = gameObject.transform.GetComponent<PhotonView>();
    //    bool isMine = pv.IsMine;
    //    Debug.Log("photon view " + isMine);

    //    if (isMine)
    //    {
    //        ProcessRecruit(id, true);
    //    }
    //    else
    //    {
    //        ProcessRecruit(id, false);
    //    }
    //}

    //private void ProcessRecruit(int id, bool isMine)
    //{
    //    PlayerController playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
    //    EnemyController enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

    //    Transform handTransform = isMine ? hand.transform : enemyHand.transform;
    //    Transform fieldTransform = isMine ? playerController.transform : enemyController.transform;

    //    for (int i = 0; i < handTransform.childCount; i++)
    //    {
    //        if (handTransform.GetChild(i).childCount == 0)
    //        {
    //            CardDetails cardClicked = cardDetails.Find(item => item.id == id);
    //            int level = (int)(cardClicked.levelRequired);

    //            if (IsRecruit(playerController.totalXP, level))
    //            {
    //                GameObject miniCardParent;
    //                string prefabPath = cardClicked.cardName;
    //                GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);

    //                if (cardPrefab != null)
    //                {
    //                    miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, handTransform.GetChild(i).position, handTransform.GetChild(i).rotation);
    //                }
    //                else
    //                {
    //                    miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", handTransform.GetChild(i).position, handTransform.GetChild(i).rotation);
    //                }

    //                miniCardParent.transform.SetParent(handTransform.GetChild(i));
    //                miniCardParent.transform.localPosition = Vector3.zero;
    //                miniCardParent.transform.localScale = Vector3.one;
    //                miniCardParent.GetComponent<DragMiniCards>().enabled = true;
    //                miniCardParent.GetComponent<HoverMiniCard>().isEnable = true;

    //                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
    //                miniCard.SetMiniCard(cardClicked.id, cardClicked.ergoTokenId, cardClicked.ergoTokenAmount, cardClicked.cardName, cardClicked.attack, cardClicked.HP, cardClicked.gold, cardClicked.XP, cardClicked.cardImage, cardClicked.ability);
    //                miniCard.name = cardClicked.cardName;
    //                miniCardParent.name = cardClicked.cardName;

    //                if (PhotonNetwork.IsMasterClient)
    //                {
    //                    UpdateGold(cardClicked.gold, true);
    //                }
    //                else
    //                {
    //                    UpdateGold(cardClicked.gold, false);
    //                }
    //            }
    //            else
    //            {
    //                ShowError("You can not recruit this card!!!");
    //            }
    //            break;
    //        }
    //    }
    //}

    //private void UpdateGold(int cardCost, bool isMasterClient)
    //{
    //    int currentGold = Gold.instance.GetGold();
    //    int updatedGold = currentGold - cardCost;
    //    Gold.instance.SetGold(updatedGold);

    //    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    //    if (isMasterClient)
    //    {
    //        properties["masterGold"] = updatedGold;
    //    }
    //    else
    //    {
    //        properties["clientGold"] = updatedGold;
    //    }
    //    PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //}
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
        Debug.Log("is recruit called " + playerXP + " level " + level);
        if (playerXP >= 0 && playerXP < 200)
        {
            if (level < 1)
            {
                Debug.Log(level + " level value");
                return true;
            }
        }
        else if (playerXP >= 0 && playerXP < 400)
        {
            if (level <= 1)
            {
                Debug.Log(level + " level value");
                return true;
            }
        }
        else if (playerXP >= 0 && playerXP < 600)
        {
            if (level <= 2)
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

    //private void UpdateGold(int newGold, bool isMaster)
    //{
    //    Gold.instance.SetGold(newGold);
    //    if (isMaster)
    //    {
    //        properties["masterGold"] = newGold;
    //    }
    //    else
    //    {
    //        properties["clientGold"] = newGold;
    //    }
    //    PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //}

    //private void UpdateGoldLocally(int newGold, bool isMaster, List<int> gList)
    //{
    //    Debug.Log(newGold + " new gold " + isMaster + " master ");
    //    if (isMaster)
    //    {
    //        Debug.Log("master " + gList.Count);
    //        //int totalDeductGold = 0;
    //        //for(int i = 0; i < gList.Count; i++)
    //        //{
    //        //    Debug.Log(gList[i] + " gold saperate value");
    //        //    totalDeductGold += gList[i];
    //        //}
    //        int getGold = Gold.instance.GetGold();
    //        int availableGold = getGold - totalDeductGold;
    //        Debug.Log(getGold + " get gold " + totalDeductGold + " deduct gold " + availableGold + " parent og gold " + Gold.instance.gameObject.transform.parent.parent.name);
    //        Gold.instance.SetGold(availableGold);
    //        properties["masterGold"] = availableGold;
    //        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //        goldList.Clear();
    //    }
    //    else
    //    {
    //        Debug.Log("!master " + gList.Count);
    //        int totalDeductGold = 0;
    //        //for (int i = 0; i < gList.Count; i++)
    //        //{
    //        //    Debug.Log(gList[i] + " gold saperate value");
    //        //    totalDeductGold += gList[i];
    //        //}
    //        int getGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
    //        int availableGold = getGold - totalDeductGold;
    //        Debug.Log(getGold + " get gold " + totalDeductGold + " deduct gold " + availableGold);
    //        properties["clientGold"] = availableGold;
    //        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //        goldList.Clear();
    //    }
    //    //if(isMaster)
    //    //{
    //    //    Gold.instance.SetGold(newGold);
    //    //    goldUpdateQueue.Enqueue(newGold);
    //    //    if (!isProcessingGoldUpdate)
    //    //    {
    //    //        StartCoroutine(ProcessGoldUpdateQueue(isMaster));
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    Gold.instance.SetGold(newGold);
    //    //    goldUpdateQueue.Enqueue(newGold);
    //    //    if (!isProcessingGoldUpdate)
    //    //    {
    //    //        StartCoroutine(ProcessGoldUpdateQueue(isMaster));
    //    //    }
    //    //}

    //}

    //private IEnumerator ProcessGoldUpdateQueue(bool isMaster)
    //{
    //    isProcessingGoldUpdate = true;
    //    Debug.Log(goldUpdateQueue.Count + " gold queue ccount");
    //    while (goldUpdateQueue.Count > 0)
    //    {
    //        int newGold = goldUpdateQueue.Dequeue();
    //        if(isMaster)
    //            properties["masterGold"] = newGold;
    //        else
    //            properties["clientGold"] = newGold;
    //        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    //        yield return null; 
    //    }

    //    isProcessingGoldUpdate = false;
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Time.timeScale = 1;
        }
    }
}
