using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEnterExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   
{
    public CursorManager cursorManagerFriends;
    public void OnPointerEnter(PointerEventData eventData) 
    {
        cursorManagerFriends.CursorSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorManagerFriends.CursorNormal();
    }


}
