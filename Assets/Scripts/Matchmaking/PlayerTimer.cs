using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public static int timerLeft;
    public static string whichTimer;
    public bool completeTime = false;
    public bool isTimerOn;
    public TMP_Text seconds;
    public TMP_Text minute;
    public GameObject resultPanel;
    public int currentTime;
    private Coroutine timerCoroutine;

    public void RefreshTime()
    {
        string min = Mathf.FloorToInt(currentTime / 60).ToString("0");
        string sec = Mathf.FloorToInt(currentTime % 60).ToString("00");

        minute.text = (min);
        seconds.text = (sec);
    }

    public void InitTimers(string name, int time)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        currentTime = time;
        whichTimer = name;
        RefreshTime();
        timerCoroutine = StartCoroutine(Timers());
    }

    public void EndBid()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentTime = 0;
        RefreshTime();
    }

    public IEnumerator Timers()
    {
        yield return new WaitForSeconds(1);
        currentTime -= 1;

        if (currentTime <= 0)
        {
            completeTime = true;
        }
        else
        {
            RefreshTime();
            timerCoroutine = StartCoroutine(Timers());
        }
    }

    public void PauseTimer(string dir)
    {
        //Debug.LogError(" Pause timer called with direction " + dir);
        StopAllCoroutines();
        //Debug.LogError(" timer coroutine " + timerCoroutine);
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        if (dir == "down")
        {
            PlayerPrefs.SetInt("Down", currentTime);
        }
    }

    private void Start()
    {
        currentTime = 180;
    }
}