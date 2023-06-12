using Photon.Pun;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviourPunCallbacks
{
    public static bool isGlobalCountDown = false;
    public static bool isBiddingTime = false;
    public static bool isCompletedBid = false;
    public static bool isAfterCompletedBid = false;
    public static int timerLeft = 60;
    public const string CountdownStartTime = "StartTime";
    private float Countdown;
    
    private bool isTimerRunning;
    private int startTime;
    private string trackPanel = "";
    public bool isTimerOn;
    public TMP_Text seconds;
    public TMP_Text minute;
    public GameObject gameBoard;
    private float currentTime;
    private Coroutine timerCoroutine;
    private PhotonView view;

    private void Start()
    {
    }

    public void Update()
    {
        if (!this.isTimerRunning) return;
        if(PhotonNetwork.IsMasterClient)
        {
            float countdown = TimeRemaining();
            if (countdown > 0.0f)
            {
                view = gameBoard.GetComponent<PhotonView>();
                view.RPC("UpdateTime", RpcTarget.All, countdown);
            }

            if (countdown <= 0)
            {
                EndTime();
            }
        }
    }

    private void EndTime()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (trackPanel == "GC")
            {
                isGlobalCountDown = false;
                isBiddingTime = true;
            }
            else if (trackPanel == "BT")
            {
                isBiddingTime = false;
                isCompletedBid = true;
            }
            else if (trackPanel == "CB")
            {
                isCompletedBid = false;
                isAfterCompletedBid = true;
            }
        }
    }

    private void OnTimerRuns()
    {
        this.isTimerRunning = true;
        this.enabled = true;
    }


    public void RefreshTime(float currentTime)
    {
        string min = Mathf.FloorToInt(currentTime / 60).ToString("0");
        string sec = Mathf.FloorToInt(currentTime % 60).ToString("00");

        minute.SetText(min);
        seconds.SetText(sec);
    }

    public void InitTimers(string name, int time)
    {
        if(PhotonNetwork.IsMasterClient) 
        {
            if (name == "GC")
            {
                isGlobalCountDown = true;
                Countdown = time;
                SetStartTime();
                Initialize();
            }
            else if (name == "BT")
            {
                isBiddingTime = true;
                Countdown = time;
                SetStartTime();
                Initialize();
            }
            else if (name == "CB")
            {
                isCompletedBid = true;
                Countdown = time;
                SetStartTime();
                Initialize();
            }

            trackPanel = name;
        }
    }

    private void Initialize()
    {
        int propStartTime;
        if (TryGetStartTime(out propStartTime))
        {
            this.startTime = propStartTime;
            this.isTimerRunning = TimeRemaining() > 0;

            if (this.isTimerRunning)
                OnTimerRuns();
        }
    }

    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - this.startTime;
        return this.Countdown - timer / 1000f;
    }

    public static bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;

        object startTimeFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTime, out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            return true;
        }
        return false;
    }

    public static void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {Timer.CountdownStartTime, (int)PhotonNetwork.ServerTimestamp}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

    }

    public void PauseTimer()
    {
        StopAllCoroutines();
    }
}

