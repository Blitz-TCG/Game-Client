using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    #region Variables
    [SerializeField] private GameObject cardListParent;
    [SerializeField] public GameObject biddingPanel;
    [SerializeField] public GameObject countDownPanel;
    [SerializeField] public GameObject afterBiddingPanel;
    [SerializeField] private TMP_Text player1NameText;
    [SerializeField] private TMP_Text player2NameText;
    [SerializeField] private TMP_Text goldAmountText;
    [SerializeField] private TMP_Text winnerNameText;
    [SerializeField] private GameObject moreGoldPanel;
    [SerializeField] private TMP_InputField search;
    [SerializeField] private TMP_Text error;
    [SerializeField] public PhotonView pv;
    [SerializeField] private TMP_InputField field;
    [SerializeField] private Button bidButton;
    [SerializeField] private TMP_Text TMPT;
    [SerializeField] private Button turnButton;
    [SerializeField] public TMP_Text upMinText;
    [SerializeField] public TMP_Text downMinText;
    [SerializeField] public TMP_Text upSecText;
    [SerializeField] public TMP_Text downSecText;
    [SerializeField] private PlayerTimer timeUp;
    [SerializeField] private PlayerTimer timeDown;
    [SerializeField] private TimeLeft timeLeftTimer;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Sprite[] profileImages;
    [SerializeField] private GameObject downProfileIamge;
    [SerializeField] private GameObject upProfileImage;
    [SerializeField] private GameObject cardError;
    [SerializeField] private GameObject leftPlayerPanel;
    [SerializeField] private TMP_Text leftPlayerText;
    [SerializeField] private TMP_Text countdownMinText;
    [SerializeField] private TMP_Text countDownSecText;

    private SkirmishManager skirmishManager;
    private List<CardDetails> cardDetails;
    private List<CardDetails> currentCards = new List<CardDetails>();
    private List<CardDetails> sortedList = new List<CardDetails>();
    private List<CardDetails> selectedCardList = new List<CardDetails>();
    private GameObject loadingPanel;
    private GameObject gameBoardParent;
    private Timer timer;
    private Timer countdownTimer;
    private Timer bidTimer;
    private Timer afterBidTimer;
    private int player1Bid;
    private int player2Bid;
    private int randomPlayer;
    private int initaialGold;
    private string searchText;
    public static bool player1Turn;
    public static bool player2Turn;
    public static bool connectUsing;
    private ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
    private bool isBidPanelInitialize = true;
    private bool isBidEnds = true;
    private bool isAfterComplete = true;
    private const string ACTIVE_PLAYER_KEY = "ActivePlayer";
    private Room room;
    private Photon.Realtime.Player photonPlayer;
    private Coroutine coroutine;
    private bool endGame = false;
    private Card attackingcard;
    #endregion

    private void Start()
    {
        skirmishManager = SkirmishManager.instance;
        gameBoardParent = GameObject.Find("Game Board Parent");
        countdownTimer = countDownPanel.GetComponent<Timer>();
        bidTimer = biddingPanel.GetComponentInChildren<Timer>();
        afterBidTimer = afterBiddingPanel.GetComponent<Timer>();

        if (PhotonNetwork.IsMasterClient)
        {
            customProp["master"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            customProp["client"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
        }

        turnButton.onClick.AddListener(TurnButton);
        leaveButton.onClick.AddListener(GotoSkirmishMenu);
        turnButton.gameObject.SetActive(false);
        room = PhotonNetwork.CurrentRoom;

        PhotonNetwork.AutomaticallySyncScene = true;
        cardDetails = CardDataBase.instance.cardDetails;

        if (pv.IsMine)
            InitCards();

        initaialGold = Gold.instance.GetGold();
        CardDetails clickedCard = cardDetails.Find(item => item.id == 1);

        GetRandomWinner();
        countDownPanel.SetActive(true);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Count() == 2)
        {
            if (Timer.isBiddingTime)
            {
                Timer.isBiddingTime = false;
                if (isBidPanelInitialize)
                {
                    isBidPanelInitialize = false;
                    pv.RPC("BiddingPanel", RpcTarget.All);
                }
            }
            else if (Timer.isCompletedBid)
            {
                Timer.isCompletedBid = false;
                if (isBidEnds)
                {
                    isBidEnds = false;
                    pv.RPC("BidComplete", RpcTarget.All);
                }
            }
            else if (Timer.isAfterCompletedBid)
            {
                if (isAfterComplete)
                {
                    isAfterComplete = false;
                    Timer.isCompletedBid = false;
                    pv.RPC("AfterBidComplete", RpcTarget.All);
                }
            }
        }

        if (timeDown.completeTime)
        {
            timeDown.completeTime = false;
            downMinText.SetText("0");
            downSecText.SetText("00");
            timeDown.PauseTimer("down");
            timeUp.PauseTimer("up");
            StopAllCoroutines();
            resultPanel.SetActive(true);
            endGame = true;
            pv.RPC("CompleteGame", RpcTarget.Others);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                GameManager.instance.clicked = 0;
                attackingcard = null;
                ResetAnimation("player");
                ResetAnimation("field");
            }
            else
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>())
                {
                    if (player1Turn && PhotonNetwork.IsMasterClient)
                    {
                        attackingcard = EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>().gameObject.GetComponentInChildren<Card>();
                        if (attackingcard.IsAttack())
                        {
                            cardError.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You already attacked with this card. So, You can not attack with this card in this turn.");
                            Invoke("RemoveErrorObject", 2f);
                        }
                        else if (attackingcard.dropPosition == 0)
                        {
                            cardError.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You put the card hand to field. So can not attack with card in this turn");
                            Invoke("RemoveErrorObject", 2f);
                        }
                        else if (!attackingcard.IsAttack() && attackingcard.dropPosition == 1)
                        {
                            if (attackingcard.transform.parent.parent.tag == "Front Line Player")
                            {
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.tag == "Back Line Player")
                            {
                                cardError.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                                Invoke("RemoveErrorObject", 2f);
                            }
                        }
                    }
                    else if (!player1Turn && !PhotonNetwork.IsMasterClient)
                    {
                        attackingcard = EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>().gameObject.GetComponentInChildren<Card>();
                        if (attackingcard.IsAttack())
                        {
                            cardError.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You already attacked with this card. So, You can not attack with this card in this turn.");
                            Invoke("RemoveErrorObject", 2f);

                        }
                        else if (attackingcard.dropPosition == 0)
                        {
                            cardError.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You put the card hand to field. So can not attack with card in this turn.");
                            Invoke("RemoveErrorObject", 2f);

                        }
                        else if (!attackingcard.IsAttack() && attackingcard.dropPosition == 1)
                        {
                            if (attackingcard.transform.parent.parent.tag == "Front Line Player")
                            {
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.tag == "Back Line Player")
                            {
                                cardError.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                                Invoke("RemoveErrorObject", 2f);
                            }
                        }
                    }

                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>())
                {
                    if (GameManager.instance.clicked == 1)
                    {
                        Card targetCard = EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>().GetComponentInChildren<Card>();
                        GameObject attackingcardParent = attackingcard.transform.parent.parent.gameObject;

                        GameObject targetCardParent = EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>().transform.parent.gameObject;

                        AttackCard(attackingcard, attackingcardParent, targetCard, targetCardParent);

                        GameManager.instance.clicked = 0;
                        attackingcardParent = null;
                    }
                }
                else
                {
                    GameManager.instance.clicked = 0;
                    attackingcard = null;
                }
            }
        }
    }

    private void RemoveErrorObject()
    {
        cardError.SetActive(false);
    }

    private void AttackCard(Card attacking, GameObject attackParent, Card target, GameObject targetParent)
    {
        if (player1Turn && PhotonNetwork.IsMasterClient)
        {
            attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);
            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);

            attackingcard.SetAttackValue(true);

            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId);
        }
        else if (!player1Turn && !PhotonNetwork.IsMasterClient)
        {
            attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);

            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);

            attackingcard.SetAttackValue(true);

            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId);
        }
    }

    [PunRPC]
    private void AttackCardRPC(int attackId, int targetId)
    {
        GameObject attackParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetChild(attackId - 1).gameObject;
        GameObject targetParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetChild(targetId - 1).gameObject;

        Card attacking = attackParent.GetComponentInChildren<Card>();
        Card target = targetParent.GetComponentInChildren<Card>();

        attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
        target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);

    }

    public void InitCards()
    {
        int[] matchCards = ErgoQuery.instance.cardIdCurrentStore[skirmishManager.deckId - 1];
        int cardLength = ErgoQuery.instance.cardIdCurrentStore[skirmishManager.deckId - 1].Length;

        for (int i = 0; i < cardLength; i++)
        {
            if (cardDetails.Where(card => card.id == matchCards[i]).Count() == 1)
            {
                currentCards.Add(cardDetails[matchCards[i] - 1]);
            }
        }
        sortedList = currentCards.OrderBy(level => level.levelRequired).ToList();
        for (int i = 0; i < sortedList.Count; i++)
        {
            if (pv.IsMine)
            {
                GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", cardListParent.transform.position, cardListParent.transform.rotation);
                miniCardParent.transform.SetParent(cardListParent.transform);
                miniCardParent.transform.localScale = cardListParent.transform.localScale;
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                miniCard.SetMiniCard(sortedList[i].id, sortedList[i].ergoTokenId, sortedList[i].ergoTokenAmount, sortedList[i].cardName, sortedList[i].attack, sortedList[i].HP, sortedList[i].gold, sortedList[i].XP, sortedList[i].cardImage);
                miniCard.name = sortedList[i].cardName;
                miniCardParent.name = sortedList[i].cardName;
            }
        }
    }

    public void TurnButton()
    {
        string minText = downMinText.text;
        string secText = downSecText.text;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(EndTheTurn(0));
        pv.RPC("CoroutineMethod", RpcTarget.Others, 0f, minText, secText);
    }

    public void SetPlayerName()
    {
        player1NameText.SetText(PhotonNetwork.PlayerList[0].NickName);
        player2NameText.SetText(PhotonNetwork.PlayerList[1].NickName);
    }

    private void GetRandomWinner()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            randomPlayer = GenerateRandomNumber();
            customProp["randomPlayer"] = randomPlayer;
            PhotonNetwork.MasterClient.SetCustomProperties(customProp);
        }
    }

    private void DisablePanel()
    {
        biddingPanel.SetActive(false);
    }

    public void LiveSearch()
    {
        searchText = search.GetComponent<TMP_InputField>().text.ToLower();
        DestroyCardList();
        selectedCardList.Clear();

        if (searchText != "")
        {
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].cardName.ToLower().Contains(searchText))
                {
                    selectedCardList.Add(sortedList[i]);
                }
            }
        }
        else if (searchText == "")
        {
            for (int i = 0; i < sortedList.Count; i++)
            {
                selectedCardList.Add(sortedList[i]);
            }
        }

        for (int i = 0; i < selectedCardList.Count; i++)
        {
            GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", cardListParent.transform.position, cardListParent.transform.rotation);
            miniCardParent.transform.SetParent(cardListParent.transform);
            miniCardParent.transform.localScale = cardListParent.transform.localScale;
            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
            miniCard.SetMiniCard(selectedCardList[i].id, selectedCardList[i].ergoTokenId, selectedCardList[i].ergoTokenAmount, selectedCardList[i].cardName, selectedCardList[i].attack, selectedCardList[i].HP, selectedCardList[i].gold, selectedCardList[i].XP, selectedCardList[i].cardImage);
            miniCard.name = selectedCardList[i].cardName;
            miniCardParent.name = selectedCardList[i].cardName;
        }
    }

    private void DestroyCardList()
    {
        foreach (Transform child in cardListParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void ButtonClick(Button button)
    {
        if (Hover.clickCounter == 1)
        {
            Hover.clickCounter = 0;
            if (player1Turn && PhotonNetwork.IsMasterClient)
            {
                int currentPos = int.Parse(button.name.Split(" ")[2]);
                Hover.cardParent.transform.GetComponent<PhotonView>();
                int previousPos = int.Parse(Hover.cardParent.transform.parent.name.Split(" ")[2]);
                Hover.cardParent.transform.SetParent(EventSystem.current.currentSelectedGameObject.gameObject.transform);
                Hover.cardParent.transform.position = EventSystem.current.currentSelectedGameObject.gameObject.transform.position;
                Destroy(Hover.cardParent.GetComponent<DragMiniCards>());
                Hover.cardParent.AddComponent<DragFieldCard>();
                pv.RPC("MoveCard", RpcTarget.Others, previousPos, currentPos);
            }
            else if (!player1Turn && !PhotonNetwork.IsMasterClient)
            {
                int currentPos = int.Parse(button.name.Split(" ")[2]);
                Hover.cardParent.transform.GetComponent<PhotonView>();
                int previousPos = int.Parse(Hover.cardParent.transform.parent.name.Split(" ")[2]);
                Hover.cardParent.transform.SetParent(EventSystem.current.currentSelectedGameObject.gameObject.transform);
                Hover.cardParent.transform.position = EventSystem.current.currentSelectedGameObject.gameObject.transform.position;
                Destroy(Hover.cardParent.GetComponent<DragMiniCards>());
                Hover.cardParent.AddComponent<DragFieldCard>();
                pv.RPC("MoveCard", RpcTarget.Others, previousPos, currentPos);
            }
        }
        ResetAnimation("field");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hover.clickCounter = 0;
        GameObject gameObj = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Hand").gameObject;

        for (int i = 0; i < gameObj.transform.childCount; i++)
        {
            if (gameObj.transform.GetChild(i).childCount == 1)
            {
                gameObj.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
            }
        }
    }

    public void GetUserData()
    {
        if (string.IsNullOrEmpty(field.text))
        {
            moreGoldPanel.SetActive(true);
            error.SetText("Please enter bid amount");
            Invoke("DisableMoreGoldPanel", 2f);
            return;
        }
        int bidText = int.Parse(field.text);

        if (bidText <= initaialGold)
        {
            customProp["enterdNum"] = bidText;
            bidButton.interactable = false;
            Gold.instance.SetGold((initaialGold - bidText));
        }
        else
        {
            customProp["enterdNum"] = 0;
            moreGoldPanel.SetActive(true);
            error.SetText("You can not enter more gold than you have.");
            Invoke("DisableMoreGoldPanel", 2f);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.PlayerList[0].CustomProperties["enterdNum"] = bidText;
            PhotonNetwork.PlayerList[0].SetCustomProperties(customProp);
        }
        else
        {
            PhotonNetwork.PlayerList[1].CustomProperties["enterdNum"] = bidText;
            PhotonNetwork.PlayerList[1].SetCustomProperties(customProp);
        }
    }

    public void GetWinnerName()
    {
        StartCoroutine(Winner());
    }

    IEnumerator Winner()
    {
        player1Bid = (int)PhotonNetwork.PlayerList[0].CustomProperties["enterdNum"];
        player2Bid = (int)PhotonNetwork.PlayerList[1].CustomProperties["enterdNum"];

        yield return new WaitForSeconds(0.2f);
        if (player1Bid > player2Bid)
        {
            winnerNameText.SetText(PhotonNetwork.PlayerList[0].NickName + " moves first.");
            player1Turn = true;
            if (PhotonNetwork.IsMasterClient && player1Turn)
            {
                var myId = PhotonNetwork.LocalPlayer.ActorNumber;
                StartCoroutine(WaitForPanelDisableAllPanel(myId, 5));
            }
            if (!PhotonNetwork.IsMasterClient)
            {
                Gold.instance.SetGold(initaialGold);
            }
        }
        else if (player1Bid < player2Bid)
        {
            winnerNameText.SetText(PhotonNetwork.PlayerList[1].NickName + " moves first.");
            player1Turn = false;
            if (!PhotonNetwork.IsMasterClient && !player1Turn)
            {
                var myId = PhotonNetwork.LocalPlayer.ActorNumber;
                PlayerPrefs.SetInt("Down", 180);
                StartCoroutine(WaitForPanelDisableAllPanel(myId, 5));
            }
            if (PhotonNetwork.IsMasterClient)
            {
                Gold.instance.SetGold(initaialGold);
            }
        }
        else if (player2Bid == player1Bid)
        {
            randomPlayer = (int)PhotonNetwork.MasterClient.CustomProperties["randomPlayer"];
            Gold.instance.SetGold(initaialGold);

            if (randomPlayer == 1)
            {
                winnerNameText.SetText(PhotonNetwork.PlayerList[0].NickName + " moves first.");
                player1Turn = true;

                if (PhotonNetwork.IsMasterClient && player1Turn)
                {
                    var myId = PhotonNetwork.LocalPlayer.ActorNumber;
                    PlayerPrefs.SetInt("Down", 180);
                    StartCoroutine(WaitForPanelDisableAllPanel(myId, 5));
                }

            }
            else if (randomPlayer == 2)
            {
                player1Turn = false;
                winnerNameText.SetText(PhotonNetwork.PlayerList[1].NickName + " moves first.");

                if (!PhotonNetwork.IsMasterClient && !player1Turn)
                {
                    var myId = PhotonNetwork.LocalPlayer.ActorNumber;
                    PlayerPrefs.SetInt("Down", 180);
                    StartCoroutine(WaitForPanelDisableAllPanel(myId, 5));
                }
            }
        }
        Invoke("DisablePanel", 5f);
    }

    private void SetActivePlayer(int id)
    {
        var hash = new ExitGames.Client.Photon.Hashtable();
        hash[ACTIVE_PLAYER_KEY] = id;
        room.SetCustomProperties(hash);
    }

    private void DisableMoreGoldPanel()
    {
        moreGoldPanel.SetActive(false);
    }

    public int GenerateRandomNumber()
    {
        System.Random random = new System.Random();
        int player = random.Next(1, 3);
        return player;
    }

    IEnumerator WaitForPanelDisableAllPanel(int myId, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetActivePlayer(myId);
    }

    IEnumerator EndTheTurn(float time)
    {
        ResetAnimation("player");
        ResetAnimation("field");
        ResetAllCard();
        yield return new WaitForSeconds(time);
        if (PhotonNetwork.PlayerList.Count() == 2)
        {
            photonPlayer = PhotonNetwork.LocalPlayer;
            var nextPlayer = photonPlayer.GetNext();

            var nextPlayerID = nextPlayer.ActorNumber;
            string minText = downMinText.text;
            string secText = downSecText.text;

            if (!endGame)
            {
                if (player1Turn && PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    pv.RPC("ChangePlayerTurn", RpcTarget.All, false);
                }
                else if (!player1Turn && !PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    pv.RPC("ChangePlayerTurn", RpcTarget.All, true);
                }
            }
        }
    }

    public void ResetAnimation(string whichPosition)
    {
        if (whichPosition.Equals("player"))
        {
            GameObject gameObj = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Hand").gameObject;
            for (int i = 0; i < gameObj.transform.childCount; i++)
            {
                if (gameObj.transform.GetChild(i).childCount == 1)
                {
                    gameObj.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
                }
            }
        }
        else if (whichPosition.Equals("field"))
        {
            GameObject gameObj = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;

            for (int i = 0; i < gameObj.transform.childCount; i++)
            {
                if (gameObj.transform.GetChild(i).childCount == 1)
                {
                    gameObj.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
                }
            }
        }
    }

    public void GotoSkirmishMenu()
    {
        pv.RPC("DisconnectAllPlayer", RpcTarget.All);
    }

    public void ResetAllStaticValues()
    {
        Timer.isBiddingTime = false;
        Timer.isCompletedBid = false;
        Timer.isAfterCompletedBid = false;
    }

    public void ResetAllCard()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;

        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                Card currentCard = playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>();
                currentCard.SetAttackValue(false);
                currentCard.SetDropPosition(1);
            }
        }
        GameManager.instance.clicked = 0;
    }

    #region RPC Methods
    [PunRPC]
    public void CoroutineMethod(float time, string min, string sec)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            upMinText.SetText(min);
            upSecText.SetText(sec);
        }
        coroutine = StartCoroutine(EndTheTurn(time));
    }

    [PunRPC]
    public void BiddingPanel()
    {
        Timer.isBiddingTime = false;
        biddingPanel.SetActive(true);
        countDownPanel.SetActive(false);
        afterBiddingPanel.SetActive(true);
        bidTimer.InitTimers("BT", 25);
        SetPlayerName();
    }

    [PunRPC]
    public void BidComplete()
    {
        Timer.isCompletedBid = false;
        afterBidTimer.InitTimers("CB", 5);
        afterBiddingPanel.SetActive(false);
        GetWinnerName();
    }

    [PunRPC]
    public void AfterBidComplete()
    {
        biddingPanel.SetActive(false);
    }

    [PunRPC]
    public void CompleteGame()
    {
        StopAllCoroutines();
        resultPanel.SetActive(true);
        timeUp.PauseTimer("up");
        timeDown.PauseTimer("down");
        upMinText.SetText("0");
        upSecText.SetText("00");
        endGame = true;
    }

    [PunRPC]
    public void MoveCard(int prevPos, int currPos)
    {
        GameObject enemyHand = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        GameObject selectedObject = enemyHand.transform.GetChild(prevPos - 1).GetChild(0).gameObject;
        GameObject selectedObjectParent = enemyField.transform.GetChild(currPos - 1).gameObject;
        selectedObject.transform.SetParent(selectedObjectParent.transform);
        selectedObject.transform.position = selectedObjectParent.transform.position;
        selectedObject.AddComponent<DropFieldCard>();
        if (selectedObjectParent.tag == "Front Line Enemy")
        {
            selectedObject.SetActive(true);
        }
        else if (selectedObjectParent.tag == "Back Line Enemy")
        {
            selectedObject.SetActive(false);
        }
    }

    [PunRPC]
    public void SetDownTimeText(string min, string sec)
    {
        downMinText.SetText(min);
        downSecText.SetText(sec);
    }

    [PunRPC]
    public void PauseTimerForTurnEndPlayer(int val)
    {
        timeDown.PauseTimer("down");
        timeUp.InitTimers("up", val);
    }

    [PunRPC]
    public void ChangePlayerTurn(bool value)
    {
        player1Turn = value;
    }
    #endregion

    [PunRPC]
    public void DisconnectAllPlayer()
    {
        connectUsing = true;
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(3);

        StopAllCoroutines();
        ResetAllStaticValues();
    }
    #region Custom Properties
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (changedProps.ContainsKey("enterdNum"))
            {
                player1Bid = (int)changedProps["enterdNum"];
            }
            else
            {
                player1Bid = 0;
            }
        }
        else
        {
            if (changedProps.ContainsKey("enterdNum"))
            {
                player2Bid = (int)changedProps["enterdNum"];
            }
            else
            {
                player2Bid = 0;
            }

        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("master") && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("client"))
        {
            countDownPanel.SetActive(true);

            if (gameBoardParent == null)
                gameBoardParent = GameObject.Find("Game Board Parent");

            if (gameBoardParent.transform.GetChild(0))
            {
                loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
                loadingPanel.SetActive(false);
            }

            GameBoardManager board = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();

            board.gameObject.SetActive(true);
            if (countdownTimer == null)
                countdownTimer = countDownPanel.GetComponent<Timer>();

            if (PhotonNetwork.IsMasterClient)
                countdownTimer.InitTimers("GC", 5);

            int playerDeckProfileId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
            Player currPlayer = PhotonNetwork.LocalPlayer;
            Player nextPlayer = currPlayer.GetNext();
            int opponentDeckProfileId = (int)nextPlayer.CustomProperties["deckId"];
            downProfileIamge.GetComponent<Image>().sprite = profileImages[playerDeckProfileId - 1];
            upProfileImage.GetComponent<Image>().sprite = profileImages[opponentDeckProfileId - 1];

            Invoke("RemoveProperties", 0.5f);
        }

        if (!propertiesThatChanged.TryGetValue(ACTIVE_PLAYER_KEY, out var newActiveID)) return;

        if (!(newActiveID is int newActvieIDValue))
        {
            Debug.LogError("For some reason \"ACTIVE_PLAYER_KEY\" is not an int!?");
            return;
        }

        ApplyActivePlayer(newActvieIDValue);
    }

    private void RemoveProperties()
    {
        PhotonNetwork.CurrentRoom.CustomProperties.Remove("master");
        PhotonNetwork.CurrentRoom.CustomProperties.Remove("client");
    }

    private void ApplyActivePlayer(int id)
    {
        photonPlayer = PhotonNetwork.LocalPlayer;

        var activePlayer = photonPlayer.Get(id);
        var iAmActive = PhotonNetwork.LocalPlayer.ActorNumber == id;
        if (!endGame)
        {
            turnButton.gameObject.SetActive(iAmActive);

            if (iAmActive)
            {
                string minText = downMinText.text;
                string secText = downSecText.text;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(EndTheTurn(21));
                int totalSec = (int.Parse(minText) * 60) + int.Parse(secText);

                timeUp.PauseTimer("up");
                string min = Mathf.FloorToInt(timeUp.currentTime / 60).ToString("0");
                string sec = Mathf.FloorToInt(timeUp.currentTime % 60).ToString("00");
                upMinText.SetText(min);
                upSecText.SetText(sec);

                pv.RPC("SetDownTimeText", RpcTarget.Others, min, sec);
                timeDown.InitTimers("Down", totalSec);
                pv.RPC("CoroutineMethod", RpcTarget.Others, 21f, minText, secText);
                pv.RPC("PauseTimerForTurnEndPlayer", RpcTarget.Others, totalSec);
            }
        }
    }

    #endregion

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //base.OnPlayerLeftRoom(otherPlayer);
        leftPlayerPanel.SetActive(true);
        leftPlayerText.SetText(otherPlayer.NickName + " Was left the game. Press Continue to Go Skirmish screen.");
        StopTimers();
        StopAllCoroutines();
    }

    public void GotoSkirmish()
    {
        connectUsing = true;
        PhotonNetwork.LoadLevel(3);
        StopAllCoroutines();
        ResetAllStaticValues();
        //StopTimers();
    }

    private void StopTimers()
    {
        if (countdownTimer.gameObject.activeSelf)
        {
            countdownTimer.PauseTimer();
            countdownTimer.enabled = false;
        }
        if (afterBidTimer.gameObject.activeSelf)
        {
            afterBidTimer.PauseTimer();
            afterBidTimer.enabled = false;
        }
        if (bidTimer.gameObject.activeSelf)
        {
            bidTimer.PauseTimer();
            bidTimer.enabled = false;
        }
        if (timeDown.currentTime != 180 || timeUp.currentTime != 180)
        {
            timeDown.PauseTimer("down");
            timeUp.PauseTimer("up");
        }


    }


    [PunRPC]
    private void UpdateTime(float time)
    {
        if (countDownPanel.activeSelf)
        {
            RefreshTimer(time, 0);
        }
        if (biddingPanel.activeSelf)
        {
            RefreshTimer(time, 1);
        }
        if (afterBiddingPanel.activeSelf)
        {
            RefreshTimer(time, 2);
        }
    }


    public void RefreshTimer(float time, int type)
    {
        Timer timerPanel = countdownTimer;
        string min = Mathf.FloorToInt(time / 60).ToString("0");
        string sec = Mathf.FloorToInt(time % 60).ToString("00");
        if (type == 0)
        {
            timerPanel = countdownTimer;
        }
        else if (type == 1)
        {
            timerPanel = bidTimer;
        }
        else if (type == 2)
        {
            timerPanel = afterBidTimer;
        }

        timerPanel.minute.SetText(min);
        timerPanel.seconds.SetText(sec);
    }
}
