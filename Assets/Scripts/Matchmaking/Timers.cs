using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Timers : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    //public Text timeText;

    public static bool isGlobalCountDown = false;
    public static bool isBiddingTime = false;
    public static bool isCompletedBid = false;
    public static bool isAfterCompletedBid = false;
    public static int timerLeft = 60;
    private float Countdown;

    private bool isTimerRunning;
    private int startTime;
    private string trackPanel = "";
    public bool isTimerOn;
    public TMP_Text seconds;
    public TMP_Text minute;
    //public GameObject gameBoard;
    private float currentTime;
    private Coroutine timerCoroutine;
    private PhotonView view;

    private void Start()
    {
        // Starts the timer automatically
        Debug.Log(transform.parent.parent.parent.name + " game board name *** ");
    }

    public void PauseTimer()
    {
        StopAllCoroutines();
    }

    public void InitTimers(string name, int time)
    {
        Debug.Log("*** init timer");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("*** master player " + name);
            if (name == "GC")
            {
                timerIsRunning = true;
                Debug.Log(name + " *** panel name " + time + " time ");
                isGlobalCountDown = true;
                timeRemaining = time;
            }
            else if (name == "BT")
            {
                timerIsRunning = true;
                //Debug.LogError(name + " panel name " + time + " time ");
                isBiddingTime = true;
                timeRemaining = time;
            }
            else if (name == "CB")
            {
                timerIsRunning = true;
                //Debug.LogError(name + " panel name " + time + " time ");
                isCompletedBid = true;
                timeRemaining = time;
            }

            trackPanel = name;
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

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                EndTime();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        string min = Mathf.FloorToInt(timeToDisplay / 60).ToString("0");
        string sec = Mathf.FloorToInt(timeToDisplay % 60).ToString("00");

        minute.SetText(min);
        seconds.SetText(sec);
    }

    //public void RefreshTime(float currentTime)
    //{
    //    string min = Mathf.FloorToInt(currentTime / 60).ToString("0");
    //    string sec = Mathf.FloorToInt(currentTime % 60).ToString("00");

    //    minute.SetText(min);
    //    seconds.SetText(sec);
    //}
}