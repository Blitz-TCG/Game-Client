using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;

public class CursorHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorManager manager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.CursorSelect();
        if (transform.name == "Enemy Profile" || transform.name == "Player Profile") return;
        manager.AudioHoverButtonStandard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.CursorNormal();
    }

    public void AudioClickHandler()
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.AudioClickButtonStandard();
    }

}
