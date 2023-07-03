using System;
using System.Collections;
using UnityEngine;

public class TimeLeft : MonoBehaviour
{
    public int timerLeft = 60;
    public bool timeUp = false;

    private int currentTime;
    private Coroutine timerCoroutine;

    public void InitTimers(int time)
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        Debug.LogError("Start time " + DateTime.Now);
        timerLeft = time;
        Debug.LogError(time + " time value " + timerLeft);

        currentTime = timerLeft;
        timerCoroutine = StartCoroutine(Timers());
    }

    public void EndBid()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentTime = 0;
    }

    private IEnumerator Timers()
    {
        yield return new WaitForSeconds(1);
        currentTime -= 1;


        if (currentTime < 0)
        {
            Debug.LogError("End time " + DateTime.Now);
            timeUp = true;
        }
        else
        {
            timerCoroutine = StartCoroutine(Timers());
        }
    }

    public void PauseTimer()
    {
        timeUp = false;
        StopAllCoroutines();
    }
}
