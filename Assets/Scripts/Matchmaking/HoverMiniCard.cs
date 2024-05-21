using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverMiniCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isEnable = false;

    private Color normalColor = Color.white;
    private Color hoverColor = new Vector4(241f / 255f, 241f / 255f, 85f / 255f, 255f / 255f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("*** enable called " + isEnable);
        if (isEnable)
        {
            Debug.Log("*** enable true in enter ");
            gameObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = hoverColor;
            gameObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("*** enable called " + isEnable);
        if (isEnable)
        {
            if(GetComponent<ClickedMiniCard>().enabled && GetComponent<ClickedMiniCard>().isClicked)
            {
                return;
                //Debug.Log("*** enable true in exit ");
                //gameObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = normalColor;
                //gameObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = normalColor;
            }
            else
            {
                Debug.Log("*** enable true in exit ");
                gameObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = normalColor;
                gameObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = normalColor;
            }
            
        }
    }
}
