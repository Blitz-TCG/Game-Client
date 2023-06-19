using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image progressImage;
    private float totalValue = 2000f;

    private void Start()
    {
       
    }

    public void SetFillValue(float value)
    {
        Debug.Log(" value " + value);
        float fillAmount = value / totalValue;
        progressImage = GetComponent<Image>();
        Debug.Log(progressImage + " progress image ");
        progressImage.fillAmount = fillAmount;
    }
}
