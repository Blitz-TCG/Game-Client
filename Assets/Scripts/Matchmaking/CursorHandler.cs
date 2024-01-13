using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.instance.AudioHoverButtonStandard();
        CursorManager.instance.CursorSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.instance.CursorNormal();
    }
}
