using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropMiniCard : MonoBehaviourPunCallbacks, IDropHandler
{
    #region Variables
    private PhotonView pv;
    public static bool dropEnd = false;
    public static bool dropOnField = false;
    DragMiniCards draggable;
    private Canvas canvas;
    #endregion

    private void Start()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        draggable = eventData.pointerDrag.GetComponent<DragMiniCards>();
        if (draggable != null && transform.childCount == 0 && draggable.previousParent != "Player Field")
        {
            draggable.parentAfterDrag = this.transform;
            dropOnField = true;
        }

        dropEnd = true;
        pv = draggable.GetComponent<PhotonView>();

        GameObject gameObj = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find("Player Field").gameObject;

        for (int i = 0; i < gameObj.transform.childCount; i++)
        {
            if (gameObj.transform.GetChild(i).childCount == 1)
            {
                gameObj.transform.GetChild(i).GetChild(0).transform.GetComponent<Animator>().SetBool("Scale", false);
            }
        }
    }
}
