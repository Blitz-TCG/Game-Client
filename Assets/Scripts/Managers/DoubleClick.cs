using System.Collections;
using UnityEngine;
using System.Timers;

public class DoubleClick : MonoBehaviour
{
    private float firstLeftClickTime;
    private float timeBetweenLeftClick = 0.25f;
    private bool isTimeCheckAllowed = true;
    private bool doubleClick = false;
    private int leftClickNum = 0;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            leftClickNum += 1;
        }
        if (leftClickNum == 1 && isTimeCheckAllowed)
        {
            firstLeftClickTime = Time.time;
            StartCoroutine(DetectDoubleLeftClick());
        }

    }

    IEnumerator DetectDoubleLeftClick()
    {
        isTimeCheckAllowed = false;
        while (Time.time < firstLeftClickTime + timeBetweenLeftClick)
        {
            if (leftClickNum == 2)
            {
                Debug.Log("double click");
                doubleClick = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        if (doubleClick == false)
        {
            Debug.Log("single click");
        }

        leftClickNum = 0;
        doubleClick = false;
        isTimeCheckAllowed = true;
    }
}
