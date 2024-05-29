using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCards : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
    private void OnDisable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
