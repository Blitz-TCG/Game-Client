using UnityEngine;

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
    public AudioSource chatSendButtonStandard;
    public AudioSource loadingSoundLoop;

    //private void Awake()
    //{
    //    Debug.Log("Awake called ");
    //    if (transform.parent == null)
    //    {
    //        DontDestroyOnLoad(gameObject);

    //        if (instance == null)
    //        {
    //            instance = this;
    //        }
    //        else if (instance != this)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        // If not the root, destroy it, or handle it as appropriate for your use case
    //        Destroy(gameObject);
    //    }
    //}

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

    public void ChatSendButtonStandard()
    {
        chatSendButtonStandard.Play();
    }

    public void LoadingSoundLoopPlay()
    {
        loadingSoundLoop.Play();
    }
    public void LoadingSoundLoopStop()
    {
        loadingSoundLoop.Stop();
    }
}
