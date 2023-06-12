using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameboardHoverAndSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image gameboardImage;
    public Image gameboardSelected;

    public TMP_Text gameboardNameAvailable;
    public TMP_Text gameboardNameSelected;

    public GameObject gameboardsPopup;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameboardImage != null && !gameboardImage.color.ToString().Contains("0.502"))
        {
            gameboardImage.color = new Vector4(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
        }
        else if (gameboardImage != null && gameboardImage.color.ToString().Contains("0.502"))
        {
            gameboardsPopup.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameboardImage != null && !gameboardImage.color.ToString().Contains("0.502"))
        {
            gameboardImage.color = new Vector4(1f, 1f, 1f, 1f);
        }
        else if (gameboardImage != null && gameboardImage.color.ToString().Contains("0.502"))
        {
            gameboardsPopup.SetActive(false);
        }
    }

    public void SelectGameboard()
    {
        if (!gameboardImage.color.ToString().Contains("0.502"))
        {
            gameboardSelected.sprite = gameboardImage.sprite;
            gameboardNameSelected.text = gameboardNameAvailable.text;
        }
    }
}
