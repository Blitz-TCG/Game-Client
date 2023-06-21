using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject initialLoading;
    [SerializeField] Image deckProfile;
    [SerializeField] Sprite profileImage;
    [SerializeField] public TMP_Text skirmishOutputTextError;
    public static string[] playersName;

    private bool connected = false;
    private ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    private SkirmishManager skirmishManager;

    private void Start()
    {
        connected = false;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        skirmishManager = SkirmishManager.instance;
    }

    private void Update()
    {
        if (!connected)
        {
            initialLoading.SetActive(true);
        }
        else
        {
            initialLoading.SetActive(false);
        }
        if(GameBoardManager.connectUsing)
        {
            GameBoardManager.connectUsing = false;
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        //if (!PhotonNetwork.IsConnected)
        //{
        //    PhotonNetwork.ConnectUsingSettings();
        //}
        //if (PhotonNetwork.IsConnected)
        //{
        //    initialLoading.SetActive(false);
        //}
    }


    public override void OnConnectedToMaster()
    {
        connected = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = FirebaseManager.instance.user.DisplayName;
    }

    public override void OnJoinedLobby()
    {
        //print("Joined lobby ======");
    }

    public void PlayGame()
    {
        StartCoroutine(VersionCheck());
    }

    private IEnumerator VersionCheck() //check for if user has latest game build
    {
        var version = FirebaseDatabase.DefaultInstance.GetReference("versionCheck").GetValueAsync();
        yield return new WaitUntil(predicate: () => version.IsCompleted);
        if (version.IsFaulted)
        {
            loadingPanel.SetActive(true);
            skirmishOutputTextError.text = "Unable to validate current game version";
            Invoke(nameof(HideLoadingPanel), 5f);
        }
        else if (version.IsCompleted)
        {
            DataSnapshot versionData = version.Result;

            Debug.Log("Current Version: " + versionData.Value.ToString());

            if (versionData.Value.ToString() == "0")
            {

                loadingPanel.SetActive(true);
                skirmishOutputTextError.text = "Blitz is down for maintenance";
                Invoke(nameof(HideLoadingPanel), 5f);
            }
            else if (versionData.Value.ToString() != FirebaseManager.instance.versionCheck)
            {
                loadingPanel.SetActive(true);
                skirmishOutputTextError.text = "Please download latest version: " + versionData.Value.ToString();
                Invoke(nameof(HideLoadingPanel), 5f);
            }
            else
            {
                if (connected && PhotonNetwork.IsConnectedAndReady && skirmishManager.deckId >= 1) //making sure that they have a deck selected before matchmaking begins
                {
                    PhotonNetwork.JoinRandomRoom();
                    loadingPanel.SetActive(true);
                }
            }
        }
    }

    private void HideLoadingPanel()
    {
        if (loadingPanel.activeSelf)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void CancelMatch()
    {
        Debug.Log(connected);
        if (connected)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                PhotonNetwork.Disconnect();
                PhotonNetwork.ConnectUsingSettings();
                connected = false;
            }
            loadingPanel.SetActive(false);
        }
        deckProfile.sprite = profileImage;
        skirmishOutputTextError.text = "";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateNewRoom();
    }

    public void CreateNewRoom()
    {
        int roomId = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom("Random_Room_" + roomId, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateNewRoom();
    }

    public override void OnJoinedRoom()
    {
        customProp["enterdNum"] = 0;
        int deckGeneralId = ErgoQuery.instance.deckGeneralStore[skirmishManager.deckId - 1];
        string deckGeneralField = ErgoQuery.instance.gameboardCurrentStore[skirmishManager.deckId - 1];

        customProp["deckId"] = deckGeneralId;
        customProp["deckField"] = deckGeneralField;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 1)
        {
            Invoke("LeaveTheRoom", 60f);
        }

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");
        string jsonData = File.ReadAllText(path);

        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(jsonData);
        Debug.LogError(data.gold + " gold data " + data.xp + " xp data ");

        int currentPlayerXP = PlayerPrefs.GetInt("totalXP", 0);
        if (PhotonNetwork.IsMasterClient)
        {
            properties["masterGold"] = 500;
            properties["masterXP"] = data.xp;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            properties["clientGold"] = 500;
            properties["clientXP"] = data.xp;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
             PhotonNetwork.LoadLevel(4);
        }
    }

    private void LeaveTheRoom()
    {
        CancelMatch();
    }

}
    