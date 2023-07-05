using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class DropFieldCard : MonoBehaviourPunCallbacks
{
    private void OnMouseDown()
    {
        Debug.Log("Test");
        Debug.Log("Mouse down on " + transform.name);
    }
}
