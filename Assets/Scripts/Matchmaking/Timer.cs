using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
            Debug.LogError(" master " + PhotonNetwork.IsMasterClient);
            Debug.LogError(" Timer " + TimeRemaining());
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

    public void OnTimerRuns()
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
                Debug.LogError(name + " panel name " + time + " time ");
                isGlobalCountDown = true;
                Countdown = time;
                SetStartTime();
                Initialize();
            }
            else if (name == "BT")
            {
                Debug.LogError(name + " panel name " + time + " time ");
                isBiddingTime = true;
                Countdown = time;
                SetStartTime();
                Initialize();
            }
            else if (name == "CB")
            {
                Debug.LogError(name + " panel name " + time + " time ");
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
        Debug.Log("initialize called ");
        int propStartTime;
        if (TryGetStartTime(out propStartTime))
        {
            Debug.Log(propStartTime + " start prop time ");
            this.startTime = propStartTime;
            this.isTimerRunning = TimeRemaining() > 0;

            if (this.isTimerRunning)
            {
                Debug.Log("inside time remainig ");
                OnTimerRuns();
            }
        }
    }

    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - this.startTime;
        return this.Countdown - timer / 1000f;
    }

    public static bool TryGetStartTime(out int startTimestamp)
    {
        Debug.Log(" try get start time");
        startTimestamp = PhotonNetwork.ServerTimestamp;

        object startTimeFromProps;
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTime, out startTimeFromProps) + " value");
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTime, out startTimeFromProps))
        {
            Debug.Log(startTimestamp + " start stamp ");
            startTimestamp = (int)startTimeFromProps;
            Debug.Log(startTimestamp + " start stamp ");
            return true;
        }
        return false;
    }

    public static void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);
        Debug.Log(startTime + " start time " + wasSet);
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

