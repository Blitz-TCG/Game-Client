using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarotControl : MonoBehaviour
{
    void Start()
    {
        GetComponentInChildren<TMP_SelectionCaret>().raycastTarget = false;
    }

}
