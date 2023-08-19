using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

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
    private MatchData matchData;

    private void Awake()
    {
        Debug.Log("Awake called");
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("connected");
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("in room " + PhotonNetwork.InRoom);
                PhotonNetwork.LeaveRoom();
            }
            Debug.Log(" already connected ");
            PhotonNetwork.Disconnect();
            Debug.Log("PhotonNetwork.IsConnected " + PhotonNetwork.IsConnected);
        }
    }

    private void Start()
    {
        Debug.Log("Nmae " + SceneManager.GetActiveScene().name);
        connected = false;
        Debug.Log("start called");
        //PhotonNetwork.Disconnect();
        
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("PhotonNetwork.IsConnected " + PhotonNetwork.IsConnected);
        PhotonNetwork.AutomaticallySyncScene = true;
        skirmishManager = SkirmishManager.instance;
        Debug.Log("Nmae " + SceneManager.GetActiveScene().name);
        Debug.Log(PhotonNetwork.InRoom + " photon room ");
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
                connected = false;
            }
            loadingPanel.SetActive(false);
        }
        deckProfile.sprite = profileImage;
        skirmishOutputTextError.text = "";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        CreateNewRoom();
    }

    public void CreateNewRoom()
    {
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
        Debug.Log("OnCreateRoomFailed");
        CreateNewRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined room");
        customProp["enterdNum"] = 0;
        int deckGeneralId = ErgoQuery.instance.deckGeneralStore[skirmishManager.deckId - 1];
        string deckGeneralField = ErgoQuery.instance.gameboardCurrentStore[skirmishManager.deckId - 1];

        customProp["deckId"] = deckGeneralId;
        customProp["deckField"] = deckGeneralField;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 1)
        {
            Invoke("LeaveTheRoom", 60f);
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
        Debug.Log("player enterd game " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("2 player enterd game " + PhotonNetwork.CurrentRoom.PlayerCount);
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
