using System.Collections;
using UnityEngine;

public class TimeLeft : MonoBehaviour
{
    public int timerLeft = 60;
    public static bool timeUp = false;

    private int currentTime;
    private Coroutine timerCoroutine;

    public void InitTimers(int time)
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        timerLeft = time;

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
