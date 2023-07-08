using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public Texture2D cursorTextureDrag;
    public Texture2D cursorTextureIbeam;
    public Texture2D cursorTextureSelect;
    public Vector2 hotspotDrag = new Vector2(12.8f, 9.6f);
    public Vector2 hotspotIbeam = new Vector2(16f, 16f);
    public Vector2 hotspotSelect = new Vector2(5.4f, 5.4f);
    public CursorMode cursorMode = CursorMode.Auto;

    public AudioSource audioHoverButtonStandard;
    public AudioSource audioClickButtonStandard;

    public void CursorIbeam()
    {
        Cursor.SetCursor(cursorTextureIbeam, hotspotIbeam, cursorMode);
    }

    public void CursorDrag()
    {
        Cursor.SetCursor(cursorTextureDrag, hotspotDrag, cursorMode);
    }
    public void CursorSelect()
    {
        Cursor.SetCursor(cursorTextureSelect, hotspotSelect, cursorMode);
    }

    public void CursorNormal()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    public void AudioHoverButtonStandard()
    {
        audioHoverButtonStandard.Play();
    }

    public void AudioClickButtonStandard()
    {
        audioClickButtonStandard.Play();
    }


    //Logic for buttons that can be disabled --- in hindsight, this could have been avoided, but not worth reworking at this point
    public void CursorSelectDisabledButton()
    {
        if (!DeckManager.instance.editDeckObject.activeSelf)
        {
            GameObject editDeck = GameObject.FindWithTag("DisabledButtonEdit");
            Button editButton = editDeck.GetComponent<Button>();
            if (editDeck != null && editButton.enabled)
            {
                Cursor.SetCursor(cursorTextureSelect, hotspotSelect, cursorMode);
            }
        }
        else if (DeckManager.instance.editDeckObject.activeSelf)
        {
            GameObject deleteDeck = GameObject.FindWithTag("DisabledButtonDelete");
            Button deleteButton = deleteDeck.GetComponent<Button>();
            if (deleteDeck != null && deleteButton.enabled)
            {
                Cursor.SetCursor(cursorTextureSelect, hotspotSelect, cursorMode);
            }
        }
    }

    public void AudioHoverButtonDisabledButton()
    {
        if (!DeckManager.instance.editDeckObject.activeSelf)
        {
            GameObject editDeck = GameObject.FindWithTag("DisabledButtonEdit");
            Button editButton = editDeck.GetComponent<Button>();
            if (editDeck != null && editButton.enabled)
            {
                audioHoverButtonStandard.Play();
            }
        }
        else if (DeckManager.instance.editDeckObject.activeSelf)
        {
            GameObject deleteDeck = GameObject.FindWithTag("DisabledButtonDelete");
            Button deleteButton = deleteDeck.GetComponent<Button>();
            if (deleteDeck != null && deleteButton.enabled)
            {
                audioHoverButtonStandard.Play();
            }
        }
    }
    public void AudioClickButtonDisabledButton()
    {
        if (!DeckManager.instance.editDeckObject.activeSelf)
        {
            GameObject editDeck = GameObject.FindWithTag("DisabledButtonEdit");
            Button editButton = editDeck.GetComponent<Button>();
            if (editDeck != null && editButton.enabled)
            {
                audioClickButtonStandard.Play();
            }
        }
        else if (DeckManager.instance.editDeckObject.activeSelf)
        {
            GameObject deleteDeck = GameObject.FindWithTag("DisabledButtonDelete");
            Button deleteButton = deleteDeck.GetComponent<Button>();
            if (deleteDeck != null && deleteButton.enabled)
            {
                audioClickButtonStandard.Play();
            }
        }
    }
}
