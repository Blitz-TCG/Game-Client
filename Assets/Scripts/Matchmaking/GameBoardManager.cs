using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Timeline;

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


    [SerializeField] private Button leaveButton;
    [SerializeField] private Sprite[] profileImages;
    [SerializeField] private GameObject downProfileIamge;
    [SerializeField] private GameObject upProfileImage;
    [SerializeField] private GameObject cardError;
    [SerializeField] private GameObject leftPlayerPanel;
    [SerializeField] private TMP_Text leftPlayerText;
    [SerializeField] private TMP_Text countdownMinText;
    [SerializeField] private TMP_Text countDownSecText;
    [SerializeField] private Sprite[] playerFields;
    [SerializeField] private Sprite[] playerBrokenFields;
    [SerializeField] private Image bottomImage;
    [SerializeField] private Image topImage;
    [SerializeField] private GameObject enemyWall;
    [SerializeField] private GameObject playerWall;
    //[SerializeField] private Image playerXPProgressBar;	
    //[SerializeField] private Image enemyXPProgressBar;
    [SerializeField] private GameObject playerDeck;
    [SerializeField] private GameObject enemyDeck;
    [SerializeField] private TMP_Text winnerPlayerName;
    [SerializeField] private TMP_Text matchLengthText;
    [SerializeField] private TMP_Text totalTurnText;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text tokenText;


    private SkirmishManager skirmishManager;
    private List<CardDetails> cardDetails;
    private List<CardDetails> currentCards = new List<CardDetails>();
    private List<CardDetails> sortedList = new List<CardDetails>();
    private List<CardDetails> selectedCardList = new List<CardDetails>();
    private GameObject loadingPanel;
    private GameObject gameBoardParent;
    private Timer timer;
    private Timers countdownTimer;
    private Timers bidTimer;
    private Timers afterBidTimer;
    private int player1Bid;
    private int player2Bid;
    private int randomPlayer;
    private int initialGold;
    private string searchText;
    public static bool player1Turn;
    public static bool player2Turn;
    public static bool connectUsing;
    public static bool isWallDestroyed = false;
    private ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    private bool isBidPanelInitialize = true;
    private bool isBidEnds = true;
    private bool isAfterComplete = true;
    private const string ACTIVE_PLAYER_KEY = "ActivePlayer";
    private Room room;
    private Photon.Realtime.Player photonPlayer;
    private Coroutine coroutine;
    private static bool endGame = false;
    private Card attackingcard;
    Player currPlayer;
    Player nextPlayer;
    private GameObject playerXPProgressBar;
    private GameObject enemyXPProgressBar;
    private DateTime initialStartTime;
    private DateTime endTime;
    private int turnCountMaster = 0;
    private int turnCountClient = 0;
    private GameObject resultPanel;
    private TimeLeft winTimer;
    private Slider xpSlider;
    private bool isXPSet = false;
    //private int gainedGoldPlayer = 0;	
    //private int gainedXPPlayer = 0;	
    //private int gainedGoldEnemy = 0;	
    //private int gainedXPEnemy = 0;

    private int currentPlayerXP;
    private int opponentXP;
    private int currentPlayerGold;
    private int opponentGold;
    private PlayerController playerController;
    private EnemyController enemyController;
    private int masterPlayerXP = 0;
    private int clientPlayerXP = 0;
    private bool isComp = false;
    private MatchStatus status;
    private MatchData matchData;
    private GameMode mode;
    string masterID = "", clientId = "";
    int masterMmr = 0, clientMmr = 0;
    DeckGeneral masterDeck = DeckGeneral.Unknown, clientDeck = DeckGeneral.Unknown;
    string matchId = "ABCD";
    private string leavePlayer;
    private bool leaveBtn = false;
    public static bool isCompleted = false;
    private bool isMatchNotLoaded = false;

    #endregion

    private void Start()
    {
        Debug.LogWarning("start called ");
        //PhotonManager.RemoveSceneFromBuildIndex();
        isWallDestroyed = false;
        endGame = false;
        skirmishManager = SkirmishManager.instance;
        gameBoardParent = GameObject.Find("Game Board Parent");
        countDownPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Global Countdown").GetChild(0).gameObject;
        biddingPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Bid Panel").GetChild(0).gameObject;
        afterBiddingPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("After Bid Panel").GetChild(0).gameObject;
        countdownTimer = countDownPanel.GetComponent<Timers>();
        bidTimer = biddingPanel.transform.GetChild(0).GetChild(3).GetComponent<Timers>();
        afterBidTimer = afterBiddingPanel.GetComponent<Timers>();
        Debug.LogError("countdownTimer " + countdownTimer + " bidTimer " + bidTimer + " afterBidTimer " + afterBidTimer);
        Debug.LogError("countDownPanel " + countDownPanel + " biddingPanel " + biddingPanel + " afterBiddingPanel " + afterBiddingPanel);
        winTimer = gameBoardParent.transform.GetChild(1).GetChild(1).GetComponent<TimeLeft>();

        timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
        timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
        Debug.LogError("timer down " + timeDown + " time up " + timeUp);

        customProp["totalTurnCount"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"] + " photon player name " + PhotonNetwork.LocalPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            customProp["master"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
            PlayerPrefs.SetInt("masterCount", 0);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            customProp["client"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
            PlayerPrefs.SetInt("clientCount", 0);
        }

        turnButton.onClick.AddListener(TurnButton);
        leaveButton.onClick.AddListener(() =>
        {
            LeavePlayer();
        });
        turnButton.gameObject.SetActive(false);
        room = PhotonNetwork.CurrentRoom;

        PhotonNetwork.AutomaticallySyncScene = true;
        cardDetails = CardDataBase.instance.cardDetails;

        if (pv.IsMine)
            InitCards();

        CardDetails clickedCard = cardDetails.Find(item => item.id == 1);

        properties["masterGainedGold"] = 0;
        properties["clientGainedGold"] = 0;
        properties["masterGainedXP"] = 0;
        properties["clientGainedXP"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

        GetRandomWinner();
        countDownPanel.SetActive(true);

        if (pv.IsMine)
        {
            playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
            enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
        }
        mode = GameMode.OpenToPlay;
        leaveBtn = false;
        isMatchNotLoaded  = false;
        ResetAllStaticValues();
        Debug.Log("start called " + PhotonNetwork.IsMasterClient + " photon player " + PhotonNetwork.LocalPlayer.NickName);
        Invoke("LeaveBothPlayerAccidently", 45f);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Count() == 2)
        {
            if (Timers.isGlobalCountDown)
            {
                pv.RPC("Global", RpcTarget.Others, countdownTimer.seconds.text);
            }
            if (Timers.isBiddingTime)
            {
                pv.RPC("Bidding", RpcTarget.Others, bidTimer.seconds.text);
                //Timers.isBiddingTime = false;
                if (isBidPanelInitialize)
                {
                    isBidPanelInitialize = false;
                    pv.RPC("BiddingPanel", RpcTarget.All);
                }
            }
            else if (Timers.isCompletedBid)
            {
                Timers.isCompletedBid = false;
                if (isBidEnds)
                {
                    isBidEnds = false;
                    pv.RPC("BidComplete", RpcTarget.All);
                }
            }
            else if (Timers.isAfterCompletedBid)
            {
                if (isAfterComplete)
                {
                    isAfterComplete = false;
                    Timers.isCompletedBid = false;
                    pv.RPC("AfterBidComplete", RpcTarget.All);
                }
            }
        }

        if (timeDown.completeTime)
        {
            timeDown.completeTime = false;
            downMinText.SetText("0");
            downSecText.SetText("00");
            timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
            timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
            timeDown.PauseTimer("down");
            timeUp.PauseTimer("up");
            //Debug.LogError(" time up parent " + timeUp.transform.parent.parent.parent.name + " down " + timeDown.transform.parent.parent.parent);
            StopAllCoroutines();
            //resultPanel.SetActive(true);
            //winTimer.InitTimers(30);
            endGame = true;
            status = MatchStatus.Normal;
            PhotonNetwork.AutomaticallySyncScene = false;
            CalculateWinner();
            pv.RPC("CompleteGame", RpcTarget.Others);
        }

        if (winTimer.timeUp && endGame)
        {
            //Debug.LogError("the timer up and end game called before " + endGame + " " + PhotonNetwork.LocalPlayer.NickName);
            endGame = false;
            //Debug.LogError("the timer up and end game called after " + endGame + " " + PhotonNetwork.LocalPlayer.NickName);
            LeaveBothPlayer();
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
                            if (attackingcard.transform.parent.parent.CompareTag("Front Line Player"))
                            {
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
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
                            if (attackingcard.transform.parent.parent.CompareTag("Front Line Player"))
                            {
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
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
                else if (EventSystem.current.currentSelectedGameObject.tag == "EnemyWall")
                {
                    if (player1Turn && PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        if (GameManager.instance.clicked == 1)
                        {
                            GameObject enemyHealthObject = enemyWall.transform.Find("Remaining Health").gameObject;
                            int currentHealth = int.Parse(enemyHealthObject.GetComponent<TMP_Text>().text.ToString());
                            int remainingHealth = currentHealth - attackingcard.attack;
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                nextPlayer = PhotonNetwork.LocalPlayer.GetNext();
                                string opponentField = (string)nextPlayer.CustomProperties["deckField"];
                                int opponentId = GetFieldIndex(opponentField);
                                topImage.sprite = playerBrokenFields[opponentId];
                                enemyWall.GetComponent<PolygonCollider2D>().enabled = false;
                                enemyWall.GetComponent<Button>().enabled = false;
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(0.ToString());
                                pv.RPC("AttackWall", RpcTarget.Others, 0);
                                isWallDestroyed = true;
                                ShowHiddenCard();
                                GameManager.instance.clicked = 0;
                            }
                            else
                            {
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(remainingHealth.ToString());
                                pv.RPC("AttackWall", RpcTarget.Others, remainingHealth);
                                GameManager.instance.clicked = 0;
                            }
                        }
                    }
                    else if (!player1Turn && !PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        if (GameManager.instance.clicked == 1)
                        {
                            GameObject enemyHealthObject = enemyWall.transform.Find("Remaining Health").gameObject;
                            int currentHealth = int.Parse(enemyHealthObject.GetComponent<TMP_Text>().text.ToString());
                            int remainingHealth = currentHealth - attackingcard.attack;
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                nextPlayer = PhotonNetwork.LocalPlayer.GetNext();
                                string opponentField = (string)nextPlayer.CustomProperties["deckField"];
                                int opponentId = GetFieldIndex(opponentField);
                                topImage.sprite = playerBrokenFields[opponentId];
                                enemyWall.GetComponent<PolygonCollider2D>().enabled = false;
                                enemyWall.GetComponent<Button>().enabled = false;
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(0.ToString());
                                pv.RPC("AttackWall", RpcTarget.Others, 0);
                                isWallDestroyed = true;
                                ShowHiddenCard();
                                GameManager.instance.clicked = 0;
                            }
                            else
                            {
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(remainingHealth.ToString());
                                pv.RPC("AttackWall", RpcTarget.Others, remainingHealth);
                                GameManager.instance.clicked = 0;
                            }
                        }
                    }

                }
                else if (EventSystem.current.currentSelectedGameObject.name == "Enemy Profile")
                {
                    if (GameManager.instance.clicked == 1)
                    {
                        if (isWallDestroyed)
                        {
                            GameObject enemyProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
                            GameObject enemyHealth = enemyProfile.transform.Find("Enemy Deck Health").Find("Remaining Health").gameObject;
                            int currentHealth = int.Parse(enemyHealth.GetComponent<TMP_Text>().text.ToString());
                            int remainingHealth = currentHealth - attackingcard.attack;
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                //win or lose panel open
                                Debug.Log("Win the game");
                                enemyHealth.GetComponent<TMP_Text>().SetText(0.ToString());
                                pv.RPC("GetGameResult", RpcTarget.Others);
                                CalculateWinner();
                                GameManager.instance.clicked = 0;
                            }
                            else
                            {
                                enemyHealth.GetComponent<TMP_Text>().SetText(remainingHealth.ToString());
                                pv.RPC("AttackDeck", RpcTarget.Others, remainingHealth);
                                GameManager.instance.clicked = 0;
                            }

                        }
                        else
                        {
                            cardError.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the direct deck");
                            Invoke("RemoveErrorObject", 2f);
                        }
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

    private void CalculateWinner()
    {
        timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
        timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
        timeDown.PauseTimer("down");
        timeUp.PauseTimer("up");
        pv.RPC("CalculateScore", RpcTarget.All);
    }

    [PunRPC]
    private void Bidding(string value)
    {
        Debug.LogError(value + " value");
        bidTimer.seconds.SetText(value);
    }

    [PunRPC]
    private void Global(string value)
    {
        Debug.LogError(value + " value");
        countdownTimer.seconds.SetText(value);
    }

    [PunRPC]
    private void CalculateScore()
    {
        resultPanel.transform.GetChild(0).gameObject.SetActive(true);
        winTimer.InitTimers(30);

        totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
        matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
        experienceText = resultPanel.transform.GetChild(0).Find("Experience").GetChild(1).GetComponent<TMP_Text>();
        winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();
        GameObject mainMenu = resultPanel.transform.GetChild(0).Find("Main Menu").gameObject;
        GameObject playerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;
        int playerHealth = int.Parse(playerProfile.transform.Find("Player Deck Health").Find("Remaining Health").GetComponent<TMP_Text>().text);
        GameObject enemyProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
        int enemyHealth = int.Parse(enemyProfile.transform.Find("Enemy Deck Health").Find("Remaining Health").GetComponent<TMP_Text>().text);
        xpSlider = resultPanel.transform.GetChild(0).Find("XP Progress Bar").GetComponent<Slider>();
        xpSlider.interactable = false;
        string winnerName = "";
        int turnCounter = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"];
        string winnerId = "", loserId = "";
        int winnerXP = 0, loserXP = 0, winnerMmrChange = 0, loserMmrChange = 0;
        DeckGeneral winnerDeck = DeckGeneral.Unknown, loserDeck = DeckGeneral.Unknown;
        string matchId = "ABCD";

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogError(PlayerPrefs.GetInt("masterCount") + " master value " + PhotonNetwork.LocalPlayer.NickName);
            totalTurnText.SetText(turnCounter.ToString() + " Turns.");
            int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            int gainedMasterXp = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"];
            int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
            Debug.Log(" master xp " + gainedMasterXp + " gained " + totalMasterXP + " total " + totalPlayerGold + " total gold");
            xpSlider.value = (totalMasterXP / 2000f);
            PlayerPrefs.SetInt("totalGold", totalPlayerGold);
            PlayerPrefs.SetInt("totalXP", totalMasterXP);
            Debug.LogError(" player health " + playerHealth + " enemy health " + enemyHealth);

            if (playerHealth > enemyHealth)
            {
                winnerName = PhotonNetwork.MasterClient.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                masterPlayerXP += 100;
                clientPlayerXP += 25;
                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];

                string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                winnerDeck = GetDeckGeneral(playerField);
                loserDeck = GetDeckGeneral(opponentField);

                Debug.LogError(winnerDeck + " winner deck");
                Debug.LogError(loserDeck + " loser deck");

                winnerXP = masterPlayerXP;
                loserXP = clientPlayerXP;

                winnerMmrChange = 0;
                loserMmrChange = 0;
                //winnerDeck = PhotonNetwork.LocalPlayer.GetNext();
                experienceText.SetText(masterPlayerXP.ToString());
                xpSlider.value = (masterPlayerXP / 2000f);
            }
            else if (enemyHealth > playerHealth)
            {
                winnerName = PhotonNetwork.LocalPlayer.GetNext().NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                clientPlayerXP += 100;
                masterPlayerXP += 25;
                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];

                string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                winnerDeck = GetDeckGeneral(opponentField);
                loserDeck = GetDeckGeneral(playerField);

                Debug.LogError(winnerDeck + " winner deck");
                Debug.LogError(loserDeck + " loser deck");

                winnerXP = clientPlayerXP;
                loserXP = masterPlayerXP;

                winnerMmrChange = 0;
                loserMmrChange = 0;
                experienceText.SetText(masterPlayerXP.ToString());
                xpSlider.value = (masterPlayerXP / 2000f);
            }
            else if (playerHealth == enemyHealth)
            {
                winnerPlayerName.SetText(" It's Draw!");
                masterPlayerXP += 50;
                clientPlayerXP += 50;

                winnerMmrChange = 0;
                loserMmrChange = 0;
                experienceText.SetText(masterPlayerXP.ToString());
                xpSlider.value = (masterPlayerXP / 2000f);
            }
            //mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer("master"));
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer());
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            int gainedClientXp = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"];
            int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
            xpSlider.value = (totalClientXP / 2000f);
            Debug.LogError(PlayerPrefs.GetInt("clientCount") + " client value " + PhotonNetwork.LocalPlayer.NickName);
            totalTurnText.SetText(turnCounter.ToString() + " Turns.");
            Debug.Log(" client xp " + gainedClientXp + " gained " + totalClientXP + " total " + totalClientGold + " total client gold");
            PlayerPrefs.SetInt("totalGold", totalClientGold);
            PlayerPrefs.SetInt("totalXP", totalClientXP);
            Debug.LogError(" player health " + playerHealth + " enemy health " + enemyHealth);

            if (playerHealth > enemyHealth)
            {
                winnerName = PhotonNetwork.LocalPlayer.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                clientPlayerXP += 100;
                masterPlayerXP += 25;
                //winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                //loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                experienceText.SetText(clientPlayerXP.ToString());
                xpSlider.value = (clientPlayerXP / 2000f);
            }
            else if (enemyHealth > playerHealth)
            {
                winnerName = PhotonNetwork.MasterClient.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                masterPlayerXP += 100;
                clientPlayerXP += 25;
                //winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                //loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                experienceText.SetText(clientPlayerXP.ToString());
                xpSlider.value = (clientPlayerXP / 2000f);
            }
            else if (playerHealth == enemyHealth)
            {
                winnerPlayerName.SetText(" It's Draw!");
                masterPlayerXP += 50;
                clientPlayerXP += 50;
                experienceText.SetText(clientPlayerXP.ToString());
                xpSlider.value = (clientPlayerXP / 2000f);
            }
            //mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer("client"));
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer());
        }

        int turnCounters = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"];
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"] + " photon player name " + PhotonNetwork.LocalPlayer.NickName + " TURN counter " + turnCounters);

        playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
        enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

        //<----- Here set the xp for player to the database (playerController.totalXP) ------->

        //if (PhotonNetwork.IsMasterClient)	
        //{	
        //    //Debug.LogError("master turn " + turnCountMaster);	
        //}	
        //else	
        //{	
        //    //Debug.LogError(" client turn " + turnCountClient);	
        //}


        endTime = DateTime.Now;
        int totalSeconds = (int)(endTime - initialStartTime).TotalSeconds;
        status = MatchStatus.Normal;
        endGame = true;
        timeDown.PauseTimer("down");
        timeUp.PauseTimer("up");
        StopAllCoroutines();
        //int minutes = (int)totalSeconds / 60;
        //int seconds = (int)totalSeconds % 60;
        //Debug.LogError(" minutes " + minutes + " seconds " + seconds);	
        //matchLengthText.SetText($"{minutes} : {seconds}");	
        //Debug.LogError(" winner name " + winnerName);
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("SetUserSpendTime", RpcTarget.All, totalSeconds);
            matchData = new MatchData(matchId, mode, winnerId, loserId, winnerDeck, loserDeck, winnerXP, loserXP, winnerMmrChange, loserMmrChange, totalSeconds, turnCounter, status);
            Debug.Log("leave player name " + PhotonNetwork.IsMasterClient + " winnerId " + winnerId + " loser id " + loserId + " winnerxp " + winnerXP + " loserxp " + loserXP + " winner mmr " + winnerMmrChange + " loser mmr " + loserMmrChange + " winner deck " + winnerDeck + " loser deck " + loserDeck + " status " + status);
            // Here you need to store the data
        }

        PhotonNetwork.AutomaticallySyncScene = false;

        Debug.LogError("End game after " + endGame + " player name " + PhotonNetwork.LocalPlayer);

        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"] + " mgg ");
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"] + " cgg ");
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"] + " mgx ");
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"] + " cgx ");
    }

    [PunRPC]
    private void SetUserSpendTime(int sec)
    {
        matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
        matchLengthText.SetText($"{sec} seconds");
    }

    [PunRPC]
    private void AttackDeck(int health)
    {
        GameObject palyerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;
        GameObject enemyHealth = palyerProfile.transform.Find("Player Deck Health").Find("Remaining Health").gameObject;
        enemyHealth.GetComponent<TMP_Text>().SetText(health.ToString());
    }

    [PunRPC]
    private void GetGameResult()
    {
        GameObject palyerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;
        GameObject enemyHealth = palyerProfile.transform.Find("Player Deck Health").Find("Remaining Health").gameObject;
        enemyHealth.GetComponent<TMP_Text>().SetText(0.ToString());
    }

    [PunRPC]
    private void AttackWall(int attackValue)
    {
        GameObject playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
        GameObject playersFieldParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Game Play Field Border").gameObject;
        GameObject playerHealthObject = playerWall.transform.Find("Remaining Health").gameObject;

        playerHealthObject.GetComponent<TMP_Text>().SetText(attackValue.ToString());

        if (attackValue == 0)
        {
            playerWall.GetComponent<PolygonCollider2D>().enabled = false;
            playerWall.GetComponent<Button>().enabled = false;
            string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
            int playerId = GetFieldIndex(playerField);
            GameObject bottomField = playersFieldParent.transform.Find("Bottom Field").gameObject;
            bottomField.GetComponent<Image>().sprite = playerBrokenFields[playerId];
            ChangeTag();
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
            GameObject playerCard = attacking.gameObject;
            GameObject enemyCard = target.gameObject;
            bool destroyPlayer = attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            bool destroyEnemy = target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);
            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);

            if (destroyPlayer)
            {
                enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                //enemyController.totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "master");
                //InitCards();
                //int goldPlayer = attacking.gold;
                //int goldOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                //int goldGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"]);
                //goldGainedOther += goldPlayer;
                ////gainedGoldEnemy += goldPlayer;	
                //int totalGoldForOtherDeck = goldPlayer + goldOtherDeck;
                ////PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalGoldForOtherDeck;	
                ////PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"] = goldGainedOther;	
                //properties["clientGold"] = totalGoldForOtherDeck;
                //properties["clientGainedGold"] = goldGainedOther;
                ////Debug.LogError(" gained gold enemy " + gainedGoldEnemy + " destroyplayer master " + attacking.gold);	
                //int xpPlayer = attacking.XP;
                //int xpOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);
                //int xpGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"]);
                //xpGainedOther += xpPlayer;
                //int totalXPForOtherDeck = xpPlayer + xpOtherDeck;
                ////PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalXPForOtherDeck;	
                //properties["clientXP"] = totalXPForOtherDeck;
                //properties["clientGainedXP"] = xpGainedOther;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                //Debug.LogError(" gained xp other " + xpGainedOther + " destroyplayer master " + attacking.XP);
                //Debug.LogError(" enemy progress bar parent " + enemyXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);
                //enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherDeck);
                //pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherDeck, "player");
                //pv.RPC("SetGoldValue", RpcTarget.Others, "master");

                ////enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                ////enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "master");
            }

            if (destroyEnemy)
            {
                Debug.LogError(" Destroy enemy called mastyer");
                playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                //playerController.totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                playerController.DestributeGoldAndXPForPlayer(playerCard.transform.parent.GetComponent<PhotonView>(), enemyCard.GetComponent<Card>().gold, enemyCard.GetComponent<Card>().XP, "master");
                //InitCards();
                //int goldEnemy = target.gold;
                //int goldPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                //int goldGainedPlayer = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"]);
                //goldGainedPlayer += goldEnemy;
                //int totalGoldForOtherPlayer = goldEnemy + goldPlayerDeck;
                //Gold.instance.SetGold(totalGoldForOtherPlayer);
                //properties["masterGold"] = totalGoldForOtherPlayer;
                //properties["masterGainedGold"] = goldGainedPlayer;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                ////Debug.LogError(" gained gold player " + gainedGoldPlayer + " destroy enemy master " + target.gold);	
                //int xpEnemy = target.XP;
                //int xpPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
                //int xpGainedPlayer = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"]);
                //xpGainedPlayer += xpEnemy;
                //int totalXPForOtherPlayer = xpEnemy + xpPlayerDeck;
                ////Gold.instance.SetGold(totalGoldForOtherPlayer);	
                //properties["masterXP"] = totalXPForOtherPlayer;
                //properties["masterGainedXP"] = xpGainedPlayer;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                ////Debug.LogError(" gained xp player " + gainedXPPlayer + " destroy enemy master " + target.XP);	
                //Debug.LogError(" player progress bar parent " + playerXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);
                //playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherPlayer);
                //pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherPlayer, "enemy");

                ////playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                ////playerController.DestributeGoldAndXPForPlayer(playerCard.transform.parent.GetComponent<PhotonView>(), enemyCard.GetComponent<Card>().gold, enemyCard.GetComponent<Card>().XP, "master");
            }

            attackingcard.SetAttackValue(true);

            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId);
        }
        else if (!player1Turn && !PhotonNetwork.IsMasterClient)
        {
            GameObject playerCard = attacking.gameObject;
            GameObject enemyCard = target.gameObject;
            bool destroyPlayer = attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            bool destroyEnemy = target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);

            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);

            if (destroyPlayer)
            {
                Debug.LogError(" Destroy player called client ");
                enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                //enemyController.totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "client");
                //InitCards();
                //int goldPlayer = attacking.gold;
                //int goldOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                //int goldGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"]);
                //goldGainedOther += goldPlayer;
                //int totalGoldForOtherDeck = goldPlayer + goldOtherDeck;
                //properties["masterGold"] = totalGoldForOtherDeck;
                //properties["masterGainedGold"] = goldGainedOther;
                ////Debug.LogError(" gained gold enemy " + gainedGoldEnemy + " destroy player in client " + attacking.gold);	
                //int xpPlayer = attacking.XP;
                //int xpOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
                //int xpGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"]);
                //xpGainedOther += xpPlayer;
                //int totalXPForOtherDeck = xpPlayer + xpOtherDeck;
                //properties["masterXP"] = totalXPForOtherDeck;
                //properties["masterGainedXP"] = xpGainedOther;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                ////Debug.LogError(" gained xp enemy " + gainedXPEnemy + " destroy player in client " + attacking.XP);	
                //Debug.LogError(" enemy progress bar parent " + enemyXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);
                //enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherDeck);
                //pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherDeck, "player");
                ////pv.RPC("SetGoldValue", RpcTarget.Others, "client", gainedGoldEnemy, totalGoldForOtherDeck, gainedXPEnemy, totalXPForOtherDeck);

                ////enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                ////enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "client");
            }

            if (destroyEnemy)
            {
                Debug.LogError(" destroy enemy called ");
                playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                //playerController.totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                playerController.DestributeGoldAndXPForPlayer(playerCard.transform.parent.GetComponent<PhotonView>(), enemyCard.GetComponent<Card>().gold, enemyCard.GetComponent<Card>().XP, "client");
                //InitCards();
                //int goldEnemy = target.gold;
                //int goldPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                //int goldGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"]);
                //goldGainedOther += goldEnemy;
                //int totalGoldForOtherPlayer = goldEnemy + goldPlayerDeck;
                ////Debug.LogError(" gained gold player " + gainedGoldPlayer + " destroy enemy in client " + target.gold);	
                //Gold.instance.SetGold(totalGoldForOtherPlayer);
                //properties["clientGold"] = totalGoldForOtherPlayer;
                //properties["clientGainedGold"] = goldGainedOther;
                ////PhotonNetwork.CurrentRoom.SetCustomProperties(properties);	
                //int xpEnemy = target.XP;
                //int xpPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);
                //int xpGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"]);
                //xpGainedOther += xpEnemy;
                //int totalXPForOtherPlayer = xpEnemy + xpPlayerDeck;
                ////Debug.LogError(" gained xp player " + gainedXPPlayer + " destroy enemy in client " + target.XP);	
                ////Gold.instance.SetGold(totalGoldForOtherPlayer);	
                //properties["clientXP"] = totalXPForOtherPlayer;
                //properties["clientGainedXP"] = xpGainedOther;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                //Debug.LogError(" player progress bar parent " + playerXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);
                //playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherPlayer);
                //pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherPlayer, "enemy");

                ////playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                ////playerController.DestributeGoldAndXPForPlayer(playerCard.transform.parent.GetComponent<PhotonView>(), enemyCard.GetComponent<Card>().gold, enemyCard.GetComponent<Card>().XP, "client");
            }

            attackingcard.SetAttackValue(true);

            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId);
        }
    }

    //[PunRPC]
    //private void SetProgessBar(int progressValue, string name)
    //{
    //    if (name == "player")
    //    {
    //        playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(progressValue);
    //        Debug.LogError(" player progress bar parent " + playerXPProgressBar.transform.parent.parent.name + " master or client " + PhotonNetwork.IsMasterClient + " progress value " + progressValue);
    //    }
    //    else if (name == "enemy")
    //    {
    //        enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(progressValue);
    //        Debug.LogError(" enemy progress bar parent " + enemyXPProgressBar.transform.parent.parent.name + " master or client " + PhotonNetwork.IsMasterClient + " progress value " + progressValue);
    //    }
    //}
    //[PunRPC]
    //private void SetGoldValue(string player)
    //{
    //    //Debug.LogError("SetGoldValue called " + player + " player " + gold  +  " gold value " + totalGold + " total gold value " + xp + " xp value " + totalXP + " total xp value " );	
    //    if (player == "master")
    //    {
    //        Debug.LogError(" inside master in SetGoldValue");
    //        int totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
    //        int totalXP = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]); ;
    //        Gold.instance.SetGold(totalGold);
    //        Debug.LogError("master gold " + totalGold + " xp " + totalXP);
    //    }
    //    else if (player == "client")
    //    {
    //        Debug.LogError(" inside client in SetGoldValue");
    //        //gainedGoldEnemy = gold;	
    //        int totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
    //        int totalXP = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
    //        Gold.instance.SetGold(totalGold);
    //        Debug.LogError("master client " + totalGold + " xp " + totalXP);
    //    }
    //}

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

    public void ShowHiddenCard()
    {
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            if (enemyField.transform.GetChild(i).tag == "Back Line Enemy")
            {
                if (enemyField.transform.GetChild(i).childCount == 1)
                {
                    enemyField.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

    public void InitCards()
    {
        Debug.Log("init card called");
        int[] matchCards = ErgoQuery.instance.cardIdCurrentStore[skirmishManager.deckId - 1];
        int cardLength = ErgoQuery.instance.cardIdCurrentStore[skirmishManager.deckId - 1].Length;

        DestroyCardList();
        currentCards.Clear();

        for (int i = 0; i < cardLength; i++)
        {
            if (cardDetails.Where(card => card.id == matchCards[i]).Count() == 1)
            {
                currentCards.Add(cardDetails[matchCards[i] - 1]);
            }
        }
        //sortedList = SortCardsByLevel(currentCards);
        //sortedList = currentCards.OrderBy(card => card.levelRequired).ToList();
        currentCards.Sort(SortByLevelAndOrder);
        sortedList = currentCards;
        foreach (CardDetails card in sortedList)
        {
            Debug.Log($"Card Name: {card.cardName}, Level: {card.levelRequired}");
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Debug.Log("inside for " + PhotonNetwork.LocalPlayer.NickName + " player name " + pv.IsMine + " is mine ");
            PhotonView view = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
            Debug.Log(view + " view " + gameBoardParent.transform.GetChild(1).gameObject + " gameboard " + gameBoardParent.transform.GetChild(1).gameObject.name);
            if (view.IsMine)
            {
                Debug.LogError(" inside the mine called");
                GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", cardListParent.transform.position, cardListParent.transform.rotation);
                miniCardParent.transform.SetParent(cardListParent.transform);
                miniCardParent.transform.localScale = cardListParent.transform.localScale;
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
                Debug.Log(" completed card level " + completeCard.levelRequired);
                int level = int.Parse(completeCard.levelRequired.Split(" ")[1]);
                miniCard.SetMiniCard(sortedList[i].id, sortedList[i].ergoTokenId, sortedList[i].ergoTokenAmount, sortedList[i].cardName, sortedList[i].attack, sortedList[i].HP, sortedList[i].gold, sortedList[i].XP, sortedList[i].cardImage);
                miniCard.name = sortedList[i].cardName;
                miniCardParent.name = sortedList[i].cardName;
                miniCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                Debug.LogError(" photon view " + miniCardParent.GetComponent<PhotonView>());
                DisplayWithXP(miniCard.gameObject, level);
            }
        }
    }

    //List<CardDetails> SortCardsByLevel(List<CardDetails> cards)
    //{
    //    for (int i = 1; i < cards.Count; i++)
    //    {
    //        CardDetails currentCard = cards[i];
    //        int j = i - 1;

    //        int cardLevelReq = int.Parse(cards[j].levelRequired.Split(" ")[1]);
    //        int currCardReq = int.Parse(currentCard.levelRequired.Split(" ")[1]);

    //        while (j >= 0 && cardLevelReq > currCardReq)
    //        {
    //            cards[j + 1] = cards[j];
    //            j--;
    //        }

    //        cards[j + 1] = currentCard;
    //    }
    //    return cards;
    //}

    private int SortByLevelAndOrder(CardDetails a, CardDetails b)
    {
        int levelA = int.Parse(a.levelRequired.Split(" ")[1]);
        int levelB = int.Parse(b.levelRequired.Split(" ")[1]);

        int levelComparison = levelA.CompareTo(levelB);
        if (levelComparison != 0)
        {
            return levelComparison;
        }
        else
        {
            // If levels are the same, maintain the original order
            return currentCards.IndexOf(a).CompareTo(currentCards.IndexOf(b));
        }
    }

    public void DisplayWithXP(GameObject card, int level)
    {
        playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
        Debug.Log(playerController.playerGainedXP + " player controller ");
        Debug.Log(card + " card " + card.GetComponent<Card>());

        bool shouldDisplay = false;
        if (playerController.playerGainedXP < 200 && level <= 1)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP);
            shouldDisplay = true;
        }
        else if ((playerController.playerGainedXP >= 200 && playerController.playerGainedXP < 400) && level <= 4)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP);
            shouldDisplay = true;
        }
        else if ((playerController.playerGainedXP >= 400 && playerController.playerGainedXP < 600) && level <= 8)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP);
            shouldDisplay = true;
        }
        else if (playerController.playerGainedXP >= 600)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP);
            shouldDisplay = true;
        }
        Debug.LogError(shouldDisplay + " shouldDisplay");

        if (shouldDisplay)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP + " card display");
            DisplayCard(card.GetComponent<Card>(), Color.white);
        }


        //if (playerController.totalXP < 200 && level > 1)
        //{
        //    Debug.Log("less 200");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 0.6f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}
        //else
        //{
        //    Debug.Log("200 else");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 1f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}

        //if (playerController.totalXP < 400 && level > 4)
        //{
        //    Debug.Log("400 less ");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 0.6f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}
        //else
        //{
        //    Debug.Log("400 else");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 1f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}

        //if (playerController.totalXP < 600 && level > 8)
        //{
        //    Debug.Log("600 less");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 0.6f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}
        //else
        //{
        //    Debug.Log("600 else");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 1f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}

        //if (playerController.totalXP > 600)
        //{
        //    Debug.Log("greter 600");
        //    Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //    currentColor.a = 1f;
        //    card.transform.GetChild(0).GetComponent<Image>().color = currentColor;
        //}
    }

    private void DisplayCard(Card card, Color color)
    {
        Debug.LogError("Display card called");
        Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //currentColor.a = 1f;
        card.transform.GetChild(0).GetComponent<Image>().color = color;

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
            var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
            Debug.Log(" completed card level " + completeCard.levelRequired);
            int level = int.Parse(completeCard.levelRequired.Split(" ")[1]);
            miniCard.SetMiniCard(selectedCardList[i].id, selectedCardList[i].ergoTokenId, selectedCardList[i].ergoTokenAmount, selectedCardList[i].cardName, selectedCardList[i].attack, selectedCardList[i].HP, selectedCardList[i].gold, selectedCardList[i].XP, selectedCardList[i].cardImage);
            miniCard.name = selectedCardList[i].cardName;
            miniCardParent.name = selectedCardList[i].cardName;
            //DisplayWithXP(miniCard.gameObject, level);
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

        if (bidText <= initialGold)
        {
            customProp["enterdNum"] = bidText;
            bidButton.interactable = false;
            if (PhotonNetwork.IsMasterClient)
            {
                int initialGoldvalue = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                int gold = (initialGoldvalue - bidText);
                //Gold.instance.SetGold(gold);
                properties["masterGold"] = gold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                int initialGoldvalue = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                int gold = (initialGoldvalue - bidText);
                //Gold.instance.SetGold(gold);
                properties["clientGold"] = gold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
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
                int masterGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                Gold.instance.SetGold(masterGold);
                StartCoroutine(WaitForPanelDisableAllPanel(myId, 5));
            }
            if (!PhotonNetwork.IsMasterClient)
            {
                Gold.instance.SetGold(initialGold);
                properties["clientGold"] = initialGold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
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
                int clientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                Gold.instance.SetGold(clientGold);
                StartCoroutine(WaitForPanelDisableAllPanel(myId, 5));
            }
            if (PhotonNetwork.IsMasterClient)
            {
                Gold.instance.SetGold(initialGold);
                properties["masterGold"] = initialGold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
        }
        else if (player2Bid == player1Bid)
        {
            randomPlayer = (int)PhotonNetwork.MasterClient.CustomProperties["randomPlayer"];
            Gold.instance.SetGold(initialGold);

            if (PhotonNetwork.IsMasterClient)
            {
                properties["masterGold"] = initialGold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                properties["clientGold"] = initialGold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }

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
                    turnCountMaster = PlayerPrefs.GetInt("masterCount") + 1;
                    PlayerPrefs.SetInt("masterCount", turnCountMaster);
                    pv.RPC("ChangePlayerTurn", RpcTarget.All, false);
                }
                else if (!player1Turn && !PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    turnCountClient = PlayerPrefs.GetInt("clientCount") + 1;
                    PlayerPrefs.SetInt("clientCount", turnCountClient);
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
        Timers.isBiddingTime = false;
        Timers.isCompletedBid = false;
        Timers.isAfterCompletedBid = false;
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

    private void ChangeTag()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            playerField.transform.GetChild(i).tag = "Front Line Player";
        }
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
        Timers.isBiddingTime = false;
        //RemoveProperties();
        biddingPanel.SetActive(true);
        countDownPanel.SetActive(false);
        afterBiddingPanel.SetActive(true);
        bidTimer.InitTimers("BT", 20);
        SetPlayerName();
    }

    [PunRPC]
    public void BidComplete()
    {
        Timers.isCompletedBid = false;
        countDownPanel.SetActive(false);
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
        endGame = true;
        status = MatchStatus.Normal;
        PhotonNetwork.AutomaticallySyncScene = false;
        StopAllCoroutines();
        timeUp.PauseTimer("up");
        timeDown.PauseTimer("down");
        upMinText.SetText("0");
        upSecText.SetText("00");
        winTimer.InitTimers(30);
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
            Debug.LogWarning(" inside both player ");
            if (!isComp)
            {
                Debug.LogWarning(" inside both player not comp");
                isComp = true;

                Debug.LogWarning("mine " + PhotonNetwork.LocalPlayer.NickName);
                initialStartTime = DateTime.Now;

                if (gameBoardParent == null)
                {
                    gameBoardParent = GameObject.Find("Game Board Parent");
                    Debug.LogError("gameboard parent " + gameBoardParent.name);
                }

                if (gameBoardParent.transform.GetChild(0))
                {
                    loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
                    loadingPanel.SetActive(false);
                }

                GameBoardManager board = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
                GameBoardManager[] gameboards = GameObject.FindObjectsOfType<GameBoardManager>();
                Debug.Log(GameObject.Find("Gameboard(Clone)") + " gameboard clone");
                GameBoardManager clone = GameObject.Find("Gameboard(Clone)").GetComponent<GameBoardManager>();
                Debug.Log(clone + " clone ");
                GameObject CloneCountdown = clone.gameObject.transform.GetChild(0).Find("Global Countdown").gameObject;
                Debug.Log(" count down " + CloneCountdown);
                CloneCountdown.SetActive(false);

                board.gameObject.SetActive(true);
                countdownTimer = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Global Countdown").GetChild(0).GetComponent<Timers>();
                countDownPanel.SetActive(true);
                bidTimer = biddingPanel.transform.GetChild(0).GetChild(3).GetComponent<Timers>();
                afterBidTimer = afterBiddingPanel.GetComponent<Timers>();
                Debug.LogError("countdownTimer " + countdownTimer + " bidTimer " + bidTimer + " afterBidTimer " + afterBidTimer);
                resultPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Result panel").gameObject;
                //Debug.LogError("result panel " + resultPanel + " " + resultPanel.transform.parent.name + " " + resultPanel.transform.parent.parent.name);

                //if (countdownTimer == null)
                //    countdownTimer = countDownPanel.GetComponent<Timer>();

                //if (PhotonNetwork.IsMasterClient)
                //    countdownTimer.InitTimers("GC", 5);

                int playerDeckProfileId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"] - 1;
                string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                Debug.LogError(playerDeckProfileId + " player deck profile id " + playerField + " player field " + PhotonNetwork.LocalPlayer.NickName);
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                int opponentDeckProfileId = (int)nextPlayer.CustomProperties["deckId"] - 1;
                string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                Debug.LogError(opponentDeckProfileId + " enemy deck profile id " + opponentField + " enemy field " + PhotonNetwork.LocalPlayer.NickName);

                int playerId = GetFieldIndex(playerField);
                int opponentId = GetFieldIndex(opponentField);

                Debug.LogError(playerId + " player id");
                Debug.LogError(opponentId + " opponent id");

                bottomImage.sprite = playerFields[playerId];
                topImage.sprite = playerFields[opponentId];
                bottomImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId);
                bottomImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, "down");
                topImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId);
                topImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, "up");


                downProfileIamge.GetComponent<Image>().sprite = profileImages[playerDeckProfileId];
                upProfileImage.GetComponent<Image>().sprite = profileImages[opponentDeckProfileId];

                if (countdownTimer == null)
                {
                    Debug.Log(countdownTimer + " not timer");
                    countdownTimer = countDownPanel.GetComponent<Timers>();
                    Debug.Log(" count down timer " + countdownTimer);
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("master ");
                    countdownTimer.InitTimers("GC", 5);
                }

                //initialStartTime = DateTime.Now;

                //if (gameBoardParent == null)
                //{
                //    gameBoardParent = GameObject.Find("Game Board Parent");
                //    Debug.LogError("gameboard parent " + gameBoardParent.name);
                //}

                //if (gameBoardParent.transform.GetChild(0))
                //{
                //    loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
                //    loadingPanel.SetActive(false);
                //}

                //GameBoardManager board = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
                //GameBoardManager[] gameboards = GameObject.FindObjectsOfType<GameBoardManager>();
                //Debug.Log(GameObject.Find("Gameboard(Clone)") + " gameboard clone");
                //GameBoardManager clone = GameObject.Find("Gameboard(Clone)").GetComponent<GameBoardManager>();
                //Debug.Log(clone + " clone ");
                //GameObject CloneCountdown = clone.gameObject.transform.GetChild(0).Find("Global Countdown").gameObject;
                //Debug.Log(" count down " + CloneCountdown);
                //CloneCountdown.SetActive(false);

                //board.gameObject.SetActive(true);
                //countdownTimer = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Global Countdown").GetChild(0).GetComponent<Timer>();
                //countDownPanel.SetActive(true);
                //bidTimer = biddingPanel.transform.GetChild(0).GetChild(3).GetComponent<Timer>();
                //afterBidTimer = afterBiddingPanel.GetComponent<Timer>();
                //Debug.LogError("countdownTimer " + countdownTimer + " bidTimer " + bidTimer + " afterBidTimer " + afterBidTimer);
                //resultPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Result panel").gameObject;
                ////Debug.LogError("result panel " + resultPanel + " " + resultPanel.transform.parent.name + " " + resultPanel.transform.parent.parent.name);

                ////if (countdownTimer == null)
                ////    countdownTimer = countDownPanel.GetComponent<Timer>();

                ////if (PhotonNetwork.IsMasterClient)
                ////    countdownTimer.InitTimers("GC", 5);

                //int playerDeckProfileId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"] - 1;
                //string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                //Debug.LogError(playerDeckProfileId + " player deck profile id " + playerField + " player field " + PhotonNetwork.LocalPlayer.NickName);
                //Player currPlayer = PhotonNetwork.LocalPlayer;
                //Player nextPlayer = currPlayer.GetNext();
                //int opponentDeckProfileId = (int)nextPlayer.CustomProperties["deckId"] - 1;
                //string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                //Debug.LogError(opponentDeckProfileId + " enemy deck profile id " + opponentField + " enemy field " + PhotonNetwork.LocalPlayer.NickName);

                //int playerId = GetFieldIndex(playerField);
                //int opponentId = GetFieldIndex(opponentField);

                //Debug.LogError(playerId + " player id");
                //Debug.LogError(opponentId + " opponent id");

                //bottomImage.sprite = playerFields[playerId];
                //topImage.sprite = playerFields[opponentId];
                //bottomImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId);
                //bottomImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, "down");
                //topImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId);
                //topImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, "up");


                //downProfileIamge.GetComponent<Image>().sprite = profileImages[playerDeckProfileId];
                //upProfileImage.GetComponent<Image>().sprite = profileImages[opponentDeckProfileId];

                //if (countdownTimer == null)
                //{

                //    countdownTimer = countDownPanel.GetComponent<Timer>();
                //}

                //if (PhotonNetwork.IsMasterClient)
                //{

                //    countdownTimer.InitTimers("GC", 5);
                //}
            }
            //if (countdownTimer == null)
            //{
            //    Debug.Log(countdownTimer + " not timer");
            //    countdownTimer = countDownPanel.GetComponent<Timer>();
            //    Debug.Log(" count down timer " + countdownTimer);
            //}

            //if (PhotonNetwork.IsMasterClient)
            //{
            //    Debug.Log("master ");
            //    countdownTimer.InitTimers("GC", 5);
            //}

            if (!isXPSet)
            {
                Debug.LogError("xp set called");
                playerXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Bottom Progress bar").gameObject;
                enemyXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Top Progress bar").gameObject;

                isXPSet = true;
                int masterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
                int clientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];

                Debug.Log(" master xp " + masterXP + " client xp " + clientXP);
                Debug.Log(" player " + playerXPProgressBar + " client " + enemyXPProgressBar);

                if (PhotonNetwork.IsMasterClient)
                {
                    playerXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Bottom Progress bar").gameObject;
                    enemyXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Top Progress bar").gameObject;

                    initialGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                    playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(masterXP);
                    enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(clientXP);

                    currentPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                    opponentGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    currentPlayerXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
                    opponentXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
                    playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                    enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                    playerController.totalGold = currentPlayerGold;
                    playerController.totalXP = currentPlayerXP;
                    enemyController.totalGold = opponentGold;
                    enemyController.totalXP = opponentXP;

                    string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                    Player currPlayer = PhotonNetwork.LocalPlayer;
                    Player nextPlayer = currPlayer.GetNext();
                    string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                    masterDeck = GetDeckGeneral(playerField);
                    clientDeck = GetDeckGeneral(opponentField);
                }
                else if (!PhotonNetwork.IsMasterClient)
                {
                    playerXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Bottom Progress bar").gameObject;
                    enemyXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Top Progress bar").gameObject;
                    initialGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(masterXP);
                    playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(clientXP);

                    currentPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    opponentGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                    currentPlayerXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
                    opponentXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
                    playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                    enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                    playerController.totalGold = currentPlayerGold;
                    playerController.totalXP = currentPlayerXP;
                    enemyController.totalGold = opponentGold;
                    enemyController.totalXP = opponentXP;

                    string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                    Player currPlayer = PhotonNetwork.LocalPlayer;
                    Player nextPlayer = currPlayer.GetNext();
                    string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                    masterDeck = GetDeckGeneral(opponentField);
                    clientDeck = GetDeckGeneral(playerField);
                }

                masterID = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                clientId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                masterMmr = 0;
                clientMmr = 0;
                matchId = "ABCD";
            }
        }

        Invoke("RemoveProperties", 0.5f);

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
                int turnCounter = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"];
                turnCounter++;
                customProp["totalTurnCount"] = turnCounter;
                PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
                Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"] + " photon player name " + PhotonNetwork.LocalPlayer.NickName + " TURN counter " + turnCounter);

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
        Debug.Log("player 1 onleft player");
        loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
        Debug.Log("is match not loaded " + isMatchNotLoaded);
        if (!endGame && !loadingPanel.activeSelf && !isMatchNotLoaded)
        {
            //leftPlayerPanel.SetActive(true);
            //leftPlayerText.SetText(otherPlayer.NickName + " Was left the game. Press Continue to Go Skirmish screen.");
            //pv.RPC("CalculateLeftWinner", RpcTarget.All);
            resultPanel.transform.GetChild(0).gameObject.SetActive(true);
            winTimer.InitTimers(30);

            totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
            matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
            experienceText = resultPanel.transform.GetChild(0).Find("Experience").GetChild(1).GetComponent<TMP_Text>();
            winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();
            GameObject mainMenu = resultPanel.transform.GetChild(0).Find("Main Menu").gameObject;

            xpSlider = resultPanel.transform.GetChild(0).Find("XP Progress Bar").GetComponent<Slider>();
            xpSlider.interactable = false;

            string winnerName = PhotonNetwork.LocalPlayer.NickName;
            winnerPlayerName.SetText(winnerName + " is Victorious!");
            int turnCounter = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"];

            string winnerId = "", loserId = "";
            int winnerXP = 0, loserXP = 0, winnerMmrChange = 0, loserMmrChange = 0;
            DeckGeneral winnerDeck = DeckGeneral.Unknown, loserDeck = DeckGeneral.Unknown;
            string matchId = "ABCD";

            Debug.LogError("leave player " + leavePlayer);
            if (leaveBtn)
            {
                Debug.Log(leaveBtn + " leave button");
                leaveBtn = false;
                if (leavePlayer == "master")
                {
                    Debug.Log("master leave ");
                    totalTurnText.SetText(turnCounter.ToString() + " Turns.");
                    clientPlayerXP += 100;
                    experienceText.SetText(clientPlayerXP.ToString());
                    xpSlider.value = (clientPlayerXP / 2000f);
                }
                else if (leavePlayer == "client")
                {
                    Debug.Log(" client leave ");
                    totalTurnText.SetText(turnCounter.ToString() + " Turns.");
                    masterPlayerXP += 100;
                    experienceText.SetText(masterPlayerXP.ToString());
                    xpSlider.value = (masterPlayerXP / 2000f);

                }
            }
            else
            {
                Debug.Log(leaveBtn + " leave button");
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log(" master remaining ");
                    totalTurnText.SetText(turnCounter.ToString() + " Turns.");
                    masterPlayerXP += 100;
                    experienceText.SetText(masterPlayerXP.ToString());
                    xpSlider.value = (masterPlayerXP / 2000f);
                }
                else
                {
                    Debug.Log("client remaining");
                    totalTurnText.SetText(turnCounter.ToString() + " Turns.");
                    clientPlayerXP += 100;
                    experienceText.SetText(clientPlayerXP.ToString());
                    xpSlider.value = (clientPlayerXP / 2000f);
                }
            }


            Debug.LogError(" main menu " + mainMenu);
            //mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer());
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeaveRemainigPlayer());

            playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
            enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

            endTime = DateTime.Now;
            int totalSeconds = (int)(endTime - initialStartTime).TotalSeconds;

            matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
            matchLengthText.SetText($"{totalSeconds} seconds");

            timeDown.PauseTimer("down");
            timeUp.PauseTimer("up");
            StopAllCoroutines();

            status = MatchStatus.PlayerQuit;

            Debug.Log(" leave player name " + leavePlayer);
            if (leavePlayer == "client")
            {
                Debug.Log(masterID + " master id " + clientId + " client id " + masterPlayerXP + " master player xp " + clientPlayerXP + " client player xp " + masterDeck + " master deck " + clientDeck + " client deck ");
                winnerId = masterID;
                loserId = clientId;
                winnerXP = masterPlayerXP;
                loserXP = clientPlayerXP;
                winnerMmrChange = masterMmr;
                loserMmrChange = clientMmr;
                winnerDeck = masterDeck;
                loserDeck = clientDeck;
            }
            else if (leavePlayer == "master")
            {

                Debug.Log(masterID + " master id " + clientId + " client id " + masterPlayerXP + " master player xp " + clientPlayerXP + " client player xp " + masterDeck + " master deck " + clientDeck + " client deck ");
                winnerId = clientId;
                loserId = masterID;
                winnerXP = clientPlayerXP;
                loserXP = masterPlayerXP;
                winnerMmrChange = clientMmr;
                loserMmrChange = masterMmr;
                winnerDeck = clientDeck;
                loserDeck = masterDeck;
            }
            Debug.LogError(" leave player name " + leavePlayer);
            Debug.Log("leave player name " + leavePlayer + " winnerId " + winnerId + " loser id " + loserId + " winnerxp " + winnerXP + " loserxp " + loserXP + " winner mmr " + winnerMmrChange + " loser mmr " + loserMmrChange + " winner deck " + winnerDeck + " loser deck " + loserDeck + " status " + status);
            matchData = new MatchData(matchId, mode, winnerId, loserId, winnerDeck, loserDeck, winnerXP, loserXP, winnerMmrChange, loserMmrChange, totalSeconds, turnCounter, status);

            PhotonNetwork.AutomaticallySyncScene = false;
            endGame = true;
            StopTimers();
            StopAllCoroutines();
        }
        else if (endGame)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            Debug.LogError("end game true " + endGame);
            resultPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Result panel").gameObject;
            Debug.LogError("resultPanel " + resultPanel);
            Debug.LogError("resultPanel.transform.GetChild(0) " + resultPanel.transform.GetChild(0));
            Debug.LogError("resultPanel.transform.GetChild(0).Find(\"Main Menu\") " + resultPanel.transform.GetChild(0).Find("Main Menu"));
            GameObject mainMenu = resultPanel.transform.GetChild(0).Find("Main Menu").gameObject;
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeaveRemainigPlayer());
            status = MatchStatus.Normal;
        }
    }

    public void GotoSkirmish()
    {
        connectUsing = true;
        PhotonNetwork.LoadLevel(3);
        StopAllCoroutines();
        ResetAllStaticValues();
    }

    private void StopTimers()
    {
        if (countdownTimer.gameObject.activeSelf)
        {
            countdownTimer.timerIsRunning = false;
            countdownTimer.enabled = false;
        }
        if (afterBidTimer.gameObject.activeSelf)
        {
            afterBidTimer.timerIsRunning = false;
            afterBidTimer.enabled = false;
        }
        if (bidTimer.gameObject.activeSelf)
        {
            bidTimer.timerIsRunning = false;
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
        Timers timerPanel = countdownTimer;
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

    int GetFieldIndex(string fieldName)
    {
        Debug.LogError(fieldName + " field name");
        switch (fieldName)
        {
            case "Green Field": return 0;
            case "Purple Moon": return 1;
            case "Dark Matter": return 2;
            case "Fairytales": return 3;
            case "Masquerades": return 4;
            case "The Old Kingdom": return 5;
            case "Tinkerers": return 6;
            default: return -1;
        }
    }

    DeckGeneral GetDeckGeneral(string name)
    {
        Debug.LogError(name + " General name");
        switch (name)
        {
            case "Green Field": return DeckGeneral.GreenField;
            case "Purple Moon": return DeckGeneral.PurpleField;
            case "Dark Matter": return DeckGeneral.DarkMatter;
            case "Fairytales": return DeckGeneral.Fairytales;
            case "Masquerades": return DeckGeneral.Masquerades;
            case "The Old Kingdom": return DeckGeneral.OldKingdom;
            case "Tinkerers": return DeckGeneral.Tinkerers;
            default: return DeckGeneral.Unknown;
        }
    }

    public void GetDamegeToWall()
    {
        //Debug.LogError("Damage called");	
        //Debug.LogError(GameManager.instance.clicked + " Instance value");	
        //Debug.LogError(EventSystem.current.currentSelectedGameObject + " current game object");	
    }

    public void LeavePlayer()
    {
        Debug.Log("leave player called");
        PhotonNetwork.AutomaticallySyncScene = false;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //connectUsing = true;
            leavePlayer = "master";
            Debug.LogError(leavePlayer + " leave player master ");
            Player foundPlayer = null;
            string clientName = PhotonNetwork.LocalPlayer.GetNext().NickName;
            bool playerFound = FindPlayerByNickname(clientName, out foundPlayer);
            leaveBtn = true;
            pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer, true);
            if (playerFound)
            {
                Player targetPlayer = foundPlayer;
                PhotonNetwork.SetMasterClient(targetPlayer);
                Debug.Log("Found player name: " + targetPlayer);
                Debug.Log(PhotonNetwork.LocalPlayer.IsMasterClient + " previous master ");
                Debug.Log(PhotonNetwork.LocalPlayer.GetNext().IsMasterClient + " current master");

            }
            else
            {
                Debug.Log("Player with nickname '" + clientName + "' not found.");
            }
            GenerateGameObject();
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
            SceneManager.LoadScene(3);
            //Invoke("LeaveRoom", 1f);
            
        }
        else if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
            Debug.LogError(pv + " pv " + pv.IsMine);
            Debug.LogError("Leave player called " + PhotonNetwork.LocalPlayer.NickName + " mine " + pv.IsMine);
            if (pv.IsMine)
            {
                leavePlayer = "client";
                Debug.LogError(leavePlayer + " leave player client ");
                Debug.Log(pv + " photon view");
                Debug.Log(photonView + " photon view");
                Debug.Log("leave called");
                leaveBtn = true;
                pv.RPC("MasterCall", RpcTarget.MasterClient);
                pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer, leaveBtn);
            }

            //    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            //    leavePlayer = "client";
            //    Player foundPlayer = null;
            //    string clientName = PhotonNetwork.LocalPlayer.NickName;
            //    bool playerFound = FindPlayerByNickname(clientName, out foundPlayer);

            //    pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer, true);
            //    if (playerFound)
            //    {
            //        connectUsing = true;
            //        Player targetPlayer = foundPlayer;
            //        PhotonNetwork.SetMasterClient(targetPlayer);
            //        Debug.Log(" client is setted maseter " + PhotonNetwork.IsMasterClient);
            //        Debug.Log("Found player name: " + targetPlayer);
            //        Debug.Log(PhotonNetwork.LocalPlayer.IsMasterClient + " previous master ");
            //        Debug.Log(PhotonNetwork.LocalPlayer.GetNext().IsMasterClient + " current master");
            //        SceneManager.LoadScene(3);
            //    }
            //    else
            //    {
            //        Debug.Log("Player with nickname '" + clientName + "' not found.");
            //    }


            //    //Debug.Log(" client is setted maseter " + PhotonNetwork.IsMasterClient);
            //    //PhotonNetwork.AutomaticallySyncScene = false;
            //    //leavePlayer = "client";
            //    //pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer);
            //    //connectUsing = true;
            //    //SceneManager.LoadScene(3);
            //    //pv.RPC("ChangeMaster", RpcTarget.Others);



            //    //if (photonView.IsMine)
            //    //{
            //    //    leavePlayer = "client";
            //    //    Debug.LogError(leavePlayer + " leave player client ");
            //    //    Debug.Log(pv + " photon view");
            //    //    Debug.Log(photonView + " photon view");
            //    //    Debug.Log("leave called");
            //    //    leaveBtn = true;
            //    //    photonView.RPC("MasterCall", RpcTarget.MasterClient);
            //    //    photonView.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer, leaveBtn);
            //    //}
            //    //leavePlayer = "client";
            //    //Debug.LogError(leavePlayer + " leave player client ");
            //    //if(PhotonNetwork.InRoom)
            //    //    PhotonNetwork.LeaveRoom();
            //    //PhotonNetwork.Disconnect();
            //    //pv.RPC("kickThisPlayer", RpcTarget.Others);
            //    //pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer);

        }
        ////SceneManager.LoadScene(3);
        //////PhotonNetwork.LoadLevel(3);
        //////skirmishManager.GetComponent<SkirmishManager>().enabled = false;
        ////Debug.Log("Nmae " + SceneManager.GetActiveScene().name);
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    private void ChangeMaster()
    {
        Debug.Log(" local player name " + PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        Debug.Log("photon network master " + PhotonNetwork.IsMasterClient);
    }

    [PunRPC]
    public void MasterCall()
    {
        Debug.Log(PhotonNetwork.IsMasterClient + " master or not ");
        pv.RPC("KickPlayerRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void KickPlayerRPC()
    {
        Debug.Log("KickPlayerRPC " + PhotonNetwork.IsMasterClient + " master or not ");
        //connectUsing = true;
        //SceneManager.LoadScene(1);
        isCompleted = true;
        PhotonView view = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        PhotonTransformView transformView = gameBoardParent.transform.GetChild(1).GetComponent<PhotonTransformView>();
        Debug.Log(view + " view " + transformView + " transform view ");
        view.enabled = false;
        transformView.enabled = false;
        Destroy(view);
        Debug.Log(view.enabled + "  view " + transformView.enabled + " tra view");
        GenerateGameObject();
        //GameObject copied =  Instantiate(gameBoardParent.transform.GetChild(1)).gameObject;
        //Destroy(copied.GetComponent<GameBoardManager>());
        //copied.transform.SetParent(gameBoardParent.transform);
        //copied.transform.localScale = gameBoardParent.transform.localScale;
        //copied.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        //copied.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        //Vector3 currentPosition = copied.transform.position;
        //currentPosition.z = 0f;
        //copied.transform.localPosition = currentPosition;
        //copied.name = "GameBoards";
        //PhotonNetwork.Destroy(view);
        //PhotonNetwork.Destroy(transformView);
        ////Invoke("LeaveRoom", 1f);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(3);
    }


    //[PunRPC]
    //private void kickThisPlayer()
    //{
    //    Debug.Log("kick this player called");
    //    Player client = PhotonNetwork.LocalPlayer.GetNext();
    //    Debug.Log(client.NickName + " client name");
    //    PhotonNetwork.CloseConnection(client);
    //}

    [PunRPC]
    private void SetLeavePlayer(string playerName, bool leaveValue)
    {
        leavePlayer = playerName;
        leaveBtn = leaveValue;
        //SkirmishManager.instance.deckId = -1;
        Debug.LogError(" leave palyer " + leavePlayer + " player name " + playerName);
    }

    public void LeaveRemainigPlayer()
    {
        Debug.LogError("called Remaining player " + PhotonNetwork.IsMasterClient);
        PhotonNetwork.AutomaticallySyncScene = false;
        //SkirmishManager.instance.deckId = -1;
        connectUsing = true;
        GenerateGameObject();
        SceneManager.LoadScene(3);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        //if (PhotonNetwork.InRoom)
        //{
        //    PhotonNetwork.LeaveRoom();
        //    PhotonNetwork.Disconnect();
        //}
    }

    private void LeaveBothPlayer()
    {
        Debug.LogError("called in both  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
        status = MatchStatus.Normal;
        //SkirmishManager.instance.deckId = -1;
        GenerateGameObject();
        SceneManager.LoadScene(3);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }

    private void LeaveBothPlayerAccidently()
    {
        loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
        Debug.Log("loading panel " + loadingPanel);
        if (loadingPanel.activeSelf)
        {
            Debug.Log("inside loading panel " + loadingPanel.activeSelf);
            pv.RPC("MatchNotLoaded", RpcTarget.All);
        }
        Debug.LogError("called in both  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
    }

    [PunRPC]
    private void MatchNotLoaded()
    {
        Debug.Log("match not loaded in both");
        connectUsing = true;
        Debug.Log("match not loaded value " + isMatchNotLoaded);
        isMatchNotLoaded = true;
        Debug.Log("match not loaded value " + isMatchNotLoaded);
        Debug.Log(" match not loaded");
        status = MatchStatus.Unknown;
        //SkirmishManager.instance.deckId = -1;
        SceneManager.LoadScene(3);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }

    private bool FindPlayerByNickname(string nickname, out Player foundPlayer)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName.Equals(nickname))
            {
                foundPlayer = player;
                return true;
            }
        }

        foundPlayer = null;
        return false;
    }

    public void GenerateGameObject()
    {
        GameObject board = Instantiate(gameBoardParent.transform.GetChild(1)).gameObject;
        Destroy(board.GetComponent<GameBoardManager>());
        board.transform.SetParent(gameBoardParent.transform);
        board.transform.localScale = gameBoardParent.transform.localScale;
        board.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        board.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        Vector3 currentPosition = board.transform.position;
        currentPosition.z = 0f;
        board.transform.localPosition = currentPosition;
        board.name = "GameBoards";
    }
}
