using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuTooltips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static MainMenuTooltips instance;

    [Header("Button References")]
    [SerializeField]
    private GameObject Tooltip;

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (Tooltip != null)
        {
            if (MainMenuUIManager.instance != null &&
                MainMenuUIManager.instance.settingsHelpTextEnabled != null &&
                !MainMenuUIManager.instance.settingsHelpTextEnabled.activeSelf)
            {
                if (name == "Skirmish" || name == "Deck Builder" || name == "Exit"
                    || name == "Global Chat Switch" || name == "Global Chat" || name == "Whisper Chat")
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
            if (MainMenuUIManager.instance != null &&
                MainMenuUIManager.instance.settingsHelpTextEnabled != null &&
                !MainMenuUIManager.instance.settingsHelpTextEnabled.activeSelf)
            {
                if (name == "Skirmish" || name == "Deck Builder" || name == "Exit"
                    || name == "Global Chat Switch" || name == "Global Chat" || name == "Whisper Chat")
                {
                    Tooltip.SetActive(false);
                }
            }
        }
    }
}
