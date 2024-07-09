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
    [SerializeField] private List<Sprites> playerFields;
    [SerializeField] private List<Sprites> playerBrokenFields;
    [SerializeField] private List<Sprite> playerLeftDivider;
    [SerializeField] private List<Sprite> playerRightDivider;
    [SerializeField] private Image bottomLeftImage;
    [SerializeField] private Image topLeftImage;
    [SerializeField] private Image bottomRightImage;
    [SerializeField] private Image topRightImage;
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
    [SerializeField] private GameObject notMatchedPanel;
    [SerializeField] private List<GameObject> playerParent;
    [SerializeField] private List<GameObject> enemyParent;
    [SerializeField] private Image leftDivider;
    [SerializeField] private Image rightDivider;


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
    string masterDeckGeneral = "unKnown", clientDeckGeneral = "unKnown";
    string matchId = "ABCD";
    private string leavePlayer;
    private bool leaveBtn = false;
    public static bool isCompleted = false;
    private bool isMatchNotLoaded = false;
    private GameObject playerNPC;
    private GameObject enemyNPC;
    private int masterCount = 0;
    private int clientCount = 0;
    private bool isDestroyedMaster = false;
    private bool isDestroyedclient = false;
    private GameBoardManager manager;
    private bool isSpawnMaster = false;
    private bool isSpawnClient = false;
    private string winnerPlayerId = "", loserPlayerId = "";
    private bool completedActivate = false;
    private AudioManager audioManager;
    public static bool completeGame = false;
    private GameObject currentXP;
    private GameObject totalXP;
    private bool isPanelOpen = false;
    private float openTimeThreshold = 2.0f;
    private float timePanelHasBeenOpen = 0.0f;
    private bool identifiedPlayerIsMaster = false;
    private Transform cardParent = null;
    private Color normalColor = Color.white;
    private GameObject playerGlass;
    private GameObject enemyGlass;

    #endregion

    private void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        StartCoroutine(audioManager.PlayInGameMusic());
        //Debug.LogWarning("start called ");
        isWallDestroyed = false;
        endGame = false;
        MainMenuUIManager.instance.isUserXPLoaded = false;
        skirmishManager = SkirmishManager.instance;
        gameBoardParent = GameObject.Find("Game Board Parent");
        countDownPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Global Countdown").GetChild(0).gameObject;
        biddingPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Bid Panel").GetChild(0).gameObject;
        afterBiddingPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("After Bid Panel").GetChild(0).gameObject;
        countdownTimer = countDownPanel.GetComponent<Timers>();
        bidTimer = biddingPanel.transform.GetChild(0).GetChild(3).GetComponent<Timers>();
        afterBidTimer = afterBiddingPanel.GetComponent<Timers>();
        //Debug.LogError("countdownTimer " + countdownTimer + " bidTimer " + bidTimer + " afterBidTimer " + afterBidTimer);
        //Debug.LogError("countDownPanel " + countDownPanel + " biddingPanel " + biddingPanel + " afterBiddingPanel " + afterBiddingPanel);
        winTimer = gameBoardParent.transform.GetChild(1).GetChild(1).GetComponent<TimeLeft>();

        timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
        timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
        //Debug.LogError("timer down " + timeDown + " time up " + timeUp);
        cardError = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Announcements Area").gameObject;
        //Debug.Log(cardError.name + " card error ");

        customProp["totalTurnCount"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"] + " photon player name " + PhotonNetwork.LocalPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            customProp["master"] = true;
            customProp["totalMasterTurn"] = 0;
            customProp["masterUserId"] = FirebaseManager.instance.user.UserId;
            customProp["masterCurrentXP"] = MainMenuUIManager.instance.currentUserXP;
            customProp["masterCurrentLevel"] = MainMenuUIManager.instance.currentUserLevel;
            customProp["masterRequiredXPForNext"] = MainMenuUIManager.instance.nextXpRequired;
            //Debug.Log(" current master userId " + FirebaseManager.instance.user.UserId);
            //Debug.Log(" current master currentUserXP " + MainMenuUIManager.instance.currentUserXP);
            //Debug.Log(" current master currentUserLevel " + MainMenuUIManager.instance.currentUserLevel);
            //Debug.Log(" current master max " + MainMenuUIManager.instance.maxXPForCurrentLevel);
            //Debug.Log(" current master nextXpRequired " + MainMenuUIManager.instance.nextXpRequired);
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
            identifiedPlayerIsMaster = true;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            customProp["client"] = true;
            //PlayerPrefs.SetInt("clientCount", 0);
            customProp["totalClientTurn"] = 0;
            customProp["clientCurrentXP"] = MainMenuUIManager.instance.currentUserXP;
            customProp["clientCurrentLevel"] = MainMenuUIManager.instance.currentUserLevel;
            customProp["clientRequiredXPForNext"] = MainMenuUIManager.instance.nextXpRequired;
            customProp["clientUserId"] = FirebaseManager.instance.user.UserId;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
            identifiedPlayerIsMaster = false;
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
        isMatchNotLoaded = false;
        completeGame = false;
        ResetAllStaticValues();
        //Debug.Log("start called " + PhotonNetwork.IsMasterClient + " photon player " + PhotonNetwork.LocalPlayer.NickName);
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
            endGame = false;
            LeaveBothPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("left click " + EventSystem.current.currentSelectedGameObject);
            ResetClickedColor();
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                GameManager.instance.clicked = 0;
                attackingcard = null;
                ResetAnimation("player");
                ResetAnimation("field");
            }
            else
            {
                Debug.Log(" else " + EventSystem.current.currentSelectedGameObject.name);
                if (EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>())
                {
                    Debug.Log(" EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>() " + EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>());
                    if (player1Turn && PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("player1Turn && PhotonNetwork.IsMasterClient");
                        attackingcard = EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>().gameObject.GetComponentInChildren<Card>();
                        Debug.Log("attackingcard " + attackingcard.name);
                        if (attackingcard.IsAttack())
                        {
                            Debug.Log("attackingcard.IsAttack()");
                            cardError.transform.GetChild(0).gameObject.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You already attacked with this card. So, You can not attack with this card in this turn.");
                            Invoke("RemoveErrorObject", 2f);
                        }
                        else if (attackingcard.dropPosition == 0)
                        {
                            Debug.Log("attackingcard.dropPosition == 0");
                            cardError.transform.GetChild(0).gameObject.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You put the card hand to field. So can not attack with card in this turn");
                            Invoke("RemoveErrorObject", 2f);
                        }
                        else if (!attackingcard.IsAttack() && attackingcard.dropPosition == 1)
                        {
                            Debug.Log("!attackingcard.IsAttack() && attackingcard.dropPosition == 1");
                            if (attackingcard.transform.parent.parent.CompareTag("Front Line Player"))
                            {
                                Debug.Log("attackingcard.transform.parent.parent.CompareTag('Front Line Player')");
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
                            {
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
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
                            cardError.transform.GetChild(0).gameObject.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You already attacked with this card. So, You can not attack with this card in this turn.");
                            Invoke("RemoveErrorObject", 2f);

                        }
                        else if (attackingcard.dropPosition == 0)
                        {
                            cardError.transform.GetChild(0).gameObject.SetActive(true);
                            cardError.GetComponentInChildren<TMP_Text>().SetText("You put the card hand to field. So can not attack with card in this turn.");
                            Invoke("RemoveErrorObject", 2f);

                        }
                        else if (!attackingcard.IsAttack() && attackingcard.dropPosition == 1)
                        {
                            if (attackingcard.transform.parent.parent.CompareTag("Front Line Player"))
                            {
                                Debug.Log("GameManager.instance.clicked = 1;");
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
                            {
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                                Invoke("RemoveErrorObject", 2f);
                            }
                        }
                    }

                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>())
                {
                    Debug.Log("EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>() " + GameManager.instance.clicked);

                    if (GameManager.instance.clicked == 1)
                    {
                        Debug.Log("GameManager.instance.clicked == 1");
                        Card targetCard = EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>().GetComponentInChildren<Card>();
                        GameObject attackingcardParent = attackingcard.transform.parent.parent.gameObject;
                        GameObject targetCardParent = EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>().transform.parent.gameObject;
                        Debug.Log(targetCard + " target card");

                        AttackCard(attackingcard, attackingcardParent, targetCard, targetCardParent);

                        GameManager.instance.clicked = 0;
                        attackingcardParent = null;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.tag == "EnemyWall")
                {
                    manager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
                    pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();

                    bottomRightImage = manager.bottomRightImage;
                    topRightImage = manager.topRightImage;
                    topLeftImage = manager.topLeftImage;
                    bottomLeftImage = manager.bottomLeftImage;

                    //bottomImage = manager.bottomImage;
                    //topImage = manager.topImage;
                    GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
                    if (player1Turn && PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        if (GameManager.instance.clicked == 1)
                        {
                            //if (attackingcard.requirements != AbilityRequirements.OnAttack 
                            //    //|| attackingcard.requirements != AbilityRequirements.Goaded
                            //    )
                            //{
                            //    cardError.SetActive(true);
                            //    cardError.GetComponentInChildren<TMP_Text>().SetText($"You can not attack. This ability {attackingcard.ability.ToString()} is not able to attack");
                            //    Invoke("RemoveErrorObject", 2f);
                            //    return;
                            //}
                            if (attackingcard.GetParalyzedCard())
                            {
                                Debug.Log("GetParalyzedCard called " + attackingcard.GetParalyzedCard());
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText($"You are paralysed by the enemy's card. Please wait for {attackingcard.GetParalyzedCardCount()} turn.");
                                Invoke("RemoveErrorObject", 2f);
                                return;
                            }

                            if (IsGoaded(enemyField))
                            {
                                Debug.Log("Goaded called");
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You must attack enemy's goaded card.");
                                Invoke("RemoveErrorObject", 2f);
                                return;
                            }
                            enemyWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Wall").gameObject;
                            GameObject enemyHealthObject = enemyWall.transform.Find("Remaining Health").gameObject;
                            int currentHealth = int.Parse(enemyHealthObject.GetComponent<TMP_Text>().text.ToString());
                            int damageVal;
                            if (attackingcard.GetComponent<Crit>())
                            {
                                Debug.Log("crit called");
                                Crit crit = attackingcard.GetComponent<Crit>();
                                damageVal = crit.UseAbilityAndCalculateDamage(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Buster>())
                            {
                                Debug.Log("Buster called");
                                Buster buster = attackingcard.GetComponent<Buster>();
                                damageVal = buster.UseBusterAbility(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Berserker>())
                            {
                                Debug.Log("Berserker called");
                                Berserker berserker = attackingcard.GetComponent<Berserker>();
                                damageVal = berserker.UseBerserkerAbility(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Kamikaze>())
                            {
                                Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                                Debug.Log(" Kamikaze card " + kamikaze);
                                damageVal = kamikaze.CalculateDamage(attackingcard.attack);
                                string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                                Destroy(kamikaze.transform.parent.gameObject);
                                pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, 1);
                            }
                            else
                            {
                                Debug.Log("normal attack called");
                                damageVal = attackingcard.attack;
                            }

                            int remainingHealth = currentHealth - damageVal;
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                nextPlayer = PhotonNetwork.LocalPlayer.GetNext();
                                string opponentField = (string)nextPlayer.CustomProperties["deckField"];
                                int opponentId = GetFieldIndex(opponentField);
                                topRightImage.sprite = playerBrokenFields[1].sprites[opponentId];
                                topLeftImage.sprite = playerBrokenFields[2].sprites[opponentId];
                                //topImage.sprite = playerBrokenFields[opponentId];
                                enemyWall.GetComponent<PolygonCollider2D>().enabled = false;
                                enemyWall.GetComponent<Button>().enabled = false;
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(0.ToString());
                                pv.RPC("AttackWall", RpcTarget.Others, 0);
                                isWallDestroyed = true;
                                ShowHiddenCard();
                                ChangeEnemyTag();
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
                            //if (attackingcard.requirements != AbilityRequirements.OnAttack 
                            //    //|| attackingcard.requirements != AbilityRequirements.Goaded
                            //    )
                            //{
                            //    cardError.SetActive(true);
                            //    cardError.GetComponentInChildren<TMP_Text>().SetText($"You can not attack. This ability {attackingcard.ability.ToString()} is not able to attack");
                            //    Invoke("RemoveErrorObject", 2f);
                            //    return;
                            //}

                            if (attackingcard.GetParalyzedCard())
                            {
                                Debug.Log("GetParalyzedCard called " + attackingcard.GetParalyzedCard());
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText($"You are paralysed by the enemy's card. Please wait for {attackingcard.GetParalyzedCardCount()} turn.");
                                Invoke("RemoveErrorObject", 2f);
                                return;
                            }
                            if (IsGoaded(enemyField))
                            {
                                Debug.Log("goaded called");
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You must attack enemy's goaded card.");
                                Invoke("RemoveErrorObject", 2f);
                                return;
                            }
                            enemyWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Wall").gameObject;
                            GameObject enemyHealthObject = enemyWall.transform.Find("Remaining Health").gameObject;
                            int currentHealth = int.Parse(enemyHealthObject.GetComponent<TMP_Text>().text.ToString());
                            int damageVal;
                            if (attackingcard.GetComponent<Crit>())
                            {
                                Debug.Log("crit called");
                                Crit crit = attackingcard.GetComponent<Crit>();
                                damageVal = crit.UseAbilityAndCalculateDamage(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Buster>())
                            {
                                Debug.Log("Buster called");
                                Buster buster = attackingcard.GetComponent<Buster>();
                                damageVal = buster.UseBusterAbility(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Berserker>())
                            {
                                Debug.Log("Berserker called");
                                Berserker berserker = attackingcard.GetComponent<Berserker>();
                                damageVal = berserker.UseBerserkerAbility(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Kamikaze>())
                            {
                                Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                                Debug.Log(" Kamikaze card " + kamikaze);
                                damageVal = kamikaze.CalculateDamage(attackingcard.attack);
                                string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                                Destroy(kamikaze.transform.parent.gameObject);
                                pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, 2);
                            }
                            else
                            {
                                Debug.Log("normal attack called");
                                damageVal = attackingcard.attack;
                            }

                            int remainingHealth = currentHealth - damageVal;
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                nextPlayer = PhotonNetwork.LocalPlayer.GetNext();
                                string opponentField = (string)nextPlayer.CustomProperties["deckField"];
                                int opponentId = GetFieldIndex(opponentField);
                                //topImage.sprite = playerBrokenFields[opponentId];
                                topRightImage.sprite = playerBrokenFields[1].sprites[opponentId];
                                topLeftImage.sprite = playerBrokenFields[2].sprites[opponentId];
                                enemyWall.GetComponent<PolygonCollider2D>().enabled = false;
                                enemyWall.GetComponent<Button>().enabled = false;
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(0.ToString());
                                pv.RPC("AttackWall", RpcTarget.Others, 0);
                                isWallDestroyed = true;
                                ShowHiddenCard();
                                ChangeEnemyTag();
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
                else if (EventSystem.current.currentSelectedGameObject.tag == "NPC_Enemy")
                {
                    Debug.Log(EventSystem.current.currentSelectedGameObject.tag + " EventSystem.current.currentSelectedGameObject.tag");
                    pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
                    //attackingcard.SetAttackValue(true);
                    if (player1Turn && PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        Debug.Log("player1Turn && PhotonNetwork.IsMasterClient && pv.IsMine");
                        if (GameManager.instance.clicked == 1)
                        {
                            Debug.Log("GameManager.instance.clicked == 1");
                            GameObject enemyNPC = EventSystem.current.currentSelectedGameObject;
                            Debug.Log(enemyNPC.name + enemyNPC);
                            AttackNPC(enemyNPC, attackingcard);
                            GameManager.instance.clicked = 0;
                            //attackingcard.SetAttackValue(true);
                        }
                    }
                    else if (!player1Turn && !PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        if (GameManager.instance.clicked == 1)
                        {
                            Debug.Log("GameManager.instance.clicked == 1");
                            GameObject enemyNPC = EventSystem.current.currentSelectedGameObject;
                            Debug.Log(enemyNPC.name + enemyNPC);
                            AttackNPC(enemyNPC, attackingcard);
                            GameManager.instance.clicked = 0;
                            //attackingcard.SetAttackValue(true);
                        }
                    }

                }
                else if (EventSystem.current.currentSelectedGameObject.name == "Enemy Glass")
                {
                    Debug.Log(" %%%% EventSystem.current.currentSelectedGameObject.name " + EventSystem.current.currentSelectedGameObject.name);
                    pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
                    GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
                    if (GameManager.instance.clicked == 1)
                    {
                        if (isWallDestroyed)
                        {
                            //if (attackingcard.requirements != AbilityRequirements.OnAttack 
                            //    //|| attackingcard.requirements != AbilityRequirements.Goaded
                            //    )
                            //{
                            //    cardError.SetActive(true);
                            //    cardError.GetComponentInChildren<TMP_Text>().SetText($"You can not attack. This ability {attackingcard.ability.ToString()} is not able to attack");
                            //    Invoke("RemoveErrorObject", 2f);
                            //    return;
                            //}

                            if (attackingcard.GetParalyzedCard())
                            {
                                Debug.Log("GetParalyzedCard called " + attackingcard.GetParalyzedCard());
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText($"You are paralysed by the enemy's card. Please wait for {attackingcard.GetParalyzedCardCount()} turn.");
                                Invoke("RemoveErrorObject", 2f);
                                return;
                            }
                            if (IsGoaded(enemyField))
                            {
                                Debug.Log("goaded called");
                                cardError.transform.GetChild(0).gameObject.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You must attack enemy's goaded card.");
                                Invoke("RemoveErrorObject", 2f);
                                return;
                            }
                            GameObject enemyProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
                            GameObject enemyHealth = enemyProfile.transform.Find("Enemy Deck Health").Find("Remaining Health").gameObject;
                            int currentHealth = int.Parse(enemyHealth.GetComponent<TMP_Text>().text.ToString());
                            int damageVal;
                            if (attackingcard.GetComponent<Crit>())
                            {
                                Debug.Log("crit called");
                                Crit crit = attackingcard.GetComponent<Crit>();
                                damageVal = crit.UseAbilityAndCalculateDamage(attackingcard.attack);
                            }
                            else if (attackingcard.GetComponent<Kamikaze>())
                            {
                                Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                                Debug.Log(" Kamikaze card " + kamikaze);
                                damageVal = kamikaze.CalculateDamage(attackingcard.attack);
                                string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                                Destroy(kamikaze.transform.parent.gameObject);
                                pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, PhotonNetwork.IsMasterClient ? 1 : 2);
                            }
                            else if (attackingcard.GetComponent<Berserker>())
                            {
                                Debug.Log("Berserker called");
                                Berserker berserker = attackingcard.GetComponent<Berserker>();
                                damageVal = berserker.UseBerserkerAbility(attackingcard.attack);
                            }
                            else
                            {
                                Debug.Log("normal attack called");
                                damageVal = attackingcard.attack;
                            }

                            int remainingHealth = currentHealth - damageVal;
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
                            cardError.transform.GetChild(0).gameObject.SetActive(true);
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

        if (DatabaseIntegration.instance.isPalyerDataUpadated)
        {
            DatabaseIntegration.instance.isPalyerDataUpadated = false;
            //Debug.Log("databaseExampleDeleteAfterReview.instance.isPalyerDataUpadated in update");
            StartCoroutine(MainMenuUIManager.instance.LoadLevel());
        }

        if (MainMenuUIManager.instance.isUserXPLoaded)
        {
            //Debug.Log("MainMenuUIManager.instance.isUserXPLoaded");
            MainMenuUIManager.instance.isUserXPLoaded = false;
            //Debug.Log(MainMenuUIManager.instance.currentUserXP + " user Xp ");
            //Debug.Log(MainMenuUIManager.instance.currentUserLevel + " user level ");
            //Debug.Log(MainMenuUIManager.instance.nextXpRequired + " next required ");
            //Debug.Log(MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp level ");
            xpSlider = resultPanel.transform.GetChild(0).Find("XP Progress Bar").GetComponent<Slider>();
            currentXP = xpSlider.gameObject.transform.Find("xp").gameObject;
            totalXP = xpSlider.gameObject.transform.Find("total").gameObject;
            //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
            xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
            //Debug.Log(" current xp " + currentXP + " total xp " + totalXP + " xp slider " + xpSlider.name);
            currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
            totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
            xpSlider.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("player currnt turn master " + player1Turn + " photon network master client " + PhotonNetwork.IsMasterClient + " gameboard name " + gameObject.name);
            if (!gameObject.name.ToLower().Contains("clone"))
            {
                if (PhotonNetwork.IsMasterClient && player1Turn)
                {
                    //Debug.Log("PhotonNetwork.IsMasterClient && player1Turn");
                    TurnButton();
                }
                else if (!PhotonNetwork.IsMasterClient && !player1Turn)
                {
                    //Debug.Log("!PhotonNetwork.IsMasterClient && !player1Turn");
                    TurnButton();
                }
            }
        }

        HidePanel(cardError);
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
        //Debug.LogError(value + " value");
        bidTimer?.seconds.SetText(value);
    }

    [PunRPC]
    private void Global(string value)
    {
        //Debug.LogError(value + " value");
        countdownTimer?.seconds.SetText(value);
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
        currentXP = xpSlider.gameObject.transform.Find("xp").gameObject;
        totalXP = xpSlider.gameObject.transform.Find("total").gameObject;
        string winnerName = "";
        int turnCounter = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"];
        string winnerId = "", loserId = "";
        int winnerXP = 0, loserXP = 0, winnerMmrChange = 0, loserMmrChange = 0;
        int winnerTurnCount = 0, loserTurnCount = 0;
        string winnerDeck = "unKnown", loserDeck = "unKnown";
        string matchStatusVal = "normal";

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.LogError(PlayerPrefs.GetInt("masterCount") + " master value " + PhotonNetwork.LocalPlayer.NickName);
            totalTurnText.SetText(turnCounter.ToString());
            int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            int gainedMasterXp = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"];
            int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
            //Debug.Log(" master xp " + gainedMasterXp + " gained " + totalMasterXP + " total " + totalPlayerGold + " total gold");
            //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
            xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
            currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
            totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
            xpSlider.interactable = false;
            PlayerPrefs.SetInt("totalGold", totalPlayerGold);
            PlayerPrefs.SetInt("totalXP", totalMasterXP);
            //Debug.LogError(" player health " + playerHealth + " enemy health " + enemyHealth);

            if (playerHealth > enemyHealth)
            {
                winnerName = PhotonNetwork.MasterClient.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                masterPlayerXP += 100;
                clientPlayerXP += 25;
                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];

                int playerField = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                int opponentField = (int)nextPlayer.CustomProperties["deckId"];

                winnerDeck = GetPlayerDeck(playerField);
                loserDeck = GetPlayerDeck(opponentField);

                //Debug.LogError(winnerDeck + " winner deck");
                //Debug.LogError(loserDeck + " loser deck");

                winnerXP = masterPlayerXP;
                loserXP = clientPlayerXP;

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = winnerPlayerTurn;
                loserTurnCount = loserPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);
                status = MatchStatus.Normal;

                winnerMmrChange = 0;
                loserMmrChange = 0;
                //winnerDeck = PhotonNetwork.LocalPlayer.GetNext();
                experienceText.SetText(masterPlayerXP.ToString());
                //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                //xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                //currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
                //totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
                //xpSlider.interactable = false;
            }
            else if (enemyHealth > playerHealth)
            {
                winnerName = PhotonNetwork.LocalPlayer.GetNext().NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                clientPlayerXP += 100;
                masterPlayerXP += 25;
                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];

                int playerField = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                int opponentField = (int)nextPlayer.CustomProperties["deckId"];

                winnerDeck = GetPlayerDeck(opponentField);
                loserDeck = GetPlayerDeck(playerField);

                //Debug.LogError(winnerDeck + " winner deck");
                //Debug.LogError(loserDeck + " loser deck");

                winnerXP = clientPlayerXP;
                loserXP = masterPlayerXP;

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = loserPlayerTurn;
                loserTurnCount = winnerPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);

                winnerMmrChange = 0;
                loserMmrChange = 0;
                experienceText.SetText(masterPlayerXP.ToString());
                //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                //xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                //currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
                //totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
                //xpSlider.interactable = false;
            }
            else if (playerHealth == enemyHealth)
            {
                winnerPlayerName.SetText("Draw!");
                masterPlayerXP += 50;
                clientPlayerXP += 50;

                winnerMmrChange = 0;
                loserMmrChange = 0;

                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = winnerPlayerTurn;
                loserTurnCount = loserPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);

                int playerField = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                int opponentField = (int)nextPlayer.CustomProperties["deckId"];

                winnerDeck = GetPlayerDeck(playerField);
                loserDeck = GetPlayerDeck(opponentField);

                matchStatusVal = "draw";

                experienceText.SetText(masterPlayerXP.ToString());
                //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                //xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                //currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
                //totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
                //xpSlider.interactable = false;
            }
            //mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer("master"));
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer());
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            int gainedClientXp = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"];
            int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
            //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
            xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
            currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
            totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
            xpSlider.interactable = false;
            //Debug.LogError(PlayerPrefs.GetInt("clientCount") + " client value " + PhotonNetwork.LocalPlayer.NickName);
            totalTurnText.SetText(turnCounter.ToString());
            //Debug.Log(" client xp " + gainedClientXp + " gained " + totalClientXP + " total " + totalClientGold + " total client gold");
            PlayerPrefs.SetInt("totalGold", totalClientGold);
            PlayerPrefs.SetInt("totalXP", totalClientXP);
            //Debug.LogError(" player health " + playerHealth + " enemy health " + enemyHealth);

            if (playerHealth > enemyHealth)
            {
                winnerName = PhotonNetwork.LocalPlayer.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                clientPlayerXP += 100;
                masterPlayerXP += 25;
                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = loserPlayerTurn;
                loserTurnCount = winnerPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);

                experienceText.SetText(clientPlayerXP.ToString());
                //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                //xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                //currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
                //totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
                //xpSlider.interactable = false;
            }
            else if (enemyHealth > playerHealth)
            {
                winnerName = PhotonNetwork.MasterClient.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
                masterPlayerXP += 100;
                clientPlayerXP += 25;
                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = winnerPlayerTurn;
                loserTurnCount = loserPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);

                experienceText.SetText(clientPlayerXP.ToString());
                //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                //xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                //currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
                //totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
                //xpSlider.interactable = false;

            }
            else if (playerHealth == enemyHealth)
            {
                winnerPlayerName.SetText("Draw!");
                masterPlayerXP += 50;
                clientPlayerXP += 50;

                winnerId = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                loserId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = winnerPlayerTurn;
                loserTurnCount = loserPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);

                matchStatusVal = "draw";
                experienceText.SetText(clientPlayerXP.ToString());
                //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                //xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                //currentXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.currentUserXP.ToString());
                //totalXP.GetComponent<TMP_Text>().SetText(MainMenuUIManager.instance.maxXPForCurrentLevel.ToString());
                //xpSlider.interactable = false;
            }
            //mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer("client"));
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer());
        }

        int turnCounters = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"];
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"] + " photon player name " + PhotonNetwork.LocalPlayer.NickName + " TURN counter " + turnCounters);

        playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
        enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();

        endTime = DateTime.Now;
        int totalSeconds = (int)(endTime - initialStartTime).TotalSeconds;
        status = MatchStatus.Normal;
        endGame = true;
        timeDown.PauseTimer("down");
        timeUp.PauseTimer("up");
        StopAllCoroutines();

        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("SetUserSpendTime", RpcTarget.All, totalSeconds);
            //Debug.Log("winnerDeck.ToString() " + winnerDeck + " loserDeck.ToString() " + loserDeck + " totalSeconds " + totalSeconds + " winnerTurnCount " + winnerTurnCount + " loserTurnCount " + loserTurnCount + " winner hash ");

            StartCoroutine(DatabaseIntegration.instance.MatchDataUpdates(winnerId, loserId, winnerDeck, loserDeck, totalSeconds, winnerTurnCount, loserTurnCount, matchStatusVal));
            //Debug.Log("leave player name " + PhotonNetwork.IsMasterClient + " winnerId " + winnerId + " loser id " + loserId + " winnerxp " + winnerXP + " loserxp " + loserXP + " winner mmr " + winnerMmrChange + " loser mmr " + loserMmrChange + " winner deck " + winnerDeck + " loser deck " + loserDeck + " status " + matchStatusVal);
        }

        PhotonNetwork.AutomaticallySyncScene = false;
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
            //GameObject bottomField = playersFieldParent.transform.Find("Bottom Field").gameObject;
            //bottomField.GetComponent<Image>().sprite = playerBrokenFields[playerId];
            bottomRightImage = manager.bottomRightImage;
            bottomLeftImage = manager.bottomLeftImage;
            bottomRightImage.sprite = playerBrokenFields[0].sprites[playerId];
            bottomLeftImage.sprite = playerBrokenFields[3].sprites[playerId];
            ChangePlayerTag();
        }
    }

    private void RemoveErrorObject()
    {
        cardError.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void AttackCard(Card attacking, GameObject attackParent, Card target, GameObject targetParent)
    {
        Debug.Log("attack card");
        GameObject playerHand = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Hand").gameObject;
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;

        GameObject enemyHand = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();

        if (player1Turn && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("player1Turn && PhotonNetwork.IsMasterClient");
            GameObject playerCard = attacking.gameObject;
            GameObject enemyCard = target.gameObject;
            //Debug.Log(attacking.name + " name of card " + target.name + " name of target " + attacking.requirements.ToString());
            //if (attacking.requirements != AbilityRequirements.OnAttack 
            //    //|| attacking.requirements != AbilityRequirements.Goaded
            //    ) 
            //{
            //    Debug.Log("attacking.requirements " + attacking.requirements);
            //    Debug.Log(cardError.transform.parent.parent.name + " card error name");
            //    cardError.SetActive(true);
            //    cardError.GetComponentInChildren<TMP_Text>().SetText($"You can not attack. This ability {attacking.ability.ToString()} is not able to attack");
            //    Invoke("RemoveErrorObject", 2f);
            //    return;
            //} 


            bool destroyPlayer = false, destroyEnemy = false;
            int destroyPlayerAttackValue = 0, destroyEnemyAttackValue = 0;

            if (attacking.GetParalyzedCard())
            {
                Debug.Log("GetParalyzedCard called " + attacking.GetParalyzedCard());
                cardError.transform.GetChild(0).gameObject.SetActive(true);
                cardError.GetComponentInChildren<TMP_Text>().SetText($"You are paralysed by the enemy's card. Please wait for {attacking.GetParalyzedCardCount()} turn.");
                Invoke("RemoveErrorObject", 2f);
                return;
            }

            if (IsGoaded(enemyField))
            {
                Debug.Log("goaded card");
                if (target.GetComponent<Goad>())
                {
                    Debug.Log("target goaded");
                    if (attacking.GetComponent<Crit>())
                    {
                        Debug.Log("attack crit");
                        Crit crit = attackingcard.GetComponent<Crit>();
                        Debug.Log(" crit card " + crit);
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = crit.UseAbilityAndCalculateDamage(attacking.attack);
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                    }
                    else if (attacking.GetComponent<Kamikaze>())
                    {
                        Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                        Debug.Log(" Kamikaze card " + kamikaze);
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = kamikaze.CalculateDamage(attacking.attack);
                        destroyPlayer = false;
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                        string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                        enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                        enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "master");
                        Destroy(kamikaze.transform.parent.gameObject);
                        pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, 1);
                    }
                    else if (attacking.GetComponent<Hunger>())
                    {
                        Hunger hunger = attackingcard.GetComponent<Hunger>();
                        Card currentCard = attacking.GetComponent<Card>();
                        CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                        Debug.Log(" Hunger card " + hunger);
                        int totalHealth = hunger.UseAbility(attackingcard, originalCard.HP);
                        string parentId = hunger.transform.parent.parent.name.Split(" ")[2];
                        pv.RPC("CalculateHealBeforeAttack", RpcTarget.Others, hunger.id, parentId, totalHealth);
                        Debug.Log(hunger.name + " name " + attacking.HP + " hp value after updated ");
                        Debug.Log(target.attack + " target attack called");
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = attacking.attack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                    }
                    else if (attacking.GetComponent<Scattershot>())
                    {
                        Scattershot scattershot = attacking.GetComponent<Scattershot>();
                        string targetId = target.transform.parent.parent.name.Split(" ")[2];
                        Card currentCard = scattershot.GetComponent<Card>();
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = attacking.attack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);

                        List<int> enemyFieldList = CardDataBase.instance.GetSurroundingPositions(int.Parse(targetId));
                        //Debug.Log(string.Join(", ", enemyFieldList) + "  player field list ");

                        DamagingToTheSurroundingCard(enemyFieldList, enemyField, attacking, pv, scattershot.damageAmount);


                    }
                    else if (attacking.GetComponent<Berserker>())
                    {
                        Berserker berserker = attackingcard.GetComponent<Berserker>();
                        Card currentCard = attacking.GetComponent<Card>();
                        CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                        Debug.Log(" berserker card " + berserker);
                        int totalAttack = berserker.UseBerserkerAbility(attacking.attack);
                        if (totalAttack == 0)
                        {
                            attackingcard.SetAttackValue(true);
                            return;
                        }
                        string parentId = berserker.transform.parent.parent.name.Split(" ")[2];
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = totalAttack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                        //pv.RPC("Calculate", RpcTarget.Others, hunger.id, parentId, totalHealth);
                    }
                    else
                    {
                        Debug.Log("not attack crit");
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = attacking.attack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                    }

                    Debug.Log($" attack with goaded card value1 {destroyPlayerAttackValue} value2 {destroyEnemyAttackValue} ");
                }
                else
                {
                    Debug.Log("not goaded");
                    cardError.transform.GetChild(0).gameObject.SetActive(true);
                    cardError.GetComponentInChildren<TMP_Text>().SetText("You must attack enemy's goaded card.");
                    Invoke("RemoveErrorObject", 2f);
                    return;
                }
            }
            else if (attacking.GetComponent<Crit>())
            {
                Crit crit = attackingcard.GetComponent<Crit>();
                Debug.Log(" crit card " + crit);
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = crit.UseAbilityAndCalculateDamage(attacking.attack);
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
            }
            else if (attacking.GetComponent<Kamikaze>())
            {
                Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                Debug.Log(" Kamikaze card " + kamikaze);
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = kamikaze.CalculateDamage(attacking.attack);
                destroyPlayer = false;
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "master");
                Destroy(kamikaze.transform.parent.gameObject);
                pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, 1);
            }
            else if (attacking.GetComponent<Hunger>())
            {
                Hunger hunger = attackingcard.GetComponent<Hunger>();
                Card currentCard = attacking.GetComponent<Card>();
                CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                Debug.Log(" Hunger card " + hunger);
                int totalHealth = hunger.UseAbility(attackingcard, originalCard.HP);
                string parentId = hunger.transform.parent.parent.name.Split(" ")[2];
                pv.RPC("CalculateHealBeforeAttack", RpcTarget.Others, hunger.id, parentId, totalHealth);
                //Debug.Log(hunger.name + " name " + attacking.HP + " hp value after updated ");
                //Debug.Log(target.attack + " target attack called");
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = attacking.attack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
            }
            else if (attacking.GetComponent<Berserker>())
            {
                Berserker berserker = attackingcard.GetComponent<Berserker>();
                Card currentCard = attacking.GetComponent<Card>();
                CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                Debug.Log(" berserker card " + berserker);
                int totalAttack = berserker.UseBerserkerAbility(attacking.attack);
                if (totalAttack == 0)
                {
                    attackingcard.SetAttackValue(true);
                    return;
                }
                string parentId = berserker.transform.parent.parent.name.Split(" ")[2];
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = totalAttack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                //pv.RPC("Calculate", RpcTarget.Others, hunger.id, parentId, totalHealth);
            }
            else if (attacking.GetComponent<Scattershot>())
            {
                Scattershot scattershot = attacking.GetComponent<Scattershot>();
                string targetId = target.transform.parent.parent.name.Split(" ")[2];
                Card currentCard = scattershot.GetComponent<Card>();
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = attacking.attack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);

                List<int> enemyFieldList = CardDataBase.instance.GetSurroundingPositions(int.Parse(targetId));
                //Debug.Log(string.Join(", ", enemyFieldList) + "  player field list ");

                DamagingToTheSurroundingCard(enemyFieldList, enemyField, attacking, pv, scattershot.damageAmount);


            }
            else
            {
                Debug.Log("normal card attack called");
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = attacking.attack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
            }

            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);

            Debug.Log(destroyPlayer + " destroy player " + destroyEnemy + " destroy enemy");

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

            //Tuple<int, int, string, string> result = GetDestroyDataCount(playerField, enemyField);
            //int playerCount = result.Item1;
            //int enemyCount = result.Item2;
            //string playerJson = result.Item3;
            //string enemyJson = result.Item4;

            //Debug.Log(" player json " + playerJson + " enemy json  " + enemyJson);

            attackingcard.SetAttackValue(true);
            Debug.Log(destroyPlayerAttackValue + " destroy player attack value " + destroyEnemyAttackValue + " destroy enemy attack value");
            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId, destroyPlayer, destroyEnemy, 1, destroyPlayerAttackValue, destroyEnemyAttackValue);
        }
        else if (!player1Turn && !PhotonNetwork.IsMasterClient)
        {
            Debug.Log("!player1Turn && !PhotonNetwork.IsMasterClient");
            GameObject playerCard = attacking.gameObject;
            GameObject enemyCard = target.gameObject;
            //Debug.Log(playerCard + " player card " + enemyCard + " requirement " + attacking.requirements);
            //if (attacking.requirements != AbilityRequirements.OnAttack 
            //    //|| attacking.requirements != AbilityRequirements.Goaded
            //    ) 
            //{
            //    Debug.Log("not satisfy req");
            //    cardError.SetActive(true);
            //    cardError.GetComponentInChildren<TMP_Text>().SetText($"You can not attack. This ability {attacking.ability.ToString()} is not able to attack");
            //    Invoke("RemoveErrorObject", 2f);
            //    return;
            //}
            bool destroyPlayer = false, destroyEnemy = false;
            int destroyPlayerAttackValue = 0, destroyEnemyAttackValue = 0;
            if (IsGoaded(enemyField))
            {
                Debug.Log("goaded called");
                if (target.GetComponent<Goad>())
                {
                    Debug.Log("target goaded");
                    if (attacking.GetComponent<Crit>())
                    {
                        Debug.Log("attack crit");
                        Crit crit = attackingcard.GetComponent<Crit>();
                        Debug.Log(" crit card " + crit);
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = crit.UseAbilityAndCalculateDamage(attacking.attack);
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                    }
                    else if (attacking.GetComponent<Kamikaze>())
                    {
                        Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                        Debug.Log(" Kamikaze card " + kamikaze);
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = kamikaze.CalculateDamage(attacking.attack);
                        destroyPlayer = false;
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                        string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                        enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                        enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "client");
                        Destroy(kamikaze.transform.parent.gameObject);
                        pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, 2);
                    }
                    else if (attacking.GetComponent<Hunger>())
                    {
                        Hunger hunger = attackingcard.GetComponent<Hunger>();
                        Card currentCard = attacking.GetComponent<Card>();
                        CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                        Debug.Log(" Hunger card " + hunger);
                        int totalHealth = hunger.UseAbility(attackingcard, originalCard.HP);
                        string parentId = hunger.transform.parent.parent.name.Split(" ")[2];
                        pv.RPC("CalculateHealBeforeAttack", RpcTarget.Others, hunger.id, parentId, totalHealth);
                        Debug.Log(hunger.name + " name " + attacking.HP + " hp value after updated ");
                        Debug.Log(target.attack + " target attack called");
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = attacking.attack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                    }
                    else if (attacking.GetComponent<Berserker>())
                    {
                        Berserker berserker = attackingcard.GetComponent<Berserker>();
                        Card currentCard = attacking.GetComponent<Card>();
                        CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                        Debug.Log(" berserker card " + berserker);
                        int totalAttack = berserker.UseBerserkerAbility(attacking.attack);
                        if (totalAttack == 0)
                        {
                            attackingcard.SetAttackValue(true);
                            return;
                        }
                        string parentId = berserker.transform.parent.parent.name.Split(" ")[2];
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = totalAttack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                        //pv.RPC("Calculate", RpcTarget.Others, hunger.id, parentId, totalHealth);
                    }
                    else if (attacking.GetComponent<Scattershot>())
                    {
                        Scattershot scattershot = attacking.GetComponent<Scattershot>();
                        string targetId = target.transform.parent.parent.name.Split(" ")[2];
                        Card currentCard = scattershot.GetComponent<Card>();
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = attacking.attack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);

                        List<int> enemyFieldList = CardDataBase.instance.GetSurroundingPositions(int.Parse(targetId));
                        Debug.Log(string.Join(", ", enemyFieldList) + "  player field list ");

                        DamagingToTheSurroundingCard(enemyFieldList, enemyField, attacking, pv, scattershot.damageAmount);
                    }
                    else
                    {
                        Debug.Log("not attack crit");
                        destroyPlayerAttackValue = target.attack;
                        destroyEnemyAttackValue = attacking.attack;
                        destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                        destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                    }
                    Debug.Log($" attack with goaded card value1 {destroyPlayerAttackValue} value2 {destroyEnemyAttackValue} ");
                }
                else
                {
                    Debug.Log("target not goaded");
                    cardError.transform.GetChild(0).gameObject.SetActive(true);
                    cardError.GetComponentInChildren<TMP_Text>().SetText("You must attack enemy's goaded card.");
                    Invoke("RemoveErrorObject", 2f);
                    return;
                }
            }
            else if (attacking.GetComponent<Crit>())
            {
                Debug.Log("crit called");
                Crit crit = attackingcard.GetComponent<Crit>();
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = crit.UseAbilityAndCalculateDamage(attacking.attack);
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
            }
            else if (attacking.GetComponent<Kamikaze>())
            {
                Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
                Debug.Log(" Kamikaze card " + kamikaze);
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = kamikaze.CalculateDamage(attacking.attack);
                destroyPlayer = false;
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                string parentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
                enemyController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
                enemyController.DestributeGoldAndXPForEnemy(enemyCard.transform.parent.GetComponent<PhotonView>(), playerCard.GetComponent<Card>().gold, playerCard.GetComponent<Card>().XP, "client");
                Destroy(kamikaze.transform.parent.gameObject);
                pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, parentId, 2);
            }
            else if (attacking.GetComponent<Hunger>())
            {
                Hunger hunger = attackingcard.GetComponent<Hunger>();
                Card currentCard = attacking.GetComponent<Card>();
                CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                Debug.Log(" Hunger card " + hunger);
                int totalHealth = hunger.UseAbility(attackingcard, originalCard.HP);
                string parentId = hunger.transform.parent.parent.name.Split(" ")[2];
                pv.RPC("CalculateHealBeforeAttack", RpcTarget.Others, hunger.id, parentId, totalHealth);
                Debug.Log(hunger.name + " name " + attacking.HP + " hp value after updated ");
                Debug.Log(target.attack + " target attack called");
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = attacking.attack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
            }
            else if (attacking.GetComponent<Berserker>())
            {
                Berserker berserker = attackingcard.GetComponent<Berserker>();
                Card currentCard = attacking.GetComponent<Card>();
                CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                Debug.Log(" berserker card " + berserker);
                int totalAttack = berserker.UseBerserkerAbility(attacking.attack);
                if (totalAttack == 0)
                {
                    attackingcard.SetAttackValue(true);
                    return;
                }
                string parentId = berserker.transform.parent.parent.name.Split(" ")[2];
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = totalAttack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
                //pv.RPC("Calculate", RpcTarget.Others, hunger.id, parentId, totalHealth);
            }
            else if (attacking.GetComponent<Scattershot>())
            {
                Scattershot scattershot = attacking.GetComponent<Scattershot>();
                string targetId = target.transform.parent.parent.name.Split(" ")[2];
                Card currentCard = scattershot.GetComponent<Card>();
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = attacking.attack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);

                List<int> enemyFieldList = CardDataBase.instance.GetSurroundingPositions(int.Parse(targetId));
                Debug.Log(string.Join(", ", enemyFieldList) + "  player field list ");

                DamagingToTheSurroundingCard(enemyFieldList, enemyField, attacking, pv, scattershot.damageAmount);


            }
            else
            {
                Debug.Log(" normal attack called");
                destroyPlayerAttackValue = target.attack;
                destroyEnemyAttackValue = attacking.attack;
                destroyPlayer = attacking.DealDamage(destroyPlayerAttackValue, attackParent.transform.GetChild(0).gameObject);
                destroyEnemy = target.DealDamage(destroyEnemyAttackValue, targetParent.transform.GetChild(0).gameObject);
            }

            //bool destroyPlayer = attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            //bool destroyEnemy = target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);

            Debug.Log(destroyPlayer + " destroy player " + destroyEnemy);

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

            //Tuple<int, int, string, string> result = GetDestroyDataCount(playerField, enemyField);
            //int playerCount = result.Item1;
            //int enemyCount = result.Item2;
            //string playerJson = result.Item3;
            //string enemyJson = result.Item4;

            //Debug.Log(" player json " + playerJson + " enemy json  " + enemyJson);

            attackingcard.SetAttackValue(true);
            Debug.Log(destroyPlayerAttackValue + " destroy player attack value " + destroyEnemyAttackValue + " destroy enemy attack value");
            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId, destroyPlayer, destroyEnemy, 2, destroyPlayerAttackValue, destroyEnemyAttackValue);
        }
    }

    private void DamagingToTheSurroundingCard(List<int> positions, GameObject enemyField, Card attacking, PhotonView view, int damageAmt)
    {
        for (int element = 0; element < positions.Count; element++)
        {
            if (enemyField.transform.GetChild(positions[element] - 1).childCount == 1)
            {
                Card targetCard = enemyField.transform.GetChild(positions[element] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                targetCard.DealDamage(damageAmt, targetCard.transform.parent.gameObject);
                view.RPC("CardDestroyedOnOthers", RpcTarget.Others, damageAmt, positions[element] - 1);
            }
            //bool scattershot.UseScattershotAbility(playerFieldList, currentCard, pv);
        }
    }

    [PunRPC]
    private void CardDestroyedOnOthers(int attack, int position)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        if (playerField.transform.GetChild(position).childCount == 1)
        {
            Card targetCard = playerField.transform.GetChild(position).GetChild(0).GetChild(0).GetComponent<Card>();
            targetCard.DealDamage(attack, targetCard.transform.parent.gameObject);
        }
    }

    private void AttackNPC(GameObject npcObj, Card card)
    {
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        //if (card.requirements != AbilityRequirements.OnAttack 
        //    //|| card.requirements != AbilityRequirements.Goaded
        //    )
        //{
        //    cardError.SetActive(true);
        //    cardError.GetComponentInChildren<TMP_Text>().SetText($"You can not attack. This ability {card.ability.ToString()} is not able to attack");
        //    Invoke("RemoveErrorObject", 2f);
        //    return;
        //}
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        int damage;

        if (card.GetParalyzedCard())
        {
            Debug.Log("GetParalyzedCard called " + card.GetParalyzedCard());
            cardError.transform.GetChild(0).gameObject.SetActive(true);
            cardError.GetComponentInChildren<TMP_Text>().SetText($"You are paralysed by the enemy's card. Please wait for {card.GetParalyzedCardCount()} turn.");
            Invoke("RemoveErrorObject", 2f);
            return;
        }
        if (IsGoaded(enemyField))
        {
            Debug.Log("is goaded called");
            cardError.transform.GetChild(0).gameObject.SetActive(true);
            cardError.GetComponentInChildren<TMP_Text>().SetText("You must attack enemy's goaded card.");
            Invoke("RemoveErrorObject", 2f);
            return;
        }
        if (card.GetComponent<Crit>())
        {
            Debug.Log("crit card called");
            Crit crit = card.GetComponent<Crit>();
            damage = crit.UseAbilityAndCalculateDamage(card.attack);
        }
        else if (card.GetComponent<Farmer>())
        {
            Farmer farmer = card.GetComponent<Farmer>();
            damage = farmer.UseFarmerAbility(card.attack);
        }
        else if (card.GetComponent<Hunger>())
        {
            Debug.Log("Hunger called");
            Hunger hunger = card.GetComponent<Hunger>();
            CardDetails originalCard = cardDetails.Find(cardId => cardId.id == card.id);
            Debug.Log(" Hunger card " + hunger);
            int totalHealth = hunger.UseAbility(card, originalCard.HP);
            string cardParentId = hunger.transform.parent.parent.name.Split(" ")[2];
            pv.RPC("CalculateHealBeforeAttack", RpcTarget.Others, hunger.id, cardParentId, totalHealth);
            Debug.Log(hunger.name + " name " + card.HP + " hp value after updated ");
            damage = card.attack;
        }
        else if (card.GetComponent<Berserker>())
        {
            Debug.Log("Berserker called");
            Berserker berserker = card.GetComponent<Berserker>();
            damage = berserker.UseBerserkerAbility(card.attack);
        }
        else if (card.GetComponent<Kamikaze>())
        {
            Kamikaze kamikaze = attackingcard.GetComponent<Kamikaze>();
            Debug.Log(" Kamikaze card " + kamikaze);
            damage = kamikaze.CalculateDamage(card.attack);
            string cardParentId = kamikaze.transform.parent.parent.name.Split(" ")[2];
            Destroy(kamikaze.transform.parent.gameObject);
            pv.RPC("DestroyCardOnOthers", RpcTarget.Others, kamikaze.id, cardParentId, PhotonNetwork.IsMasterClient ? 1 : 2);
        }
        else
        {
            Debug.Log("normal npc attack called");
            damage = card.attack;
        }
        Debug.Log(" attack npc " + npcObj + " card " + card + " damage " + damage + " pv name " + pv.name);
        npcObj.GetComponent<NPCManager>().DealDamage(damage, npcObj);
        card.SetAttackValue(true);
        Debug.Log(npcObj.transform.parent + " npcObj.transform.parent ");
        int parentId = int.Parse(npcObj.transform.parent.name.Split(" ")[1]) - 1;
        Debug.Log("paren id " + parentId);
        pv.RPC("AttackNPCOther", RpcTarget.Others, parentId, damage);
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

    public void ClosePanel()
    {
        notMatchedPanel.SetActive(false);
        Debug.Log("close panel called");
    }

    [PunRPC]
    private void AttackCardRPC(int attackId, int targetId, bool destroyPlayer, bool destroyEnemy, int ind, int attackVal, int targetVal)
    {
        Debug.Log(" Attack card rpc " + attackId + " attackId " + targetId + " targetId " + destroyPlayer + " destroyPlayer " + destroyEnemy + " destroyEnemy " + ind + " ind " + attackVal + " attackVal " + targetVal + " targetVal");
        GameObject attackParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetChild(attackId - 1).gameObject;
        GameObject targetParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetChild(targetId - 1).gameObject;

        if (attackParent.transform.childCount > 0)
        {
            Debug.Log(" normal ");
        }
        else
        {
            cardError.transform.GetChild(0).gameObject.SetActive(true);
            cardError.GetComponentInChildren<TMP_Text>().SetText("The card(attackParent) is already destroyed and you can try to destroy again.");
            Invoke("RemoveErrorObject", 2f);
            return;
        }

        if (targetParent.transform.childCount > 0)
        {
            Debug.Log(" normal ");
        }
        else
        {
            cardError.transform.GetChild(0).gameObject.SetActive(true);
            cardError.GetComponentInChildren<TMP_Text>().SetText("The card(targetParent) is already destroyed and you can try to destroy again.");
            Invoke("RemoveErrorObject", 2f);
            return;
        }

        Debug.Log(attackParent + " attack parent " + targetParent);

        Card attacking = attackParent.GetComponentInChildren<Card>();
        Card target = targetParent.GetComponentInChildren<Card>();

        Debug.Log("attacking " + attacking + " target " + target);
        Debug.Log("attacking " + attacking.HP + " target " + target.HP);
        Debug.Log(attackVal + " attackVal " + targetVal + " targetVal");
        bool disAttackPlayer = attacking.DealDamage(attackVal, attackParent.transform.GetChild(0).gameObject);
        bool disTargetPlayer = target.DealDamage(targetVal, targetParent.transform.GetChild(0).gameObject);

        Debug.Log(disAttackPlayer + " dis attack player " + disTargetPlayer);

        //GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;

        //GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        //Tuple<int, int, string, string> result = GetDestroyDataCount(playerField, enemyField);
        //int playerCount = result.Item1;
        //int enemyCount = result.Item2;
        //string playerJson = result.Item3;
        //string enemyJson = result.Item4;

        Debug.Log(" master index " + ind);
        Debug.Log("destry player " + destroyPlayer + " destroy enemy " + destroyEnemy);
        Debug.Log("disPlayer player " + disAttackPlayer + " disPlayer enemy " + disTargetPlayer);

        //if(destroyPlayer != disAttackPlayer)
        //{
        //    Debug.Log(" not both same " + attackParent + " attack parent " + attackParent.transform.parent + " attack pp " + attackParent.transform.childCount);
        //    if(attackParent.transform.childCount == 1)
        //    {
        //        Debug.Log("child 1 and destroy");
        //        Destroy(attackParent.transform.GetChild(0).gameObject);
        //    }
        //}

        //if(destroyEnemy != disTargetPlayer)
        //{
        //    Debug.Log(" not both same " + targetParent + " targetParent parent " + targetParent.transform.parent + " targetParent pp " + targetParent.transform.childCount);
        //    if (targetParent.transform.childCount == 1)
        //    {
        //        Debug.Log("child 1 and destroy");
        //        Destroy(targetParent.transform.GetChild(0).gameObject);
        //    }
        //}

        //if(ind == 1)
        //{
        //    Debug.Log("inside 1 ");
        //    if(destroyPlayer)
        //    {
        //        Debug.Log(" destroy player " + targetParent.transform.childCount);
        //        if(targetParent.transform.childCount == 1)
        //        {
        //            notMatchedPanel.SetActive(true);
        //            Invoke("ClosePanel", 2f);
        //            Debug.Log("Destroy cards");
        //            Destroy(targetParent.transform.GetChild(0).gameObject);
        //        }
        //    }

        //    if (destroyEnemy)
        //    {
        //        Debug.Log(" destroy enemy " + attackParent.transform.childCount);
        //        if (attackParent.transform.childCount == 1)
        //        {
        //            notMatchedPanel.SetActive(true);
        //            Invoke("ClosePanel", 2f);
        //            Debug.Log("Destroy cards");
        //            Destroy(attackParent.transform.GetChild(0).gameObject);
        //        }
        //    }

        //}
        //else if(ind == 2)
        //{
        //    Debug.Log("inside 2");
        //    if (destroyPlayer)
        //    {
        //        Debug.Log(" destroy player " + targetParent.transform.childCount);
        //        if (targetParent.transform.childCount == 1)
        //        {
        //            notMatchedPanel.SetActive(true);
        //            Invoke("ClosePanel", 2f);
        //            Debug.Log("Destroy cards");
        //            Destroy(targetParent.transform.GetChild(0).gameObject);
        //        }
        //    }

        //    if (destroyEnemy)
        //    {
        //        Debug.Log(" destroy enemy " + attackParent.transform.childCount);
        //        if (attackParent.transform.childCount == 1)
        //        {
        //            notMatchedPanel.SetActive(true);
        //            Invoke("ClosePanel", 2f);
        //            Debug.Log("Destroy cards");
        //            Destroy(attackParent.transform.GetChild(0).gameObject);
        //        }
        //    }
        //}

        //if(eCount != playerCount)
        //{
        //    Debug.Log(enemyData + " <=== enemy data ===>");
        //    Debug.Log(playerJson + " <=== player json ===>");

        //    notMatchedPanel.SetActive(true);
        //    Invoke("ClosePanel", 2f);

        //    List<int> enemyList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(enemyData);
        //    List<int> playerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(playerJson);
        //    Dictionary<int, int> enemyDict = new Dictionary<int, int>();

        //    for (int i = 0; i < enemyList.Count; i++)
        //    {
        //        if (enemyList[i] != playerList[i])
        //        {
        //            Debug.Log(i + " i " + enemyList[i] + " enemy list ");
        //            enemyDict.Add(i, enemyList[i]);
        //        }
        //    }

        //    for (int i = 0; i < enemyDict.Keys.Count; i++)
        //    {
        //        CardDetails card = cardDetails.Find(cards => cards.id == enemyDict[i]);
        //        int dicId = enemyDict.ElementAt(i).Key;
        //        int dicVal = enemyDict.ElementAt(i).Value;
        //        Debug.Log(" dic id " + dicId + " dic val " + dicVal);
        //        GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", playerField.transform.GetChild(dicId).position, playerField.transform.GetChild(dicId).rotation);
        //        miniCardParent.transform.SetParent(playerField.transform.GetChild(dicId).transform);
        //        miniCardParent.transform.localScale = playerField.transform.GetChild(dicId).transform.localScale;
        //        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
        //        var completeCard = cardDetails.Find(card => card.id == dicVal);
        //        Debug.Log(" completed card level " + completeCard.levelRequired);
        //        int level = (int)(completeCard.levelRequired);
        //        miniCard.SetMiniCard(completeCard.id, completeCard.ergoTokenId, completeCard.ergoTokenAmount, completeCard.cardName, completeCard.attack, completeCard.HP, completeCard.gold, completeCard.XP, completeCard.cardImage);
        //        miniCard.name = completeCard.cardName;
        //        miniCardParent.name = selectedCardList[i].cardName;
        //    }

        //}

        //if(pCount != enemyCount)
        //{
        //    Debug.Log(playerData + " <=== player data ===>");
        //    Debug.Log(enemyJson + " <=== enemy json ===>");

        //    notMatchedPanel.SetActive(true);
        //    Invoke("ClosePanel", 2f);

        //    //if (masterIndex == 1)
        //    //{
        //    List<int> playerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(playerData);
        //        List<int> enemyList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(enemyJson);
        //        Dictionary<int, int> playerDict = new Dictionary<int, int>();

        //        for (int i = 0; i < playerList.Count; i++)
        //        {
        //            if(playerList[i] != enemyList[i])
        //            {
        //                Debug.Log(i + " i : playerList[i] " + playerList[i]);
        //                playerDict.Add(i, playerList[i]);
        //            }
        //        }

        //        for(int i = 0; i < playerDict.Keys.Count; i++)
        //        {
        //            CardDetails card = cardDetails.Find(cards => cards.id == playerDict[i]);
        //            int dicId = playerDict.ElementAt(i).Key;
        //            int dicVal = playerDict.ElementAt(i).Value;
        //            Debug.Log(" dic id " + dicId + " dic val " + dicVal);
        //            GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyField.transform.GetChild(dicId).position, enemyField.transform.GetChild(dicId).rotation);
        //            miniCardParent.transform.SetParent(enemyField.transform.GetChild(dicId).transform);
        //            miniCardParent.transform.localScale = enemyField.transform.GetChild(dicId).transform.localScale;
        //            Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
        //            var completeCard = cardDetails.Find(card => card.id == dicVal);
        //            Debug.Log(" completed card level " + completeCard.levelRequired);
        //            int level = (int)(completeCard.levelRequired);
        //            miniCard.SetMiniCard(completeCard.id, completeCard.ergoTokenId, completeCard.ergoTokenAmount, completeCard.cardName, completeCard.attack, completeCard.HP, completeCard.gold, completeCard.XP, completeCard.cardImage);
        //            miniCard.name = completeCard.cardName;
        //            //miniCardParent.name = selectedCardList[i].cardName;
        //        }
        //    //}
        //    //else if (masterIndex == 2)
        //    //{
        //    //    List<int> playerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(playerData);
        //    //    List<int> enemyList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(enemyData);
        //    //    Dictionary<int, int> playerDict = new Dictionary<int, int>();

        //    //    for (int i = 0; i < playerList.Count; i++)
        //    //    {
        //    //        if (playerList[i] != enemyList[i])
        //    //        {
        //    //            playerDict.Add(i, playerList[i]);
        //    //        }
        //    //    }

        //    //    for (int i = 0; i < playerDict.Keys.Count; i++)
        //    //    {
        //    //        CardDetails card = cardDetails.Find(cards => cards.id == playerDict[i]);
        //    //        int dicId = playerDict.ElementAt(i).Key;
        //    //        int dicVal = playerDict.ElementAt(i).Value;
        //    //        GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyField.transform.GetChild(dicId).position, enemyField.transform.GetChild(dicId).rotation);
        //    //        miniCardParent.transform.SetParent(enemyField.transform.GetChild(dicId).transform);
        //    //        miniCardParent.transform.localScale = enemyField.transform.GetChild(dicId).transform.localScale;
        //    //        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
        //    //        var completeCard = cardDetails.Find(card => card.id == dicVal);
        //    //        Debug.Log(" completed card level " + completeCard.levelRequired);
        //    //        int level = (int)(completeCard.levelRequired);
        //    //        miniCard.SetMiniCard(completeCard.id, completeCard.ergoTokenId, completeCard.ergoTokenAmount, completeCard.cardName, completeCard.attack, completeCard.HP, completeCard.gold, completeCard.XP, completeCard.cardImage);
        //    //        miniCard.name = completeCard.cardName;
        //    //        miniCardParent.name = selectedCardList[i].cardName;
        //    //    }
        //    //}
        //}

    }


    [PunRPC]
    private void AttackNPCOther(int id, int dmg)
    {
        Debug.Log("AttackNPCOther " + id + " dmg " + dmg);

        playerParent.Clear();
        for (int i = 0; i < gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Player Parent").transform.childCount; i++)
        {
            playerParent.Add(gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Player Parent").transform.GetChild(i).gameObject);
        }

        //playerParent = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>().playerParent;
        GameObject npcObj = playerParent[id].transform.GetChild(0).gameObject;
        bool isPlayerDestroy = npcObj.GetComponent<NPCManager>().DealDamage(dmg, npcObj);
        Debug.Log("player destroyed " + isPlayerDestroy);
        if (isPlayerDestroy)
        {
            manager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
            Debug.Log(" ==> PhotonNetwork.IsMasterClient " + PhotonNetwork.IsMasterClient + " name " + gameObject.name);
            if (PhotonNetwork.IsMasterClient
                //&& !gameObject.name.ToLower().Contains("clone")
                )
            {
                manager.isDestroyedMaster = true;
                Debug.Log("inside master " + manager.isDestroyedMaster + " gameobject name " + gameObject.name);
                int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
                Debug.Log(totalPlayerGold + " total player gold before " + npcObj.GetComponent<NPCManager>().gold + " npc gold");
                totalPlayerGold += npcObj.GetComponent<NPCManager>().gold;
                Debug.Log(totalPlayerGold + "==> After update gold");
                Debug.Log(totalPlayerGold + " total player gold ");
                totalMasterXP += npcObj.GetComponent<NPCManager>().XP;
                PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalPlayerGold;
                PhotonNetwork.CurrentRoom.CustomProperties["masterXP"] = totalMasterXP;
                Gold.instance.SetGold(totalPlayerGold);
            }
            else if (!PhotonNetwork.IsMasterClient
                //&& !gameObject.name.ToLower().Contains("clone")
                )
            {
                manager.isDestroyedclient = true;
                Debug.Log("inside not master " + manager.isDestroyedclient + " gameobject name " + gameObject.name);
                int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
                Debug.Log(totalClientGold + " total player gold before " + npcObj.GetComponent<NPCManager>().gold + " npc gold");
                totalClientGold += npcObj.GetComponent<NPCManager>().gold;
                Debug.Log(totalClientGold + "==> After update gold");
                PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
                PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
                Debug.Log(totalClientGold + " total client gold ");
                Gold.instance.SetGold(totalClientGold);
            }
        }

        //if (isMaster)
        //{
        //    //Debug.Log(" parent id = 4 " + parentId);
        //    int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
        //    int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
        //    totalPlayerGold += (2 * selectedNPC.GetComponent<NPCManager>().gold);
        //    Debug.Log(totalPlayerGold + " total player gold ");
        //    totalMasterXP += selectedNPC.GetComponent<NPCManager>().XP;
        //    PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalPlayerGold;
        //    PhotonNetwork.CurrentRoom.CustomProperties["masterXP"] = totalMasterXP;
        //    Gold.instance.SetGold(totalPlayerGold);
        //    Destroy(selectedNPC.gameObject);
        //    manager.isDestroyedMaster = true;
        //    Debug.Log("isDestroyedMaster " + manager.isDestroyedMaster + " gameobject name " + gameObject.name);
        //    pv.RPC("DestroyOther", RpcTarget.Others, 4);
        //}
        //else
        //{
        //    //Debug.Log(" parentId == 4 " + parentId);
        //    int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
        //    int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
        //    totalClientGold += (2 * selectedNPC.GetComponent<NPCManager>().gold);
        //    PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
        //    PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
        //    Debug.Log(totalClientGold + " total client gold ");
        //    Gold.instance.SetGold(totalClientGold);
        //    Destroy(selectedNPC.gameObject);
        //    manager.isDestroyedclient = true;
        //    Debug.Log("isDestroyedclient " + manager.isDestroyedclient + " gameobject name " + gameObject.name);
        //    pv.RPC("DestroyOther", RpcTarget.Others, 4);
        //}
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

        for (int i = 0; i < sortedList.Count; i++)
        {
            Debug.Log("inside for " + PhotonNetwork.LocalPlayer.NickName + " player name " + pv.IsMine + " is mine ");
            PhotonView view = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
            Debug.Log(view + " view " + gameBoardParent.transform.GetChild(1).gameObject + " gameboard " + gameBoardParent.transform.GetChild(1).gameObject.name);
            if (view.IsMine)
            {
                Debug.LogError(" inside the mine called");
                GameObject miniCardParent;
                string prefabPath = sortedList[i].cardName;
                GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);
                Debug.Log("CardDataBase.instance.cardPath " + CardDataBase.instance.cardPath + " sortedList[i].cardName " + sortedList[i].cardName);
                Debug.Log(cardPrefab + " cardPrefab " + prefabPath + " prefab path");
                if (cardPrefab != null)
                {
                    Debug.LogError("Prefab found at path: " + prefabPath);
                    Debug.LogError("prefab base gold value : " + cardPrefab?.gameObject?.GetComponent<GoodFavor>()?.baseGoldValue);
                    miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, cardListParent.transform.position, cardListParent.transform.rotation); miniCardParent.transform.SetParent(cardListParent.transform);
                    miniCardParent.transform.localScale = cardListParent.transform.localScale;
                    Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                    var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
                    Debug.Log(" completed card level " + completeCard.levelRequired);
                    int level = (int)(completeCard.levelRequired);
                    miniCard.SetMiniCard(sortedList[i].id, sortedList[i].ergoTokenId, sortedList[i].ergoTokenAmount, sortedList[i].cardName, sortedList[i].attack, sortedList[i].HP, sortedList[i].gold, sortedList[i].XP, sortedList[i].cardImage, sortedList[i].ability
                        //, sortedList[i].requirements, sortedList[i].abilityLevel
                        );
                    miniCard.name = sortedList[i].cardName;
                    miniCardParent.name = sortedList[i].cardName;
                    miniCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                    Debug.LogError(" photon view " + miniCardParent.GetComponent<PhotonView>());
                    DisplayWithXP(miniCard.gameObject, level);

                }
                else
                {
                    miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", cardListParent.transform.position, cardListParent.transform.rotation);
                    miniCardParent.transform.SetParent(cardListParent.transform);
                    miniCardParent.transform.localScale = cardListParent.transform.localScale;
                    Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                    var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
                    Debug.Log(" completed card level " + completeCard.levelRequired);
                    int level = (int)(completeCard.levelRequired);
                    miniCard.SetMiniCard(sortedList[i].id, sortedList[i].ergoTokenId, sortedList[i].ergoTokenAmount, sortedList[i].cardName, sortedList[i].attack, sortedList[i].HP, sortedList[i].gold, sortedList[i].XP, sortedList[i].cardImage, sortedList[i].ability
                        //, sortedList[i].requirements, sortedList[i].abilityLevel
                        );
                    UpdateSkill(sortedList[i].ability, miniCard);
                    miniCard.name = sortedList[i].cardName;
                    miniCardParent.name = sortedList[i].cardName;
                    miniCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                    Debug.LogError(" photon view " + miniCardParent.GetComponent<PhotonView>());
                    DisplayWithXP(miniCard.gameObject, level);

                }
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

    public void UpdateSkill(CardAbility ability, Card card)
    {
        switch (ability)
        {
            case CardAbility.GoodFavor:
                GoodFavor favor = card.gameObject.AddComponent<GoodFavor>();
                favor.ability = CardAbility.GoodFavor;
                favor.SetAbility();
                break;
            case CardAbility.Crit:
                Crit crit = card.gameObject.AddComponent<Crit>();
                crit.ability = CardAbility.Crit;
                crit.SetAbility();
                break;
            case CardAbility.Goad:
                Goad goad = card.gameObject.AddComponent<Goad>();
                goad.ability = CardAbility.Goad;
                break;
            case CardAbility.Meteor:
                Meteor meteor = card.gameObject.AddComponent<Meteor>();
                meteor.ability = CardAbility.Meteor;
                meteor.InitializedCard();
                break;
            case CardAbility.Kamikaze:
                Kamikaze kamikaze = card.gameObject.AddComponent<Kamikaze>();
                kamikaze.ability = CardAbility.Kamikaze;
                break;
            case CardAbility.Renewal:
                Renewal renewal = card.gameObject.AddComponent<Renewal>();
                renewal.ability = CardAbility.Renewal;
                break;
            case CardAbility.Hunger:
                Hunger hunger = card.gameObject.AddComponent<Hunger>();
                hunger.ability = CardAbility.Hunger;
                break;
            case CardAbility.Evolve:
                Evolve evolve = card.gameObject.AddComponent<Evolve>();
                evolve.ability = CardAbility.Evolve;
                evolve.InitializedCard();
                break;
            case CardAbility.Clone:
                Clone clone = card.gameObject.AddComponent<Clone>();
                clone.ability = CardAbility.Clone;
                clone.InitializedCard();
                break;
            case CardAbility.Mutate:
                Mutate mutate = card.gameObject.AddComponent<Mutate>();
                mutate.ability = CardAbility.Mutate;
                break;
            case CardAbility.Malignant:
                Malignant malignant = card.gameObject.AddComponent<Malignant>();
                malignant.ability = CardAbility.Malignant;
                break;
            case CardAbility.Farmer:
                Farmer farmer = card.gameObject.AddComponent<Farmer>();
                farmer.ability = CardAbility.Farmer;
                break;
            case CardAbility.Buster:
                Buster buster = card.gameObject.AddComponent<Buster>();
                buster.ability = CardAbility.Buster;
                break;
            case CardAbility.Summon:
                Summon summon = card.gameObject.AddComponent<Summon>();
                summon.ability = CardAbility.Summon;
                break;
            case CardAbility.Repair:
                Repair repair = card.gameObject.AddComponent<Repair>();
                repair.ability = CardAbility.Repair;
                break;
            case CardAbility.GeneralBoon:
                GeneralBoon boon = card.gameObject.AddComponent<GeneralBoon>();
                boon.ability = CardAbility.GeneralBoon;
                break;
            case CardAbility.Serenity:
                Serenity serenity = card.gameObject.AddComponent<Serenity>();
                serenity.ability = CardAbility.Serenity;
                break;
            case CardAbility.Scattershot:
                Scattershot scattershot = card.gameObject.AddComponent<Scattershot>();
                scattershot.ability = CardAbility.Serenity;
                break;
            case CardAbility.Berserker:
                Berserker berserker = card.gameObject.AddComponent<Berserker>();
                berserker.ability = CardAbility.Berserker;
                break;
            case CardAbility.Mason:
                Mason mason = card.gameObject.AddComponent<Mason>();
                mason.ability = CardAbility.Mason;
                break;
            case CardAbility.Paralyze:
                Paralyze paralyze = card.gameObject.AddComponent<Paralyze>();
                paralyze.ability = CardAbility.Paralyze;
                break;
            case CardAbility.Curse:
                Curse curse = card.gameObject.AddComponent<Curse>();
                curse.ability = CardAbility.Curse;
                break;
            case CardAbility.Smite:
                Smite smite = card.gameObject.AddComponent<Smite>();
                smite.ability = CardAbility.Smite;
                break;
            case CardAbility.Doom:
                Doom doom = card.gameObject.AddComponent<Doom>();
                doom.ability = CardAbility.Doom;
                break;
            case CardAbility.Gambit:
                Gambit gambit = card.gameObject.AddComponent<Gambit>();
                gambit.ability = CardAbility.Gambit;
                break;
            case CardAbility.GeneralBane:
                GeneralBane generalBane = card.gameObject.AddComponent<GeneralBane>();
                generalBane.ability = CardAbility.GeneralBane;
                break;
            case CardAbility.Blackhole:
                Blackhole blackHole = card.gameObject.AddComponent<Blackhole>();
                blackHole.ability = CardAbility.Blackhole;
                break;
            case CardAbility.Sacrifice:
                Sacrifice sacrifice = card.gameObject.AddComponent<Sacrifice>();
                sacrifice.ability = CardAbility.Sacrifice;
                break;
            case CardAbility.Mimic:
                Mimic mimic = card.gameObject.AddComponent<Mimic>();
                mimic.ability = CardAbility.Mimic;
                break;
            case CardAbility.Endgame:
                EndGame endGame = card.gameObject.AddComponent<EndGame>();
                endGame.ability = CardAbility.Endgame;
                endGame.SetAbility();
                break;
            case CardAbility.Silence:
                Silence silence = card.gameObject.AddComponent<Silence>();
                silence.ability = CardAbility.Silence;
                break;
            default: break;
        }
    }

    private Type GetTheType(CardAbility ability)
    {
        Type currentType = null;

        switch (ability)
        {
            case CardAbility.GoodFavor:
                currentType = typeof(GoodFavor);
                break;
            case CardAbility.Crit:
                currentType = typeof(GoodFavor);
                break;
            case CardAbility.Goad:
                currentType = typeof(GoodFavor);
                break;
        }
        return currentType;
    }


    private int SortByLevelAndOrder(CardDetails a, CardDetails b)
    {
        int levelA = (int)(a.levelRequired);
        int levelB = (int)(b.levelRequired);

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
        if (playerController.playerGainedXP < 200 && level < 1)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP);
            shouldDisplay = true;
        }
        else if ((playerController.playerGainedXP >= 200 && playerController.playerGainedXP < 400) && level <= 1)
        {
            Debug.Log("level " + level + " playerController.playerGainedXP " + playerController.playerGainedXP);
            shouldDisplay = true;
        }
        else if ((playerController.playerGainedXP >= 400 && playerController.playerGainedXP < 600) && level <= 2)
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
    }

    private void DisplayCard(Card card, Color color)
    {
        Debug.LogError("Display card called");
        Color currentColor = card.transform.GetChild(0).GetComponent<Image>().color;
        //currentColor.a = 1f;
        card.transform.parent.GetComponent<HoverMiniCard>().isEnable = true;
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
        coroutine = StartCoroutine(EndTheTurn(0, false));
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
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
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
            GameObject miniCardParent;
            string prefabPath = selectedCardList[i].cardName;
            GameObject cardPrefab = Resources.Load<GameObject>(prefabPath);
            if (cardPrefab != null)
            {
                Debug.LogError("Prefab found at path: " + prefabPath);
                miniCardParent = PhotonNetwork.Instantiate(cardPrefab.name, cardListParent.transform.position, cardListParent.transform.rotation); miniCardParent.transform.SetParent(cardListParent.transform);
                miniCardParent.transform.localScale = cardListParent.transform.localScale;
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
                Debug.Log(" completed card level " + completeCard.levelRequired);
                int level = (int)(completeCard.levelRequired);
                miniCard.SetMiniCard(selectedCardList[i].id, selectedCardList[i].ergoTokenId, selectedCardList[i].ergoTokenAmount, selectedCardList[i].cardName, selectedCardList[i].attack, selectedCardList[i].HP, selectedCardList[i].gold, selectedCardList[i].XP, selectedCardList[i].cardImage, selectedCardList[i].ability
                    //, selectedCardList[i].requirements, selectedCardList[i].abilityLevel
                    );
                miniCard.name = selectedCardList[i].cardName;
                miniCardParent.name = selectedCardList[i].cardName;
                miniCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                DisplayWithXP(miniCard.gameObject, level);
            }
            else
            {
                miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", cardListParent.transform.position, cardListParent.transform.rotation);
                miniCardParent.transform.SetParent(cardListParent.transform);
                miniCardParent.transform.localScale = cardListParent.transform.localScale;
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
                Debug.Log(" completed card level " + completeCard.levelRequired);
                int level = (int)(completeCard.levelRequired);
                miniCard.SetMiniCard(selectedCardList[i].id, selectedCardList[i].ergoTokenId, selectedCardList[i].ergoTokenAmount, selectedCardList[i].cardName, selectedCardList[i].attack, selectedCardList[i].HP, selectedCardList[i].gold, selectedCardList[i].XP, selectedCardList[i].cardImage, selectedCardList[i].ability
                //, selectedCardList[i].requirements, selectedCardList[i].abilityLevel
                );
                UpdateSkill(selectedCardList[i].ability, miniCard);
                miniCard.name = selectedCardList[i].cardName;
                miniCardParent.name = selectedCardList[i].cardName;
                miniCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                Debug.LogError(" photon view " + miniCardParent.GetComponent<PhotonView>());
                DisplayWithXP(miniCard.gameObject, level);

            }

            //GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", cardListParent.transform.position, cardListParent.transform.rotation);
            //miniCardParent.transform.SetParent(cardListParent.transform);
            //miniCardParent.transform.localScale = cardListParent.transform.localScale;
            //Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
            //var completeCard = cardDetails.Find(card => card.id == sortedList[i].id);
            //Debug.Log(" completed card level " + completeCard.levelRequired);
            //int level = (int)(completeCard.levelRequired);
            //miniCard.SetMiniCard(selectedCardList[i].id, selectedCardList[i].ergoTokenId, selectedCardList[i].ergoTokenAmount, selectedCardList[i].cardName, selectedCardList[i].attack, selectedCardList[i].HP, selectedCardList[i].gold, selectedCardList[i].XP, selectedCardList[i].cardImage, selectedCardList[i].ability
            //    //, selectedCardList[i].requirements, selectedCardList[i].abilityLevel
            //    );
            //miniCard.name = selectedCardList[i].cardName;
            //miniCardParent.name = selectedCardList[i].cardName;
            ////DisplayWithXP(miniCard.gameObject, level);
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
        Debug.Log("ButtonClick " + button.name);
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        Debug.Log(pv + " pv " + Hover.clickCounter);
        Transform settedParent = null;

        if (Hover.clickCounter == 1)
        {
            Hover.clickCounter = 0;
            if (player1Turn && PhotonNetwork.IsMasterClient)
            {
                GameObject playerHand = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Hand").gameObject;

                GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
                Debug.Log("==== master-start ====");
                int currentPos = int.Parse(button.name.Split(" ")[2]);
                Debug.Log(currentPos + " currentPos");
                Hover.cardParent.transform.GetComponent<PhotonView>();
                int previousPos = int.Parse(Hover.cardParent.transform.parent.name.Split(" ")[2]);
                Debug.Log(previousPos + " previousPos ");
                int cardId = Hover.cardParent.transform.parent.GetChild(0).GetChild(0).GetComponent<Card>().id;
                Card hoveredCard = Hover.cardParent.transform.parent.GetChild(0).GetChild(0).GetComponent<Card>();
                GameObject playerToBePositioned = button.gameObject;
                Debug.Log(cardId + " card id " + hoveredCard + " hovered card " + playerToBePositioned);
                bool isSatisfy = IsSatisfyRequirements(hoveredCard, playerField, playerToBePositioned, false);
                Debug.Log(isSatisfy + " is satisfy");
                if (!isSatisfy)
                {
                    cardError.transform.GetChild(0).gameObject.SetActive(true);
                    cardError.GetComponentInChildren<TMP_Text>().SetText("The card can not satisfy the requirement to put the field.");
                    Invoke("RemoveErrorObject", 2f);
                    ResetAnimation("player");
                    ResetAnimation("field");
                    return;
                }
                settedParent = EventSystem.current.currentSelectedGameObject.gameObject.transform;
                Hover.cardParent.transform.SetParent(settedParent);
                Hover.cardParent.transform.position = settedParent.position;
                Destroy(Hover.cardParent.GetComponent<DragMiniCards>());
                Hover.cardParent.AddComponent<DragFieldCard>();
                Debug.Log(Hover.cardParent.name + " Hover.cardParent");
                Debug.Log("EventSystem.current.currentSelectedGameObject.gameObject " + EventSystem.current.currentSelectedGameObject.gameObject.name);
                settedParent.transform.GetChild(0).gameObject.GetComponent<ClickedMiniCard>().isEnable = true;
                playerHand.transform.GetChild(previousPos - 1).gameObject.SetActive(false);
                Tuple<int, int> result = GetTotalCardsCount(playerHand, playerField);
                int handCount = result.Item1;
                int fieldCount = result.Item2;
                Debug.Log(handCount + " hand count " + fieldCount + " field count");
                Debug.Log("move card called not in others " + handCount + " hand count " + fieldCount);
                Debug.Log("Card setted ");
                pv.RPC("MoveCard", RpcTarget.Others, previousPos, currentPos, handCount, fieldCount, cardId);
                Debug.Log(" hover card " + hoveredCard + " settedParent " + settedParent);
                OnSetCard(hoveredCard, PhotonNetwork.IsMasterClient, settedParent);
                Debug.Log("==== master-end ====");
            }
            else if (!player1Turn && !PhotonNetwork.IsMasterClient)
            {
                GameObject playerHand = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Hand").gameObject;

                GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;

                Debug.Log("==== client-start ====");
                int currentPos = int.Parse(button.name.Split(" ")[2]);
                Debug.Log(currentPos + " currentPos");
                Hover.cardParent.transform.GetComponent<PhotonView>();
                int previousPos = int.Parse(Hover.cardParent.transform.parent.name.Split(" ")[2]);
                Debug.Log(previousPos + " previousPos ");
                int cardId = Hover.cardParent.transform.parent.GetChild(0).GetChild(0).GetComponent<Card>().id;
                Card hoveredCard = Hover.cardParent.transform.parent.GetChild(0).GetChild(0).GetComponent<Card>();
                GameObject playerToBePositioned = button.gameObject;
                Debug.Log(cardId + " card id " + hoveredCard + " hovered card " + playerToBePositioned);
                bool isSatisfy = IsSatisfyRequirements(hoveredCard, playerField, playerToBePositioned, false);
                Debug.Log("is satisfy " + isSatisfy);
                if (!isSatisfy)
                {
                    cardError.transform.GetChild(0).gameObject.SetActive(true);
                    cardError.GetComponentInChildren<TMP_Text>().SetText("The card can not satisfy the requirement to put the field.");
                    Invoke("RemoveErrorObject", 2f);
                    ResetAnimation("player");
                    ResetAnimation("field");
                    return;
                }

                settedParent = EventSystem.current.currentSelectedGameObject.gameObject.transform;
                Hover.cardParent.transform.SetParent(settedParent);
                Hover.cardParent.transform.position = settedParent.position;
                Destroy(Hover.cardParent.GetComponent<DragMiniCards>());
                Hover.cardParent.AddComponent<DragFieldCard>();
                Debug.Log(Hover.cardParent.name + " Hover.cardParent");
                Debug.Log("EventSystem.current.currentSelectedGameObject.gameObject " + EventSystem.current.currentSelectedGameObject.gameObject.name);
                settedParent.transform.GetChild(0).gameObject.GetComponent<ClickedMiniCard>().isEnable = true;
                playerHand.transform.GetChild(previousPos - 1).gameObject.SetActive(false);
                Tuple<int, int> result = GetTotalCardsCount(playerHand, playerField);
                int handCount = result.Item1;
                int fieldCount = result.Item2;
                Debug.Log(handCount + " hand count " + fieldCount + " field count");
                Debug.Log("move card called not in others " + handCount + " hand count " + fieldCount);
                Debug.Log("card setted ");
                Debug.Log("**## MoveCard " + previousPos + " previousPos " + currentPos + " currentPos " + handCount + " handCount " + fieldCount + " fieldCount " + cardId + " cardId ");
                pv.RPC("MoveCard", RpcTarget.Others, previousPos, currentPos, handCount, fieldCount, cardId);
                OnSetCard(hoveredCard, PhotonNetwork.IsMasterClient, settedParent);
                Debug.Log("==== client-end ====");
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
            randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
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

    IEnumerator EndTheTurn(float time, bool rpc)
    {
        completedActivate = false;
        ResetAnimation("player");
        ResetAnimation("field");
        ResetAllCard();
        ResetClickedColor();
        yield return new WaitForSeconds(time);
        if (PhotonNetwork.PlayerList.Count() == 2)
        {
            photonPlayer = PhotonNetwork.LocalPlayer;
            var nextPlayer = photonPlayer.GetNext();

            var nextPlayerID = nextPlayer.ActorNumber;
            string minText = downMinText.text;
            string secText = downSecText.text;
            manager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();

            if (!endGame)
            {
                //Debug.Log(" player 1 tuen " + player1Turn + " PhotonNetwork.IsMasterClient " + PhotonNetwork.IsMasterClient);
                if (player1Turn && PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    //turnCountMaster = PlayerPrefs.GetInt("masterCount") + 1;
                    //PlayerPrefs.SetInt("masterCount", turnCountMaster);
                    //Debug.Log(" player1Turn && PhotonNetwork.IsMasterClient  ");
                    PlayerTurnEnd();
                    pv.RPC("ChangePlayerTurn", RpcTarget.All, false);
                    //Debug.Log(manager.isDestroyedMaster + " isDestroyedMaster " + " gonamr " + gameObject.name);
                    ResetSelectedCards();
                }
                else if (!player1Turn && !PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    //turnCountClient = PlayerPrefs.GetInt("clientCount") + 1;
                    //PlayerPrefs.SetInt("clientCount", turnCountClient);
                    //Debug.Log("!player1Turn && !PhotonNetwork.IsMasterClient");
                    PlayerTurnEnd();
                    pv.RPC("ChangePlayerTurn", RpcTarget.All, true);
                    //Debug.Log(isDestroyedclient + " isDestroyedclient " + " gonamr " + gameObject.name);
                    ResetSelectedCards();
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
            GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

            for (int i = 0; i < gameObj.transform.childCount; i++)
            {
                if (gameObj.transform.GetChild(i).childCount == 1)
                {
                    gameObj.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
                }
            }

            for (int i = 0; i < enemyField.transform.childCount; i++)
            {
                if (enemyField.transform.GetChild(i).childCount == 1)
                {
                    enemyField.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
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

    private void ChangePlayerTag()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            playerField.transform.GetChild(i).tag = "Front Line Player";
        }
    }

    private void ChangeEnemyTag()
    {
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        //Debug.Log(" enemy field " + enemyField);
        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            enemyField.transform.GetChild(i).tag = "Front Line Enemy";
        }
    }

    private void SpawnNPC()
    {
        //Debug.Log("Spawn NPC");
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        //Debug.Log(pv.IsMine + " pv is mine " + gameBoardParent.transform.GetChild(1).name);
        //Debug.Log("master client " + PhotonNetwork.IsMasterClient + " PhotonNetwork.IsMasterClient " + pv.IsMine + " mine " + gameObject.name);

        playerParent.Clear();
        for (int i = 0; i < gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Player Parent").transform.childCount; i++)
        {
            playerParent.Add(gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Player Parent").transform.GetChild(i).gameObject);
        }
        //playerParent = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>().playerParent;


        if (PhotonNetwork.IsMasterClient && pv.IsMine
            && !gameObject.name.ToLower().Contains("clone".ToLower())
            )
        {
            //Debug.Log(pv.IsMine + " pv is master  " + gameBoardParent.transform.GetChild(1).name + " is master " + PhotonNetwork.IsMasterClient + " gameobject " + gameObject.name);

            GameObject npc = PhotonNetwork.Instantiate("NPC_Player_1", playerParent[0].transform.position, Quaternion.Euler(0, 0, 0));
            npc.tag = "NPC_Player";
            npc.transform.SetParent(playerParent[0].transform);
            npc.transform.localScale = playerParent[0].transform.localScale;
            //Debug.Log(npc + " npc in mine " + pv.IsMine);
            npc.GetComponent<NPCManager>().SetNPCProperties(5, 4, 8, 6, 30);
            pv.RPC("SpawnOther", RpcTarget.Others);


        }
        else if (!PhotonNetwork.IsMasterClient && pv.IsMine
            && !gameObject.name.ToLower().Contains("clone".ToLower())
            )
        {
            //Debug.Log(pv.IsMine + " pv is client " + gameBoardParent.transform.GetChild(1).name + " is master " + PhotonNetwork.IsMasterClient + " gameobject " + gameObject.name);

            GameObject npc = PhotonNetwork.Instantiate("NPC_Enemy_2", playerParent[0].transform.position, Quaternion.Euler(0, 0, 0));
            npc.tag = "NPC_Player";
            npc.transform.SetParent(playerParent[0].transform);
            npc.transform.localScale = playerParent[0].transform.localScale;
            //Debug.Log(npc + " npc in mine " + pv.IsMine);
            npc.GetComponent<NPCManager>().SetNPCProperties(5, 4, 8, 6, 30);
            pv.RPC("SpawnOther", RpcTarget.Others);


        }
    }

    [PunRPC]
    private void SpawnOther()
    {
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        //Debug.Log("pv " + pv);
        //Debug.Log("pv name " + pv.gameObject.name + " SpawnOther");

        enemyParent.Clear();
        for (int i = 0; i < gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Enemy Parent").transform.childCount; i++)
        {
            enemyParent.Add(gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Enemy Parent").transform.GetChild(i).gameObject);
        }

        if (//!gameObject.name.Contains("clone") && 
            pv.IsMine
            && PhotonNetwork.IsMasterClient)
        {
            gameBoardParent = GameObject.Find("Game Board Parent");
            //Debug.Log(" game board parent " + gameBoardParent);
            //Debug.Log("gameBoardParent.transform.GetChild(1) " + gameBoardParent.transform.GetChild(0));
            //Debug.Log("gameBoardParent.transform.GetChild(1) " + gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>());
            //enemyParent = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>().enemyParent;
            GameObject npc = PhotonNetwork.Instantiate("NPC_Enemy_1", enemyParent[0].transform.position, Quaternion.Euler(0, 0, 0));
            npc.tag = "NPC_Enemy";
            npc.transform.SetParent(enemyParent[0].transform);
            npc.transform.localScale = enemyParent[0].transform.localScale;
            //Debug.Log(npc + " npc in enemy ");
            //Debug.Log(enemyParent[0].transform.parent.parent.parent.name + " enemy parent name ");
            npc.GetComponent<NPCManager>().SetNPCProperties(5, 4, 8, 6, 30);
        }
        else if (//!gameObject.name.Contains("clone") && 
            pv.IsMine && !PhotonNetwork.IsMasterClient)
        {
            gameBoardParent = GameObject.Find("Game Board Parent");
            //Debug.Log(" game board parent " + gameBoardParent);
            //Debug.Log("gameBoardParent.transform.GetChild(1) " + gameBoardParent.transform.GetChild(0));
            //Debug.Log("gameBoardParent.transform.GetChild(1) " + gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>());
            //enemyParent = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>().enemyParent;
            GameObject npc = PhotonNetwork.Instantiate("NPC_Player_2", enemyParent[0].transform.position, Quaternion.Euler(0, 0, 0));
            npc.tag = "NPC_Enemy";
            npc.transform.SetParent(enemyParent[0].transform);
            npc.transform.localScale = enemyParent[0].transform.localScale;
            //Debug.Log(npc + " npc in enemy ");
            //Debug.Log(enemyParent[0].transform.parent.parent.parent.name + " enemy parent name ");
            npc.GetComponent<NPCManager>().SetNPCProperties(5, 4, 8, 6, 30);
        }
    }

    private void SpawnNextNPC()
    {

        //Debug.Log("Spawn NPC");
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        //Debug.Log(pv.IsMine + " pv is mine " + gameBoardParent.transform.GetChild(1).name);
        //Debug.Log("master client " + PhotonNetwork.IsMasterClient + " PhotonNetwork.IsMasterClient " + pv.IsMine + " mine " + gameObject.name);

        playerParent.Clear();
        for (int i = 0; i < gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Player Parent").transform.childCount; i++)
        {
            playerParent.Add(gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Player Parent").transform.GetChild(i).gameObject);
        }

        //playerParent = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>().playerParent;
        manager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();


        if (PhotonNetwork.IsMasterClient && pv.IsMine
            //&& !gameObject.name.ToLower().Contains("clone".ToLower())
            )
        {
            //Debug.Log(pv.IsMine + " pv is master  " + gameBoardParent.transform.GetChild(1).name + " is master " + PhotonNetwork.IsMasterClient + " gameobject " + gameObject.name);

            GameObject npc = PhotonNetwork.Instantiate("NPC_Player_1", playerParent[0].transform.position, Quaternion.Euler(0, 0, 0));
            npc.tag = "NPC_Player";
            npc.transform.SetParent(playerParent[0].transform);
            npc.transform.localScale = playerParent[0].transform.localScale;
            //Debug.Log(npc + " npc in mine " + pv.IsMine);
            npc.GetComponent<NPCManager>().SetNPCProperties(5, 4, 8, 6, 30);
            manager.isSpawnMaster = true;
            //Debug.Log(manager.isSpawnMaster + " isSpawn " + PhotonNetwork.IsMasterClient);
            pv.RPC("SpawnOther", RpcTarget.Others);


        }
        else if (!PhotonNetwork.IsMasterClient && pv.IsMine
            //&& !gameObject.name.ToLower().Contains("clone".ToLower())
            )
        {
            //Debug.Log(pv.IsMine + " pv is client " + gameBoardParent.transform.GetChild(1).name + " is master " + PhotonNetwork.IsMasterClient + " gameobject " + gameObject.name);

            GameObject npc = PhotonNetwork.Instantiate("NPC_Enemy_2", playerParent[0].transform.position, Quaternion.Euler(0, 0, 0));
            npc.tag = "NPC_Player";
            npc.transform.SetParent(playerParent[0].transform);
            npc.transform.localScale = playerParent[0].transform.localScale;
            // Debug.Log(npc + " npc in mine " + pv.IsMine);
            npc.GetComponent<NPCManager>().SetNPCProperties(5, 4, 8, 6, 30);
            manager.isSpawnClient = true;
            //Debug.Log(manager.isSpawnClient + " isSpawn " + PhotonNetwork.IsMasterClient);
            pv.RPC("SpawnOther", RpcTarget.Others);
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
        coroutine = StartCoroutine(EndTheTurn(time, true));
    }

    [PunRPC]
    public void BiddingPanel()
    {
        Timers.isBiddingTime = false;
        //RemoveProperties();
        biddingPanel?.SetActive(true);
        countDownPanel?.SetActive(false);
        afterBiddingPanel?.SetActive(true);
        bidTimer?.InitTimers("BT", 20);
        SetPlayerName();
    }

    [PunRPC]
    public void BidComplete()
    {
        Timers.isCompletedBid = false;
        countDownPanel?.SetActive(false);
        afterBidTimer?.InitTimers("CB", 5);
        afterBiddingPanel?.SetActive(false);
        GetWinnerName();
        //Debug.Log("bid complete");
        SpawnNPC();
    }

    [PunRPC]
    public void AfterBidComplete()
    {
        biddingPanel?.SetActive(false);
        //Debug.Log("After bid complete");
        //SpawnNPC();
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
        completeGame = true;
        Debug.Log(" set the true value CompleteGame rpc " + completeGame);
    }

    [PunRPC]
    public void MoveCard(int prevPos, int currPos, int hCount, int fCount, int id)
    {
        Debug.Log("**## inside MoveCard " + prevPos + " previousPos " + currPos + " currentPos " + hCount + " handCount " + fCount + " fieldCount " + id + " cardId ");
        Debug.Log($"MoveCard called with prevPos: {prevPos}, currPos: {currPos}, hCount: {hCount}, fCount: {fCount}, id: {id}");
        
        //Debug.Log(" move card " + hCount + " h count " + fCount + " f count");
        CardDetails clickedCard = cardDetails.Find(card => card.id == id);
        //Debug.Log(clickedCard.id + " click card id");
        GameObject enemyHand = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Hand").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        Debug.Log($"{enemyHand.transform.childCount} enemyHand : {enemyField.transform.childCount} enemyField");

        if (prevPos - 1 < 0 || prevPos - 1 >= enemyHand.transform.childCount)
        {
            Debug.LogError($"prevPos {prevPos} is out of bounds for enemyHand with {enemyHand.transform.childCount} children.");
            return;
        }

        if (currPos - 1 < 0 || currPos - 1 >= enemyField.transform.childCount)
        {
            Debug.LogError($"currPos {currPos} is out of bounds for enemyField with {enemyField.transform.childCount} children.");
            return;
        }

        if(enemyHand.transform.GetChild(prevPos - 1).childCount <= 0)
        {
            GameObject go = gameBoardParent.transform.Find("TestGameObject").gameObject;
            Debug.LogError($"child count prevPos is 0 enemyHand.transform.GetChild(prevPos - 1)");
            go.SetActive(true);
            return;
        }
        
        if(enemyField.transform.GetChild(currPos - 1).childCount > 0)
        {
            GameObject go =  gameBoardParent.transform.Find("TestGameObject").gameObject;
            Debug.Log(go.name + " gameobject name enemyField.transform.GetChild(currPos - 1)");
            go.SetActive(true);
            Debug.LogError($"child count currPos is >s 0");
            return;
        }

        Debug.Log(enemyHand.transform.GetChild(prevPos - 1) + " previous pos 1 ");
        Debug.Log(enemyHand.transform.GetChild(prevPos - 1).childCount + " previous pos 1 total child ");
        GameObject selectedObject = enemyHand.transform.GetChild(prevPos - 1).GetChild(0).gameObject;
        GameObject selectedObjectParent = enemyField.transform.GetChild(currPos - 1).gameObject;
        Debug.Log("GameObject selectedObjectParent = enemyField.transform.GetChild(currPos - 1).gameObject;");
        selectedObject.transform.SetParent(selectedObjectParent.transform);
        Debug.Log("selectedObject.transform.SetParent(selectedObjectParent.transform);");
        selectedObject.transform.position = selectedObjectParent.transform.position;
        Debug.Log("selectedObject.transform.position = selectedObjectParent.transform.position;");
        enemyHand.transform.GetChild(prevPos - 1).gameObject.SetActive(false);
        selectedObject.AddComponent<DropFieldCard>();

        //Debug.Log(selectedObjectParent.name + " selected obje parent name ");
        //Debug.Log(selectedObject.name + " selected obje  name ");
        //Debug.Log("selectedObjectParent.transform.childCount " + selectedObjectParent.transform.childCount);
        //if (selectedObjectParent.transform.childCount == 1)
        //{
        //    //Debug.Log(" child count 1" + selectedObjectParent.name + " selected card parent " + clickedCard + " selected card ");

        //    //Debug.Log(" selectedcard.transform.GetChild(0).GetComponent<Card>().id  " + selectedObject.transform.GetChild(0).GetComponent<Card>().id + " click card id " + clickedCard.id);
        //    ////Destroy(selectedObjectParent.transform.GetChild(0).gameObject);
        //    //Debug.Log("Destroyed if already present " + selectedObjectParent.transform.GetChild(0));
        //    if (selectedObject.transform.GetChild(0).GetComponent<Card>().id != id)
        //    {
        //        //Debug.Log("not matched");
        //        Destroy(selectedObjectParent.transform.GetChild(0).gameObject);
        //        GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", selectedObjectParent.transform.position, selectedObjectParent.transform.rotation);
        //        miniCardParent.transform.SetParent(selectedObjectParent.transform);
        //        miniCardParent.transform.localScale = selectedObjectParent.transform.localScale;
        //        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
        //        //Debug.Log(" completed card level " + clickedCard.levelRequired);
        //        int level = (int)(clickedCard.levelRequired);
        //        miniCard.SetMiniCard(clickedCard.id, clickedCard.ergoTokenId, clickedCard.ergoTokenAmount, clickedCard.cardName, clickedCard.attack, clickedCard.HP, clickedCard.gold, clickedCard.XP, clickedCard.cardImage, clickedCard.ability
        //            //, clickedCard.requirements, clickedCard.abilityLevel
        //            );
        //        miniCard.name = clickedCard.cardName;
        //        miniCardParent.name = clickedCard.cardName;
        //    }
        //}
        //Debug.Log("selectedObjectParent.transform.childCount" + selectedObjectParent.transform.childCount);
        //if (selectedObjectParent.transform.childCount == 0)
        //{
        //    //Debug.Log(" child count 0" + selectedObjectParent.name + " selected card parent " + clickedCard + " selected card ");

        //    //Debug.Log(" selectedcard.transform.GetChild(0).GetComponent<Card>().id  " + selectedObject.transform.GetChild(0).GetComponent<Card>().id + " click card id " + clickedCard.id);
        //    GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", selectedObjectParent.transform.position, selectedObjectParent.transform.rotation);
        //    miniCardParent.transform.SetParent(selectedObjectParent.transform);
        //    miniCardParent.transform.localScale = selectedObjectParent.transform.localScale;
        //    Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
        //    //Debug.Log(" completed card level " + clickedCard.levelRequired);
        //    int level = (int)(clickedCard.levelRequired);
        //    miniCard.SetMiniCard(clickedCard.id, clickedCard.ergoTokenId, clickedCard.ergoTokenAmount, clickedCard.cardName, clickedCard.attack, clickedCard.HP, clickedCard.gold, clickedCard.XP, clickedCard.cardImage, clickedCard.ability
        //        //, clickedCard.requirements, clickedCard.abilityLevel
        //        );
        //    miniCard.name = clickedCard.cardName;
        //    miniCardParent.name = clickedCard.cardName;
        //}

        //Tuple<int, int> result = GetTotalCardsCount(enemyHand, enemyField);
        //int handCount = result.Item1;
        //int fieldCount = result.Item2;
        //Debug.Log(handCount + " hand count " + fieldCount + " field count");

        //Debug.Log("move card called in others " + handCount + " hand count " + fieldCount);

        //if (handCount != hCount)
        //{
        //    cardError.transform.GetChild(0).gameObject.SetActive(true);
        //    cardError.GetComponentInChildren<TMP_Text>().SetText("The hand count and h count not same");
        //    Debug.Log("The hand count and h count not same");
        //    Invoke("RemoveErrorObject", 2f);
        //}

        //if (fieldCount != fCount)
        //{
        //    cardError.transform.GetChild(0).gameObject.SetActive(true);
        //    cardError.GetComponentInChildren<TMP_Text>().SetText("The fieldCount and f count not same");
        //    Debug.Log("The fieldCount and f count not same");
        //    Invoke("RemoveErrorObject", 2f);
        //}

        if (selectedObjectParent.tag == "Front Line Enemy")
        {
            Debug.Log(selectedObjectParent.name + " selectedObjectParent fornt line " + selectedObjectParent.tag);
            Debug.Log(selectedObject.name + " selectedObject fornt line ");
            selectedObject.SetActive(true);
        }
        else if (selectedObjectParent.tag == "Back Line Enemy")
        {
            Debug.Log(selectedObjectParent.name + " selectedObjectParent back line " + selectedObjectParent.tag);
            Debug.Log(selectedObject.name + " selectedObject back line ");
            selectedObject.SetActive(false);
        }
        HideBackLineCards();
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
            //Debug.LogWarning(" inside both player ");
            if (!isComp)
            {
                //Debug.LogWarning(" inside both player not comp");
                isComp = true;

                //Debug.LogWarning("mine " + PhotonNetwork.LocalPlayer.NickName);
                initialStartTime = DateTime.Now;

                if (gameBoardParent == null)
                {
                    gameBoardParent = GameObject.Find("Game Board Parent");
                    //Debug.LogError("gameboard parent " + gameBoardParent.name);
                }

                if (gameBoardParent.transform.GetChild(0))
                {
                    loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
                    loadingPanel.SetActive(false);
                }

                GameBoardManager board = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
                GameBoardManager[] gameboards = GameObject.FindObjectsOfType<GameBoardManager>();
                //Debug.Log(GameObject.Find("Gameboard(Clone)") + " gameboard clone");
                GameBoardManager clone = GameObject.Find("Gameboard(Clone)").GetComponent<GameBoardManager>();
                //Debug.Log(clone + " clone ");
                GameObject CloneCountdown = clone.gameObject.transform.GetChild(0).Find("Global Countdown").gameObject;
                //Debug.Log(" count down " + CloneCountdown);
                CloneCountdown.SetActive(false);

                board.gameObject.SetActive(true);
                countdownTimer = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Global Countdown").GetChild(0).GetComponent<Timers>();
                countDownPanel.SetActive(true);
                bidTimer = biddingPanel.transform.GetChild(0).GetChild(3).GetComponent<Timers>();
                afterBidTimer = afterBiddingPanel.GetComponent<Timers>();
                //Debug.LogError("countdownTimer " + countdownTimer + " bidTimer " + bidTimer + " afterBidTimer " + afterBidTimer);
                resultPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Result panel").gameObject;
                //Debug.LogError("result panel " + resultPanel + " " + resultPanel.transform.parent.name + " " + resultPanel.transform.parent.parent.name);

                //if (countdownTimer == null)
                //    countdownTimer = countDownPanel.GetComponent<Timer>();

                //if (PhotonNetwork.IsMasterClient)
                //    countdownTimer.InitTimers("GC", 5);

                int playerDeckProfileId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"] - 1;
                string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
                //Debug.LogError(playerDeckProfileId + " player deck profile id " + playerField + " player field " + PhotonNetwork.LocalPlayer.NickName);
                Player currPlayer = PhotonNetwork.LocalPlayer;
                Player nextPlayer = currPlayer.GetNext();
                int opponentDeckProfileId = (int)nextPlayer.CustomProperties["deckId"] - 1;
                string opponentField = (string)nextPlayer.CustomProperties["deckField"];

                //Debug.LogError(opponentDeckProfileId + " enemy deck profile id " + opponentField + " enemy field " + PhotonNetwork.LocalPlayer.NickName);

                //int playerId = GetFieldIndex(playerField);
                //int opponentId = GetFieldIndex(opponentField);                
                int playerId = GetFieldIndex(playerField);
                int opponentId = GetFieldIndex(opponentField);

                //Debug.LogError(playerId + " player id");
                //Debug.LogError(opponentId + " opponent id");

                //bottomImage.sprite = playerFields[0][playerId];
                //topImage.sprite = playerFields[opponentId];
                //bottomImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId);
                //bottomImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, "down");
                //topImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId);
                //topImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, "up");
                bottomRightImage.sprite = playerFields[0].sprites[playerId];
                topRightImage.sprite = playerFields[1].sprites[opponentId];
                topLeftImage.sprite = playerFields[2].sprites[opponentId];
                bottomLeftImage.sprite = playerFields[3].sprites[playerId];

                bottomRightImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId, 0);
                topRightImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId, 1);
                topLeftImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId, 2);
                bottomLeftImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId, 3);

                bottomRightImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, 0);
                topRightImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, 1);
                topLeftImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, 2);
                bottomLeftImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, 3);

                downProfileIamge.GetComponent<Image>().sprite = profileImages[playerDeckProfileId];
                upProfileImage.GetComponent<Image>().sprite = profileImages[opponentDeckProfileId];
                leftDivider.sprite = playerLeftDivider[playerId];
                rightDivider.sprite = playerRightDivider[opponentId];

                if (countdownTimer == null)
                {
                    //Debug.Log(countdownTimer + " not timer");
                    countdownTimer = countDownPanel.GetComponent<Timers>();
                    //Debug.Log(" count down timer " + countdownTimer);
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    //Debug.Log("master ");
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
                //Debug.LogError("xp set called");
                playerXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Bottom Progress bar").gameObject;
                enemyXPProgressBar = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Top Progress bar").gameObject;

                isXPSet = true;
                int masterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
                int clientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];

                //Debug.Log(" master xp " + masterXP + " client xp " + clientXP);
                //Debug.Log(" player " + playerXPProgressBar + " client " + enemyXPProgressBar);

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
                    int playerFieldId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                    int opponentFieldId = (int)nextPlayer.CustomProperties["deckId"];
                    masterDeckGeneral = GetPlayerDeck(playerFieldId);
                    clientDeckGeneral = GetPlayerDeck(opponentFieldId);
                    //Debug.Log(masterDeckGeneral + " masterDeckGeneral" + clientDeckGeneral + " clientDeckGeneral");
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

                    int playerFieldId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                    int opponentFieldId = (int)nextPlayer.CustomProperties["deckId"];
                    masterDeckGeneral = GetPlayerDeck(opponentFieldId);
                    clientDeckGeneral = GetPlayerDeck(playerFieldId);
                    //Debug.Log(masterDeckGeneral + " masterDeckGeneral" + clientDeckGeneral + " clientDeckGeneral");
                }

                masterID = (string)PhotonNetwork.CurrentRoom.CustomProperties["masterUserId"];
                clientId = (string)PhotonNetwork.CurrentRoom.CustomProperties["clientUserId"];
                //Debug.Log(masterID + " master user id " + clientId + " client user id");
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

        Debug.Log("active player called ");

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
                playerGlass = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Glass").gameObject; 
                enemyGlass = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Glass").gameObject;
                Debug.Log(playerGlass + " playerGlass " + enemyGlass + " enemyGlass " + enemyGlass.transform.parent.parent);
                EnableOrDisablePointerImage(playerGlass, false);
                EnableOrDisablePointerImage(enemyGlass, true);
                //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["totalTurnCount"] + " photon player name " + PhotonNetwork.LocalPlayer.NickName + " TURN counter " + turnCounter);

                string minText = downMinText.text;
                string secText = downSecText.text;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(EndTheTurn(41, false));
                int totalSec = (int.Parse(minText) * 60) + int.Parse(secText);

                timeUp.PauseTimer("up");
                string min = Mathf.FloorToInt(timeUp.currentTime / 60).ToString("0");
                string sec = Mathf.FloorToInt(timeUp.currentTime % 60).ToString("00");
                upMinText.SetText(min);
                upSecText.SetText(sec);
                manager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
                //Debug.Log("player turn " + player1Turn);
                if (PhotonNetwork.IsMasterClient)
                {
                    //PlayerPrefs.SetInt("masterCount", 0);
                    if (!completedActivate)
                    {
                        //Debug.Log("turnCountMaster before " + turnCountMaster + " name " + gameObject.name);
                        turnCountMaster = ((int)customProp["totalMasterTurn"]) + 1;
                        //Debug.Log("turnCountMaster after " + turnCountMaster + " name " + gameObject.name);
                        //PlayerPrefs.SetInt("masterCount", turnCountMaster);
                        customProp["totalMasterTurn"] = turnCountMaster;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
                    }

                    //GetComponent<Good_Favor>().StartTurn(PhotonNetwork.IsMasterClient);
                    if (manager.isSpawnMaster)
                    {
                        manager.isSpawnMaster = false;
                        //Debug.Log("isspawn " + manager.isSpawnMaster);
                    }
                    else
                    {
                        //Debug.Log("isspawn " + manager.isSpawnMaster);
                        MovePlayer();
                    }

                    if (!gameObject.name.ToLower().Contains("clone"))
                        StartTheTurn();
                    //if (!gameObject.name.ToLower().Contains("clone"))
                    //{
                    //    GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
                    //    GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

                    //    for (int i = 0; i < playerField.transform.childCount; i++)
                    //    {
                    //        if (playerField.transform.GetChild(i).childCount == 1)
                    //        {
                    //            Debug.Log(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().requirements.ToString() + " requirements");

                    //            Debug.Log(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability.ToString() + " requirements");
                    //            string name = playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability.ToString();

                    //            Type.GetType(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability.ToString());
                    //            Debug.Log(Type.GetType(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability.ToString()));

                    //            Type getType = GetTheType(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability);
                    //            Debug.Log(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().GetComponent(getType) + " requirements");

                    //            Component[] components = playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().GetComponents<Component>();
                    //            Debug.Log(components.Length + " length of component");
                    //        } 
                    //    }
                    //}
                }
                else if (!PhotonNetwork.IsMasterClient)
                {
                    //turnCountClient = PlayerPrefs.GetInt("clientCount") + 1;
                    if (!completedActivate)
                    {
                        completedActivate = true;
                        //Debug.Log("turnCountClient before " + turnCountClient + " name " + gameObject.name);
                        turnCountClient = ((int)customProp["totalClientTurn"]) + 1;
                        //Debug.Log("turnCountClient after " + turnCountClient + " name " + gameObject.name);
                        //PlayerPrefs.SetInt("totalClientTurn", turnCountClient);
                        customProp["totalClientTurn"] = turnCountClient;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
                    }


                    //GetComponent<Good_Favor>().StartTurn(PhotonNetwork.IsMasterClient);
                    if (manager.isSpawnClient)
                    {
                        manager.isSpawnClient = false;
                        //Debug.Log("isspawn " + manager.isSpawnMaster);
                    }
                    else
                    {
                        //Debug.Log("isspawn " + manager.isSpawnClient);
                        MovePlayer();
                    }

                    if (!gameObject.name.ToLower().Contains("clone"))
                        StartTheTurn();
                }
                //isSpawn = false;
                //Debug.Log("isspawn " + isSpawn);
                //MovePlayer();



                // Debug.Log("apply active player " + id + " gameobject name " + gameObject.name + " manager name " + manager.gameObject.name);
                if (PhotonNetwork.IsMasterClient)
                {
                    //if (!gameObject.name.ToLower().Contains("clone".ToLower()))
                    //{
                    //Debug.Log("master called " + gameObject.name + " isDestroyedMaster " + manager.isDestroyedMaster + " count " + manager.masterCount);
                    if (manager.isDestroyedMaster)
                    {
                        manager.masterCount++;
                        //Debug.Log("masterCount " + manager.masterCount);
                    }
                    if (manager.masterCount == 3)
                    {
                        //Debug.Log(" masterCount == 3 " + manager.masterCount);
                        SpawnNextNPC();
                        manager.isDestroyedMaster = false;
                        manager.masterCount = 0;
                    }
                    //}
                }
                else
                {
                    //if (!gameObject.name.ToLower().Contains("clone".ToLower()))
                    //{
                    //Debug.Log(" not master called " + gameObject.name + " isDestroyedclient " + manager.isDestroyedclient + " count " + manager.clientCount);
                    if (manager.isDestroyedclient)
                    {
                        manager.clientCount++;
                        //Debug.Log("clientCount " + manager.clientCount);
                    }
                    if (manager.clientCount == 3)
                    {
                        //Debug.Log("clientCount == 3 " + clientCount);
                        SpawnNextNPC();
                        manager.isDestroyedclient = false;
                        manager.clientCount = 0;
                    }
                    //}
                }

                pv.RPC("SetDownTimeText", RpcTarget.Others, min, sec);
                timeDown.InitTimers("Down", totalSec);
                pv.RPC("CoroutineMethod", RpcTarget.Others, 41f, minText, secText);
                pv.RPC("PauseTimerForTurnEndPlayer", RpcTarget.Others, totalSec);
            }
            else
            {
                playerGlass = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Glass").gameObject;
                enemyGlass = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Glass").gameObject;
                Debug.Log(playerGlass + " playerGlass " + enemyGlass + " enemyGlass " + enemyGlass.transform.parent.parent);
                EnableOrDisablePointerImage(playerGlass, false);
                EnableOrDisablePointerImage(enemyGlass, true);
            }
        }
    }

    #endregion

    public void StartTheTurn()
    {
        //Debug.Log(" start the turn called ");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        GameObject playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
        GameObject enemyWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Wall").gameObject;

        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();

        bool isPlayerWallDestroyed = IsWallDestroyed(playerWall);
        FieldManager.instance.ResetCounters();
        FieldManager.instance.CalculateAbilityCounter(playerField);
        //Debug.Log("player field " + playerField.name + " child count " + playerField.transform.childCount + " is wall destroyed " + isPlayerWallDestroyed + " count of field " + (isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2));
        for (int i = 0; i < (isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2); i++)
        {
            //Debug.Log(" i value " + i + " playerField.transform.GetChild(i).childCount " + playerField.transform.GetChild(i).childCount);
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                //Debug.Log(playerField.transform.GetChild(i)?.GetChild(0)?.GetChild(0).GetComponent<Card>().requirements + " requirements");
                //if(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().requirements == AbilityRequirements.AtTheStartOfTurn)
                //{
                GameObject cardObj = playerField.transform.GetChild(i).GetChild(0).GetChild(0).gameObject;
                Debug.Log(cardObj + " card  gameobject");

                if (cardObj.GetComponent<Card>().GetParalyzedCard())
                {
                    Card currentCard = cardObj.GetComponent<Card>();
                    int count = currentCard.GetParalyzedCardCount();
                    //int parentId = int.Parse(playerField.transform.GetChild(i).name.Split(" ")[2]);
                    int paralyzedVal = count > 0 ? -1 : 0;
                    Debug.Log(currentCard + " currentCard " + count + " count " + paralyzedVal);
                    currentCard.SetParalyzedCard(paralyzedVal, currentCard.id);
                    //pv.RPC("ChangesOnParalyzedCount", RpcTarget.Others, parentId, paralyzedVal);
                }

                //Debug.Log("i value before " + i);
                if (cardObj.GetComponent<Card>().ability == CardAbility.None)
                {
                    Debug.Log("cardObj.GetComponent<Card>().ability == CardAbility.None");
                    continue;
                }

                if (cardObj.GetComponent<Card>().ability == CardAbility.DeActivate)
                {
                    Debug.Log("cardObj.GetComponent<Card>().ability == CardAbility.DeActivate");
                    continue;
                }

                //Debug.Log("i value after " + i);

                if (cardObj.GetComponent<GoodFavor>())
                {
                    cardObj.GetComponent<GoodFavor>().UseAbility(PhotonNetwork.IsMasterClient);
                    UpdateGoldUI();
                }
                else if (cardObj.GetComponent<Meteor>())
                {
                    Meteor meteor = cardObj.GetComponent<Meteor>();
                    Debug.Log(meteor + " meteor called");
                    meteor.SetRoundNumber(1);
                    if (meteor.IsUseAbility())
                    {
                        //Debug.Log(enemyField + " enemy field " + meteor.IsUseAbility() + " is use ability");

                        int damage = meteor.damageAmount;
                        meteor.ResetRound();
                        bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                        for (int j = 0; j < (isEnemyWallDestroyed ? enemyField.transform.childCount : playerField.transform.childCount / 2); j++)
                        {
                            //if (enemyField.transform.GetChild(j).tag.Contains("Front Line"))
                            //{
                            if (enemyField.transform.GetChild(j).childCount == 1)
                            {
                                if (enemyField.transform.GetChild(j).GetChild(0).childCount == 1)
                                {
                                    Card attackedCard = enemyField.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Card>();
                                    Debug.Log(attackedCard.name + " attack card ");
                                    attackedCard.DealDamage(meteor.damageAmount, attackedCard.transform.parent.gameObject);
                                }
                            }
                            //}
                        }
                        Debug.Log(damage + " damage amount value");
                        pv.RPC("ApplyDamageToOthers", RpcTarget.Others, damage);
                    }
                }
                else if (cardObj.GetComponent<Renewal>())
                {
                    Renewal renewal = cardObj.GetComponent<Renewal>();
                    Card currentCard = renewal.GetComponent<Card>();
                    CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                    Debug.Log(renewal + " Renewal called");
                    int cardHP = renewal.UseAbility(currentCard, originalCard.HP);
                    string parentId = renewal.transform.parent.parent.name.Split(" ")[2];
                    pv.RPC("HealCardToOthers", RpcTarget.Others, cardHP, parentId);
                }
                else if (cardObj.GetComponent<Evolve>())
                {
                    Evolve evolve = cardObj.GetComponent<Evolve>();
                    Debug.Log(evolve + " evlove called");
                    evolve.SetRoundNumber(1);
                    if (evolve.IsUseAbility())
                    {
                        Debug.Log("evolve is useability called ");
                        int attack = evolve.requiredEvolveAttack;
                        int health = evolve.requiredEvolveHealth;
                        Card attackCard = evolve.GetComponent<Card>();
                        CardDetails originalCard = cardDetails.Find(cardId => cardId.id == attackCard.id);
                        Debug.Log(attack + " attack value " + health + " health value");
                        evolve.ResetRound();

                        int attackValue = attackCard.SetCardAttack(attack, originalCard.attack);
                        int healthValue = attackCard.HealCard(health, originalCard.HP);
                        Debug.Log(attackValue + " att value " + healthValue + " hea val ");
                        string parentId = evolve.transform.parent.parent.name.Split(" ")[2];
                        Debug.Log(gameObject.name + " gameobject name " + pv + " pv name ");
                        pv.RPC("EvolveCardInOthers", RpcTarget.Others, attackValue, healthValue, parentId);
                    }
                }
                else if (cardObj.GetComponent<Mutate>())
                {
                    Mutate mutate = cardObj.GetComponent<Mutate>();
                    Card currentCard = mutate.GetComponent<Card>();
                    Debug.Log(mutate + " Mutate called");
                    CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                    Tuple<int, int> result = mutate.MutatingCard(currentCard, originalCard);

                    Debug.Log(currentCard.attack + " attack " + currentCard.HP + " hp");

                    currentCard.SetAttack(result.Item1);
                    currentCard.SetHP(result.Item2);
                    string parentId = mutate.transform.parent.parent.name.Split(" ")[2];
                    pv.RPC("MutateCardToOthers", RpcTarget.Others, result.Item1, result.Item2, parentId);
                }
                else if (cardObj.GetComponent<Malignant>())
                {
                    Malignant malignant = cardObj.GetComponent<Malignant>();
                    Card currentCard = malignant.GetComponent<Card>();
                    Debug.Log(malignant + " Malignant called");
                    List<int> enemyFieldCardsList = new();
                    enemyFieldCardsList.Clear();

                    bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                    for (int j = 0; j < (isEnemyWallDestroyed ? enemyField.transform.childCount : enemyField.transform.childCount / 2); j++)
                    {
                        //if (enemyField.transform.GetChild(j).tag.Contains("Front Line"))
                        //{
                        if (enemyField.transform.GetChild(j).childCount == 1)
                        {
                            if (enemyField.transform.GetChild(j).GetChild(0).childCount == 1)
                            {
                                int parentId = int.Parse(enemyField.transform.GetChild(j).name.Split(" ")[2]);
                                enemyFieldCardsList.Add(parentId);
                            }
                        }
                        //}
                    }
                    //Debug.Log("list added =>");
                    List<int> listOfEnemyCardsToBeDamaged = malignant.GetRandomPositionListOfEnemyCards(enemyFieldCardsList);
                    //Debug.Log(listOfEnemyCardsToBeDamaged.Count + " count value listOfEnemyCardsToBeDamaged ");
                    //Debug.Log("Random Elements: " + string.Join(", ", listOfEnemyCardsToBeDamaged));

                    for (int element = 0; element < listOfEnemyCardsToBeDamaged.Count; element++)
                    {
                        //Debug.Log(element + " element " + listOfEnemyCardsToBeDamaged + " ")
                        if (enemyField.transform.GetChild(listOfEnemyCardsToBeDamaged[element] - 1) != null)
                        {
                            Card card = enemyField.transform.GetChild(listOfEnemyCardsToBeDamaged[element] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                            //Debug.Log(card + "  card ");
                            //Debug.Log(card.name + " card name ");
                            //Debug.Log(malignant.name + " malifnant name ");
                            //Debug.Log(card.transform.parent + " card parent name ");
                            //Debug.Log(card.transform.parent.parent + " card parent parent name ");
                            bool destroyed = malignant.GiveDamageToEnemyField(card.transform.parent.gameObject, card);
                            Debug.Log(destroyed + "         ");
                            int currentHealth = 0;
                            if (!destroyed)
                            {
                                currentHealth = card.GetCardHealth();
                                Debug.Log(currentHealth + " current attack ");
                            }
                            else
                            {
                                currentHealth = 0;
                                Debug.Log(currentHealth + " current attack ");
                                //if (PhotonNetwork.IsMasterClient)
                                //{
                                //    playerController.DestributeGoldAndXPForPlayer(currentCard.transform.parent.GetComponent<PhotonView>(), card.GetComponent<Card>().gold, card.GetComponent<Card>().XP, "master");
                                //}
                                //else
                                //{
                                //    playerController = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
                                //    playerController.DestributeGoldAndXPForPlayer(currentCard.transform.parent.GetComponent<PhotonView>(), card.GetComponent<Card>().gold, card.GetComponent<Card>().XP, "client");
                                //}

                                Debug.Log(currentHealth + " current attack ");
                            }
                            pv.RPC("MalignantOnOthers", RpcTarget.Others, listOfEnemyCardsToBeDamaged[element], currentHealth, destroyed);
                        }
                    }

                }
                else if (cardObj.GetComponent<Summon>())
                {
                    if ((playerField.transform.GetChild(i).tag.Contains("Front Line")))
                    {
                        Summon summon = cardObj.GetComponent<Summon>();
                        Card currentCard = summon.GetComponent<Card>();
                        CardDetails bigCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                        int parentId = int.Parse(playerField.transform.GetChild(i).name.Split(" ")[2]);
                        Debug.Log(summon + " Summon called");
                        List<int> playerFieldList = summon.GetPosition(playerField);
                        //Debug.Log(playerFieldList.Count + "  player field list ");
                        if (playerFieldList.Count > 0)
                        {
                            for (int element = 0; element < playerFieldList.Count; element++)
                            {
                                GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", playerField.transform.GetChild(playerFieldList[element] - 1).position, playerField.transform.GetChild(playerFieldList[element] - 1).rotation);
                                miniCardParent.transform.SetParent(playerField.transform.GetChild(playerFieldList[element] - 1));
                                miniCardParent.transform.localScale = playerField.transform.GetChild(playerFieldList[element] - 1).localScale;
                                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                                miniCard.SetMiniCard(bigCard.id, bigCard.ergoTokenId, bigCard.ergoTokenAmount, bigCard.cardName, bigCard.attack, bigCard.HP, bigCard.gold, bigCard.XP, bigCard.cardImage, CardAbility.None
                        //, sortedList[i].requirements, sortedList[i].abilityLevel
                        );
                                miniCardParent.AddComponent<DragFieldCard>();
                            }
                            string playerFieldListString = ConvertListToString(playerFieldList);
                            pv.RPC("SpawnSummonCardsToOthers", RpcTarget.Others, playerFieldListString, parentId);
                        }

                    }
                }
                else if (cardObj.GetComponent<Serenity>())
                {
                    //if ((playerField.transform.GetChild(i).tag.Contains("Front Line")))
                    //{
                    Serenity serenity = cardObj.GetComponent<Serenity>();
                    Card currentCard = serenity.GetComponent<Card>();
                    CardDetails bigCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                    int parentId = int.Parse(playerField.transform.GetChild(i).name.Split(" ")[2]);
                    Debug.Log(serenity + " Serenity called");


                    List<int> playerFieldList = CardDataBase.instance.GetSurroundingPositions(parentId);
                    //Debug.Log(string.Join(", ", playerFieldList) + "  player field list ");

                    serenity.UseSerenityAbility(playerFieldList, playerField, pv);
                    //}
                }
                else if (cardObj.GetComponent<Mason>())
                {
                    //if (playerField.transform.GetChild(i).tag.Contains("Front Line"))
                    //{
                    //GameObject playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
                    Mason mason = cardObj.GetComponent<Mason>();
                    Card currentCard = mason.GetComponent<Card>();
                    Debug.Log(mason + " Mason called");
                    CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                    mason.OnSetAndActiveHealWall(playerWall, pv);
                    //}
                }
                else if (cardObj.GetComponent<EndGame>())
                {
                    //Debug.Log(" inside card end gamje " + cardObj.name);
                    EndGame endGame = cardObj.GetComponent<EndGame>();
                    Card currentCard = endGame.GetComponent<Card>();
                    Debug.Log(endGame + " endGame called");
                    Tuple<int, int> result = endGame.SetAttackAndHealth(currentCard);
                    int attackValue = result.Item1;
                    int healthValue = result.Item2;
                    string parentId = endGame.transform.parent.parent.name.Split(" ")[2];
                    //Debug.Log(attackValue + " attack value " + healthValue + " health value ");
                    pv.RPC("EndGameInOthers", RpcTarget.Others, attackValue, healthValue, parentId);
                }
                else if (cardObj.GetComponent<Silence>())
                {
                    Debug.Log(" inside card end gamje " + cardObj.name);
                    Silence silence = cardObj.GetComponent<Silence>();
                    Card currentCard = silence.GetComponent<Card>();
                    Debug.Log(endGame + " silence called");
                    //Tuple<int, int> result = endGame.SetAttackAndHealth(currentCard);
                    //int attackValue = result.Item1;
                    //int healthValue = result.Item2;
                    //string parentId = endGame.transform.parent.parent.name.Split(" ")[2];
                    ////Debug.Log(attackValue + " attack value " + healthValue + " health value ");
                    //pv.RPC("EndGameInOthers", RpcTarget.Others, attackValue, healthValue, parentId);
                }
            }
        }
    }

    private void UpdateGoldUI()
    {
        //Debug.Log("UpdateGoldUI called " + PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            int masterGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            Gold.instance.SetGold(masterGold);
            //Debug.Log(Gold.instance.gameObject.transform.parent.parent.name + " parent ");
        }
        else
        {
            int clientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            Gold.instance.SetGold(clientGold);
            //Debug.Log(Gold.instance.gameObject.transform.parent.parent.name + " parent ");
        }
    }

    public void MovePlayer()
    {
        //Debug.Log(" move player called");
        //Debug.Log(player1Turn + " player 1 turn " + PhotonNetwork.IsMasterClient + " is master ");
        playerParent = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>().playerParent;
        if (!player1Turn && PhotonNetwork.IsMasterClient)
        {
            GameObject selectedNPC = null;
            int parentId = -1;

            //Debug.Log(player1Turn + " player 1 turn " + PhotonNetwork.IsMasterClient + " master client");

            for (int i = 0; i < playerParent.Count; i++)
            {
                if (playerParent[i].transform.childCount == 1)
                {
                    selectedNPC = playerParent[i].transform.GetChild(0).gameObject;
                    parentId = i;
                    break;
                }
            }

            if (selectedNPC != null)
            {
                //Debug.Log("selected npc null " + selectedNPC);
                if (parentId < 4)
                {
                    //Debug.Log(" parent id < 4 " + parentId);
                    StartCoroutine(MoveObject(selectedNPC, playerParent[parentId + 1].transform.gameObject, 0.8f));
                    pv.RPC("MoveOther", RpcTarget.Others, player1Turn, parentId);
                }

                if (parentId == 3)
                {
                    StartCoroutine(SetTheNPC(1.5f, true, selectedNPC));
                    //Debug.Log(" parent id = 4 " + parentId);
                    //int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                    //int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
                    //totalPlayerGold += selectedNPC.GetComponent<NPCManager>().gold;
                    //Debug.Log(totalPlayerGold + " total player gold ");
                    //totalMasterXP += selectedNPC.GetComponent<NPCManager>().XP;
                    //PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalPlayerGold;
                    //PhotonNetwork.CurrentRoom.CustomProperties["masterXP"] = totalMasterXP;
                    //Gold.instance.SetGold(totalPlayerGold);
                    //Destroy(selectedNPC.gameObject);
                    //pv.RPC("DestroyOther", RpcTarget.Others, 4);
                }
            }

        }
        else if (player1Turn && !PhotonNetwork.IsMasterClient)
        {
            GameObject selectedNPC = null;
            int parentId = -1;

            //Debug.Log("!player1Turn " + player1Turn + " !PhotonNetwork.IsMasterClient " + PhotonNetwork.IsMasterClient);

            for (int i = 0; i < playerParent.Count; i++)
            {
                if (playerParent[i].transform.childCount == 1)
                {
                    selectedNPC = playerParent[i].transform.GetChild(0).gameObject;
                    parentId = i;
                    break;
                }
            }
            if (selectedNPC != null)
            {
                //Debug.Log("selectedNPC " + selectedNPC);
                if (parentId < 4)
                {
                    //Debug.Log(" parent id < 4 " + parentId);
                    StartCoroutine(MoveObject(selectedNPC, playerParent[parentId + 1].transform.gameObject, 0.8f));
                    pv.RPC("MoveOther", RpcTarget.Others, player1Turn, parentId);

                    //if (parentId == 3)
                    //{
                    //    Debug.Log(" parentId == 4 " + parentId);
                    //    int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    //    int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
                    //    totalClientGold += selectedNPC.GetComponent<NPCManager>().gold;
                    //    PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
                    //    PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
                    //    Debug.Log(totalClientGold + " total client gold ");
                    //    Gold.instance.SetGold(totalClientGold);
                    //    Destroy(selectedNPC.gameObject);
                    //    pv.RPC("DestroyOther", RpcTarget.Others, 4);
                    //}
                }

                if (parentId == 3)
                {
                    StartCoroutine(SetTheNPC(1.5f, false, selectedNPC));
                    //Debug.Log(" parentId == 4 " + parentId);
                    //int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    //int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
                    //totalClientGold += selectedNPC.GetComponent<NPCManager>().gold;
                    //PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
                    //PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
                    //Debug.Log(totalClientGold + " total client gold ");
                    //Gold.instance.SetGold(totalClientGold);
                    //Destroy(selectedNPC.gameObject);
                    //pv.RPC("DestroyOther", RpcTarget.Others, 3);
                }
            }
        }
    }

    IEnumerator SetTheNPC(float time, bool isMaster, GameObject selectedNPC)
    {
        manager = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        yield return new WaitForSeconds(time);
        //Debug.Log("==> set the NPC " + isMaster + " isMaster " + gameObject.name + " name");
        if (isMaster && !gameObject.name.ToLower().Contains("clone"))
        {
            //Debug.Log(" parent id = 4 " + parentId);
            int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
            //Debug.Log(totalPlayerGold + " ==> before gold " + selectedNPC.GetComponent<NPCManager>().gold + " NPC gold");
            totalPlayerGold += (2 * selectedNPC.GetComponent<NPCManager>().gold);
            //Debug.Log(totalPlayerGold + "==> total player gold after");
            totalMasterXP += selectedNPC.GetComponent<NPCManager>().XP;
            PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalPlayerGold;
            PhotonNetwork.CurrentRoom.CustomProperties["masterXP"] = totalMasterXP;
            Gold.instance.SetGold(totalPlayerGold);
            Destroy(selectedNPC.gameObject);
            manager.isDestroyedMaster = true;
            //Debug.Log("isDestroyedMaster " + manager.isDestroyedMaster + " gameobject name " + gameObject.name);
            pv.RPC("DestroyOther", RpcTarget.Others, 4);
        }
        else if (!isMaster && !gameObject.name.ToLower().Contains("clone"))
        {
            //Debug.Log(" parentId == 4 " + parentId);
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
            //Debug.Log(totalClientGold + " ==> before gold " + selectedNPC.GetComponent<NPCManager>().gold + " NPC gold");
            totalClientGold += (2 * selectedNPC.GetComponent<NPCManager>().gold);
            PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
            PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
            //Debug.Log(totalClientGold + " ==> total client gold ");
            Gold.instance.SetGold(totalClientGold);
            Destroy(selectedNPC.gameObject);
            manager.isDestroyedclient = true;
            //Debug.Log("isDestroyedclient " + manager.isDestroyedclient + " gameobject name " + gameObject.name);
            pv.RPC("DestroyOther", RpcTarget.Others, 4);
        }
    }

    [PunRPC]
    public void DestroyOther(int id)
    {
        enemyParent.Clear();
        for (int i = 0; i < gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Enemy Parent").transform.childCount; i++)
        {
            enemyParent.Add(gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Enemy Parent").transform.GetChild(i).gameObject);
        }

        //Debug.Log(id + " parent id " + id + " Destroy other called " + gameObject.name + " enemy parent count " + enemyParent.Count);
        //Destroy(enemyParent[id].transform.GetChild(0).gameObject);
        for (int i = 0; i < enemyParent.Count; i++)
        {
            //Debug.Log("enemy parent name " + enemyParent[i].transform.parent + " gameboard name " + enemyParent[i].transform.parent.parent.parent.name);

            //Debug.Log("child count " + enemyParent[i].transform.childCount);
            if (enemyParent[i].transform.childCount > 0)
            {
                foreach (Transform transform in enemyParent[i].transform)
                {
                    Debug.Log(transform.name + " transform name");
                    Destroy(transform.gameObject);
                }
            }
        }
    }

    [PunRPC]
    private void MoveOther(bool playerTurn, int parentId)
    {
        enemyParent.Clear();
        for (int i = 0; i < gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Enemy Parent").transform.childCount; i++)
        {
            enemyParent.Add(gameBoardParent.transform.GetChild(1).GetChild(0).Find("NPC Enemy Parent").transform.GetChild(i).gameObject);
        }
        Debug.Log("enemy parent " + enemyParent.Count + " parent id " + parentId + " player turn " + playerTurn);

        if (playerTurn)
        {
            GameObject selectedNPC = null;
            Debug.Log("playerTurn " + playerTurn);
            selectedNPC = enemyParent[parentId].transform.GetChild(0).gameObject;
            //int parentId = -1;
            //for (int i = 0; i < enemyParent.Length; i++)
            //{
            //    if (enemyParent[i].transform.childCount == 1)
            //    {
            //        selectedNPC = enemyParent[parentId - 1].transform.GetChild(0).gameObject;
            //        parentId = i;
            //        break;
            //    }
            //}
            if (parentId < 4)
            {
                Debug.Log(" player id other parentId < 4 " + parentId);
                StartCoroutine(MoveObject(selectedNPC, enemyParent[parentId + 1].transform.gameObject, 0.8f));
            }
        }
        else
        {
            GameObject selectedNPC = null;
            selectedNPC = enemyParent[parentId].transform.GetChild(0).gameObject;
            //int parentId = -1;
            //for (int i = 0; i < enemyParent.Length; i++)
            //{
            //    if (enemyParent[i].transform.childCount == 1)
            //    {
            //        selectedNPC = enemyParent[parentId - 1].transform.GetChild(0).gameObject;
            //        parentId = i;
            //        break;
            //    }
            //}
            if (parentId < 4)
            {
                Debug.Log(" player id other parentId < 4 " + parentId);
                StartCoroutine(MoveObject(selectedNPC, enemyParent[parentId + 1].transform.gameObject, 0.8f));
            }
        }
        //if (player1Turn && PhotonNetwork.IsMasterClient)
        //{
        //    GameObject selectedNPC = null;

        //    for (int i = 0; i < playerParent.Length; i++)
        //    {
        //        if (playerParent[i].transform.childCount == 1)
        //        {
        //            selectedNPC = playerParent[i].transform.GetChild(0).gameObject;
        //            break;
        //        }
        //    }
        //    StartCoroutine(MoveObject(selectedNPC, selectedNPC.transform.gameObject, 1f));
        //    pv.RPC("MoveOther", RpcTarget.Others);
        //}
        //else if (!player1Turn && !PhotonNetwork.IsMasterClient)
        //{
        //    GameObject selectedNPC = null;

        //    for (int i = 0; i < playerParent.Length; i++)
        //    {
        //        if (playerParent[i].transform.childCount == 1)
        //        {
        //            selectedNPC = playerParent[i].transform.GetChild(0).gameObject;
        //            break;
        //        }
        //    }
        //    StartCoroutine(MoveObject(selectedNPC, selectedNPC.transform.gameObject, 1f));
        //    pv.RPC("MoveOther", RpcTarget.Others);
        //}
    }

    IEnumerator MoveObject(GameObject objectToMove, GameObject destination, float duration)
    {
        Debug.Log("move object " + objectToMove + " object to move " + destination + " destination " + duration + " duration");
        float time = 0;
        if (objectToMove == null) yield break;
        Vector3 start = objectToMove.transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            objectToMove.transform.position = Vector3.Lerp(start, destination.transform.position, time / duration);
            yield return null;
        }

        objectToMove?.transform.SetParent(destination.transform);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.Log("player 1 onleft player " + gameObject.name + " master or client " + PhotonNetwork.IsMasterClient);
        loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
        //Debug.Log("is match not loaded " + isMatchNotLoaded + " name " + gameObject.name + " master or client " + PhotonNetwork.IsMasterClient);
        if (gameObject.name.ToLower().Contains("clone")) return;
        if (!endGame && !loadingPanel.activeSelf && !isMatchNotLoaded)
        {
            //leftPlayerPanel.SetActive(true);
            //leftPlayerText.SetText(otherPlayer.NickName + " Was left the game. Press Continue to Go Skirmish screen.");
            //pv.RPC("CalculateLeftWinner", RpcTarget.All);
            completeGame = true;
            //Debug.Log(" set the true value left game rpc " + completeGame);
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
            int winnerTurnCount = 0, loserTurnCount = 0;
            string winnerDeck = "unKnown", loserDeck = "unKnown";
            string matchId = "ABCD";


            //Debug.LogError("leave player " + leavePlayer);
            if (leaveBtn)
            {
                //Debug.Log(leaveBtn + " leave button");
                leaveBtn = false;
                if (leavePlayer == "master")
                {
                    //Debug.Log("master leave ");
                    totalTurnText.SetText(turnCounter.ToString());
                    clientPlayerXP += 100;
                    experienceText.SetText(clientPlayerXP.ToString());
                    //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                    xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                }
                else if (leavePlayer == "client")
                {
                    //Debug.Log(" client leave ");
                    totalTurnText.SetText(turnCounter.ToString());
                    masterPlayerXP += 100;
                    experienceText.SetText(masterPlayerXP.ToString());
                    //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                    xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);

                }
            }
            else
            {
                //Debug.Log(leaveBtn + " leave button");
                if (PhotonNetwork.IsMasterClient)
                {
                    //Debug.Log(" master remaining ");
                    totalTurnText.SetText(turnCounter.ToString());
                    masterPlayerXP += 100;
                    experienceText.SetText(masterPlayerXP.ToString());
                    //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                    xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                }
                else
                {
                    //Debug.Log("client remaining");
                    totalTurnText.SetText(turnCounter.ToString());
                    clientPlayerXP += 100;
                    experienceText.SetText(clientPlayerXP.ToString());
                    //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                    //Debug.Log(MainMenuUIManager.instance.currentUserXP + " current xp " + MainMenuUIManager.instance.maxXPForCurrentLevel + " max xp");
                    xpSlider.value = ((float)MainMenuUIManager.instance.currentUserXP / (float)MainMenuUIManager.instance.maxXPForCurrentLevel);
                }
            }


            //Debug.LogError(" main menu " + mainMenu);
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

            //Debug.Log(" leave player name " + leavePlayer);
            //if (leavePlayer == "client")
            //Debug.Log("remaining player " + PhotonNetwork.IsMasterClient + " identifiedPlayerIsMaster " + identifiedPlayerIsMaster);
            if (PhotonNetwork.IsMasterClient && identifiedPlayerIsMaster)
            {
                //Debug.Log("master remainig " + PhotonNetwork.LocalPlayer.NickName + " identifiedPlayerIsMaster " + identifiedPlayerIsMaster);
                //int playerFieldId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                //Player currPlayer = PhotonNetwork.LocalPlayer;
                //Player nextPlayer = currPlayer.GetNext();
                //int opponentFieldId = (int)nextPlayer.CustomProperties["deckId"];

                //masterDeckGeneral = GetPlayerDeck(playerFieldId);
                //clientDeckGeneral = GetPlayerDeck(opponentFieldId);
                //Debug.Log(masterDeckGeneral + " masterDeckGeneral" + clientDeckGeneral + " clientDeckGeneral");

                //Debug.Log(masterID + " master id " + clientId + " client id " + masterPlayerXP + " master player xp " + clientPlayerXP + " client player xp " + masterDeck + " master deck " + clientDeck + " client deck ");
                winnerId = masterID;
                loserId = clientId;
                winnerXP = masterPlayerXP;
                loserXP = clientPlayerXP;
                winnerMmrChange = masterMmr;
                loserMmrChange = clientMmr;
                winnerDeck = masterDeckGeneral;
                loserDeck = clientDeckGeneral;

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = winnerPlayerTurn;
                loserTurnCount = loserPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);
                //winnerTurnCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalMasterTurn"];
                //loserTurnCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalClientTurn"];

                //Debug.Log("leave player name " + leavePlayer + " winnerId " + winnerId + " loser id " + loserId + " winnerxp " + winnerXP + " loserxp " + loserXP + " winner mmr " + winnerMmrChange + " loser mmr " + loserMmrChange + " winner deck " + winnerDeck + " loser deck " + loserDeck + " status " + status + " winner turn count " + winnerTurnCount + " loser turn count ");

                //Debug.Log("winnerDeck " + winnerDeck + " loserDeck " + loserDeck + " totalSeconds " + totalSeconds + " winnerTurnCount " + winnerTurnCount + " loserTurnCount " + loserTurnCount);

            }
            else if (PhotonNetwork.IsMasterClient && !identifiedPlayerIsMaster)
            {
                //Debug.Log("master remainig " + PhotonNetwork.LocalPlayer.NickName + " identifiedPlayerIsMaster " + identifiedPlayerIsMaster);
                //int playerFieldId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"];
                //Player currPlayer = PhotonNetwork.LocalPlayer;
                //Player nextPlayer = currPlayer.GetNext();
                //int opponentFieldId = (int)nextPlayer.CustomProperties["deckId"];

                //masterDeckGeneral = GetPlayerDeck(playerFieldId);
                //clientDeckGeneral = GetPlayerDeck(opponentFieldId);
                //Debug.Log(masterDeckGeneral + " masterDeckGeneral" + clientDeckGeneral + " clientDeckGeneral");

                //Debug.Log(masterID + " master id " + clientId + " client id " + masterPlayerXP + " master player xp " + clientPlayerXP + " client player xp " + masterDeck + " master deck " + clientDeck + " client deck ");
                winnerId = clientId;
                loserId = masterID;
                winnerXP = clientPlayerXP;
                loserXP = masterPlayerXP;
                winnerMmrChange = clientMmr;
                loserMmrChange = masterMmr;
                winnerDeck = clientDeckGeneral;
                loserDeck = masterDeckGeneral;

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item2;
                int loserPlayerTurn = result.Item1;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = winnerPlayerTurn;
                loserTurnCount = loserPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);
                //winnerTurnCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalMasterTurn"];
                //loserTurnCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalClientTurn"];

                //Debug.Log("leave player name " + leavePlayer + " winnerId " + winnerId + " loser id " + loserId + " winnerxp " + winnerXP + " loserxp " + loserXP + " winner mmr " + winnerMmrChange + " loser mmr " + loserMmrChange + " winner deck " + winnerDeck + " loser deck " + loserDeck + " status " + status + " winner turn count " + winnerTurnCount + " loser turn count ");

                //Debug.Log("winnerDeck " + winnerDeck + " loserDeck " + loserDeck + " totalSeconds " + totalSeconds + " winnerTurnCount " + winnerTurnCount + " loserTurnCount " + loserTurnCount);

            }
            else if (!PhotonNetwork.IsMasterClient)
            {

                //Debug.Log(masterDeckGeneral + " masterDeckGeneral" + clientDeckGeneral + " clientDeckGeneral");

                //Debug.Log(masterID + " master id " + clientId + " client id " + masterPlayerXP + " master player xp " + clientPlayerXP + " client player xp " + masterDeck + " master deck " + clientDeck + " client deck ");
                winnerId = clientId;
                loserId = masterID;
                winnerXP = clientPlayerXP;
                loserXP = masterPlayerXP;
                winnerMmrChange = clientMmr;
                loserMmrChange = masterMmr;
                winnerDeck = clientDeckGeneral;
                loserDeck = masterDeckGeneral;

                randomPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["randomPlayer"];
                //Debug.Log(turnCounter + " turn counter " + randomPlayer + " random player");
                Tuple<int, int> result = GetTurnCount(turnCounter, randomPlayer);
                int winnerPlayerTurn = result.Item1;
                int loserPlayerTurn = result.Item2;
                //Debug.Log(winnerPlayerTurn + " winnerPlayerTurn " + loserPlayerTurn + " loserPlayerTurn");

                winnerTurnCount = loserPlayerTurn;
                loserTurnCount = winnerPlayerTurn;
                //Debug.Log(" winner turn count " + winnerTurnCount + " loser turn count " + loserTurnCount);
                //winnerTurnCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalClientTurn"];
                //loserTurnCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["totalMasterTurn"];

                //Debug.Log("leave player name " + leavePlayer + " winnerId " + winnerId + " loser id " + loserId + " winnerxp " + winnerXP + " loserxp " + loserXP + " winner mmr " + winnerMmrChange + " loser mmr " + loserMmrChange + " winner deck " + winnerDeck + " loser deck " + loserDeck + " status " + status + " winner turn count " + winnerTurnCount + " loser turn count ");

            }
            //Debug.LogError(" leave player name " + leavePlayer);
            //Debug.Log(winnerId + " winner id");
            //Debug.Log(loserId + " loser id");
            //Debug.Log(winnerDeck + " winner deck");
            //Debug.Log(loserDeck + " loser deck");
            //Debug.Log(totalSeconds + " seconds");
            //Debug.Log(winnerTurnCount + " winner turn count");
            //Debug.Log(loserTurnCount + " loser turn count");
            //Debug.Log("normal " + " status");
            //Debug.Log(DatabaseIntegration.instance + " instance ");
            StartCoroutine(DatabaseIntegration.instance.MatchDataUpdates(winnerId, loserId, winnerDeck, loserDeck, totalSeconds, winnerTurnCount, loserTurnCount, "normal"));

            PhotonNetwork.AutomaticallySyncScene = false;
            endGame = true;
            StopTimers();
            StopAllCoroutines();
        }
        else if (endGame)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            //Debug.LogError("end game true " + endGame);
            completeGame = true;
            resultPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Result panel").gameObject;
            //Debug.LogError("resultPanel " + resultPanel);
            //Debug.LogError("resultPanel.transform.GetChild(0) " + resultPanel.transform.GetChild(0));
            //Debug.LogError("resultPanel.transform.GetChild(0).Find(\"Main Menu\") " + resultPanel.transform.GetChild(0).Find("Main Menu"));
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
        //Debug.LogError(fieldName + " field name");
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
        //Debug.LogError(name + " General name");
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
        //Debug.Log("leave player called");
        PhotonNetwork.AutomaticallySyncScene = false;
        completeGame = true;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //connectUsing = true;
            leavePlayer = "master";
            //Debug.LogError(leavePlayer + " leave player master ");
            Player foundPlayer = null;
            string clientName = PhotonNetwork.LocalPlayer.GetNext().NickName;
            bool playerFound = FindPlayerByNickname(clientName, out foundPlayer);
            leaveBtn = true;
            pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer, true);
            if (playerFound)
            {
                //pv.RPC("ChangeMaster", RpcTarget.All, true);
                Player targetPlayer = foundPlayer;
                //PhotonNetwork.SetMasterClient(targetPlayer);
                //Debug.Log("Found player name: " + targetPlayer);
                //Debug.Log(PhotonNetwork.LocalPlayer.IsMasterClient + " previous master " + pv + " pv " + pv.gameObject.name + " gameobject name " + gameObject.name);
                //Debug.Log(PhotonNetwork.LocalPlayer.GetNext().IsMasterClient + " current master");
            }
            else
            {
                Debug.Log("Player with nickname '" + clientName + "' not found. "
                    + pv + " pv " + pv.gameObject.name + " gameobject name " + gameObject.name);
                //pv.RPC("ChangeMaster", RpcTarget.All, false);
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
            //Debug.LogError(pv + " pv " + pv.IsMine + " pv " + pv.gameObject.name + " gameobject name " + gameObject.name);
            //Debug.LogError("Leave player called " + PhotonNetwork.LocalPlayer.NickName + " mine " + pv.IsMine);
            if (pv.IsMine)
            {
                leavePlayer = "client";
                //Debug.LogError(leavePlayer + " leave player client ");
                //Debug.Log(pv + " photon view");
                //Debug.Log(photonView + " photon view");
                //Debug.Log("leave called");
                leaveBtn = true;
                pv.RPC("MasterCall", RpcTarget.MasterClient);
                pv.RPC("SetLeavePlayer", RpcTarget.All, leavePlayer, leaveBtn);
                //pv.RPC("ChangeMaster", RpcTarget.All, false);
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

    //[PunRPC]
    //private void ChangeMaster(bool masterValue)
    //{
    //    Debug.Log("ChangeMaster called " + masterValue);
    //    changedMaster = masterValue;
    //    Debug.Log("ChangeMaster called after" + masterValue);
    //}

    [PunRPC]
    public void MasterCall()
    {
        //Debug.Log(PhotonNetwork.IsMasterClient + " master or not ");
        pv.RPC("KickPlayerRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void KickPlayerRPC()
    {
        //Debug.Log("KickPlayerRPC " + PhotonNetwork.IsMasterClient + " master or not ");
        //connectUsing = true;
        //SceneManager.LoadScene(1);
        isCompleted = true;
        PhotonView view = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        PhotonTransformView transformView = gameBoardParent.transform.GetChild(1).GetComponent<PhotonTransformView>();
        //Debug.Log(view + " view " + transformView + " transform view ");
        view.enabled = false;
        transformView.enabled = false;
        Destroy(view);
        //Debug.Log(view.enabled + "  view " + transformView.enabled + " tra view");
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
        //Debug.LogError(" leave palyer " + leavePlayer + " player name " + playerName);
    }

    public void LeaveRemainigPlayer()
    {
        //Debug.LogError("called Remaining player " + PhotonNetwork.IsMasterClient);
        PhotonNetwork.AutomaticallySyncScene = false;
        completeGame = true;
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
        //Debug.LogError("called in both  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
        status = MatchStatus.Normal;
        completeGame = true;
        //SkirmishManager.instance.deckId = -1;
        connectUsing = true;
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
        //Debug.Log("loading panel " + loadingPanel);
        completeGame = true;
        if (loadingPanel.activeSelf)
        {
            //Debug.Log("inside loading panel " + loadingPanel.activeSelf);
            pv.RPC("MatchNotLoaded", RpcTarget.All);
        }
        //Debug.LogError("called in both  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
    }

    [PunRPC]
    private void MatchNotLoaded()
    {
        //Debug.Log("match not loaded in both");
        connectUsing = true;
        //Debug.Log("match not loaded value " + isMatchNotLoaded);
        isMatchNotLoaded = true;
        //Debug.Log("match not loaded value " + isMatchNotLoaded);
        //Debug.Log(" match not loaded");
        status = MatchStatus.Unknown;
        completeGame = true;
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

    public static Tuple<int, int> GetTotalCardsCount(GameObject hand, GameObject field)
    {
        int handCount = 0;
        int fieldCount = 0;
        for (int i = 0; i < hand.transform.childCount; i++)
        {
            if (hand.transform.GetChild(i).childCount == 1)
            {
                handCount++;
            }
        }

        for (int i = 0; i < field.transform.childCount; i++)
        {
            if (field.transform.GetChild(i).childCount == 1)
            {
                fieldCount++;
            }
        }
        return new Tuple<int, int>(handCount, fieldCount);
    }

    public static Tuple<int, int, string, string> GetDestroyDataCount(GameObject playerData, GameObject enemyData)
    {
        int playerCount = 0;
        int enemyCount = 0;
        List<int> playerListId = new();
        List<int> enemyListId = new();
        for (int i = 0; i < playerData.transform.childCount; i++)
        {
            if (playerData.transform.GetChild(i).childCount == 1)
            {
                playerCount++;
                if (playerData.transform.GetChild(i).GetChild(0).GetChild(0) != null && playerData.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>() != null)
                {
                    playerListId.Add(playerData.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().id);
                }
            }
            else
            {
                playerListId.Add(0);
            }
        }

        for (int i = 0; i < enemyData.transform.childCount; i++)
        {
            if (enemyData.transform.GetChild(i).childCount == 1)
            {
                enemyCount++;
                if (enemyData.transform.GetChild(i).GetChild(0).GetChild(0) != null && enemyData.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>() != null)
                {
                    enemyListId.Add(enemyData.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().id);
                }
            }
            else
            {
                enemyListId.Add(0);
            }
        }

        //for (int i = 0; i < playerListId.Count; i++)
        //{
        //    Debug.Log(playerListId[i] + " <== player id ");
        //}

        //for (int i = 0; i < enemyListId.Count; i++)
        //{
        //    Debug.Log(enemyListId[i] + " <== enemy id ");
        //}

        string jsonStringPlayer = Newtonsoft.Json.JsonConvert.SerializeObject(playerListId);

        string jsonStringEnenmy = Newtonsoft.Json.JsonConvert.SerializeObject(enemyListId);

        //Debug.Log(jsonStringPlayer + " player " + jsonStringEnenmy + " jsonStringEnenmy");

        return new Tuple<int, int, string, string>(playerCount, enemyCount, jsonStringPlayer, jsonStringEnenmy);
    }

    private string GetPlayerDeck(int id)
    {
        string playerDeck = "unKnown";
        //Debug.Log(id + " id value");
        switch (id)
        {
            case 1:
                playerDeck = "Masquerades";
                break;
            case 2:
                playerDeck = "The Old Kingdom";
                break;
            case 3:
                playerDeck = "Fairytales";
                break;
            case 4:
                playerDeck = "Dark Matter";
                break;
            case 5:
                playerDeck = "Tinkerers";
                break;
            default:
                playerDeck = "unKnown";
                break;
        }
        //Debug.Log(playerDeck + " playerDeck value");
        return playerDeck;
    }

    private Tuple<int, int> GetTurnCount(int totalTurn, int randomId)
    {
        int masterCount = 0, clientCount = 0;
        if (totalTurn % 2 == 0)
        {
            //Debug.Log(totalTurn + " total turn " + " totalTurn % 2 == 0");
            masterCount = totalTurn / 2;
            clientCount = totalTurn / 2;
            //Debug.Log(masterCount + " master count " + clientCount + " client count");
        }
        else if (totalTurn % 2 == 1)
        {
            //Debug.Log("totalTurn % 2 == 1");
            if (randomId == 1)
            {
                //Debug.Log("randomId == 1 " + randomId);
                masterCount = (totalTurn / 2) + 1;
                clientCount = totalTurn / 2;
                //Debug.Log(masterCount + " master count " + clientCount + " client count");
            }
            else if (randomId == 2)
            {
                //Debug.Log("randomId == 2 " + randomId);
                masterCount = totalTurn / 2;
                clientCount = (totalTurn / 2) + 1;
                //Debug.Log(masterCount + " master count " + clientCount + " client count");
            }
        }
        //Debug.Log(masterCount + " final master count " + clientCount + " final client count");
        return new Tuple<int, int>(masterCount, clientCount);
    }

    private bool IsGoaded(GameObject enemyfield)
    {
        //Debug.Log(enemyfield + " enemy field " + " is goaded");
        bool goaded = false;
        for (int i = 0; i < enemyfield.transform.childCount; i++)
        {
            if (enemyfield.transform.GetChild(i).tag.Contains("Front Line"))
            {
                if (enemyfield.transform.GetChild(i).childCount == 1)
                {
                    if (enemyfield.transform.GetChild(i).GetChild(0).childCount == 1)
                    {
                        if (enemyfield.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability == CardAbility.Goad)
                        {
                            goaded = true;
                        }
                    }
                }
            }
        }
        return goaded;
    }

    public bool IsSatisfyRequirements(Card card, GameObject playerField, GameObject position, bool commingFromDrag)
    {
        FieldManager.instance.ResetCounters();
        FieldManager.instance.CalculateAbilityCounter(playerField);
        //int totalCardsForThisAbility = GetTotalAbilityCards(card, playerField);
        int totalCardsForThisAbility = FieldManager.instance.GetAbilityCounter(card.ability);
        //Debug.Log(totalCardsForThisAbility + " total card ability " + card.ability + " position name " + position.transform?.parent.name);
        if (position.transform?.parent.name != "Player Field" && commingFromDrag) return false;

        if (CardDataBase.instance.requirements.ContainsKey(card.ability))
        {
            //Debug.Log("inside CardDataBase.instance.requirements.ContainsKey(card.ability)");

            if (TryGetCardRequirements(card.ability, out var cardRequirements))
            {
                //Debug.Log($"GoodFavor fieldLimit: {cardRequirements.limit}, fieldPosition: {cardRequirements.position}");

                if ((int)cardRequirements.limit > totalCardsForThisAbility)
                {
                    if (cardRequirements.position == FieldPosition.FrontLine && position.tag.ToLower().Contains("Front Line".ToLower()))
                    {
                        Debug.Log("position is at FrontLine");
                        return true;
                    }
                    else if (cardRequirements.position == FieldPosition.None)
                    {
                        Debug.Log("Field position is None");
                        return true;
                    }
                    else
                    {
                        Debug.Log("Position does not meet field position requirements");
                        return false;
                    }
                }
                else
                {
                    Debug.Log("Field limit exceeded");
                    return false;
                }
            }
            else
            {
                Debug.Log("!TryGetCardRequirements(card.ability, out var cardRequirements)");
                return false;
            }


            //if (TryGetCardRequirements(card.ability, out var cardRequirements))
            //{
            //    Debug.Log($"GoodFavor fieldLimit: {cardRequirements.limit}, fieldPosition: {cardRequirements.position}");
            //    if((int)cardRequirements.limit == (int)FieldLimit.One && totalCardsForThisAbility < 1)
            //    {
            //        if(cardRequirements.position == FieldPosition.FrontLine)
            //        {
            //            if (position.tag.ToLower().Contains("Front line".ToLower()))
            //            {
            //                Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
            //                return true;
            //            }
            //            else
            //            {
            //                Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
            //                return false;
            //            }
            //        }
            //        else if(cardRequirements.position == FieldPosition.None)
            //        {
            //            Debug.Log("Field position none");
            //            return true;
            //        }
            //    }
            //    //if (cardRequirements.limit == "Unlimited" && cardRequirements.position == "None")
            //    //{
            //    //    Debug.Log("cardRequirements.fieldLimit == Unlimited && cardRequirements.fieldPosition == None");
            //    //    return true;
            //    //}
            //    //else if (cardRequirements.fieldLimit == "1" && totalCardsForThisAbility < 1 && cardRequirements.fieldPosition == "None")
            //    //{
            //    //    Debug.Log("cardRequirements.fieldLimit == 1 && totalCardsForThisAbility <= 1 && cardRequirements.fieldPosition == None");
            //    //    return true;
            //    //}
            //    //else if (cardRequirements.fieldLimit == "2" && totalCardsForThisAbility < 2 && cardRequirements.fieldPosition == "None")
            //    //{
            //    //    Debug.Log("cardRequirements.fieldLimit == 2 && totalCardsForThisAbility <= 2 && cardRequirements.fieldPosition == None");
            //    //    return true;
            //    //}
            //    //else if (cardRequirements.fieldLimit == "Unlimited" && cardRequirements.fieldPosition == "Frontline")
            //    //{
            //    //    Debug.Log("cardRequirements.fieldLimit == Unlimited && cardRequirements.fieldPosition == Frontline");
            //    //    if (position.tag.ToLower().Contains("Front line".ToLower()))
            //    //    {
            //    //        Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
            //    //        return true;
            //    //    }
            //    //    else
            //    //    {
            //    //        Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
            //    //        return false;
            //    //    }
            //    //}
            //    //else if (cardRequirements.fieldLimit == "1" && totalCardsForThisAbility < 1 && cardRequirements.fieldPosition == "Frontline")
            //    //{
            //    //    Debug.Log("cardRequirements.fieldLimit == 1 && totalCardsForThisAbility <= 1 && cardRequirements.fieldPosition == Frontline");
            //    //    if (position.tag.ToLower().Contains("Front line".ToLower()))
            //    //    {
            //    //        Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
            //    //        return true;
            //    //    }
            //    //    else
            //    //    {
            //    //        Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
            //    //        return false;
            //    //    }
            //    //}
            //    //else if (cardRequirements.fieldLimit == "2" && totalCardsForThisAbility < 2 && cardRequirements.fieldPosition == "Frontline")
            //    //{
            //    //    Debug.Log("cardRequirements.fieldLimit == 2 && totalCardsForThisAbility <= 2 && cardRequirements.fieldPosition == Frontline");
            //    //    if (position.tag.ToLower().Contains("Front line".ToLower()))
            //    //    {
            //    //        Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
            //    //        return true;
            //    //    }
            //    //    else
            //    //    {
            //    //        Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
            //    //        return false;
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    Debug.Log("false in if");
            //    //    return false;
            //    //}
            //}
            //else
            //{
            //    Debug.Log("!TryGetCardRequirements(card.ability, out var cardRequirements)");
            //    return false;
            //}

        }
        else
        {
            Debug.Log("Card ability not found in requirements");
            return false;
        }

        //if (CardDataBase.instance.requirements.ContainsKey(card.ability))
        //{
        //    Debug.Log("inside CardDataBase.instance.requirements.ContainsKey(card.ability)");
        //    if (TryGetCardRequirements(card.ability, out var cardRequirements))
        //    {
        //        Debug.Log($"GoodFavor fieldLimit: {cardRequirements.fieldLimit}, fieldPosition: {cardRequirements.fieldPosition}");
        //        if (cardRequirements.fieldLimit == "Unlimited" && cardRequirements.fieldPosition == "None")
        //        {
        //            Debug.Log("cardRequirements.fieldLimit == Unlimited && cardRequirements.fieldPosition == None");
        //            return true;
        //        }
        //        else if (cardRequirements.fieldLimit == "1" && totalCardsForThisAbility < 1 && cardRequirements.fieldPosition == "None")
        //        {
        //            Debug.Log("cardRequirements.fieldLimit == 1 && totalCardsForThisAbility <= 1 && cardRequirements.fieldPosition == None");
        //            return true;
        //        }
        //        else if (cardRequirements.fieldLimit == "2" && totalCardsForThisAbility < 2 && cardRequirements.fieldPosition == "None")
        //        {
        //            Debug.Log("cardRequirements.fieldLimit == 2 && totalCardsForThisAbility <= 2 && cardRequirements.fieldPosition == None");
        //            return true;
        //        }
        //        else if (cardRequirements.fieldLimit == "Unlimited" && cardRequirements.fieldPosition == "Frontline")
        //        {
        //            Debug.Log("cardRequirements.fieldLimit == Unlimited && cardRequirements.fieldPosition == Frontline");
        //            if (position.tag.ToLower().Contains("Front line".ToLower()))
        //            {
        //                Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
        //                return true;
        //            }
        //            else
        //            {
        //                Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
        //                return false;
        //            }
        //        }
        //        else if (cardRequirements.fieldLimit == "1" && totalCardsForThisAbility < 1 && cardRequirements.fieldPosition == "Frontline")
        //        {
        //            Debug.Log("cardRequirements.fieldLimit == 1 && totalCardsForThisAbility <= 1 && cardRequirements.fieldPosition == Frontline");
        //            if (position.tag.ToLower().Contains("Front line".ToLower()))
        //            {
        //                Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
        //                return true;
        //            }
        //            else
        //            {
        //                Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
        //                return false;
        //            }
        //        }
        //        else if (cardRequirements.fieldLimit == "2" && totalCardsForThisAbility < 2 && cardRequirements.fieldPosition == "Frontline")
        //        {
        //            Debug.Log("cardRequirements.fieldLimit == 2 && totalCardsForThisAbility <= 2 && cardRequirements.fieldPosition == Frontline");
        //            if (position.tag.ToLower().Contains("Front line".ToLower()))
        //            {
        //                Debug.Log("position.tag.ToLower().Contains(Front line.ToLower())");
        //                return true;
        //            }
        //            else
        //            {
        //                Debug.Log("!position.tag.ToLower().Contains(Front line.ToLower())");
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            Debug.Log("false in if");
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("!TryGetCardRequirements(card.ability, out var cardRequirements)");
        //        return false;
        //    }
        //}
        //else
        //{
        //    Debug.Log(" main else");
        //    return false;
        //}

        //int totalValue = 0;
        //for (int i = 0; i < playerField.transform.childCount; i++)
        //{
        //    if (playerField.transform.GetChild(i).childCount == 1)
        //    {
        //        if(card.ability == playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability)
        //        {
        //            if (CardDataBase.instance.requirements.ContainsKey(card.ability))
        //            {
        //                if (TryGetCardRequirements(card.ability, out var cardRequirements))
        //                {

        //                    Debug.Log($"GoodFavor fieldLimit: {cardRequirements.fieldLimit}, fieldPosition: {cardRequirements.fieldPosition}");
        //                }
        //                totalValue++;
        //            }
        //        }
        //        //if(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>().ability == card.ability)
        //        //{

        //        //}
        //    }
        //}
        ////if(card.ability)
        ////return false;
    }

    public bool TryGetCardRequirements(CardAbility ability, out AllField cardRequirements)
    {
        return CardDataBase.instance.requirements.TryGetValue(ability, out cardRequirements);
    }

    //private int GetTotalAbilityCards(Card card, GameObject playerField)
    //{
    //    Debug.Log("GetTotalAbilityCards ");
    //    int totalValue = 0;

    //    for (int i = 0; i < playerField.transform.childCount; i++)
    //    {
    //        if (playerField.transform.GetChild(i).childCount == 1)
    //        {
    //            Card fieldCard = playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>();
    //            Debug.Log(card.name + " card name " + card.id + " id " + fieldCard.name + " field card " + fieldCard.id);
    //            if (card.ability == fieldCard.ability
    //                //&& card.id == fieldCard.id
    //                )
    //            {
    //                Debug.Log("card.ability == fieldCard.ability && card.id == fieldCard.id");
    //                totalValue++;
    //            }

    //        }
    //    }
    //    return totalValue;
    //}

    public void HidePanel(GameObject cardError)
    {
        if (cardError == null || cardError.transform.childCount < 0) return;
        GameObject currentObject = cardError.transform.GetChild(0).gameObject;
        bool currentPanelState = currentObject.activeSelf;

        if (currentPanelState)
        {
            timePanelHasBeenOpen += Time.deltaTime;
            if (timePanelHasBeenOpen >= openTimeThreshold)
            {
                //Debug.Log("Panel has been open for at least 2 seconds!");
            }
        }
        else
        {
            timePanelHasBeenOpen = 0.0f;
        }

        isPanelOpen = currentPanelState;
    }

    public void PlayerTurnEnd()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        GameObject playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
        bool isPlayerWallDestroyed = IsWallDestroyed(playerWall);
        //Debug.Log("player field " + playerField.name + " child count " + playerField.transform.childCount + " (isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2) " + (isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2));
        for (int i = 0; i < (isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2); i++)
        {
            //Debug.Log(" i value " + i + " playerField.transform.GetChild(i).childCount " + playerField.transform.GetChild(i).childCount);
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                //Debug.Log(playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Clone>() + " clone ");
                GameObject cardObj = playerField.transform.GetChild(i).GetChild(0).GetChild(0).gameObject;
                if (cardObj.GetComponent<Card>().ability == CardAbility.None)
                {
                    return;
                }

                if (cardObj.GetComponent<Clone>())
                {
                    Clone clone = cardObj.GetComponent<Clone>();
                    Debug.Log(clone + " clone called");
                    clone.SetRoundNumber(1);
                    if (clone.IsUseAbility())
                    {
                        Card playerCard = cardObj.GetComponent<Card>();
                        CardDetails originalCard = cardDetails.Find(cardId => cardId.id == playerCard.id);
                        string parentId = clone.transform.parent.parent.name.Split(" ")[2];

                        //Debug.Log(playerCard.XP + " player xp " + playerCard.gold + " gold " + playerCard.attack + " attack " + playerCard.HP + " hp");
                        int id = clone.FindThePosition(playerField);
                        if (id == -1) return;
                        GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", playerField.transform.GetChild(id).position, playerField.transform.GetChild(id).rotation);
                        miniCardParent.transform.SetParent(playerField.transform.GetChild(id));
                        miniCardParent.transform.localScale = playerField.transform.GetChild(id).localScale;
                        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                        miniCard.SetMiniCard(originalCard.id, originalCard.ergoTokenId, originalCard.ergoTokenAmount, "Clone", originalCard.attack, originalCard.HP, originalCard.gold, originalCard.XP, originalCard.cardImage, CardAbility.None
                //, sortedList[i].requirements, sortedList[i].abilityLevel
                );
                        miniCardParent.AddComponent<DragFieldCard>();
                        miniCardParent.name = "ClonedCard";
                        miniCard.name = "ClonedCard";
                        pv.RPC("CloneCardOthers", RpcTarget.Others, id, originalCard.id);
                    }
                    //Tuple<int, int, int, int> result = clone.ReplicateCard(playerCard, originalCard);

                    //Debug.Log(playerCard.XP + " player updated xp " + playerCard.gold + " gold " + playerCard.attack + " attack " + playerCard.HP + " hp");

                    //playerCard.SetAttack(result.Item1);
                    //playerCard.SetHP(result.Item2);
                    //playerCard.SetGold(result.Item3);
                    //playerCard.SetXP(result.Item4);


                }

            }
        }
    }

    string ConvertListToString<T>(List<T> list)
    {
        return string.Join(",", list);
    }

    List<T> ConvertStringListTo<T>(string listAsString)
    {
        string[] elements = listAsString.Split(',');

        List<T> newList = new List<T>();
        foreach (string element in elements)
        {
            T convertedElement = (T)Convert.ChangeType(element, typeof(T));
            newList.Add(convertedElement);
        }

        return newList;
    }

    public void OnSetCard(Card card, bool isMaster, Transform parent)
    {
        Debug.Log(" on set card method called " + card + "   card " + isMaster + " ismaster " + parent + " parent");
        pv = gameBoardParent.transform.GetChild(1).GetComponent<PhotonView>();
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        enemyWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Wall").gameObject;
        playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
        Debug.Log(parent.transform.GetChild(0).GetChild(0).GetComponent<Card>() + " parent.transform.GetChild(0).GetChild(0).GetComponent<Card>() ");
        card = parent.transform.GetChild(0).GetChild(0).GetComponent<Card>();
        //Debug.Log(card.name + " card name");
        ResetSelectedCards();

        if (card.GetComponent<Repair>())
        {
            Debug.Log(" inside card repair " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Debug.Log("front line");
                GameObject playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
                //Debug.Log(" player wall " + playerWall);
                Repair repair = card.GetComponent<Repair>();
                repair.OnSetHealWall(playerWall, pv);
            }
        }
        else if (card.GetComponent<GeneralBoon>())
        {
            Debug.Log(" inside card general boon " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                GameObject palyerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;

                //Debug.Log(" player general " + palyerProfile);
                GeneralBoon boon = card.GetComponent<GeneralBoon>();
                boon.OnSetHealGeneral(palyerProfile, pv);
            }
        }
        else if (card.GetComponent<Mason>())
        {
            Debug.Log(" inside card mason " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                //Debug.Log(" player general " + playerWall);
                Mason mason = card.GetComponent<Mason>();
                mason.OnSetAndActiveHealWall(playerWall, pv);
            }
        }
        else if (card.GetComponent<Paralyze>())
        {
            Debug.Log(" inside card Paralyze " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Paralyze paralyze = card.GetComponent<Paralyze>();
                List<int> totalCardList = paralyze.GenerateEnemyCardList(enemyField, IsWallDestroyed(enemyWall));
                Debug.Log(totalCardList.Count + " total card list count " + paralyze + " paralyzed card ");

                for (int i = 0; i < totalCardList.Count; i++)
                {
                    Card currentCard = enemyField.transform.GetChild(totalCardList[i] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                    Debug.Log("current card " + currentCard.name);
                    currentCard.SetParalyzedCard(paralyze.cardsToBeParalyzedForTurns, currentCard.id);
                    currentCard.ability = CardAbility.None;
                    pv.RPC("ParalyzeCardInOthers", RpcTarget.Others, (totalCardList[i] - 1), paralyze.cardsToBeParalyzedForTurns);
                }
            }
        }
        else if (card.GetComponent<Curse>())
        {
            Debug.Log(" inside card Curse " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Curse curse = card.GetComponent<Curse>();
                List<int> totalCardList = curse.GenerateEnemyCardList(enemyField, IsWallDestroyed(enemyWall));
                Debug.Log(totalCardList.Count + " total card list count " + curse + " paralyzed card ");

                for (int i = 0; i < totalCardList.Count; i++)
                {
                    Card currentCard = enemyField.transform.GetChild(totalCardList[i] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                    Debug.Log("current card " + currentCard.name);
                    CardDetails originalCard = cardDetails.Find(cardId => cardId.id == currentCard.id);
                    int attackValue = currentCard.SetCardAttack(-curse.reduceAttackValue, originalCard.attack);
                    pv.RPC("CurseCardInOthers", RpcTarget.Others, (totalCardList[i] - 1), attackValue);
                }
            }
        }
        else if (card.GetComponent<Smite>())
        {
            Debug.Log(" inside card Smite " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Smite smite = card.GetComponent<Smite>();
                bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                int enemyFieldCount = isEnemyWallDestroyed ? enemyField.transform.childCount : enemyField.transform.childCount / 2;
                bool foundChild = false;
                for (int i = 0; i < enemyFieldCount; i++)
                {
                    if(enemyField.transform.GetChild(i).childCount > 0)
                    {
                        foundChild = true;
                        break;
                    }
                }

                if (!foundChild)
                {
                    Debug.Log(" no child found");
                    return;
                }

                string message = "You set smite ability card. Please select any enemy's card";
                BlurCards(0.8f, true, true, true, false, card, message);
                //int choosenEnemyCardPosition = smite.ChooseEnemyUnit(enemyField, isEnemyWallDestroyed);
                //if (enemyField.transform.GetChild(choosenEnemyCardPosition - 1) != null)
                //{
                //    Destroy(enemyField.transform.GetChild(choosenEnemyCardPosition - 1).GetChild(0).gameObject);
                //    pv.RPC("SmiteInOthers", RpcTarget.Others, (choosenEnemyCardPosition - 1));
                //}
            }
        }
        else if (card.GetComponent<Doom>())
        {
            Debug.Log(" inside card Doom " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Doom doom = card.GetComponent<Doom>();
                bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                List<int> doomedPositions = doom.GenerateDoomedCardList(enemyField, isEnemyWallDestroyed);
                for (int i = 0; i < doomedPositions.Count; i++)
                {
                    if (enemyField.transform.GetChild(doomedPositions[i] - 1) != null)
                    {
                        Card enemyCard = enemyField.transform.GetChild(doomedPositions[i] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                        enemyCard.ability = CardAbility.DeActivate;
                        pv.RPC("DoomInOthers", RpcTarget.Others, (doomedPositions[i] - 1));
                    }
                }
            }
        }
        else if (card.GetComponent<Gambit>())
        {
            Debug.Log(" inside card Gambit " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Gambit gambit = card.GetComponent<Gambit>();
                bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                bool isPlayerWallDestroyed = IsWallDestroyed(playerWall);
                int playerFieldPos = gambit.GetRandomValue(playerField, isEnemyWallDestroyed);
                int enemyFieldPos = gambit.GetRandomValue(enemyField, isEnemyWallDestroyed);

                //Debug.Log(playerFieldPos + " playerFieldPos " + enemyFieldPos + " enemyFieldPos");


                if (playerFieldPos > 0 && enemyFieldPos > 0)
                {
                    GameObject playerRandomCard = playerField.transform.GetChild(playerFieldPos - 1).GetChild(0).gameObject;
                    GameObject enemyRandomCard = enemyField.transform.GetChild(enemyFieldPos - 1).GetChild(0).gameObject;
                    Debug.Log(playerRandomCard + " playerRandomCard " + enemyRandomCard + " enemyRandomCard");
                    playerRandomCard.transform.SetParent(enemyField.transform.GetChild(enemyFieldPos - 1));
                    playerRandomCard.transform.SetPositionAndRotation(enemyField.transform.GetChild(enemyFieldPos - 1).position, enemyField.transform.GetChild(enemyFieldPos - 1).rotation);
                    enemyRandomCard.transform.SetParent(playerField.transform.GetChild(playerFieldPos - 1));
                    enemyRandomCard.transform.SetPositionAndRotation(playerField.transform.GetChild(playerFieldPos - 1).position, playerField.transform.GetChild(playerFieldPos - 1).rotation);

                    pv.RPC("GambitOnOther", RpcTarget.Others, (playerFieldPos - 1), (enemyFieldPos - 1));
                }
                //for (int i = 0; i < doomedPositions.Count; i++)
                //{
                //    if (enemyField.transform.GetChild(doomedPositions[i] - 1) != null)
                //    {
                //        Card enemyCard = enemyField.transform.GetChild(doomedPositions[i] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                //        enemyCard.ability = CardAbility.DeActivate;
                //        pv.RPC("DoomInOthers", RpcTarget.Others, (doomedPositions[i] - 1));
                //    }
                //}
            }
        }
        else if (card.GetComponent<GeneralBane>())
        {
            Debug.Log(" inside card GeneralBane " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                GeneralBane generalBane = card.GetComponent<GeneralBane>();
                GameObject enemyGeneral = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
                Debug.Log("enemy general " + enemyGeneral);
                int currentHealth = int.Parse(enemyGeneral.transform.GetChild(1).Find("Remaining Health").gameObject.GetComponent<TMP_Text>().text);

                int attackValue = generalBane.GetAttackValue(currentHealth, generalBane.damageValue);
                enemyGeneral.transform.GetChild(1).Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(attackValue.ToString());
                Debug.Log("attacki value " + attackValue);
                pv.RPC("GeneralBaneInOthers", RpcTarget.Others, attackValue);
            }
        }
        else if (card.GetComponent<Blackhole>())
        {
            Debug.Log(" inside card Blackhole " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Blackhole blackhole = card.GetComponent<Blackhole>();
                bool isPlayerWallDestroyed = IsWallDestroyed(playerWall);
                bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                int playerFieldCount = isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2;
                int enemyFieldCount = isEnemyWallDestroyed ? enemyField.transform.childCount : enemyField.transform.childCount / 2;
                blackhole.UseBlackHoleAbility(playerField, pv, playerFieldCount);
                blackhole.UseBlackHoleAbility(enemyField, pv, enemyFieldCount);
            }
        }
        else if (card.GetComponent<Nuclear>())
        {
            Debug.Log(" inside card Nuclear " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Nuclear nuclear = card.GetComponent<Nuclear>();
                nuclear.UseNuclearAbility(playerField);
                nuclear.UseNuclearAbility(enemyField);
                pv.RPC("NuclearAbilityInOthers", RpcTarget.Others);
            }
        }
        else if (card.GetComponent<Sacrifice>())
        {
            Debug.Log(" inside card Silence " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Sacrifice sacrifice = card.GetComponent<Sacrifice>();

                bool isPlayerWallDestroyed = IsWallDestroyed(playerWall);
                int playerFieldCount = isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2;
                bool foundChild = false;
                for (int i = 0; i < playerFieldCount; i++)
                {
                    if (playerField.transform.GetChild(i).childCount > 0)
                    {
                        foundChild = true;
                        break;
                    }
                }

                if (!foundChild)
                {
                    Debug.Log(" no child found");
                    return;
                }

                string message = "You choose the sacrifice ability. Please select card which you sacrifice.";
                int parentId = int.Parse(parent.name.Split(" ")[2]);
                BlurCardsForSacrifice(0.8f, true, true, true, false, sacrifice, message, parentId);
            }
            
        }
        else if (card.GetComponent<Mimic>())
        {
            Debug.LogError(" inside card Mimic " + parent.name);
            if (parent.gameObject.tag.Contains("Front Line"))
            {
                Mimic mimic = card.GetComponent<Mimic>();

                bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
                int enemyFieldCount = isEnemyWallDestroyed ? enemyField.transform.childCount : enemyField.transform.childCount / 2;
                bool foundChild = false;
                for (int i = 0; i < enemyFieldCount; i++)
                {
                    if (enemyField.transform.GetChild(i).childCount > 0)
                    {
                        foundChild = true;
                        break;
                    }
                }

                if (!foundChild)
                {
                    Debug.Log(" no child found");
                    return;
                }

                string message = "You choose the Mimic ability. Please select card enemy's one card to copy all stats of enemy.";
                int parentId = int.Parse(parent.name.Split(" ")[2]);
                BlurCards(0.8f, true, true, true, false, card, message);

            }
        }
       
        FieldManager.instance.ResetCounters();
        FieldManager.instance.CalculateAbilityCounter(playerField);
    }

    private bool IsWallDestroyed(GameObject wall)
    {
        int wallHealth = int.Parse(wall.transform.Find("Remaining Health").GetComponent<TMP_Text>().text);
        if (wallHealth <= 0) return true;
        return false;
    }

    #region Card Ability RPCS
    [PunRPC]
    private void ApplyDamageToOthers(int damege)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        Debug.Log(playerField + " player fields " + playerField.transform.parent.parent.name);
        for (int j = 0; j < playerField.transform.childCount; j++)
        {
            if (playerField.transform.GetChild(j).tag.Contains("Front Line"))
            {
                if (playerField.transform.GetChild(j).childCount == 1)
                {
                    if (playerField.transform.GetChild(j).GetChild(0).childCount == 1)
                    {
                        Card attackedCard = playerField.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Card>();
                        Debug.Log(attackedCard.name + " attack card " + damege + " damage value called on others");
                        attackedCard.DealDamage(damege, attackedCard.transform.parent.gameObject);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void HealCardToOthers(int healAmt, string id)
    {
        Debug.Log("heal card called " + healAmt + " heal amount " + id + " id ");
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        //Debug.Log(enemyField + " enemyField " + enemyField.transform.parent.parent.name);
        int parentId = int.Parse(id);
        //Debug.Log(enemyField + " enemy field " + parentId + " parent id");
        if (enemyField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                if(enemyField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if(enemyField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }else if(enemyField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Card card = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            Debug.Log(" card called " + card.name);
            card.SetHP(healAmt);
            //Debug.Log(enemyField.transform.GetChild(parentId - 1).GetChild(0).name + " name of the card");
            //Destroy(enemyField.transform.GetChild(parentId - 1).gameObject);
        }
    }

    [PunRPC]
    private void MutateCardToOthers(int attackAmt, int healAmt, string id)
    {
        Debug.Log("MutateCardToOthers " + healAmt + " heal amount " + attackAmt + " attackAmt " + id + " id ");
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        //Debug.Log(enemyField + " enemyField " + enemyField.transform.parent.parent.name);
        int parentId = int.Parse(id);
        //Debug.Log(enemyField + " enemy field " + parentId + " parent id");
        if (enemyField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Card card = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            Debug.Log(" card called " + card.name);
            card.SetHP(healAmt);
            card.SetAttack(attackAmt);
        }
    }

    [PunRPC]
    private void DestroyCardOnOthers(int id, string parId, int isMaster)
    {
        Debug.Log("destroy on others " + parId);
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        int parentId = int.Parse(parId);
        //Debug.Log(enemyField + " enemy field " + parentId + " parent id");
        if (isMaster == 1)
        {
            if (enemyField.transform.GetChild(parentId - 1) != null)
            {
                if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
                {
                    Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                    if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                    {
                        Debug.LogError($"No child or card found.");
                    }
                    else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                    {
                        Debug.LogError($"Morethan one child found");
                    }
                    else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                    {
                        Debug.LogError($"only one child ");
                    }
                    return;
                }

                //Debug.Log(enemyField.transform.GetChild(parentId - 1).GetChild(0).name + " name of the card");
                Destroy(enemyField.transform.GetChild(parentId - 1).GetChild(0).gameObject);
            }
        }
        else if (isMaster == 2)
        {
            if (enemyField.transform.GetChild(parentId - 1) != null)
            {
                if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
                {
                    Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                    if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                    {
                        Debug.LogError($"No child or card found.");
                    }
                    else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                    {
                        Debug.LogError($"Morethan one child found");
                    }
                    else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                    {
                        Debug.LogError($"only one child ");
                    }
                    return;
                }

                //Debug.Log(enemyField.transform.GetChild(parentId - 1).GetChild(0).name + " name of the card");
                Destroy(enemyField.transform.GetChild(parentId - 1).GetChild(0).gameObject);
            }
        }

    }

    [PunRPC]
    private void CalculateHealBeforeAttack(int id, string parId, int health)
    {
        Debug.Log(" CalculateHealBeforeAttackothers " + id + " parent id " + parId + "  health " + health);
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        int parentId = int.Parse(parId);
        //Debug.Log(enemyField + " enemy field " + parentId + " parent id");
        if (enemyField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Card card = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            Debug.Log(" card called " + card.name);
            card.SetHP(health);
            //Debug.Log(enemyField.transform.GetChild(parentId - 1).GetChild(0).name + " name of the card");
            //Destroy(enemyField.transform.GetChild(parentId - 1).gameObject);
        }
    }

    [PunRPC]
    private void EvolveCardInOthers(int attValue, int heaVal, string parId)
    {
        Debug.Log("EvolveCardInOthers ");
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        int parentId = int.Parse(parId);
        //Debug.Log(enemyField + " enemy field " + parentId + " parent id " + " health set to other " + heaVal + " attack set to other player ");
        if (enemyField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Card card = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            card.SetHP(heaVal);
            card.SetAttack(attValue);
        }
    }

    [PunRPC]
    private void CloneCardOthers(int pos, int cardId)
    {
        Debug.Log("CloneCardOthers ");
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        CardDetails originalCard = cardDetails.Find(cId => cId.id == cardId);

        GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyField.transform.GetChild(pos).position, enemyField.transform.GetChild(pos).rotation);
        miniCardParent.transform.SetParent(enemyField.transform.GetChild(pos));
        miniCardParent.transform.localScale = enemyField.transform.GetChild(pos).localScale;
        Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
        miniCard.SetMiniCard(originalCard.id, originalCard.ergoTokenId, originalCard.ergoTokenAmount, "Clone", originalCard.attack, originalCard.HP, originalCard.gold, originalCard.XP, originalCard.cardImage, CardAbility.None
//, sortedList[i].requirements, sortedList[i].abilityLevel
);
        Destroy(miniCardParent?.GetComponent<DragFieldCard>());
        miniCardParent.AddComponent<DragMiniCards>();
        miniCardParent.AddComponent<DropFieldCard>();
        miniCardParent.name = "ClonedCard";
        miniCard.name = "ClonedCard";
        if (enemyField.transform.GetChild(pos).tag.Contains("Back Line"))
        {
            miniCard.gameObject.SetActive(false);
        }
        //int parentId = int.Parse(parId);
        //Debug.Log(enemyField + " enemy field " + parentId + " parent id " + " health set to other " + attack + " attack set to other player " + health + " health " + gold + " gold " + xp + " xp");
        //if (enemyField.transform.GetChild(parentId - 1) != null)
        //{
        //    //Card card = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
        //    //card.SetHP(health);
        //    //card.SetAttack(attack);
        //    //card.SetGold(gold);
        //    //card.SetXP(xp);
        //}
    }

    [PunRPC]
    private void MalignantOnOthers(int parentId, int health, bool destroyed)
    {
        Debug.Log(" MalignantOnOthers called ");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        //Debug.Log(playerField + " playerField " + parentId + " parentId " + health + " attackValue " + destroyed);
        if (destroyed)
        {
            if (playerField.transform.GetChild(parentId - 1) != null)
            {
                if (parentId - 1 < 0 || parentId - 1 >= playerField.transform.childCount)
                {
                    Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
                    if (playerField.transform.GetChild(parentId - 1).childCount == 0)
                    {
                        Debug.LogError($"No child or card found.");
                    }
                    else if (playerField.transform.GetChild(parentId - 1).childCount > 1)
                    {
                        Debug.LogError($"Morethan one child found");
                    }
                    else if (playerField.transform.GetChild(parentId - 1).childCount == 1)
                    {
                        Debug.LogError($"only one child ");
                    }
                    return;
                }

                if (playerField.transform.GetChild(parentId - 1).GetChild(0) != null)
                {
                    Destroy(playerField.transform.GetChild(parentId - 1).GetChild(0).gameObject);
                }
            }
        }
        else
        {
            if (playerField.transform.GetChild(parentId - 1) != null)
            {
                if (parentId - 1 < 0 || parentId - 1 >= playerField.transform.childCount)
                {
                    Debug.LogError($"prevPos {parentId} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
                    if (playerField.transform.GetChild(parentId - 1).childCount == 0)
                    {
                        Debug.LogError($"No child or card found.");
                    }
                    else if (playerField.transform.GetChild(parentId - 1).childCount > 1)
                    {
                        Debug.LogError($"Morethan one child found");
                    }
                    else if (playerField.transform.GetChild(parentId - 1).childCount == 1)
                    {
                        Debug.LogError($"only one child ");
                    }
                    return;
                }

                Card card = playerField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                card.SetHP(health);
            }
        }
    }

    [PunRPC]
    private void SpawnSummonCardsToOthers(string fieldPos, int parentId)
    {
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        List<int> positions = ConvertStringListTo<int>(fieldPos);

        Card currentCard = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
        CardDetails bigCard = cardDetails.Find(cardId => cardId.id == currentCard.id);

        for (int i = 0; i < positions.Count; i++)
        {
            if (enemyField.transform.GetChild(positions[i] - 1).childCount == 0)
            {
                GameObject miniCardParent = PhotonNetwork.Instantiate("Mini_Card_Parent", enemyField.transform.GetChild(positions[i] - 1).position, enemyField.transform.GetChild(positions[i] - 1).rotation);
                miniCardParent.transform.SetParent(enemyField.transform.GetChild(positions[i] - 1));
                miniCardParent.transform.localScale = enemyField.transform.GetChild(positions[i] - 1).localScale;
                //Debug.Log(bigCard.cardImage.name + " big card name ");
                Card miniCard = miniCardParent.transform.GetChild(0).GetComponent<Card>();
                miniCard.SetMiniCard(bigCard.id, bigCard.ergoTokenId, bigCard.ergoTokenAmount, bigCard.cardName, bigCard.attack, bigCard.HP, bigCard.gold, bigCard.XP, bigCard.cardImage, CardAbility.None
                    //, sortedList[i].requirements, sortedList[i].abilityLevel
                    );
                Destroy(miniCardParent?.GetComponent<DragFieldCard>());
                miniCardParent.AddComponent<DragMiniCards>();
                miniCardParent.AddComponent<DropFieldCard>();
                if (enemyField.transform.GetChild(positions[i] - 1).tag.Contains("Back Line"))
                {
                    miniCard.gameObject.SetActive(false);
                }
                //Debug.Log(miniCard.cardImage.name + " mini card name");
            }
        }
    }

    [PunRPC]
    private void SetWallHealthToOthers(int health)
    {
        Debug.Log("SetWallHealthToOthers called " + health);
        enemyWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Wall").gameObject;
        //Debug.Log("enemy wall " + enemyWall + " health " + enemyWall.transform.Find("Remaining Health"));
        enemyWall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(health.ToString());
    }

    [PunRPC]
    private void SetGeneralHealthToOthers(int health)
    {
        Debug.Log("SetGeneralHealthToOthers called " + health);
        GameObject enemyGeneral = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
        enemyGeneral.transform.Find("Enemy Deck Health").GetChild(0).gameObject.GetComponent<TMP_Text>().SetText(health.ToString());

    }

    [PunRPC]
    private void SerenityAbilityForOthers(int pos, int health)
    {
        Debug.Log("SerenityAbilityForOthers called "
            + health + " health "
            + pos + " pos ");
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        //Debug.Log(" enemy field " + enemyField);
        if (pos < 0 || pos >= enemyField.transform.childCount)
        {
            Debug.LogError($"prevPos {pos} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
            if (enemyField.transform.GetChild(pos).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (enemyField.transform.GetChild(pos).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (enemyField.transform.GetChild(pos).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        Card currentCard = enemyField.transform.GetChild(pos).GetChild(0).GetChild(0).GetComponent<Card>();
        //Debug.Log(currentCard + " current card");
        currentCard.SetHP(health);
    }

    [PunRPC]
    private void SetOrActiveWallHealthToOthers(int health)
    {
        Debug.Log("SetWallHealthToOthers called " + health);
        enemyWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Wall").gameObject;
        //Debug.Log("enemy wall " + enemyWall + " health " + enemyWall.transform.Find("Remaining Health"));
        enemyWall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(health.ToString());
    }

    [PunRPC]
    private void ParalyzeCardInOthers(int position, int turns)
    {
        Debug.Log("ParalyzeCardInOthers on others " + position + " position " + turns);
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        //Debug.Log(playerField + " player field on others");
        if (position < 0 || position >= playerField.transform.childCount)
        {
            Debug.LogError($"prevPos {position} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
            if (playerField.transform.GetChild(position).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (playerField.transform.GetChild(position).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (playerField.transform.GetChild(position).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        Card currentCard = playerField.transform.GetChild(position).GetChild(0).GetChild(0).GetComponent<Card>();
        Debug.Log(currentCard + " current card ");
        currentCard.SetParalyzedCard(turns, currentCard.id);
        currentCard.ability = CardAbility.None;
    }

    [PunRPC]
    private void ChangesOnParalyzedCount(int position, int turns)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        if (position < 0 || position >= playerField.transform.childCount)
        {
            Debug.LogError($"prevPos {position} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
            if (playerField.transform.GetChild(position).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (playerField.transform.GetChild(position).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (playerField.transform.GetChild(position).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        Card currentCard = playerField.transform.GetChild(position).GetChild(0).GetChild(0).GetComponent<Card>();
        currentCard.SetParalyzedCard(turns, currentCard.id);
        currentCard.ability = CardAbility.None;
    }

    [PunRPC]
    private void CurseCardInOthers(int position, int attack)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        if (position < 0 || position >= playerField.transform.childCount)
        {
            Debug.LogError($"prevPos {position} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
            if (playerField.transform.GetChild(position).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (playerField.transform.GetChild(position).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (playerField.transform.GetChild(position).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        Card currentCard = playerField.transform.GetChild(position).GetChild(0).GetChild(0).GetComponent<Card>();
        currentCard.SetAttack(attack);
    }

    [PunRPC]
    private void SmiteInOthers(int pos)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        if (pos < 0 || pos >= playerField.transform.childCount)
        {
            Debug.LogError($"prevPos {pos} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
            if (playerField.transform.GetChild(pos).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (playerField.transform.GetChild(pos).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (playerField.transform.GetChild(pos).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }
        Destroy(playerField.transform.GetChild(pos).GetChild(0).gameObject);
    }

    [PunRPC]
    private void DoomInOthers(int pos)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        if (pos < 0 || pos >= playerField.transform.childCount)
        {
            Debug.LogError($"prevPos {pos} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
            if (playerField.transform.GetChild(pos).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (playerField.transform.GetChild(pos).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (playerField.transform.GetChild(pos).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        Card currentCard = playerField.transform.GetChild(pos).GetChild(0).GetChild(0).GetComponent<Card>();
        currentCard.ability = CardAbility.DeActivate;
    }

    [PunRPC]
    private void GambitOnOther(int playerPos, int enemyPos)
    {
        Debug.Log(" GambitOnOther called " + playerPos + " psetlayer pos " + enemyPos + " enemy pos");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        if (playerPos < 0 || playerPos >= playerField.transform.childCount)
        {
            Debug.LogError($"prevPos {playerPos} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
            if (playerField.transform.GetChild(playerPos).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (playerField.transform.GetChild(playerPos).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (playerField.transform.GetChild(playerPos).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        if (enemyPos < 0 || enemyPos >= enemyField.transform.childCount)
        {
            Debug.LogError($"prevPos {enemyPos} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
            if (enemyField.transform.GetChild(enemyPos).childCount == 0)
            {
                Debug.LogError($"No child or card found.");
            }
            else if (enemyField.transform.GetChild(enemyPos).childCount > 1)
            {
                Debug.LogError($"Morethan one child found");
            }
            else if (enemyField.transform.GetChild(enemyPos).childCount == 1)
            {
                Debug.LogError($"only one child ");
            }
            return;
        }

        //for (int i = 0; i < playerField.transform.childCount; i++)
        //{
        //    if (playerField.transform.GetChild(i).childCount == 1)
        //    {
        //        Debug.Log(" child found " + i + " value " + playerField.transform.GetChild(i).GetChild(0));
        //    }
        //}
        //for (int i = 0; i < enemyField.transform.childCount; i++)
        //{
        //    if (enemyField.transform.GetChild(i).childCount == 1)
        //    {
        //        Debug.Log(" child found " + i + " value " + enemyField.transform.GetChild(i).GetChild(0));
        //    }
        //}

        GameObject playerRandomCard = playerField.transform.GetChild(enemyPos).GetChild(0).gameObject;
        Debug.Log(playerRandomCard + " *#* playerRandomCard ");
        GameObject enemyRandomCard = enemyField.transform.GetChild(playerPos).GetChild(0).gameObject;
        Debug.Log(enemyRandomCard + " *#* enemyRandomCard");

        playerRandomCard.transform.SetParent(enemyField.transform.GetChild(playerPos));
        enemyRandomCard.transform.SetParent(playerField.transform.GetChild(enemyPos));

        playerRandomCard.transform.SetPositionAndRotation
            (enemyField.transform.GetChild(playerPos).position, enemyField.transform.GetChild(playerPos).rotation);
        enemyRandomCard.transform.SetPositionAndRotation(playerField.transform.GetChild(enemyPos).position, playerField.transform.GetChild(enemyPos).rotation);

    }

    [PunRPC]
    private void GeneralBaneInOthers(int attackValue)
    {
        Debug.Log(" GeneralBaneInOthers called");
        GameObject playerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;
        GameObject enemyHealth = playerProfile.transform.GetChild(1).Find("Remaining Health").gameObject;
        enemyHealth.GetComponent<TMP_Text>().SetText(attackValue.ToString());
    }

    [PunRPC]
    private void BlackHoleInOthers()
    {
        Debug.Log("BlackHoleInOthers called ");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        bool isPlayerWallDestroyed = IsWallDestroyed(playerWall);
        bool isEnemyWallDestroyed = IsWallDestroyed(enemyWall);
        int playerFieldCount = isPlayerWallDestroyed ? playerField.transform.childCount : playerField.transform.childCount / 2;
        int enemyFieldCount = isEnemyWallDestroyed ? enemyField.transform.childCount : enemyField.transform.childCount / 2;

        Debug.Log(isPlayerWallDestroyed + " isplayer wall destroyed " + isEnemyWallDestroyed + " is enemy wall destroy " + playerFieldCount + " p count " + enemyFieldCount + " e count");

        for (int i = 0; i < playerFieldCount; i++)
        {
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log(i + " i value");
                Destroy(playerField.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < enemyFieldCount; i++)
        {
            if (enemyField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log(i + " i value");
                Destroy(enemyField.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

    }

    [PunRPC]
    private void NuclearAbilityInOthers()
    {
        Debug.Log("NuclearAbilityInOthers called ");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;


        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log(i + " i value");
                Destroy(playerField.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            if (enemyField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log(i + " i value");
                Destroy(enemyField.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

    }

    [PunRPC]
    private void SmiteOnOthers(string parId)
    {
        Debug.Log(" SmiteOnOthers called ");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        int parentId = int.Parse(parId);
        //Debug.Log(playerField + " playerField  " + parentId + " parent id");
        if (playerField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= playerField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId - 1} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
                if (playerField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (playerField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (playerField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Debug.Log("playerField.transform.GetChild(parentId - 1).GetChild(0) " + playerField.transform.GetChild(parentId - 1).GetChild(0).name);
            Destroy(playerField.transform.GetChild(parentId - 1).GetChild(0).gameObject);
        }
    }

    [PunRPC]
    private void MimicOnOthers(string parId, int id)
    {
        Debug.Log(" MimicOnOthers called ");
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        int parentId = int.Parse(parId);
        //Debug.Log(playerField + " playerField  " + parentId + " parent id");
        if (playerField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= playerField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId - 1} is out of bounds for enemyHand with {playerField.transform.childCount} children.");
                if (playerField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (playerField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (playerField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Debug.Log("playerField.transform.GetChild(parentId - 1).GetChild(0) " + playerField.transform.GetChild(parentId - 1).GetChild(0).name);
            CardDetails enemyCardToBeCopy = cardDetails.Find(item => item.id == id);
            Card currentCard = playerField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            Debug.Log(enemyCardToBeCopy + " enemyCardToBeCopy " + currentCard + " currentCard");
            Debug.Log(enemyCardToBeCopy.id + " enemyCardToBeCopy.id " + currentCard.ergoTokenId + " currentCard.ergoTokenId " + currentCard.ergoTokenAmount + " currentCard.ergoTokenAmount " + currentCard.cardName + " currentCard.cardName " + enemyCardToBeCopy.attack + " enemyCardToBeCopy.attack " + enemyCardToBeCopy.HP + " enemyCardToBeCopy.HP " + enemyCardToBeCopy.gold + " enemyCardToBeCopy.gold " + enemyCardToBeCopy.XP + " enemyCardToBeCopy.XP " + currentCard.cardImage + "  currentCard.cardImage " + enemyCardToBeCopy.ability + " enemyCardToBeCopy.ability");
            currentCard.SetMiniCard(enemyCardToBeCopy.id, currentCard.ergoTokenId, currentCard.ergoTokenAmount, currentCard.cardName,  enemyCardToBeCopy.attack, enemyCardToBeCopy.HP, enemyCardToBeCopy.gold, enemyCardToBeCopy.XP, currentCard.image.sprite, enemyCardToBeCopy.ability);

                
            Destroy(currentCard.GetComponent<Mimic>());
            Type cardType = FieldManager.instance.GetAbility(enemyCardToBeCopy.ability);
            currentCard.gameObject.AddComponent(cardType);
            //Debug.Log(cardType + " card type");
        }
    }

    [PunRPC]
    private void ResetCardsInOthers()
    {
        Debug.Log("ResetCardsInOthers called");
        ResetCardProperties();
    }

    [PunRPC]
    private void SacrificeOnOthers(string parId, int gainedXP)
    {
        Debug.Log(" SacrificeOnOthers called " + gainedXP);
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(gainedXP);
        int parentId = int.Parse(parId);
        //Debug.Log(enemyField + " playerField  " + parentId + " parent id");
        if (enemyField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId - 1} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Debug.Log(enemyField.transform.GetChild(parentId - 1).name + " enemyField.transform.GetChild(parentId - 1)");
            Destroy(enemyField.transform.GetChild(parentId - 1).GetChild(0).gameObject);
        }
    }

    [PunRPC]
    private void EndGameInOthers(int attackVal, int healthVal, string parId)
    {
        Debug.Log(" EndGameInOthers called " + attackVal + " atta " + healthVal + " health " + parId + " parentId");
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        int parentId = int.Parse(parId);
        //Debug.Log(enemyField + " playerField  " + parentId + " parent id");
        if (enemyField.transform.GetChild(parentId - 1) != null)
        {
            if (parentId - 1 < 0 || parentId - 1 >= enemyField.transform.childCount)
            {
                Debug.LogError($"prevPos {parentId - 1} is out of bounds for enemyHand with {enemyField.transform.childCount} children.");
                if (enemyField.transform.GetChild(parentId - 1).childCount == 0)
                {
                    Debug.LogError($"No child or card found.");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount > 1)
                {
                    Debug.LogError($"Morethan one child found");
                }
                else if (enemyField.transform.GetChild(parentId - 1).childCount == 1)
                {
                    Debug.LogError($"only one child ");
                }
                return;
            }

            Debug.Log(enemyField.transform.GetChild(parentId - 1).name + " enemyField.transform.GetChild(parentId - 1)");
            Card card = enemyField.transform.GetChild(parentId - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            card.SetHP(healthVal);
            card.SetAttack(attackVal);
        }
    }

    #endregion

    public void BlurCards(float blurValue, bool isBlurCards, bool isErrorDialogOpen, bool cardEnabled, bool isRayCast, Card currentCard, string text = "")
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        Debug.LogError("playerField " + playerField + " enemyField " + enemyField + " blurValue " + blurValue + " isBlurCards " + isBlurCards + " isErrorDialogOpen " + isErrorDialogOpen + " cardEnabled " + cardEnabled + " text " + text);
        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            Debug.Log(playerField.transform.GetChild(i).childCount + "layerField.transform.GetChild(i).childCount");
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                playerField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().alpha = blurValue;
                playerField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = isRayCast;
                //playerField.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = isBlurCards;
                Debug.Log(cardError.transform.GetChild(0).gameObject + " cardError.transform.GetChild(0).gameObject");
                cardError.transform.GetChild(0).gameObject.SetActive(isErrorDialogOpen);
                Debug.Log("cardError.GetComponentInChildren<TMP_Text>() " + cardError.GetComponentInChildren<TMP_Text>());
                if (isErrorDialogOpen)
                {
                    cardError.GetComponentInChildren<TMP_Text>().SetText(text);
                }
            }
        }

        Debug.Log(currentCard + " current card " + currentCard.transform.parent.parent);
        Transform parent = currentCard.transform.parent.parent;
        Debug.Log(parent + " current card parent");
        cardParent = parent;
        Component[] components = currentCard.GetComponents<Component>();

       
        //foreach (Component component in components)
        //{
        //    Debug.Log("*** Component Name: " + component.GetType().Name);
        //}
        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            //Debug.Log(enemyField.transform.GetChild(i).childCount + "enemyField.transform.GetChild(i).childCount");
            if (enemyField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log("enemyField.transform.GetChild(i).GetChild(0) " + enemyField.transform.GetChild(i).GetChild(0).name);
                Debug.Log("enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>() choose " + enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>());
                enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().isEnabled = cardEnabled;
                Card selectedCard = enemyField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>();
                if(currentCard.ability == CardAbility.Smite)
                {
                    enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().OnSelected.AddListener(() => SelectSmiteCard(selectedCard));
                }   
                else if(currentCard.ability == CardAbility.Mimic)
                {
                    enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().OnSelected.AddListener(() => SelectMimicCard(selectedCard));
                }
            }
        }
    }

    public void BlurCardsForSacrifice(float blurValue, bool isBlurCards, bool isErrorDialogOpen, bool cardEnabled, bool isRayCast, Sacrifice sacrifice, string text = "", int id = 1)
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        Debug.Log("playerField " + playerField + " enemyField " + enemyField + " blurValue " + blurValue + " isBlurCards " + isBlurCards + " isErrorDialogOpen " + isErrorDialogOpen + " cardEnabled " + cardEnabled + " text " + text);
        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            if (enemyField.transform.GetChild(i).childCount == 1)
            {
                enemyField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().alpha = blurValue;
                enemyField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = isRayCast;
                //enemyField.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = isBlurCards;
                Debug.Log(cardError.transform.GetChild(0).gameObject + " cardError.transform.GetChild(0).gameObject");
                cardError.transform.GetChild(0).gameObject.SetActive(isErrorDialogOpen);
                Debug.Log("cardError.GetComponentInChildren<TMP_Text>() " + cardError.GetComponentInChildren<TMP_Text>());
                if (isErrorDialogOpen)
                {
                    cardError.GetComponentInChildren<TMP_Text>().SetText(text);
                }
            }
        }

        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                if(id - 1 == i)
                {
                    continue;
                }
                Debug.Log(playerField.transform.GetChild(i).GetChild(0) + " playerField.transform.GetChild(i).GetChild(0) ");
                Debug.Log(playerField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>() + " playerField.transform.GetChild(i).GetChild(0) choose ");
                playerField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().isEnabled = cardEnabled;
                Card selectedCard = playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>();
                playerField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().OnSelected.AddListener(() => SelectSacrificeCard(selectedCard, sacrifice));
            }
        }
    }

    public void SelectSmiteCard(Card card)
    {
        Debug.Log("Card selected");
        Debug.Log(card.name + " card name");
        string parentId = card.transform.parent.parent.name.Split(" ")[2];
        Destroy(card.transform.parent.gameObject);

        pv.RPC("SmiteOnOthers", RpcTarget.Others, parentId);
        ResetSelectedCards();
    }

    public void SelectMimicCard(Card fromCard)
    {
        Debug.Log("Card selected " + cardParent);
        Card toCard = cardParent.GetChild(0).GetChild(0).GetComponent<Card>();
        Debug.Log(toCard + " to card name");
        if(fromCard == null || toCard == null)
        {
            Debug.Log("any one card null");
            return;
        }
        Component[] components = toCard.GetComponents<Component>();

        // Print the name of each component
        //foreach (Component component in components)
        //{
        //    Debug.Log("### Component Name: " + component.GetType().Name + " parent name " + cardParent);
        //}
        //Debug.Log(fromCard.name + " from card name " + toCard + " to card name" );
        //Debug.Log(fromCard.GetComponent<Card>() + " from card card " + toCard.GetComponent<Card>() + " to card card");
        string parentId = toCard.transform.parent.parent.name.Split(" ")[2];
        Component[] component1 = toCard.GetComponents<Component>();

        // Print the name of each component
        //foreach (Component component in component1)
        //{
        //    Debug.Log("*** Component Name: " + component.GetType().Name);
        //}
        Debug.Log("Mimic card " + toCard.GetComponent<Mimic>());
        toCard.GetComponent<Mimic>().SetMimicData(fromCard, toCard);
        pv.RPC("MimicOnOthers", RpcTarget.Others, parentId, fromCard.id);
        ResetSelectedCards();
    }

    public void SelectSacrificeCard(Card card, Sacrifice secCard)
    {
        Debug.Log("Card selected SelectSacrificeCard");
        Debug.Log(card.name + " card name");
        string parentId = card.transform.parent.parent.name.Split(" ")[2];
        int gainedXP = secCard.GainGoldAndXP(card.gold, card.XP, PhotonNetwork.IsMasterClient);
        playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(gainedXP);
        Destroy(card.transform.parent.gameObject);

        pv.RPC("SacrificeOnOthers", RpcTarget.Others, parentId, gainedXP);
        ResetSelectedCards();
    }

    public void ResetSelectedCards()
    {
        Debug.Log("Reset card called ");
        ResetCardProperties();
        pv.RPC("ResetCardsInOthers", RpcTarget.Others);
    }

    public void ResetCardProperties()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        //Debug.Log("player field " + playerField + " enemy field " + enemyField);
        cardParent = null;

        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            if (playerField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log("inside player child == 1");
                playerField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().isEnabled = false;
                playerField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = true;
                playerField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
                cardError.transform.GetChild(0).gameObject.SetActive(false);
                //playerField.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
            }
        }

        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            Debug.Log("inside enemy child == 1");
            Debug.Log("enemyField.transform.GetChild(i).childCount " + enemyField.transform.GetChild(i).childCount);
            
            if (enemyField.transform.GetChild(i).childCount == 1)
            {
                Debug.Log("enemyField.transform.GetChild(i).childCount " + enemyField.transform.GetChild(i).childCount);
                Debug.Log("enemyField.transform.GetChild(i).GetChild(0) " + enemyField.transform.GetChild(i).GetChild(0));
                if (enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().isEnabled)
                {
                    enemyField.transform.GetChild(i).GetChild(0).GetComponent<ChooseCard>().isEnabled = false;
                    enemyField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = true;
                    enemyField.transform.GetChild(i).GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
                    cardError.transform.GetChild(0).gameObject.SetActive(false);
                    //enemyField.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
                }
            } 
        }

    }

    //public void ClickedColor()
    //{
    //    if(player1Turn && PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log(gameObject.name + " game object name " + EventSystem.current.currentSelectedGameObject);
    //        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
    //        if (selectedObject != null)
    //        {
    //            string callerName = selectedObject.name;
    //            Debug.Log("Button clicked by: " + callerName);
    //            selectedObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = clickedColor;
    //            selectedObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = clickedColor;
    //        }
    //    }

    //    if(!player1Turn && !PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log(gameObject.name + " game object name " + EventSystem.current.currentSelectedGameObject);
    //        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
    //        if (selectedObject != null)
    //        {
    //            string callerName = selectedObject.name;
    //            Debug.Log("Button clicked by: " + callerName);
    //            selectedObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = clickedColor;
    //            selectedObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = clickedColor;
    //        }
    //    }
    //}

    private void ResetClickedColor()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        if (playerField != null) 
        { 
            for(int i = 0; i < playerField.transform.childCount; i++)
            {
                if(playerField.transform.GetChild(i).childCount > 0)
                {
                    playerField.transform.GetChild(i).GetChild(0).GetChild(0).Find("Image").GetComponent<Image>().color = normalColor;
                    playerField.transform.GetChild(i).GetChild(0).GetChild(0).Find("Frame").GetComponent<Image>().color = normalColor;
                    playerField.transform.GetChild(i).GetChild(0).GetComponent<ClickedMiniCard>().isClicked = false;
                }
            }
        }

        if (enemyField != null) 
        {
            for (int i = 0; i < enemyField.transform.childCount; i++)
            {
                if (enemyField.transform.GetChild(i).childCount > 0)
                {
                    enemyField.transform.GetChild(i).GetChild(0).GetChild(0).Find("Image").GetComponent<Image>().color = normalColor;
                    enemyField.transform.GetChild(i).GetChild(0).GetChild(0).Find("Frame").GetComponent<Image>().color = normalColor;
                    enemyField.transform.GetChild(i).GetChild(0).GetComponent<ClickedMiniCard>().isClicked = false;
                }
            }
        }
    }

    private void HideBackLineCards()
    {
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        Debug.Log("HideBackLineCards() called " + enemyField + " field " + enemyField.transform.childCount);
        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            if (enemyField.transform.GetChild(i).tag == "Back Line Enemy")
            {
                if (enemyField.transform.GetChild(i).childCount == 1)
                {
                    enemyField.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    private void EnableOrDisablePointerImage(GameObject pointer, bool value)
    {
        pointer.GetComponent<Image>().enabled = value;
    }

}

