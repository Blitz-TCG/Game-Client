using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform gameboardParent;
    [SerializeField] private GameObject loading;
    public static bool isStarted;
    private bool gameCancelled = false;
    private const string CANCEL_KEY = "isGameCancelled";

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
        Debug.LogError(gameboardParent.name + " gameboard parent");
        GameObject gameboard = PhotonNetwork.Instantiate("Gameboard", gameboardParent.position, gameboardParent.rotation, 0);
        gameboard.SetActive(false);
        gameboard.transform.SetParent(gameboardParent);
        gameboard.transform.localScale = gameboardParent.transform.localScale;
        gameboard.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        gameboard.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameboard.name = "GameBoard";
        Debug.LogError("init board called " + gameboard.name);
        Gold.instance.SetGold(500);
        //Invoke("LeaveBothPlayerAccidently", 60f);
        Debug.Log(PhotonNetwork.IsConnected + " connected ");
        Debug.Log(PhotonNetwork.InRoom + " inside room ");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " player count ");
        if(PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2)
            Invoke("CancelGame", 30f);
        if (PhotonNetwork.InRoom)
        {
            Invoke("LeaveGame", 30f);
        }
    }

    public void LeaveGame()
    {
        if (loading.gameObject.activeSelf)
        {
            SceneManager.LoadScene(3);
            if(PhotonNetwork.IsConnected)
            {
                if(PhotonNetwork.InRoom)
                    PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
    }

    public void CancelGame()
    {
        if(loading.gameObject.activeSelf)
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable  { { CANCEL_KEY, true } });
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(CANCEL_KEY) && (bool)propertiesThatChanged[CANCEL_KEY])
        {
            SceneManager.LoadScene(3);
            if (PhotonNetwork.IsConnected)
            {
                if(PhotonNetwork.InRoom)
                    PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
    }

    //private void LeaveBothPlayerAccidently()
    //{
    //    Debug.LogError(photonView.ViewID + " photon view id");
    //    if (loading.activeSelf)
    //    {
    //        photonView.RPC("MatchNotLoaded", RpcTarget.All);
    //    }
    //}

    //[PunRPC]
    //private void MatchNotLoaded()
    //{
    //    GameBoardManager.connectUsing = true;
    //    Debug.Log(" match not loaded");
    //    //SkirmishManager.instance.deckId = -1;
    //    SceneManager.LoadScene(3);
    //}
}
