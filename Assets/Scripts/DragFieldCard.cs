using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class DragFieldCard : MonoBehaviour//,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    private Canvas canvas;
    private Vector3 screenPoint;
    private List<CardDetails> cardDetails;
    private bool previousVal;
    private bool currVal;

    public string previousParent;
    public string previousSubParent;
    public static string endParent;
    public static string endSubParent;
    public static GameObject obj;
    public static bool dragEnd;
   
    public bool isDragging = false;


    private void OnMouseDown()
    {
        Debug.Log("Test");
        Debug.Log("Mouse down on " + transform.name);
        //GameManager.instance.clicked = 1;
    }

}
