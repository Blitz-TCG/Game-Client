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
    private ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();

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
        Debug.Log("isPlayerClicked " + PhotonManager.isPlayerClicked);
        Debug.Log(" in room " + PhotonNetwork.InRoom);
        if(!PhotonManager.isPlayerClicked)
        {
            PhotonManager.isPlayerClicked = false;
            SceneManager.LoadScene(3);
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
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
        if (PhotonNetwork.InRoom)
        {
            loading.SetActive(true);
            InitBoard();
            Debug.Log("in room ");
        }
        else
        {
            SceneManager.LoadScene(3);
            //PhotonNetwork.LeaveRoom();
            //PhotonNetwork.Disconnect();
            Debug.Log("not room");
        }
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
        //if(PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //    Invoke("CancelGame", 30f);
        //if (PhotonNetwork.InRoom)
        //{
        //    Invoke("LeaveGame", 30f);
        //}
        Invoke("CancelGame", 30f);
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
        //Debug.LogError("Cancel game called " + PhotonNetwork.CurrentRoom.PlayerCount);
        if(PhotonNetwork.CurrentRoom != null)
        {
            Debug.LogError("Cancel game called " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        if (loading.gameObject.activeSelf)
        {
            Debug.LogError(" inside photon nework");
            if(PhotonNetwork.CurrentRoom != null)
            {
                Debug.LogError("not null");
                if (!customProp.ContainsKey(CANCEL_KEY))
                {
                    Debug.LogError("not setted");
                    customProp[CANCEL_KEY] = true;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(customProp);
                }
                else
                {
                    Debug.LogError("not key null");
                    SceneManager.LoadScene(3);
                    if (PhotonNetwork.IsConnected)
                    {
                        if (PhotonNetwork.InRoom)
                            PhotonNetwork.LeaveRoom();
                        PhotonNetwork.Disconnect();
                    }
                }
            }
            else
            {
                Debug.LogError("null");
                SceneManager.LoadScene(3);
                if (PhotonNetwork.IsConnected)
                {
                    if (PhotonNetwork.InRoom)
                        PhotonNetwork.LeaveRoom();
                    PhotonNetwork.Disconnect();
                }
            }
            
        }
            //PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable  { { CANCEL_KEY, true } });
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log(" room property update " + PhotonNetwork.LocalPlayer.NickName + " player name " + PhotonNetwork.CurrentRoom.PlayerCount);
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
