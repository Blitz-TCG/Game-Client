using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ClickedMiniCard : MonoBehaviour
{
    public bool isEnable = false;
    public bool isClicked = false;
    private Color clickedColor = new Vector4(150f / 255f, 220f / 255f, 220f / 255f, 255f / 255f);

    public void ClickedCard()
    {
        if (GameBoardManager.player1Turn && PhotonNetwork.IsMasterClient && isEnable)
        {
            Debug.Log("*** enable true in enter ");
            gameObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = clickedColor;
            gameObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = clickedColor;
            isClicked = true;
        }
        
        if (!GameBoardManager.player1Turn && !PhotonNetwork.IsMasterClient && isEnable)
        {
            Debug.Log("*** enable true in enter ");
            gameObject.transform.GetChild(0).Find("Image").GetComponent<Image>().color = clickedColor;
            gameObject.transform.GetChild(0).Find("Frame").GetComponent<Image>().color = clickedColor;
            isClicked = true;
        }
    }
}
