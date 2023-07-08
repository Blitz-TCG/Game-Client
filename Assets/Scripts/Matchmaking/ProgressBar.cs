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
        progressImage = GetComponent<Image>();
    }

    public void SetFillValue(float value)
    {
        progressImage = GetComponent<Image>();
        float fillAmount = value / totalValue;
        progressImage.fillAmount = fillAmount;
    }
}
