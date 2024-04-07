using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManagerToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static DeckManagerToolTips instance;

    [Header("Button References")]
    [SerializeField]
    private GameObject Tooltip;

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (Tooltip != null)
        {
            if (DeckManager.instance != null && PlayerPrefs.HasKey("tooltipDisabled") == false)
            {
                if (name == "Attack" || name =="Ergo Token Amount" || name == "HP"|| name == "Gold"|| name =="XP" ||
                     name == "Field Limit" || name == "Clan"|| name =="Level" || name == "Field Position")
                {
                    Tooltip.SetActive(true);
                }
            }
        }
    }


    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (Tooltip != null)
        {
            if (DeckManager.instance != null && PlayerPrefs.HasKey("tooltipDisabled") == false)
            {
                if (name == "Attack" || name == "Ergo Token Amount" || name == "HP" || name == "Gold" || name == "XP" ||
                     name == "Field Limit" || name == "Clan" || name == "Level" || name == "Field Position")
                {
                    Tooltip.SetActive(false);
                }
            }
        }
    }
}
