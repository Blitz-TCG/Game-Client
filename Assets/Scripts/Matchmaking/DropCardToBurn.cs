using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropCardToBurn : MonoBehaviourPunCallbacks, IDropHandler
{
    #region Variables
    private PhotonView pv;
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
        Debug.Log(draggable + " ** draggable " + draggable.previousParent + " draggable.previousParent " + draggable.previousSubParent + " draggable.previousSubParent");
        if(DragMiniCards.turnEnd)
        {
            DragMiniCards.turnEnd = false;
            return;
        }
        if (draggable.previousParent != "" && draggable.previousSubParent != "")
        {
            if (draggable.previousParent == "Player Hand")
            {
                pv = draggable.GetComponent<PhotonView>();
                Debug.Log(draggable + " player hand as a main parent");
                Destroy(draggable.gameObject);
                GameObject CardParent = canvas.transform.Find("Game Board Parent").GetChild(1).GetChild(0).Find($"{draggable.previousParent}").gameObject;
                int id = int.Parse(draggable.previousSubParent.Split(" ")[2]);
                CardParent.transform.GetChild(id - 1).gameObject.SetActive(false);
                pv.RPC("DostroyCardOnOthers", RpcTarget.Others, id);
            }
        }
    }
}
