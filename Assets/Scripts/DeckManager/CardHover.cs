using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CardHover : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{

    public Image[] cardImages;
    public Image imageResult;
    public Image frameResult;
    private bool isMouseOver = false;

    public void Start()
    {
        cardImages = gameObject.transform.parent.GetComponentsInChildren<Image>();
        imageResult = Array.Find(cardImages, element => element.name == "Image" && !element.color.ToString().Contains("0.502"));
        frameResult = Array.Find(cardImages, element => element.name == "Frame" && !element.color.ToString().Contains("0.502"));
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void Update()
    {
        if (isMouseOver)
        {
            if (Draggable.hoverDisabled == false)
            {

                if (imageResult != null && frameResult != null)
                {
                    imageResult.color = new Vector4(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                    frameResult.color = new Vector4(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                }
            }
        }
        else if (!isMouseOver)
        {
            if (Draggable.hoverDisabled == false)// && cardImages != null)
            {

                if (imageResult != null && frameResult != null)
                {
                    imageResult.color = new Vector4(1f, 1f, 1f, 1f);
                    frameResult.color = new Vector4(1f, 1f, 1f, 1f);
                }
            }
        }
    }


    /*    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Draggable.hoverDisabled == false)
        {

            if (imageResult != null && frameResult != null)
            {
                imageResult.color = new Vector4(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                frameResult.color = new Vector4(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Draggable.hoverDisabled == false)// && cardImages != null)
        {

            if (imageResult != null && frameResult != null)
            {
                imageResult.color = new Vector4(1f, 1f, 1f, 1f);
                frameResult.color = new Vector4(1f, 1f, 1f, 1f);
            }
        }
    }*/
}
