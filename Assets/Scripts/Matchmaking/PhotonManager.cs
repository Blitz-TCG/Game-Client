using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    private SkirmishManager skirmishManager;
    private MatchData matchData;
    private const string CANCEL_KEY = "isGameCancelled";
    private List<string> roomNames = new List<string>();

    private void Awake()
    {
        Debug.Log("Awake called");
        //if (PhotonNetwork.IsConnected)
        //{
        //    Debug.Log("connected");
        //    if (PhotonNetwork.InRoom)
        //    {
        //        Debug.Log("in room " + PhotonNetwork.InRoom);
        //        PhotonNetwork.LeaveRoom();
        //    }
        //    Debug.Log(" already connected ");
        //    PhotonNetwork.Disconnect();
        //    Debug.Log("PhotonNetwork.IsConnected " + PhotonNetwork.IsConnected);
        //}
        //if (GameBoardManager.isCompleted)
        //{
        //    GameBoardManager.isCompleted = false;
        //    PhotonNetwork.LeaveRoom();
        //    PhotonNetwork.Disconnect();
        //}
    }

    private void Start()
    {
        Debug.Log("Nmae " + SceneManager.GetActiveScene().name);
        connected = false;
        Debug.Log("start called");
        //PhotonNetwork.Disconnect();

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("us");
        //PhotonNetwork.ConnectUsingSettings();
        Debug.Log("PhotonNetwork.IsConnected " + PhotonNetwork.IsConnected);
        PhotonNetwork.AutomaticallySyncScene = true;
        skirmishManager = SkirmishManager.instance;
        Debug.Log("Nmae " + SceneManager.GetActiveScene().name);
        Debug.Log(PhotonNetwork.InRoom + " photon room ");
        //if (PhotonNetwork.InRoom)
        //{
        //    Invoke("LeaveGame", 30f);
        //}
        //if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //    Invoke("CancelGame", 30f);
        //if (PhotonNetwork.InRoom)
        //{
        //    Invoke("LeaveGame", 30f);
        //}
        Invoke("CancelGame", 30f);
    }

    public void CancelGame()
    {
        //Debug.LogError("Cancel game called " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom != null)
        {
            Debug.LogError("Cancel game called " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        if (loadingPanel.gameObject.activeSelf || initialLoading.gameObject.activeSelf)
        {
            Debug.LogError(" inside photon nework");
            if (PhotonNetwork.CurrentRoom != null)
            {
                Debug.LogError("not null");
                if (!customProps.ContainsKey(CANCEL_KEY))
                {
                    Debug.LogError("not setted");
                    customProps[CANCEL_KEY] = true;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);
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
                if (PhotonNetwork.InRoom)
                    PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
    }

    //public void CancelGame()
    //{
    //    if (loadingPanel.gameObject.activeSelf || initialLoading.gameObject.activeSelf)
    //        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { CANCEL_KEY, true } });
    //}

    //public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    //{
    //    if (propertiesThatChanged.ContainsKey(CANCEL_KEY) && (bool)propertiesThatChanged[CANCEL_KEY])
    //    {
    //        SceneManager.LoadScene(3);
    //        if (PhotonNetwork.InRoom)
    //            PhotonNetwork.LeaveRoom();
    //    }
    //}

    public void LeaveGame()
    {
        if (loadingPanel.gameObject.activeSelf || initialLoading.gameObject.activeSelf)
        {
            SceneManager.LoadScene(3);
            PhotonNetwork.LeaveRoom();
        }
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
        if (GameBoardManager.connectUsing)
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
        Debug.Log("OnConnectedToMaster " + SceneManager.GetActiveScene().name);
        connected = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = FirebaseManager.instance.user.DisplayName;
        //skirmishManager.gameObject.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby " + SceneManager.GetActiveScene().name);
        //print("Joined lobby ======");
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame " + SceneManager.GetActiveScene().name);
        StartCoroutine(VersionCheck());
    }

    private IEnumerator VersionCheck() //check for if user has latest game build
    {
        Debug.Log("version check");
        var version = FirebaseDatabase.DefaultInstance.GetReference("versionCheck").GetValueAsync();
        yield return new WaitUntil(predicate: () => version.IsCompleted);
        if (version.IsFaulted)
        {
            Debug.Log(" Faulted ");
            loadingPanel.SetActive(true);
            skirmishOutputTextError.text = "Unable to validate current game version";
            Invoke(nameof(HideLoadingPanel), 5f);
        }
        else if (version.IsCompleted)
        {
            Debug.Log(" completed ");
            DataSnapshot versionData = version.Result;

            Debug.Log("Current Version: " + versionData.Value.ToString());

            if (versionData.Value.ToString() == "0")
            {
                Debug.Log("versionData.Value.ToString() == \"0\"");
                loadingPanel.SetActive(true);
                skirmishOutputTextError.text = "Blitz is down for maintenance";
                Invoke(nameof(HideLoadingPanel), 5f);
            }
            else if (versionData.Value.ToString() != FirebaseManager.instance.versionCheck)
            {
                Debug.Log("versionData.Value.ToString() != FirebaseManager.instance.versionCheck");
                loadingPanel.SetActive(true);
                skirmishOutputTextError.text = "Please download latest version: " + versionData.Value.ToString();
                Invoke(nameof(HideLoadingPanel), 5f);
            }
            else
            {
                Debug.Log("else ");
                if (connected && PhotonNetwork.IsConnectedAndReady && skirmishManager.deckId >= 1) //making sure that they have a deck selected before matchmaking begins
                {
                    Debug.Log("connected && PhotonNetwork.IsConnectedAndReady && skirmishManager.deckId >= 1");
                    PhotonNetwork.JoinRandomRoom();
                    loadingPanel.SetActive(true);
                }
            }
        }
    }

    private void HideLoadingPanel()
    {
        Debug.Log("HideLoadingPanel ");
        if (loadingPanel.activeSelf)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void CancelMatch()
    {
        Debug.Log("CancelMatch ");
        Debug.Log(connected);
        if (connected)
        {
            Debug.Log(" connected ");
            if (PhotonNetwork.InRoom)
            {
                Debug.Log(" PhotonNetwork ");
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                Debug.Log(" else ");
                PhotonNetwork.Disconnect();
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.ConnectToRegion("us");
                connected = false;
            }
            loadingPanel.SetActive(false);
        }
        deckProfile.sprite = profileImage;
        skirmishOutputTextError.text = "";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string region = PhotonNetwork.CloudRegion;
        Debug.Log("Current Photon Server Region: " + region);
        Debug.Log(roomNames.Count + " photon count");
        Debug.Log("OnJoinRandomFailed");
        CreateNewRoom();
    }

    public void CreateNewRoom()
    {
        string region = PhotonNetwork.CloudRegion;
        Debug.Log("Current Photon Server Region: " + region);
        Debug.Log(roomNames.Count + " photon count");
        Debug.Log("CreateNewRoom");
        int roomId = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom("Random_Room_" + roomId, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        string region = PhotonNetwork.CloudRegion;
        Debug.Log("Current Photon Server Region: " + region);
        Debug.Log(roomNames.Count + " photon count");
        Debug.Log("OnCreateRoomFailed");
        CreateNewRoom();
    }

    public override void OnJoinedRoom()
    {
        string region = PhotonNetwork.CloudRegion;
        Debug.Log("Current Photon Server Region: " + region);
        Debug.Log(roomNames.Count + " photon count");
        Debug.Log("joined room");
        customProp["enterdNum"] = 0;
        int deckGeneralId = ErgoQuery.instance.deckGeneralStore[skirmishManager.deckId - 1];
        string deckGeneralField = ErgoQuery.instance.gameboardCurrentStore[skirmishManager.deckId - 1];

        customProp["deckId"] = deckGeneralId;
        customProp["deckField"] = deckGeneralField;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 1)
        {
            Invoke("LeaveTheRoom", 45f);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            properties["masterGold"] = 500;
            properties["masterXP"] = 0;
            properties["masterUserId"] = "123"; // here give dummy id but you need to get id from firebase
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            properties["clientGold"] = 500;
            properties["clientXP"] = 0;
            properties["clientUserId"] = "456"; // here give dummy id but you need to get id from firebase
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        string region = PhotonNetwork.CloudRegion;
        Debug.Log("Current Photon Server Region: " + region);
        Debug.Log(roomNames.Count + " photon count");
        Debug.LogError("player enterd game " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.LogError("2 player enterd game " + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel(4);
            //if (!PlayerPrefs.HasKey("test"))
            //{
            //    PhotonNetwork.LoadLevel(4);
            //    PlayerPrefs.SetInt("test", 1);
            //    RemoveSceneFromBuildIndex();
            //}
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    private void LeaveTheRoom()
    {
        Debug.Log("LeaveTheRoom");
        if (loadingPanel.activeSelf)
            CancelMatch();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList)
            {
                if (!roomNames.Contains(room.Name))
                {
                    roomNames.Add(room.Name);
                }
            }
            else
            {
                roomNames.Remove(room.Name);
            }
        }
    }

    //public static void RemoveSceneFromBuildIndex()
    //{
    //    int sceneIndexToRemove = 4; // Replace with the index of the scene you want to remove

    //    if (sceneIndexToRemove >= 0 && sceneIndexToRemove < EditorBuildSettings.scenes.Length)
    //    {
    //        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
    //        scenes[sceneIndexToRemove].enabled = false;
    //        EditorBuildSettings.scenes = scenes;

    //        Debug.Log($"Scene at index {sceneIndexToRemove} removed from build index.");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Invalid scene index.");
    //    }
    //}


    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    Debug.Log("disconnect called");
    //    Debug.LogWarning($"Disconnected from Photon: {cause}");
    //    PhotonNetwork.ConnectUsingSettings();
    //    Debug.Log("connect using set");
    //}

    //public override void OnDisconnectedFromPhoton()
    //{
    //    Debug.Log("Local player disconnected from Photon server.");
    //    // Handle any necessary cleanup or actions after disconnection.
    //}
}
