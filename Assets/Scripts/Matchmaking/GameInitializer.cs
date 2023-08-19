using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform gameboardParent;
    [SerializeField] private GameObject loading;
    public static bool isStarted;

    private void Awake()
    {
        //Debug.Log(PhotonNetwork.InRoom + " photon room ");
        //if (!PhotonNetwork.InRoom)
        //{
        //    Debug.Log("!PhotonNetwork.InRoom");
        //    SceneManager.LoadScene(3);
        //}
        //else
        //{
        //    Debug.Log("else");
        //    if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        //    {
        //        Debug.Log("PhotonNetwork.CurrentRoom.PlayerCount != 2");
        //        PhotonNetwork.Disconnect();
        //        SceneManager.LoadScene(3);
        //    }
        //}
    }

    private void Start()
    {
        //Debug.Log(PhotonNetwork.InRoom + " photon room ");
        //if (!PhotonNetwork.InRoom)
        //{
        //    Debug.Log("!PhotonNetwork.InRoom");
        //    SceneManager.LoadScene(3);
        //}
        //else
        //{
        //    Debug.Log("else");
        //    if(PhotonNetwork.CurrentRoom.PlayerCount != 2)
        //    {
        //        Debug.Log("PhotonNetwork.CurrentRoom.PlayerCount != 2");
        //        PhotonNetwork.Disconnect();
        //        SceneManager.LoadScene(3);  
        //    }
        //}
        isStarted = true;

        Debug.LogError("initialized");
        Debug.Log("Nmae " + SceneManager.GetActiveScene().name);
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
        Debug.LogError("init board called " + gameboard.name);
        Gold.instance.SetGold(500);
    }
}
