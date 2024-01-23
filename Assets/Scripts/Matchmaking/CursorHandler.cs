using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorManager manager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.AudioHoverButtonStandard();
        manager.CursorSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.CursorNormal();
    }
}
