using Photon.Pun;
using UnityEngine;

public class GameInitializer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform gameboardParent;
    [SerializeField] private GameObject loading;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        loading.SetActive(true);
        InitBoard();
    }

    private void InitBoard()
    {
        GameObject gameboard = PhotonNetwork.Instantiate("Gameboard", gameboardParent.position, gameboardParent.rotation, 0);
        gameboard.SetActive(false);
        gameboard.transform.parent = gameboardParent;
        gameboard.transform.localScale = gameboardParent.transform.localScale;
        gameboard.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        gameboard.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameboard.name = "GameBoard";
        Gold.instance.SetGold(500);
    }
}
