using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using UnityEngine.Timeline;
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
    private Timer countdownTimer;
    private Timer bidTimer;
    private Timer afterBidTimer;
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

    #endregion

    private void Start()
    {
        isWallDestroyed = false;
        endGame = false;
        skirmishManager = SkirmishManager.instance;
        gameBoardParent = GameObject.Find("Game Board Parent");
        countdownTimer = countDownPanel.GetComponent<Timer>();
        bidTimer = biddingPanel.GetComponentInChildren<Timer>();
        afterBidTimer = afterBiddingPanel.GetComponent<Timer>();
        winTimer = gameBoardParent.transform.GetChild(1).GetChild(1).GetComponent<TimeLeft>();

        timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
        timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
        Debug.LogError("timer down " + timeDown + " time up " + timeUp);


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
        leaveButton.onClick.AddListener(GotoSkirmishMenu);
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

        //int masterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
        //int clientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];

        //Debug.LogError("master xp " + masterXP );
        //Debug.LogError("client xp " + clientXP );

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    initialGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
        //    Debug.Log(" player " + playerXPProgressBar + " client " + enemyXPProgressBar);
        //    playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(masterXP);
        //    enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(clientXP);
        //    Debug.Log(" master ");
        //}else if (!PhotonNetwork.IsMasterClient)
        //{
        //    initialGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
        //    Debug.Log(" player " + playerXPProgressBar + " client " + enemyXPProgressBar);
        //    enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(masterXP);
        //    playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(clientXP);
        //    Debug.Log(" client ");
        //}

        //////Debug.LogError("player deck properties " + (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"]);
        ////int playerDeckProfileId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"] - 1;
        ////string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
        ////Player currPlayer = PhotonNetwork.LocalPlayer;
        ////Player nextPlayer = currPlayer.GetNext();
        //////Debug.LogError("next deck properties " + (int)nextPlayer.CustomProperties["deckId"]);
        ////int opponentDeckProfileId = (int)nextPlayer.CustomProperties["deckId"] - 1;
        ////string opponentField = (string)nextPlayer.CustomProperties["deckField"];

        ////Debug.LogError(playerDeckProfileId + " deck id " + playerField + " field name");
        ////Debug.LogError(opponentDeckProfileId + " deck id " + opponentField + " field name");

        ////Debug.LogError("master xp " + (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
        ////Debug.LogError("client xp " + (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);

        ////int playerId = GetFieldIndex(playerField);
        ////int opponentId = GetFieldIndex(opponentField);

        ////bottomImage = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Game Play Field Border").Find("Bottom Field").GetComponent<Image>();
        ////topImage = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Game Play Field Border").Find("Top Field").GetComponent<Image>();

        ////Debug.Log(bottomImage.name + " image name " + bottomImage.transform.parent.parent.parent.name + " parent name ");
        ////Debug.Log(topImage.name + " image name " + topImage.transform.parent.parent.parent.name + " parent name ");

        ////bottomImage.sprite = playerFields[playerId];
        ////topImage.sprite = playerFields[opponentId];
        ////Debug.Log(playerId + " player id " + opponentId + " oppenent id");
        ////bottomImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId);
        ////bottomImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, "down");
        ////topImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId);
        ////topImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, "up");


        ////downProfileIamge.GetComponent<Image>().sprite = profileImages[playerDeckProfileId];
        ////upProfileImage.GetComponent<Image>().sprite = profileImages[opponentDeckProfileId];
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
            timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
            timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
            timeDown.PauseTimer("down");
            timeUp.PauseTimer("up");
            Debug.LogError(" time up parent " + timeUp.transform.parent.parent.parent.name + " down " + timeDown.transform.parent.parent.parent);
            StopAllCoroutines();
            //resultPanel.SetActive(true);
            //winTimer.InitTimers(30);
            endGame = true;
            PhotonNetwork.AutomaticallySyncScene = false;
            CalculateWinner();
            pv.RPC("CompleteGame", RpcTarget.Others);
        }

        if (winTimer.timeUp && endGame)
        {
            Debug.LogError("the timer up and end game called before " + endGame + " " +  PhotonNetwork.LocalPlayer.NickName);
            endGame = false;
            Debug.LogError("the timer up and end game called after " + endGame + " " + PhotonNetwork.LocalPlayer.NickName);
            LeaveBothPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                //Debug.LogError(" clicked and null");
                GameManager.instance.clicked = 0;
                attackingcard = null;
                ResetAnimation("player");
                ResetAnimation("field");
            }
            else
            {
                //Debug.LogError(" clicked and else " + EventSystem.current.currentSelectedGameObject);
                if (EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>())
                {
                    if (player1Turn && PhotonNetwork.IsMasterClient)
                    {
                        attackingcard = EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>().gameObject.GetComponentInChildren<Card>();
                        //Debug.LogError(" attacking card " + attackingcard);
                        if (attackingcard.IsAttack())
                        {
                            //Debug.LogError("attack value inside attack ");
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
                                //Debug.LogError("clicked = 1");
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
                            {
                                cardError.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                                Invoke("RemoveErrorObject", 2f);
                            }
                            //Debug.LogError("on the attack ");
                            //if(isWallDestroyed)
                            //{
                            //    GameManager.instance.clicked = 1;
                            //}
                            //else
                            //{
                            //    if (attackingcard.transform.parent.parent.CompareTag("Front Line Player"))
                            //    {
                            //        //Debug.LogError("clicked = 1");
                            //        GameManager.instance.clicked = 1;
                            //    }
                            //    else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
                            //    {
                            //        cardError.SetActive(true);
                            //        cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                            //        Invoke("RemoveErrorObject", 2f);
                            //    }
                            //}

                        }
                    }
                    else if (!player1Turn && !PhotonNetwork.IsMasterClient)
                    {
                        attackingcard = EventSystem.current.currentSelectedGameObject.GetComponent<DragFieldCard>().gameObject.GetComponentInChildren<Card>();
                        //Debug.LogError(" attacking card " + attackingcard);
                        if (attackingcard.IsAttack())
                        {
                            //Debug.LogError("attack value inside attack ");
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
                                //Debug.LogError("clicked = 1");
                                GameManager.instance.clicked = 1;
                            }
                            else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
                            {
                                cardError.SetActive(true);
                                cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                                Invoke("RemoveErrorObject", 2f);
                            }
                            //Debug.LogError("on the attack ");
                            //if (isWallDestroyed)
                            //{
                            //    GameManager.instance.clicked = 1;
                            //}
                            //else
                            //{
                            //    if (attackingcard.transform.parent.parent.CompareTag("Front Line Player"))
                            //    {
                            //        //Debug.LogError("clicked = 1");
                            //        GameManager.instance.clicked = 1;
                            //    }
                            //    else if (attackingcard.transform.parent.parent.CompareTag("Back Line Player"))
                            //    {
                            //        cardError.SetActive(true);
                            //        cardError.GetComponentInChildren<TMP_Text>().SetText("You cannot attack the card which is back to the wall");
                            //        Invoke("RemoveErrorObject", 2f);
                            //    }
                            //}
                        }
                    }

                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>())
                {
                    if (GameManager.instance.clicked == 1)
                    {
                        Card targetCard = EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>().GetComponentInChildren<Card>();
                        //Debug.LogError("target card " + targetCard);
                        GameObject attackingcardParent = attackingcard.transform.parent.parent.gameObject;

                        GameObject targetCardParent = EventSystem.current.currentSelectedGameObject.GetComponent<DropFieldCard>().transform.parent.gameObject;

                        AttackCard(attackingcard, attackingcardParent, targetCard, targetCardParent);

                        GameManager.instance.clicked = 0;
                        attackingcardParent = null;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.tag == "EnemyWall")
                {
                    //Debug.LogError(attackingcard + " attacking card");
                    //Debug.LogError("Enemy wall " + GameManager.instance.clicked);
                    if (player1Turn && PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        //Debug.LogError("master");
                        if (GameManager.instance.clicked == 1)
                        {
                            //Debug.Log("Clicked inside enemy wall");
                            //Debug.Log(attackingcard + " attacking card");
                            GameObject enemyHealthObject = enemyWall.transform.Find("Remaining Health").gameObject;
                            //Debug.Log(enemyHealthObject + " enemy obj");
                            int currentHealth = int.Parse(enemyHealthObject.GetComponent<TMP_Text>().text.ToString());
                            //Debug.Log(currentHealth + " remainig health ");
                            //Debug.Log(attackingcard.attack + " attack value on card");
                            int remainingHealth = currentHealth - attackingcard.attack;
                            //Debug.LogError(" current health " + currentHealth + " attack value " + attackingcard.attack + " remaining health " + remainingHealth);
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                nextPlayer = PhotonNetwork.LocalPlayer.GetNext();
                                string opponentField = (string)nextPlayer.CustomProperties["deckField"];
                                //Debug.Log(nextPlayer.NickName + " next player name " + opponentField + " opponent field ");
                                int opponentId = GetFieldIndex(opponentField);
                                //Debug.LogError("opponet id " + opponentId);
                                topImage.sprite = playerBrokenFields[opponentId];
                                //Debug.LogError("top image " + topImage.name);
                                enemyWall.GetComponent<PolygonCollider2D>().enabled = false;
                                enemyWall.GetComponent<Button>().enabled = false;
                                //Debug.LogError("enemy health before  " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                //Debug.LogError(" health is 0 " + remainingHealth);
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(0.ToString());
                                //Debug.LogError("enemy health after " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                pv.RPC("AttackWall", RpcTarget.Others, 0);
                                isWallDestroyed = true;
                                ShowHiddenCard();
                                GameManager.instance.clicked = 0;
                            }
                            else
                            {
                                //Debug.LogError("enemy health before  " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                //Debug.LogError(" Health is more than 0 " + remainingHealth);
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(remainingHealth.ToString());
                                //Debug.LogError("enemy health after " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                pv.RPC("AttackWall", RpcTarget.Others, remainingHealth);
                                GameManager.instance.clicked = 0;
                            }
                        }
                    }
                    else if (!player1Turn && !PhotonNetwork.IsMasterClient && pv.IsMine)
                    {
                        //Debug.LogError("not master");
                        if (GameManager.instance.clicked == 1)
                        {
                            //Debug.Log("Clicked inside enemy wall");
                            //Debug.Log(attackingcard + " attacking card");
                            GameObject enemyHealthObject = enemyWall.transform.Find("Remaining Health").gameObject;
                            //Debug.Log(enemyHealthObject + " enemy obj");
                            int currentHealth = int.Parse(enemyHealthObject.GetComponent<TMP_Text>().text.ToString());
                            //Debug.Log(currentHealth + " remainig health ");
                            //Debug.Log(attackingcard.attack + " attack value on card");
                            int remainingHealth = currentHealth - attackingcard.attack;
                            //Debug.LogError(" current health " + currentHealth + " attack value " + attackingcard.attack + " remaining health " + remainingHealth);
                            attackingcard.SetAttackValue(true);
                            if (remainingHealth <= 0)
                            {
                                nextPlayer = PhotonNetwork.LocalPlayer.GetNext();
                                string opponentField = (string)nextPlayer.CustomProperties["deckField"];
                                //Debug.Log(nextPlayer.NickName + " next player name " + opponentField + " opponent field ");
                                int opponentId = GetFieldIndex(opponentField);
                                //Debug.LogError("opponet id " + opponentId);
                                topImage.sprite = playerBrokenFields[opponentId];
                                //Debug.LogError("top image " + topImage.name);
                                enemyWall.GetComponent<PolygonCollider2D>().enabled = false;
                                enemyWall.GetComponent<Button>().enabled = false;
                                //Debug.LogError("enemy health before  " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                //Debug.LogError(" health is 0 " + remainingHealth);
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(0.ToString());
                                //Debug.LogError("enemy health after " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                pv.RPC("AttackWall", RpcTarget.Others, 0);
                                isWallDestroyed = true;
                                ShowHiddenCard();
                                GameManager.instance.clicked = 0;
                            }
                            else
                            {
                                //Debug.LogError("enemy health before  " + enemyHealthObject.GetComponent<TMP_Text>().text);
                                //Debug.LogError(" Health is more than 0 " + remainingHealth);
                                enemyHealthObject.GetComponent<TMP_Text>().SetText(remainingHealth.ToString());
                                //Debug.LogError("enemy health after " + enemyHealthObject.GetComponent<TMP_Text>().text);
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
                            Debug.Log("attacking card " + attackingcard);
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
                                //endTime = DateTime.Now;
                                //Debug.LogError("match end time " + endTime);
                                //float totalSeconds = (float)(endTime - initialStartTime).TotalSeconds;
                                //Debug.LogError(" total seconds " + totalSeconds);
                                pv.RPC("GetGameResult", RpcTarget.Others);
                                CalculateWinner();
                                GameManager.instance.clicked = 0;
                            }
                            else
                            {
                                enemyHealth.GetComponent<TMP_Text>().SetText(remainingHealth.ToString());
                                //Debug.LogError("enemy health after " + enemyHealth.GetComponent<TMP_Text>().text);
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
        ////resultPanel.SetActive(true);
        //resultPanel.transform.GetChild(0).gameObject.SetActive(true);
        ////winTimer = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timeleft ").GetComponent<TimeLeft>();
        //winTimer.InitTimers(30);
        //Debug.LogError(turnCountMaster + " turn count master " + turnCountClient + " turn count client");
        //Debug.LogError(PlayerPrefs.GetInt("masterCount") + " turn count master " + PlayerPrefs.GetInt("clientCount") + " turn count client");
        //totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
        //matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
        //winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    totalTurnText.SetText(PlayerPrefs.GetInt("masterCount") + " Turns.");
        //    Debug.LogError("master turn " + turnCountMaster);
        //}else
        //{
        //    totalTurnText.SetText(PlayerPrefs.GetInt("clientCount") + " Turns.");
        //    Debug.LogError(" client turn " + turnCountClient);
        //}
        //string winnerName = PhotonNetwork.LocalPlayer.NickName;
        //endTime = DateTime.Now;
        //Debug.LogError("match end time " + endTime);
        //float totalSeconds = (float)(endTime - initialStartTime).TotalSeconds;
        //Debug.LogError(" total seconds " + totalSeconds);
        //int minutes = (int)totalSeconds / 60;
        //int seconds = (int)totalSeconds % 60;
        //Debug.LogError(" minutes " + minutes + " seconds " + seconds);
        //matchLengthText.SetText($"{minutes} : {seconds}");
        //Debug.LogError(" winner name " + winnerName);
        //winnerPlayerName.SetText(winnerName + " is Victorious!");
        //pv.RPC("ResultRPC", RpcTarget.Others, minutes, seconds, winnerName);
        timeDown = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(1).GetComponent<PlayerTimer>();
        timeUp = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Timer").GetChild(0).GetComponent<PlayerTimer>();
        timeDown.PauseTimer("down");
        timeUp.PauseTimer("up");
        pv.RPC("CalculateScore", RpcTarget.All);

        //resultPanel.transform.GetChild(0).gameObject.SetActive(true);
        //winTimer.InitTimers(30);

        //totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
        //matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
        //winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();

        //GameObject playerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;
        //int playerHealth = int.Parse(playerProfile.transform.Find("Player Deck Health").Find("Remaining Health").GetComponent<TMP_Text>().text);
        //GameObject enemyProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
        //int enemyHealth = int.Parse(enemyProfile.transform.Find("Enemy Deck Health").Find("Remaining Health").GetComponent<TMP_Text>().text);
        //string winnerName = "";
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    totalTurnText.SetText(PlayerPrefs.GetInt("masterCount") + " Turns.");
        //    if (playerHealth > enemyHealth)
        //    {
        //         winnerName = PhotonNetwork.MasterClient.NickName;
        //    }
        //    else if(enemyHealth > playerHealth)
        //    {
        //        winnerName = PhotonNetwork.LocalPlayer.GetNext().NickName;
        //    } else if(playerHealth == enemyHealth)
        //    {

        //    }

        //}else 

        //xpSlider = resultPanel.transform.GetChild(0).Find("XP Progress Bar").GetComponent<Slider>();
        //xpSlider.interactable = false;

     
        //if (PhotonNetwork.IsMasterClient)
        //{
            
        //    //Debug.LogError("master turn " + turnCountMaster);
        //}
        //else
        //{
        //    totalTurnText.SetText(PlayerPrefs.GetInt("clientCount") + " Turns.");
        //    //Debug.LogError(" client turn " + turnCountClient);
        //}

        
        //endTime = DateTime.Now;
        ////Debug.LogError("match end time " + endTime);
        //float totalSeconds = (float)(endTime - initialStartTime).TotalSeconds;
        ////Debug.LogError(" total seconds " + totalSeconds);
        //int minutes = (int)totalSeconds / 60;
        //int seconds = (int)totalSeconds % 60;
        ////Debug.LogError(" minutes " + minutes + " seconds " + seconds);
        //matchLengthText.SetText($"{minutes} : {seconds}");
        ////Debug.LogError(" winner name " + winnerName);
        //winnerPlayerName.SetText(winnerName + " is Victorious!");
        //pv.RPC("ResultRPC", RpcTarget.Others, minutes, seconds, winnerName);
        //timeDown.PauseTimer("down");
        //timeUp.PauseTimer("up");
        //StopAllCoroutines();
        ////Debug.LogError("end game value before " + endGame);
        //endGame = true;
        ////Debug.LogError("end game value after " + endGame);
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"] +" mgg ") ;
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"] + " cgg ") ;
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"] + " mgx ");
        //Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"] + " cgx ");
    }

    [PunRPC]
    private void CalculateScore()
    {
        resultPanel.transform.GetChild(0).gameObject.SetActive(true);
        winTimer.InitTimers(30);

        totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
        matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
        winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();
        GameObject mainMenu = resultPanel.transform.GetChild(0).Find("Main Menu").gameObject;
        Debug.LogError("main menu " + mainMenu + " player name " + PhotonNetwork.LocalPlayer.NickName);

        GameObject playerProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Profile").gameObject;
        int playerHealth = int.Parse(playerProfile.transform.Find("Player Deck Health").Find("Remaining Health").GetComponent<TMP_Text>().text);
        GameObject enemyProfile = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
        int enemyHealth = int.Parse(enemyProfile.transform.Find("Enemy Deck Health").Find("Remaining Health").GetComponent<TMP_Text>().text);
        xpSlider = resultPanel.transform.GetChild(0).Find("XP Progress Bar").GetComponent<Slider>();
        xpSlider.interactable = false;
        PlayerData data = new PlayerData();
        string winnerName = "";
        if (PhotonNetwork.IsMasterClient)
        {
            totalTurnText.SetText(PlayerPrefs.GetInt("masterCount") + " Turns.");
            int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            int gainedMasterXp = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"];
            int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
            data.gold = totalPlayerGold;
            data.xp = totalMasterXP;
            Debug.Log(" master xp " + gainedMasterXp + " gained " +  totalMasterXP + " total " + totalPlayerGold + " total gold") ;
            xpSlider.value = (totalMasterXP/ 2000f);
            PlayerPrefs.SetInt("totalGold", totalPlayerGold);
            PlayerPrefs.SetInt("totalXP", totalMasterXP);
            Debug.LogError(" player health " + playerHealth + " enemy health " + enemyHealth);
            if (playerHealth > enemyHealth)
            {
                winnerName = PhotonNetwork.MasterClient.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
            }
            else if (enemyHealth > playerHealth)
            {
                winnerName = PhotonNetwork.LocalPlayer.GetNext().NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
            }
            else if (playerHealth == enemyHealth)
            {
                winnerPlayerName.SetText(" It's Draw!");
            }
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer("master"));
        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            int gainedClientXp = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"];
            int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
            xpSlider.value = (totalClientXP / 2000f);
            totalTurnText.SetText(PlayerPrefs.GetInt("clientCount") + " Turns.");
            Debug.Log(" client xp " + gainedClientXp + " gained " + totalClientXP + " total " + totalClientGold + " total client gold");
            PlayerPrefs.SetInt("totalGold", totalClientGold);
            PlayerPrefs.SetInt("totalXP", totalClientXP);
            data.gold = totalClientGold;
            data.xp = totalClientXP;
            Debug.LogError(" player health " + playerHealth + " enemy health " + enemyHealth);
            if (playerHealth > enemyHealth)
            {
                winnerName = PhotonNetwork.LocalPlayer.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
            }
            else if (enemyHealth > playerHealth)
            {
                winnerName = PhotonNetwork.MasterClient.NickName;
                winnerPlayerName.SetText(winnerName + " is Victorious!");
            }
            else if (playerHealth == enemyHealth)
            {
                winnerPlayerName.SetText(" It's Draw!");
            }
            mainMenu.GetComponent<Button>().onClick.AddListener(() => LeavePlayer("client"));
        }

        string jsonData = JsonConvert.SerializeObject(data);
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");

        File.WriteAllText(path, jsonData);
        Debug.LogError(data.xp + " xp value "  + data.gold + " gold ");

        //if (PhotonNetwork.IsMasterClient)
        //{

        //    //Debug.LogError("master turn " + turnCountMaster);
        //}
        //else
        //{

        //    //Debug.LogError(" client turn " + turnCountClient);
        //}


        endTime = DateTime.Now;
        //Debug.LogError("match end time " + endTime);
        float totalSeconds = (float)(endTime - initialStartTime).TotalSeconds;
        //Debug.LogError(" total seconds " + totalSeconds);
        int minutes = (int)totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;
        //Debug.LogError(" minutes " + minutes + " seconds " + seconds);
        //matchLengthText.SetText($"{minutes} : {seconds}");
        //Debug.LogError(" winner name " + winnerName);

        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("SetUserSpendTime", RpcTarget.All,minutes, seconds);
        }
        
        timeDown.PauseTimer("down");
        timeUp.PauseTimer("up");
        StopAllCoroutines();

        Debug.LogError(" time up parent " + timeUp.transform.parent.parent.parent.name + " down " + timeDown.transform.parent.parent.parent);

        Debug.LogError(" previous end game value " + endGame + " player name " + PhotonNetwork.LocalPlayer);
        endGame = true;
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.LogError("End game after " + endGame + " player name " + PhotonNetwork.LocalPlayer);
        
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"] + " mgg ");
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"] + " cgg ");
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"] + " mgx ");
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"] + " cgx ");
    }

    [PunRPC]
    private void SetUserSpendTime(int min, int sec)
    {
        matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
        matchLengthText.SetText($"{min} : {sec}");
    }

    //[PunRPC]
    //private void ResultRPC(int min, int sec, string name)
    //{
    //    ////resultPanel.SetActive(true);
    //    //resultPanel.transform.GetChild(0).gameObject.SetActive(true);
    //    //totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
    //    //matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
    //    //winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();
    //    //winTimer = gameBoardParent.transform.GetChild(1).GetChild(1).GetComponent<TimeLeft>();
    //    //winTimer.InitTimers(30);

    //    //Debug.LogError(turnCountMaster + " turn count master " + turnCountClient + " turn count client");

    //    //totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
    //    //matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
    //    //winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();

    //    //Debug.LogError(" result find " + resultPanel.transform.Find("Match Length Value"));
    //    //if (PhotonNetwork.IsMasterClient)
    //    //{
    //    //    totalTurnText.SetText(PlayerPrefs.GetInt("masterCount") + " Turns.");
    //    //    Debug.LogError("master turn " + turnCountMaster);
    //    //}
    //    //else
    //    //{
    //    //    totalTurnText.SetText(PlayerPrefs.GetInt("clientCount") + " Turns.");
    //    //    Debug.LogError(" client turn " + turnCountClient);
    //    //}
    //    //Debug.LogError(" minutes " + min + " seconds " + sec);
    //    //matchLengthText.SetText($"{min} : {sec}");
    //    //Debug.LogError(" winner name " + name);
    //    //winnerPlayerName.SetText(name + " is Victorious!");

    //    //resultPanel.SetActive(true);
    //    resultPanel.transform.GetChild(0).gameObject.SetActive(true);
    //    totalTurnText = resultPanel.transform.GetChild(0).Find("Turn").GetChild(1).GetComponent<TMP_Text>();
    //    matchLengthText = resultPanel.transform.GetChild(0).Find("Match Length").GetChild(1).GetComponent<TMP_Text>();
    //    winnerPlayerName = resultPanel.transform.GetChild(0).Find("Victory").GetComponent<TMP_Text>();
    //    winTimer = gameBoardParent.transform.GetChild(1).GetChild(1).GetComponent<TimeLeft>();
    //    winTimer.InitTimers(30);

    //    //Debug.LogError(turnCountMaster + " turn count master " + turnCountClient + " turn count client");

    //    //Debug.LogError(" result find " + resultPanel.transform.Find("Match Length Value"));
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        totalTurnText.SetText(PlayerPrefs.GetInt("masterCount") + " Turns.");
    //        //Debug.LogError("master turn " + turnCountMaster);
    //    }
    //    else
    //    {
    //        totalTurnText.SetText(PlayerPrefs.GetInt("clientCount") + " Turns.");
    //        //Debug.LogError(" client turn " + turnCountClient);
    //    }
    //    //Debug.LogError(" minutes " + min + " seconds " + sec);
    //    matchLengthText.SetText($"{min} : {sec}");
    //    //Debug.LogError(" winner name " + name);
    //    winnerPlayerName.SetText(name + " is Victorious!");
    //    timeDown.PauseTimer("down");
    //    timeUp.PauseTimer("up");
    //    StopAllCoroutines();
    //    //Debug.LogError("end game value before " + endGame);
    //    endGame = true;
    //    //Debug.LogError("end game value after " + endGame);
    //    Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"] + " mgg ");
    //    Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"] + " cgg ");
    //    Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"] + " mgx ");
    //    Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"] + " cgx ");
    //}

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
        //Debug.Log("Clicked inside enemy wall");

        GameObject playerWall = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Wall").gameObject;
        GameObject playersFieldParent = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Game Play Field Border").gameObject;
        GameObject playerHealthObject = playerWall.transform.Find("Remaining Health").gameObject;
       
        //Debug.Log(playerHealthObject + " enemy obj");
        playerHealthObject.GetComponent<TMP_Text>().SetText(attackValue.ToString());

        if (attackValue == 0)
        {
            playerWall.GetComponent<PolygonCollider2D>().enabled = false;
            playerWall.GetComponent<Button>().enabled = false;
            string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
            //Debug.LogError(" Player field " + playerField + " local player name " + PhotonNetwork.LocalPlayer.NickName);
            int playerId = GetFieldIndex(playerField);
            //Debug.LogError(" player id " + playerId);
            GameObject bottomField = playersFieldParent.transform.Find("Bottom Field").gameObject;
            bottomField.GetComponent<Image>().sprite = playerBrokenFields[playerId];
            //Debug.LogError(" bottom image name " + bottomImage.name);
            ChangeTag();
        }

        //Debug.Log(attackValue + " remainig health ");
    }

   

    private void RemoveErrorObject()
    {
        cardError.SetActive(false);
    }

    private void AttackCard(Card attacking, GameObject attackParent, Card target, GameObject targetParent)
    {
        if (player1Turn && PhotonNetwork.IsMasterClient)
        {
            bool destroyPlayer = attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            bool destroyEnemy =  target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);
            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);
            //Debug.LogError((int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]) + " master " + " is master");
            //Debug.LogError((int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]) + " client " + " is master");
            //Debug.LogError("master xp " + (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
            //Debug.LogError("client xp " + (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);

            

            if (destroyPlayer)
            {
                int goldPlayer = attacking.gold;
                int goldOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                int goldGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"]);
                goldGainedOther += goldPlayer;
                //gainedGoldEnemy += goldPlayer;
                int totalGoldForOtherDeck = goldPlayer + goldOtherDeck;
                //PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalGoldForOtherDeck;
                //PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"] = goldGainedOther;
                properties["clientGold"] = totalGoldForOtherDeck;
                properties["clientGainedGold"] = goldGainedOther;

                //Debug.LogError(" gained gold enemy " + gainedGoldEnemy + " destroyplayer master " + attacking.gold);

                int xpPlayer = attacking.XP;
                int xpOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);
                int xpGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"]);
                xpGainedOther += xpPlayer;
                int totalXPForOtherDeck = xpPlayer + xpOtherDeck;
                //PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalXPForOtherDeck;
                properties["clientXP"] = totalXPForOtherDeck;
                properties["clientGainedXP"] = xpGainedOther;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                Debug.LogError(" gained xp other " + xpGainedOther + " destroyplayer master " + attacking.XP);
                Debug.LogError(" enemy progress bar parent " + enemyXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);


                enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherDeck);
                pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherDeck, "player");

                pv.RPC("SetGoldValue", RpcTarget.Others, "master");
            }

            if (destroyEnemy)
            {
                int goldEnemy = target.gold;
                int goldPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                int goldGainedPlayer = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"]);
                goldGainedPlayer += goldEnemy;
                int totalGoldForOtherPlayer = goldEnemy + goldPlayerDeck;
                Gold.instance.SetGold(totalGoldForOtherPlayer);
                properties["masterGold"] = totalGoldForOtherPlayer;
                properties["masterGainedGold"] = goldGainedPlayer;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                //Debug.LogError(" gained gold player " + gainedGoldPlayer + " destroy enemy master " + target.gold);

                int xpEnemy = target.XP;
                int xpPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
                int xpGainedPlayer = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"]);
                xpGainedPlayer += xpEnemy;
                int totalXPForOtherPlayer = xpEnemy + xpPlayerDeck;
                //Gold.instance.SetGold(totalGoldForOtherPlayer);
                properties["masterXP"] = totalXPForOtherPlayer;
                properties["masterGainedXP"] = xpGainedPlayer;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                //Debug.LogError(" gained xp player " + gainedXPPlayer + " destroy enemy master " + target.XP);
                Debug.LogError(" player progress bar parent " + playerXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);

                playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherPlayer);
                pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherPlayer, "enemy");
            }



            attackingcard.SetAttackValue(true);

            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId);
        }
        else if (!player1Turn && !PhotonNetwork.IsMasterClient)
        {
            bool destroyPlayer = attacking.DealDamage(target.attack, attackParent.transform.GetChild(0).gameObject);
            bool destroyEnemy = target.DealDamage(attacking.attack, targetParent.transform.GetChild(0).gameObject);

            int attackcardParentId = int.Parse(attackParent.name.ToString().Split(" ")[2]);
            int targetcardParentId = int.Parse(targetParent.name.ToString().Split(" ")[2]);
            //Debug.LogError((int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]) + " master " + " is client");
            //Debug.LogError((int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]) + " client " + " is client");

            if (destroyPlayer)
            {
                int goldPlayer = attacking.gold;
                int goldOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                int goldGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedGold"]);
                goldGainedOther += goldPlayer;
                int totalGoldForOtherDeck = goldPlayer + goldOtherDeck;
                properties["masterGold"] = totalGoldForOtherDeck;
                properties["masterGainedGold"] = goldGainedOther;
                //Debug.LogError(" gained gold enemy " + gainedGoldEnemy + " destroy player in client " + attacking.gold);

                int xpPlayer = attacking.XP;
                int xpOtherDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
                int xpGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGainedXP"]);
                xpGainedOther += xpPlayer;
                int totalXPForOtherDeck = xpPlayer + xpOtherDeck;
                properties["masterXP"] = totalXPForOtherDeck;
                properties["masterGainedXP"] = xpGainedOther;

                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                //Debug.LogError(" gained xp enemy " + gainedXPEnemy + " destroy player in client " + attacking.XP);
                Debug.LogError(" enemy progress bar parent " + enemyXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);

                enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherDeck);
                pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherDeck, "player");

                //pv.RPC("SetGoldValue", RpcTarget.Others, "client", gainedGoldEnemy, totalGoldForOtherDeck, gainedXPEnemy, totalXPForOtherDeck);
            }

            if (destroyEnemy)
            {
                int goldEnemy = target.gold;
                int goldPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                int goldGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedGold"]);
                goldGainedOther += goldEnemy;
                int totalGoldForOtherPlayer = goldEnemy + goldPlayerDeck;
                //Debug.LogError(" gained gold player " + gainedGoldPlayer + " destroy enemy in client " + target.gold);
                Gold.instance.SetGold(totalGoldForOtherPlayer);
                properties["clientGold"] = totalGoldForOtherPlayer;
                properties["clientGainedGold"] = goldGainedOther;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                int xpEnemy = target.XP;
                int xpPlayerDeck = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);
                int xpGainedOther = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGainedXP"]);
                xpGainedOther += xpEnemy;
                int totalXPForOtherPlayer = xpEnemy + xpPlayerDeck;
                //Debug.LogError(" gained xp player " + gainedXPPlayer + " destroy enemy in client " + target.XP);
                //Gold.instance.SetGold(totalGoldForOtherPlayer);
                properties["clientXP"] = totalXPForOtherPlayer;
                properties["clientGainedXP"] = xpGainedOther;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                Debug.LogError(" player progress bar parent " + playerXPProgressBar.transform.parent.parent.name + " is master " + PhotonNetwork.IsMasterClient);

                playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXPForOtherPlayer);
                pv.RPC("SetProgessBar", RpcTarget.Others, totalXPForOtherPlayer, "enemy");
            }

            attackingcard.SetAttackValue(true);

            pv.RPC("AttackCardRPC", RpcTarget.Others, attackcardParentId, targetcardParentId);
        }
    }

    [PunRPC]
    private void SetProgessBar(int progressValue, string name)
    {
        if(name == "player")
        {
            playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(progressValue);
            Debug.LogError(" player progress bar parent " + playerXPProgressBar.transform.parent.parent.name + " master or client " + PhotonNetwork.IsMasterClient + " progress value " + progressValue);
        }
        else if(name == "enemy")
        {
            enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(progressValue);
            Debug.LogError(" enemy progress bar parent " + enemyXPProgressBar.transform.parent.parent.name + " master or client " + PhotonNetwork.IsMasterClient + " progress value " + progressValue);
        }
    }

    [PunRPC]
    private void SetGoldValue(string player)
    {
        //Debug.LogError("SetGoldValue called " + player + " player " + gold  +  " gold value " + totalGold + " total gold value " + xp + " xp value " + totalXP + " total xp value " );
        if(player == "master")
        {
            Debug.LogError(" inside master in SetGoldValue");
            int totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
            int totalXP = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]); ;
            Gold.instance.SetGold(totalGold);
            Debug.LogError("master gold "+ totalGold + " xp "+ totalXP);
        }
        else if(player == "client")
        {
            Debug.LogError(" inside client in SetGoldValue");
            //gainedGoldEnemy = gold;
            int totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
            int totalXP = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]); 
            Gold.instance.SetGold(totalGold);
            Debug.LogError("master client " + totalGold + " xp " + totalXP);
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

        //Debug.LogError((int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]) + " master " + " is rpc");
        //Debug.LogError((int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]) + " client " + " is rpc");

    }

    public void ShowHiddenCard()
    {
        GameObject enemyField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;

        for (int i = 0; i < enemyField.transform.childCount; i++)
        {
            //if (playerField.transform.GetChild(i).childCount == 1)
            //{
            //    Card currentCard = playerField.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Card>();
            //    currentCard.SetAttackValue(false);
            //    currentCard.SetDropPosition(1);
            //}
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
        Debug.Log("bid text " + bidText);

        if (bidText <= initialGold)
        {
            customProp["enterdNum"] = bidText;
            bidButton.interactable = false;
            if (PhotonNetwork.IsMasterClient)
            {
                int initialGoldvalue = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
                int gold = (initialGoldvalue - bidText);
                Gold.instance.SetGold(gold);
                properties["masterGold"] = gold;
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                int initialGoldvalue = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
                int gold = (initialGoldvalue - bidText);
                Gold.instance.SetGold(gold);
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

            Debug.LogError(" end game " + endGame + " player name " + PhotonNetwork.LocalPlayer);
            if (!endGame)
            {
                if (player1Turn && PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    //Debug.LogError("master active ");
                    turnCountMaster = PlayerPrefs.GetInt("masterCount") + 1;
                    PlayerPrefs.SetInt("masterCount", turnCountMaster);
                    //turnCountMaster++;
                    //Debug.LogError("change player master to client " + turnCountMaster);
                    pv.RPC("ChangePlayerTurn", RpcTarget.All, false);
                }
                else if (!player1Turn && !PhotonNetwork.IsMasterClient)
                {
                    SetActivePlayer(nextPlayerID);
                    //Debug.LogError(" not master client ");
                    //turnCountClient++;
                    turnCountClient = PlayerPrefs.GetInt("clientCount") + 1;
                    PlayerPrefs.SetInt("clientCount", turnCountClient);
                    //Debug.LogError("change player client to master " + turnCountClient);
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

    private void ChangeTag()
    {
        GameObject playerField = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Player Field").gameObject;
        for(int i = 0; i < playerField.transform.childCount; i++)
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
        Timer.isBiddingTime = false;
        //RemoveProperties();
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
            countDownPanel.SetActive(true);
            initialStartTime = DateTime.Now;
            //Debug.LogError(" Initial start time " + initialStartTime);

            if (gameBoardParent == null)
                gameBoardParent = GameObject.Find("Game Board Parent");

            if (gameBoardParent.transform.GetChild(0))
            {
                loadingPanel = gameBoardParent.transform.GetChild(0).gameObject;
                loadingPanel.SetActive(false);
            }

            GameBoardManager board = gameBoardParent.transform.GetChild(1).GetComponent<GameBoardManager>();
            GameBoardManager[] gameboards = GameObject.FindObjectsOfType<GameBoardManager>();
            
           
            board.gameObject.SetActive(true);

            resultPanel = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Result panel").gameObject;

            //Debug.LogError("result panel " + resultPanel + " " + resultPanel.transform.parent.name + " " + resultPanel.transform.parent.parent.name);

            if (countdownTimer == null)
                countdownTimer = countDownPanel.GetComponent<Timer>();

            if (PhotonNetwork.IsMasterClient)
                countdownTimer.InitTimers("GC", 5);

            //Debug.LogError("player deck properties " + (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"]);
            int playerDeckProfileId = (int)PhotonNetwork.LocalPlayer.CustomProperties["deckId"] - 1;
            string playerField = (string)PhotonNetwork.LocalPlayer.CustomProperties["deckField"];
            Player currPlayer = PhotonNetwork.LocalPlayer;
            Player nextPlayer = currPlayer.GetNext();
            //Debug.LogError("next deck properties " + (int)nextPlayer.CustomProperties["deckId"]);
            int opponentDeckProfileId = (int)nextPlayer.CustomProperties["deckId"] - 1;
            string opponentField = (string)nextPlayer.CustomProperties["deckField"];

            //Debug.LogError(playerDeckProfileId + " deck id " + playerField + " field name");
            //Debug.LogError(opponentDeckProfileId + " deck id " + opponentField + " field name");

            //Debug.LogError("master xp " + (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"]);
            //Debug.LogError("client xp " + (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"]);

            int playerId = GetFieldIndex(playerField);
            int opponentId = GetFieldIndex(opponentField);

            bottomImage.sprite = playerFields[playerId];
            topImage.sprite = playerFields[opponentId];
            Debug.Log(playerId + " player id " + opponentId + " oppenent id");
            bottomImage.GetComponent<SetFieldPosition>().SetObjectSize(playerId);
            bottomImage.GetComponent<SetFieldPosition>().SetObjectPosition(playerId, "down");
            topImage.GetComponent<SetFieldPosition>().SetObjectSize(opponentId);
            topImage.GetComponent<SetFieldPosition>().SetObjectPosition(opponentId, "up");


            downProfileIamge.GetComponent<Image>().sprite = profileImages[playerDeckProfileId];
            upProfileImage.GetComponent<Image>().sprite = profileImages[opponentDeckProfileId];
            }
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
                    initialGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
                    Debug.Log(" player " + playerXPProgressBar + " client " + enemyXPProgressBar);
                    playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(masterXP);
                    enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(clientXP);
                    Debug.Log(" master ");
                }
                else if (!PhotonNetwork.IsMasterClient)
                {
                    initialGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
                    Debug.Log(" player " + playerXPProgressBar + " client " + enemyXPProgressBar);
                    enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(masterXP);
                    playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(clientXP);
                    Debug.Log(" client ");
                }
            }

            Invoke("RemoveProperties", 0.5f);
        
        //Debug.LogError(" room property update ");
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

        Debug.LogError(" End game " + endGame + " player name " + PhotonNetwork.LocalPlayer);
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
        Debug.LogError(" End Game " + endGame + " player name " + PhotonNetwork.LocalPlayer);
        if (!endGame)
        {
            leftPlayerPanel.SetActive(true);
            leftPlayerText.SetText(otherPlayer.NickName + " Was left the game. Press Continue to Go Skirmish screen.");
            StopTimers();
            StopAllCoroutines();
        }
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

    int GetFieldIndex(string fieldName)
    {
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

    public void GetDamegeToWall()
    {
        //Debug.LogError("Damage called");
        //Debug.LogError(GameManager.instance.clicked + " Instance value");
        //Debug.LogError(EventSystem.current.currentSelectedGameObject + " current game object");
    }

    public void LeavePlayer(string player)
    {
        if(player == "master")
        {
            Debug.LogError("called in master  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
            SceneManager.LoadScene(1);
        }
        else if( player == "client")
        {
            Debug.LogError("called in master  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
            SceneManager.LoadScene(1);
        }
        //PhotonNetwork.CurrentRoom.CustomProperties.Clear();
        //PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.Disconnect();
    }

    private void LeaveBothPlayer()
    {
        Debug.LogError("called in both  " + PhotonNetwork.LocalPlayer.NickName + " end game value " + endGame);
        SceneManager.LoadScene(1);
    }
}