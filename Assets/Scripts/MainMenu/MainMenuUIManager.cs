using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Newtonsoft.Json;

public class MainMenuUIManager : MonoBehaviour

{
    public static MainMenuUIManager instance;

    [Header("UI References")]
    [SerializeField]
    private GameObject skirmishUI;
    [SerializeField]
    private GameObject blitzUI;
    [SerializeField]
    private GameObject campaignUI;
    [SerializeField]
    private GameObject practiceUI;
    [SerializeField]
    private GameObject deckBuilderUI;
    [SerializeField]
    private GameObject profileImageUI;
    [SerializeField]
    private GameObject profileImageLoading;
    [Space(5f)]

    [Header("Button References")]
    [SerializeField]
    private GameObject profileDropdown;
    [SerializeField]
    private GameObject changePfpUI;
    [Space(5f)]

    [Header("Info References")]
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text levelText;
    [Space(5f)]

    [Header("Profile Picture References")]
    [SerializeField]
    private GameObject profilePicture;
    [SerializeField]
    private TMP_Text outputText;
    [SerializeField]
    public GameObject pfpAbyss;
    [SerializeField]
    public GameObject pfpMargo;
    [SerializeField]
    public GameObject pfpMios;
    [SerializeField]
    public GameObject pfpNasse;
    [SerializeField]
    public GameObject pfpDefault;
    [Space(5f)]

    [Header("Settings")]
    [SerializeField]
    public GameObject settingsUI;
    [SerializeField]
    public GameObject settingsResolutionClosed;
    [SerializeField]
    public GameObject settingsResolutionOpen;
    [SerializeField]
    public GameObject settingsResolutionGrouper;
    [SerializeField]
    private TMP_Text settingsResolution;
    [SerializeField]
    private GameObject settingsResolution1024;
    [SerializeField]
    private GameObject settingsResolution1280;
    [SerializeField]
    private GameObject settingsResolution1920;
    [SerializeField]
    private GameObject settingsResolution2560;
    [SerializeField]
    private GameObject settingsResolution3840;
    [SerializeField]
    public GameObject settingsQualityClosed;
    [SerializeField]
    public GameObject settingsQualityOpen;
    [SerializeField]
    public GameObject settingsQualityGrouper;
    [SerializeField]
    private TMP_Text settingsQuality;
    [SerializeField]
    private GameObject settingsLow;
    [SerializeField]
    private GameObject settingsMed;
    [SerializeField]
    private GameObject settingsHigh;
    [SerializeField]
    private GameObject settingsFullscreenEnabled;
    [SerializeField]
    public GameObject settingsHelpTextEnabled;
    [SerializeField]
    public GameObject settingsDeleteDeckEnabled;
    [Space(5f)]

    [Header("Metrics")]
    [SerializeField]
    public GameObject metricsUI;
    [SerializeField]
    public TMP_Text matchesPlayedData;
    [SerializeField]
    public TMP_Text timePlayedData;
    [SerializeField]
    public TMP_Text turnsTakenData;
    [SerializeField]
    public TMP_Text experiencegainedData;
    [SerializeField]
    public TMP_Text masqueradesData;
    [SerializeField]
    public TMP_Text theOldKingdomData;
    [SerializeField]
    public TMP_Text fairytalesData;
    [SerializeField]
    public TMP_Text darkMatterData;
    [SerializeField]
    public TMP_Text tinkerersData;


    [Header("Friends List")]
    [SerializeField]
    public GameObject friendsUI;
    [SerializeField]
    private GameObject friendsBlockedUI;
    [SerializeField]
    private GameObject friendsListTabOn;
    [SerializeField]
    private GameObject friendsListTabOff;
    [SerializeField]
    private GameObject blockListTabOn;
    [SerializeField]
    private GameObject blockListTabOff;
    [SerializeField]
    private GameObject friendsObjectHolder;
    [SerializeField]
    public GameObject friendsAddSearch;
    [SerializeField]
    private TMP_InputField friendsAddInputField;
    [SerializeField]
    public GameObject friendsBlockAddSearch;
    [SerializeField]
    private TMP_InputField friendsBlockAddInputField;
    [SerializeField]
    public GameObject friendsRemoveSearch;
    [SerializeField]
    public TMP_InputField friendsRemoveInputField;
    [SerializeField]
    public GameObject friendsBlockRemoveSearch;
    [SerializeField]
    public TMP_InputField friendsBlockRemoveInputField;
    [SerializeField]
    public GameObject friendsImageLoading;
    [SerializeField]
    public TMP_Text friendsListTotal;
    [SerializeField]
    public TMP_Text blockedListTotal;
    [SerializeField]
    public TMP_Text outputTextFriendsErorr;
    [SerializeField]
    public TMP_Text outputTextFriendsSuccess;
    [Space(5f)]

    [Header("Messenger")]
    [SerializeField]
    private Image globalImage;
    [SerializeField]
    private Image whisperImage;
    [SerializeField]
    private Image[] chatImages;
    public TMP_InputField messengerInputField;
    [SerializeField]
    public TMP_Text messengerFriendUsername;
    private int globalCountCheck;
    private int globalCountTotal;
    private float lastSentTime;
    private float messageInterval = 0.2f; // 5 messages per second
    private float cooldownTime = 5f; // 2 seconds timeout
    private bool isCooldown;
    public TMP_Text onlineCountTMP;
    private Coroutine countOnlineUsers;

    [SerializeField]
    private float chatResize;

    public RectTransform chatResizeRect;

    [SerializeField]
    public TMP_Text messengerFriendUserID;
    [SerializeField]
    public TMP_Text messengerErorr;
    [SerializeField]
    public TMP_Text messengerSuccess;
    [SerializeField]
    public GameObject generalChatDropdown; //to grab the user's username
    [SerializeField]
    private BlockedWordsMessage blockedWords;
    [Space(5f)]

    public DatabaseReference dbReference;

    private EventHandler<ChildChangedEventArgs> newMessageListener; //to stop listening
    private EventHandler<ChildChangedEventArgs> newMessageListener2; //to stop listening
    public GameObject messagePrefab;
    public Transform messageContainter;
    public FriendsDeserialiseJson myFriendsJson = new FriendsDeserialiseJson();
    public GlobalDeserialiseJson globalJson = new GlobalDeserialiseJson();
    public string userID;
    public string username;
    public int retrieveCount;
    public int childCount;
    public List<string> friendsListUsernames = new List<string>();
    public List<string> friendsListUserIDs = new List<string>();
    public List<string> friendsListUserPFPs = new List<string>();
    public List<string> friendsListOnlineStatus = new List<string>();
    public List<string> blockedListUsernames = new List<string>();
    public List<string> blockedListUserIDs = new List<string>();
    public List<object> messageClonesFriends = new List<object>();
    public List<object> messageClonesGlobal = new List<object>();
    public FriendsScrollViewAdapter friendsScrollViewAdapter;
    public BlockedScrollViewAdapter blockedScrollViewAdapter;
    public int currentUserXP = 0;
    public int currentUserLevel = 1;
    public int nextXpRequired = 0;
    public int maxXPForCurrentLevel = 0;

    //CursorAudio
    public CursorManager cursorManager;
    public bool isUserXPLoaded = false;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll(); //for testing settings values

        if (instance == null) //standard awake statements
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }

        globalCountCheck = 0;

        if (PlayerPrefs.GetString("DisableGlobal") == "T")
        {
            globalImage.sprite = chatImages[1].sprite; //off
        }

        if (PlayerPrefs.GetString("DisableWhisper") == "T")
        {
            whisperImage.sprite = chatImages[3].sprite; //off
        }

        if (PlayerPrefs.HasKey("tooltipDisabled") == true)
        {
            settingsHelpTextEnabled.SetActive(true);
        }

        if (PlayerPrefs.HasKey("fullscreenDisabled") == true)
        {
            settingsFullscreenEnabled.SetActive(false);
        }

        if (PlayerPrefs.GetString("DisableDeleteDeckWarning") == "T")
        {
            settingsDeleteDeckEnabled.SetActive(true);
        }

        if (PlayerPrefs.HasKey("Resolution") == false) //settings player prefs - load resolution when a player has never set it manually
        {
            string desktopResolution = Screen.currentResolution.ToString();
            int desktopIndex = desktopResolution.IndexOf(" @");

            if (desktopIndex >= 0)
            {
                desktopResolution = desktopResolution.Substring(0, desktopIndex);
            }

            string spaceRemove = " ";
            desktopResolution = desktopResolution.Replace(spaceRemove, "");

            string[] desktopResolutionArray = desktopResolution.Split(char.Parse("x"));
            int resWidth = int.Parse(desktopResolutionArray[0]);
            int resHeight = int.Parse(desktopResolutionArray[1]);

            if (resWidth > 0 && resHeight > 0)
            {
                PlayerPrefs.SetInt("resWidth", resWidth); //this is what resValues are when the user first logs in.
                PlayerPrefs.SetInt("resHeight", resHeight); //this is what resValues are when the user first logs in.
                PlayerPrefs.Save();
                settingsResolution.text = desktopResolution;
            }
            else
            {
                Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow); //if unable to parse native resolution, default to 4k
                settingsResolution.text = "Error - Awake";
            }
        }
        else if (PlayerPrefs.HasKey("Resolution") == true) //if player has changed their resolution, save value and use that instead
        {
            if (PlayerPrefs.GetString("Resolution") == "1024")
            {
                settingsResolution.text = "1024x576";
            }
            else if (PlayerPrefs.GetString("Resolution") == "1280")
            {
                settingsResolution.text = "1280x720";
            }
            else if (PlayerPrefs.GetString("Resolution") == "1920")
            {
                settingsResolution.text = "1920x1080";
            }
            else if (PlayerPrefs.GetString("Resolution") == "2560")
            {
                settingsResolution.text = "2560x1440";
            }
            else if (PlayerPrefs.GetString("Resolution") == "3840")
            {
                settingsResolution.text = "3840x2160";
            }
        }

        if (PlayerPrefs.HasKey("Quality") == true) //mostly just a quality setting placeholder for now, might matter more later
        {
            if (PlayerPrefs.GetString("Quality") == "Low")
            {
                settingsQuality.text = "Low (MSSA Off)";
            }
            else if (PlayerPrefs.GetString("Quality") == "Med")
            {
                settingsQuality.text = "Med (MSSA 2x)";
            }
            else if (PlayerPrefs.GetString("Quality") == "High")
            {
                settingsQuality.text = "High (MSSA 8x)";
            }
        }
        else
        {
            settingsQuality.text = "Med (MSSA 2x)"; //default quality is medium
        }
    }

    private void Start()
    {
        chatResize = messengerFriendUsername.preferredWidth;

        if (FirebaseManager.instance.user != null)
        {
            FirebaseUser user = FirebaseManager.instance.user; //values used in friends list username and user searching
            dbReference = FirebaseDatabase.DefaultInstance.RootReference; //could do a FirebaseApp.DefaultIntance.SetEditorDatabaseURL("link") right before this to make sure you point to the right database

            userID = user.UserId;
            username = user.DisplayName;

            LoadProfile(); //loads the firebase auth user content
            PresenceCheck();
            StartCoroutine(RetrieveFriendsforMessaging());
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        Debug.Log(" fire base code " + FirebaseDatabase.DefaultInstance.GetHashCode());
        Debug.Log("user id " + FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID));
        Debug.Log("user id " + userID);
        //Debug.Log("audio manager " + GameObject.FindObjectOfType<AudioManager>());
        //Debug.Log("audio manager name " + GameObject.FindObjectOfType<AudioManager>().tracks[7].name);
        //Debug.Log("audio manager name " + GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>());
    }

    IEnumerator RetrieveFriendsforMessaging()
    {
        yield return StartCoroutine(RetrieveFriends());
        yield return StartCoroutine(RetrieveBlockedUsers());

        FriendsListenForNewMessages();
        yield return StartCoroutine(CountGlobalMessageTotal());
        GlobalListenForNewMessages();

	countOnlineUsers = StartCoroutine(CountOnlineUsers());

    }

    public void PresenceCheck() //todo - figure out presence check
    {

        DatabaseReference connectCheck = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");
        connectCheck.ValueChanged += CheckForDisconnect;
    }

    private void CheckForDisconnect(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        var connected = args.Snapshot.Value as bool?; // Safely cast to nullable bool

        if (connected == null)
        {
            return;
        }
        else if (connected.Value)
        {
            var userOnlineRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("online");
            userOnlineRef.OnDisconnect().SetValue("F");
            userOnlineRef.SetValueAsync("T");
        }
    }

    public IEnumerator CountOnlineUsers()
    {
        while (true)
        {
        DatabaseReference usersRef = FirebaseDatabase.DefaultInstance.GetReference("users");
        var query = usersRef.OrderByChild("online").EqualTo("T");
        var getOnlineUsersTask = query.GetValueAsync();

        yield return new WaitUntil(predicate: () => getOnlineUsersTask.IsCompleted);
        if (getOnlineUsersTask.IsFaulted)
        {
            Debug.LogError("Error getting online users: " + getOnlineUsersTask.Exception);
        }
        else if (getOnlineUsersTask.IsCompleted)
        {
            DataSnapshot snapshot = getOnlineUsersTask.Result;
            int onlineCount = 0;

            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                if (userSnapshot.HasChild("online") && userSnapshot.Child("online").Value.ToString() == "T")
                {
                    onlineCount++;
                }
            }

            onlineCountTMP.text = "Online: " + onlineCount.ToString();
            Debug.Log($"Number of online users: {onlineCount}");
        }

            yield return new WaitForSeconds(30f);
        }

    }

    private void Update() //various windows exits and checks
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsUI.activeSelf && !friendsUI.activeSelf && !changePfpUI.activeSelf &&
            !metricsUI.activeSelf && !profileDropdown.activeSelf && generalChatDropdown.activeSelf) //closing chat dropdown last
        {
            generalChatDropdown.SetActive(false);
            cursorManager.AudioClickButtonStandard();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !settingsUI.activeSelf && !friendsUI.activeSelf && !changePfpUI.activeSelf && 
            !metricsUI.activeSelf && profileDropdown.activeSelf) //closing profile dropdown second to last
        {
            profileDropdown.SetActive(false);
            cursorManager.AudioClickButtonStandard();
        }

        if (changePfpUI.activeSelf && changePfpUI != null)
        {

            if (Input.GetKeyDown(KeyCode.Escape) && !settingsUI.activeSelf && !friendsUI.activeSelf && !metricsUI.activeSelf)
            {
                changePfpUI.SetActive(false);
                cursorManager.AudioClickButtonStandard();
                ClearUI();
            }
        }

        if (friendsAddSearch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                friendsAddInputField.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                cursorManager.AudioClickButtonStandard();
                AddFriendsButton();
            }
        }

        if (friendsRemoveSearch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                friendsRemoveInputField.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                cursorManager.AudioClickButtonStandard();
                RemoveFriendsButton();
            }
        }

        if (friendsBlockAddSearch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                friendsBlockAddInputField.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                cursorManager.AudioClickButtonStandard();
                AddFriendsButton();
            }
        }

        if (friendsBlockRemoveSearch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                friendsBlockRemoveInputField.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                cursorManager.AudioClickButtonStandard();
                RemoveFriendsButton();
            }
        }

        if (settingsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            cursorManager.AudioClickButtonStandard();
            SettingsBack();
        }

        if (metricsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            cursorManager.AudioClickButtonStandard();
            MetricsBack();
        }


        if (!friendsUI.activeSelf && !settingsUI.activeSelf && !metricsUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                cursorManager.ChatSendButtonStandard();
                ChatMessengerSend();
            }
        }

        if (friendsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            cursorManager.AudioClickButtonStandard();
            FriendsBack();
        }

        if (friendsAddSearch.activeSelf && !friendsAddInputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SelectFriendsAddInputField());
            }
        }

        if (friendsRemoveSearch.activeSelf && !friendsRemoveInputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SelectFriendsRemoveInputField());
            }
        }

        if (friendsBlockAddSearch.activeSelf && !friendsBlockAddInputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SelectFriendsBlockAddInputField());
            }
        }

        if (friendsBlockRemoveSearch.activeSelf && !friendsBlockRemoveInputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SelectFriendsBlockRemoveInputField());
            }
        }

        if (!friendsUI.activeSelf && !settingsUI.activeSelf && !metricsUI.activeSelf && !messengerInputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SelectMessengerInputField());
            }
        }
    }

    public void LoadProfile() //loading firebase auth info to profile banner
    {
        if (FirebaseManager.instance.user != null)
        {
            string name = FirebaseManager.instance.user.DisplayName;
            usernameText.text = name;

            StartCoroutine(LoadImage());
            StartCoroutine(LoadLevel());
        }
    }

    public void pfpSelectAbyss()
    {
        StartCoroutine(pfpSet("Abyss"));
    }
    public void pfpSelectMargo()
    {
        StartCoroutine(pfpSet("Margo"));
    }
    public void pfpSelectMios()
    {
        StartCoroutine(pfpSet("Mios"));
    }
    public void pfpSelectNasse()
    {
        StartCoroutine(pfpSet("Nasse"));
    }

    public IEnumerator pfpSet(string pfpName)
    {
        var setData = dbReference.Child("users").Child(userID).Child("pfp").SetValueAsync(pfpName);
        yield return new WaitUntil(predicate: () => setData.IsCompleted); //todo maybe add some exception checking
        if (setData.IsFaulted)
        {
            outputText.text = "Potential error at Fetching Friend PFPs1";
        }
        else if (setData.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
        {
            LoadProfile();
            CloseChangePfpUI();
        }
    }

    private IEnumerator LoadImage()
    {
        profileImageLoading.SetActive(true);

        var setData = dbReference.Child("users").Child(userID).Child("pfp").GetValueAsync();
        yield return new WaitUntil(predicate: () => setData.IsCompleted); //todo maybe add some exception checking
        if (setData.IsFaulted)
        {
            outputText.text = "Potential error at Fetching Friend PFPs2";
            profileImageLoading.SetActive(false);
        }
        else if (setData.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
        {
            DataSnapshot snapshot = setData.Result;
            string pfpName = snapshot.Value.ToString(); //add the key of the found childs to the list. These are the usernames of their friends

            if (pfpName == "Abyss")
            {
                profilePicture.GetComponent<RawImage>().texture = pfpAbyss.GetComponent<RawImage>().texture;
                profileImageLoading.SetActive(false);
            }
            else if (pfpName == "Margo")
            {
                profilePicture.GetComponent<RawImage>().texture = pfpMargo.GetComponent<RawImage>().texture;
                profileImageLoading.SetActive(false);
            }
            else if (pfpName == "Mios")
            {
                profilePicture.GetComponent<RawImage>().texture = pfpMios.GetComponent<RawImage>().texture;
                profileImageLoading.SetActive(false);
            }
            else if (pfpName == "Nasse")
            {
                profilePicture.GetComponent<RawImage>().texture = pfpNasse.GetComponent<RawImage>().texture;
                profileImageLoading.SetActive(false);
            }
            else
            {
                profilePicture.GetComponent<RawImage>().texture = pfpDefault.GetComponent<RawImage>().texture;
                profileImageLoading.SetActive(false);
            }
        }
    }
    private int CalculateLevelFromXp(int xp) //y = 25x^2 - 50x + 25, starting at level 1 of 0 xp and hitting level 2 at 25 xp, etc.
    {
        if (xp < 25) return 1;

        int level = (int)Math.Floor((Math.Sqrt(xp) + 5) / 5);
        return level;
    }

    private int CalculateMaxXpForCurrentLevel(int level)
    {
        int xp = 25 * (level + 1) * (level + 1) - 50 * (level + 1) + 25;
        Debug.Log(level + " level " + xp + " xp ");
        return xp;
    }

    private int XpRequiredForLevel(int userLevel, int currentXp)
    {
        int totalXpForNextLevel = 25 * (userLevel + 1) * (userLevel + 1) - 50 * (userLevel + 1) + 25;

        return totalXpForNextLevel - currentXp;

        //for matchmaking, we'd just take this totalXpForNextLevel amount and have that be the end of the leveling bar
        //the start of the leveling bar would just be int currentLevelStartingXp = 25 * (userLevel) * (userLevel) - 50 * (userLevel) + 25;
        //and then we would just need to know the xpAmount.Result.Value to know how much xp the user currently has to then add the new xp to it, i.e. 400 xp + 25xp gained from the match
    }


    public IEnumerator LoadLevel()
    {
        Debug.Log(" load level called");
        var xpAmount = dbReference.Child("open").Child(userID).Child("xpOpen").GetValueAsync();
        yield return new WaitUntil(predicate: () => xpAmount.IsCompleted);

        if (xpAmount.IsFaulted)
        {
            Debug.Log("Error fetching user's experience.");
        }
        else if (xpAmount.IsCompleted)
        {
            DataSnapshot snapshot = xpAmount.Result;

            Debug.Log(xpAmount.Result.Value + " result value ");
            int xp;
            if(xpAmount.Result.Value != null)
            {
                if (int.TryParse(snapshot.Value.ToString(), out xp))

                {
                    Debug.Log("xp returned: " + xp);
                    int userLevel = CalculateLevelFromXp(xp);
                    Debug.Log("User Level: " + userLevel);

                    int xpNeededForNextLevel = XpRequiredForLevel(userLevel, xp);
                    int maxXP = CalculateMaxXpForCurrentLevel(userLevel);

                    Debug.Log("xp needed for next level: " + xpNeededForNextLevel + " max xp: " + maxXP);
                    levelText.text = "LVL " + userLevel.ToString();
                    currentUserXP = xp;
                    currentUserLevel = userLevel;
                    nextXpRequired = xpNeededForNextLevel;
                    maxXPForCurrentLevel = maxXP;
                }
                else
                {
                    Debug.Log("Unable to parse use experience.");
                    levelText.text = "LVL 1";
                    currentUserXP = 0;
                    currentUserLevel = 1;
                    nextXpRequired = 25;
                    maxXPForCurrentLevel = 25;
                }
            }
            else if (xpAmount.Result.Value == null)
            {
                Debug.Log("User has never played a match so experience is null.");
                levelText.text = "LVL 1";

                int xpNeededForNextLevel = XpRequiredForLevel(1, 0);

                Debug.Log("xp needed for next level: " + xpNeededForNextLevel);
                currentUserXP = 0;
                currentUserLevel = 1;
                nextXpRequired = 25;
                maxXPForCurrentLevel = 25;
            }
        }
        isUserXPLoaded = true;
    }

    public void ClearUI() //clear ui text fields
    {
        outputText.text = ""; //pfp banner ui related

        friendsAddInputField.text = ""; //friends list ui related
        friendsRemoveInputField.text = "";
        friendsBlockAddInputField.text = ""; //friends list ui related
        friendsBlockRemoveInputField.text = "";
        outputTextFriendsErorr.text = "";
        outputTextFriendsSuccess.text = "";

        messengerErorr.text = "";
        messengerSuccess.text = "";
    }

    public void ProfileDropdown() //profile dropdown ui interactions
    {
        if (profileDropdown.activeSelf)
        {
            profileDropdown.SetActive(false);
        }
        else if (!profileDropdown.activeSelf)
        {
            profileDropdown.SetActive(true);
        }

        changePfpUI.SetActive(false);
        ClearUI();
    }

    public void CloseChangePfpUI() //profile dropdown ui interactions
    {
        if (changePfpUI.activeSelf)
        {
            changePfpUI.SetActive(false);
            profileDropdown.SetActive(false);
        }
        else if (!changePfpUI.activeSelf)
        {
            changePfpUI.SetActive(true);
            profileDropdown.SetActive(false);
        }

        ClearUI();
    }

    public void ManageWallet() //profile dropdown ui interactions
    {
        Application.OpenURL("https://blitztcg.com/home");
        profileDropdown.SetActive(false);
        ClearUI();
    }

    public void MyCollections() //profile dropdown ui interactions
    {
        Application.OpenURL("https://blitztcg.com/collectibles");
        profileDropdown.SetActive(false);
        ClearUI();
    }

    public void BuyPacks() //profile dropdown ui interactions
    {
        Application.OpenURL("https://blitztcg.com/buy-packs");
        profileDropdown.SetActive(false);
        ClearUI();
    }

    public void OpenPacks() //profile dropdown ui interactions
    {
        Application.OpenURL("https://blitztcg.com/open-packs");
        profileDropdown.SetActive(false);
        ClearUI();
    }

    //
    //misc menu work
    //
    public void LogOutButton() //log a user out and return to main menu
    {
        StartCoroutine(LogOutEnumerator());
    }

    public IEnumerator LogOutEnumerator() //log a user out and return to main menu
    {
        var logOut = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("online").SetValueAsync("F");
        yield return new WaitUntil(predicate: () => logOut.IsCompleted); //todo maybe add some exception checking
        if (logOut.IsFaulted)
        {
            cursorManager.CursorNormal();
            RemoveListener();
		StopCoroutines();
            FirebaseManager.instance.auth.SignOut();
            GameManager.instance.ChangeScene(0);
        }
        else if (logOut.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
        {
            cursorManager.CursorNormal();
            RemoveListener();
	StopCoroutines();
            FirebaseManager.instance.auth.SignOut();
            GameManager.instance.ChangeScene(0);
        }
    }

    public void ExitUI() //cleanly exit the application and log out
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        StartCoroutine(ExitUIEnumerator());
    }

    private IEnumerator ExitUIEnumerator()
    {
        cursorManager.CursorNormal();
        var logOut = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("online").SetValueAsync("F");
        yield return new WaitUntil(predicate: () => logOut.IsCompleted); //todo maybe add some exception checking
        if (logOut.IsFaulted)
        {
            Debug.Log("could not cleanly terminate session");
            RemoveListener();
		StopCoroutines();
            FirebaseManager.instance.auth.SignOut();
            Application.Quit();
        }
        else if (logOut.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
        {
            FirebaseManager.instance.auth.SignOut();
            RemoveListener();
		StopCoroutines();
            Application.Quit();
            Debug.Log("exit");
        }
    }

    public void SettingsOpen() //open settings
    {
        settingsUI.SetActive(true);
    }

    public void SettingsBack() //closer settings and all related ui
    {
        cursorManager.CursorNormal();
        settingsUI.SetActive(false);
        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
        settingsQualityClosed.SetActive(true);
        settingsQualityOpen.SetActive(false);
        settingsQualityGrouper.SetActive(false);
    }

    public void MetricsBack() //closer settings and all related ui
    {
        cursorManager.CursorNormal();
        metricsUI.SetActive(false);
    }

    public void MetricsOpen() //open a user's friends list and retrieve their current list of friends
    {
        metricsUI.SetActive(true);

        StartCoroutine(RetrieveMetrics());

    }


    //
    //this marks the beginning of friends list and metrics related dev for retrieving firebase info
    //
    public void FriendsOpen() //open a user's friends list and retrieve their current list of friends
    {
        friendsUI.SetActive(true);

        StartCoroutine(RetrieveFriends());

    }

    public void FriendsAdd() //brings open add friends ui
    {
        if (friendsAddSearch.activeSelf && friendsObjectHolder.activeSelf)
        {
            friendsAddSearch.SetActive(false);
        }
        else if (!friendsAddSearch.activeSelf && friendsObjectHolder.activeSelf)
        {
            friendsAddSearch.SetActive(true);
            friendsRemoveSearch.SetActive(false);
            friendsAddInputField.Select();
        }
        else if (friendsBlockAddSearch.activeSelf && !friendsObjectHolder.activeSelf)
        {
            friendsBlockAddSearch.SetActive(false);
        }
        else if (!friendsBlockAddSearch.activeSelf && !friendsObjectHolder.activeSelf)
        {
            friendsBlockAddSearch.SetActive(true);
            friendsBlockRemoveSearch.SetActive(false);
            friendsBlockAddInputField.Select();
        }

        ClearUI();
    }

    public void FriendsRemove() //brings open remove friends ui
    {
        if (friendsRemoveSearch.activeSelf && friendsObjectHolder.activeSelf)
        {
            friendsRemoveSearch.SetActive(false);
        }
        else if (!friendsRemoveSearch.activeSelf && friendsObjectHolder.activeSelf)
        {
            friendsRemoveSearch.SetActive(true);
            friendsAddSearch.SetActive(false);
            friendsRemoveInputField.Select();
        }
        else if (friendsBlockRemoveSearch.activeSelf && !friendsObjectHolder.activeSelf)
        {
            friendsBlockRemoveSearch.SetActive(false);
        }
        else if (!friendsBlockRemoveSearch.activeSelf && !friendsObjectHolder.activeSelf)
        {
            friendsBlockRemoveSearch.SetActive(true);
            friendsBlockAddSearch.SetActive(false);
            friendsBlockRemoveInputField.Select();
        }

        ClearUI();
    }

    public void ActivateBlockedkList() //brings open block friends UI
    {
        if (!friendsBlockedUI.activeSelf)
        {
            cursorManager.CursorNormal();
            StartCoroutine(RetrieveBlockedUsers());
            friendsObjectHolder.SetActive(false);
            friendsBlockedUI.SetActive(true);
            friendsListTabOff.SetActive(true);
            friendsListTabOn.SetActive(false);
            blockListTabOff.SetActive(false);
            blockListTabOn.SetActive(true);


            friendsBlockAddSearch.SetActive(false);
            friendsBlockRemoveSearch.SetActive(false);
            friendsAddSearch.SetActive(false);
            friendsRemoveSearch.SetActive(false);
        }

        ClearUI();
    }
    public void ActivateFriendskList() //brings open block friends UI
    {
        if (!friendsObjectHolder.activeSelf)
        {
            cursorManager.CursorNormal();
            friendsObjectHolder.SetActive(true);
            friendsBlockedUI.SetActive(false);
            friendsListTabOff.SetActive(false);
            friendsListTabOn.SetActive(true);
            blockListTabOff.SetActive(true);
            blockListTabOn.SetActive(false);

            friendsBlockAddSearch.SetActive(false);
            friendsBlockRemoveSearch.SetActive(false);
            friendsAddSearch.SetActive(false);
            friendsRemoveSearch.SetActive(false);
        }

        ClearUI();
    }
    public void FriendsBack() //exit friends ui
    {
        friendsObjectHolder.SetActive(true);
        friendsBlockedUI.SetActive(false);
        friendsListTabOff.SetActive(false);
        friendsListTabOn.SetActive(true);
        blockListTabOff.SetActive(true);
        blockListTabOn.SetActive(false);
        friendsUI.SetActive(false);
        friendsAddSearch.SetActive(false);
        friendsRemoveSearch.SetActive(false);
        friendsBlockAddSearch.SetActive(false);
        friendsBlockRemoveSearch.SetActive(false);
        ClearUI();
    }

    public IEnumerator RetrieveMetrics()
    {
        Debug.Log(FirebaseManager.instance.user.UserId);
        var metricsLoad = FirebaseDatabase.DefaultInstance.GetReference("open").Child(FirebaseManager.instance.user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => metricsLoad.IsCompleted);
        if (metricsLoad.IsFaulted)
        {
            Debug.Log("Unable to load metrics data.");
        }
        else if (metricsLoad.IsCompleted && metricsLoad.Result.Value != null)
        {
            Debug.Log("Loaded metrics data successfully.");

            DataSnapshot dataSnapshot = metricsLoad.Result;
            var fullJSON = JsonConvert.DeserializeObject(dataSnapshot.GetRawJsonValue());
            PlayerMetrics metricsData = JsonUtility.FromJson<PlayerMetrics>(fullJSON.ToString());

            matchesPlayedData.text = (metricsData.margoDeckWinOpen + metricsData.margoDeckLossOpen + metricsData.miosDeckWinOpen + metricsData.miosDeckLossOpen + metricsData.nasseDeckWinOpen + 
                metricsData.nasseDeckLossOpen + metricsData.voidDeckWinOpen + metricsData.voidDeckLossOpen + metricsData.tootDeckWinOpen + metricsData.tootDeckLossOpen).ToString();
            timePlayedData.text = (Math.Ceiling((decimal)metricsData.totalTimePlayedOpen / 60 / 60 * 10) / 10).ToString("F1") + " hrs";
            turnsTakenData.text = metricsData.totalTurnsTakenOpen.ToString();
            experiencegainedData.text = metricsData.xpOpen.ToString();

            if ((metricsData.margoDeckWinOpen + metricsData.margoDeckLossOpen) > 0)
            {
                masqueradesData.text = ((double)metricsData.margoDeckWinOpen / (metricsData.margoDeckWinOpen + metricsData.margoDeckLossOpen) * 100).ToString("F2") + "%";
            }
            else
            {
                masqueradesData.text = "0.00%";
            }
            if ((metricsData.miosDeckWinOpen + metricsData.miosDeckLossOpen) > 0)
            {
                theOldKingdomData.text = ((double)metricsData.miosDeckWinOpen / (metricsData.miosDeckWinOpen + metricsData.miosDeckLossOpen) * 100).ToString("F2") + "%";
            }
            else
            {
                theOldKingdomData.text = "0.00%";
            }
            if ((metricsData.nasseDeckWinOpen + metricsData.nasseDeckLossOpen) > 0)
            {
                fairytalesData.text = ((double)metricsData.nasseDeckWinOpen / (metricsData.nasseDeckWinOpen + metricsData.nasseDeckLossOpen) * 100).ToString("F2") + "%";
            }
            else
            {
                fairytalesData.text = "0.00%";
            }
            if ((metricsData.voidDeckWinOpen + metricsData.voidDeckLossOpen) > 0)
            {
                darkMatterData.text = ((double)metricsData.voidDeckWinOpen / (metricsData.voidDeckWinOpen + metricsData.voidDeckLossOpen) * 100).ToString("F2") + "%";
            }
            else
            {
                darkMatterData.text = "0.00%";
            }
            if ((metricsData.tootDeckWinOpen + metricsData.tootDeckLossOpen) > 0)
            {
                tinkerersData.text = ((double)metricsData.tootDeckWinOpen / (metricsData.tootDeckWinOpen + metricsData.tootDeckLossOpen) * 100).ToString("F2") + "%";
            }
            else
            {
                tinkerersData.text = "0.00%";
            }


        }
        else if (metricsLoad.IsCompleted && metricsLoad.Result.Value == null)
        {
            Debug.Log("No metrics yet generated for this user: " + FirebaseManager.instance.user.UserId);
        }
    }

    public void GlobalChat()
    {
        if (messengerFriendUsername.text == "[Global]")
        {
            ClearUI();
        }
        else
        {
            messengerFriendUsername.color = new Color32(255, 255, 255, 255);
            messengerFriendUsername.text = "[Global]";
            messengerFriendUserID.text = "";
            ResizeChatInputField();
            ClearUI();
        }
    }

    public void ResizeChatInputField()
    {
        if (messengerFriendUsername.preferredWidth > chatResize) //chat resize is orginal size and the preferred width is the new size
        {
            float chatSizeChange = messengerFriendUsername.preferredWidth - chatResize;
            chatResize = messengerFriendUsername.preferredWidth;

            chatResizeRect.sizeDelta = new Vector2(chatResizeRect.sizeDelta.x - chatSizeChange, chatResizeRect.sizeDelta.y);

            chatResizeRect.localPosition = new Vector3(chatResizeRect.localPosition.x + chatSizeChange, chatResizeRect.localPosition.y, chatResizeRect.localPosition.z);
        }
        else if (messengerFriendUsername.preferredWidth < chatResize)
        {
            float chatSizeChange = chatResize - messengerFriendUsername.preferredWidth;
            chatResize = messengerFriendUsername.preferredWidth;

            chatResizeRect.sizeDelta = new Vector2(chatResizeRect.sizeDelta.x + chatSizeChange, chatResizeRect.sizeDelta.y);

            chatResizeRect.localPosition = new Vector3(chatResizeRect.localPosition.x - chatSizeChange, chatResizeRect.localPosition.y, chatResizeRect.localPosition.z);
        }

    }

    public void ChatMessengerSend()
    {
        float currentTime = Time.time;

        string[] messageTextArray = messengerInputField.text.Split(" ");

        if (!isCooldown)
        {
            ClearUI();
        }

        if (!isCooldown && currentTime - lastSentTime < messageInterval)
        {
            StartCoroutine(Cooldown());
            messengerErorr.text = "Slow down, 5 second timeout!";
        }
        else if (messageTextArray[0] == "/1" && !isCooldown) //quick command for global chat
        {

            messageTextArray = messageTextArray.Where((source, index) => index != 0).ToArray();
            string messageTextConcat = string.Join(" ", messageTextArray);

            int stringCheck = 0;
            stringCheck = messageTextConcat.Replace(" ", String.Empty).Length;

            if ((messageTextConcat.Length <= 100 && stringCheck > 0 &&
            !blockedListUsernames.Contains(messengerFriendUsername.text.Replace("[", "").Replace("]", "")) &&
                !blockedWords.blockedWordsMessenger.Any(s => messageTextConcat.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true))
            {
                StartCoroutine(GlobalMessengerSendEnumerator(messageTextConcat)); //global
            }
            else if (stringCheck <= 0)
            {
                messengerErorr.text = "Message cannot be blank";
            }
            else if (messageTextConcat.Length > 100)
            {
                messengerErorr.text = "Message must be less than 100 characters";
            }
            else if (blockedListUsernames.Contains(messengerFriendUsername.text.Replace("[", "").Replace("]", "")))
            {
                messengerErorr.text = messengerFriendUsername.text.Replace("[", "").Replace("]", "") + " is blocked";
            }
            else if (blockedWords.blockedWordsMessenger.Any(s => messageTextConcat.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true)
            {
                messengerErorr.text = "One of your words is blocked";
            }

        }
        else if (messageTextArray[0] == "/w" && !isCooldown)
        {
            messageTextArray = messageTextArray.Where((source, index) => index != 0).ToArray();//remove /w
            string usernameTell = messageTextArray[0]; //store the username
            messageTextArray = messageTextArray.Where((source, index) => index != 0).ToArray();//remove the username from the message
            string messageTextConcat = string.Join(" ", messageTextArray);

            int stringCheck = 0;
            stringCheck = messageTextConcat.Replace(" ", String.Empty).Length;

            if ((messageTextConcat.Length <= 100 && stringCheck > 0 && !blockedListUsernames.Contains(usernameTell) && usernameTell != username &&
                !blockedWords.blockedWordsMessenger.Any(s => messageTextConcat.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true))
            {
                StartCoroutine(CheckIfTellUserExists(usernameTell, messageTextConcat));
            }
            else if (stringCheck <= 0)
            {
                messengerErorr.text = "Message cannot be blank";
            }
            else if (messageTextConcat.Length > 100)
            {
                messengerErorr.text = "Message must be less than 100 characters";
            }
            else if (blockedListUsernames.Contains(usernameTell))
            {
                messengerErorr.text = usernameTell + " is blocked";
            }
            else if (blockedWords.blockedWordsMessenger.Any(s => messageTextConcat.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true)
            {
                messengerErorr.text = "One of your words is blocked";
            }
            else if (usernameTell == username)
            {
                messengerErorr.text = "You cannot message yourself";
            }
        }
        else if (messageTextArray[0] != "/1" && messageTextArray[0] != "/w" && !isCooldown)
        {

            int stringCheck = 0;
            stringCheck = messengerInputField.text.Replace(" ", String.Empty).Length;

            if (messengerInputField.text.Length <= 100 && stringCheck > 0 &&
            !blockedListUsernames.Contains(messengerFriendUsername.text.Replace("[", "").Replace("]", "")) &&
                !blockedWords.blockedWordsMessenger.Any(s => messengerInputField.text.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true)
            {
                if (messengerFriendUsername.text != "" && messengerFriendUsername.text != "[Global]") //whispers
                {
                    StartCoroutine(FriendsMessengerSendEnumerator(messengerFriendUsername.text, messengerFriendUserID.text, messengerInputField.text));
                }
                else
                {
                    StartCoroutine(GlobalMessengerSendEnumerator(messengerInputField.text)); //global
                }
            }
            else if (stringCheck <= 0)
            {
                messengerErorr.text = "Message cannot be blank";
            }
            else if (messengerInputField.text.Length > 100)
            {
                messengerErorr.text = "Message must be less than 100 characters";
            }
            else if (blockedListUsernames.Contains(messengerFriendUsername.text.Replace("[", "").Replace("]", "")))
            {
                messengerErorr.text = messengerFriendUsername.text.Replace("[", "").Replace("]", "") + " is blocked";
            }
            else if (blockedWords.blockedWordsMessenger.Any(s => messengerInputField.text.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true)
            {
                messengerErorr.text = "One of your words is blocked";
            }
        }

        lastSentTime = currentTime;
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    IEnumerator CheckIfTellUserExists(string usernameTell, string messageTell)
    {
        var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("username").EqualTo(usernameTell).GetValueAsync();
        yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

        DataSnapshot snapshotUsername = usernameCheck.Result;

        if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
        {
            messengerErorr.text = "Unable to find the user to whisper";
        }
        else if (snapshotUsername.Exists) //check if username is a real username that exists in the firebase users node
        {
            if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
            {
                messengerErorr.text = "Error looking up the user";
            }
            else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>(); //this is grabbing the key which is the userID of the friend being added
                foreach (DataSnapshot child in snapshotUsername.Children) //this should only ever loop once in this firebase node, but there might be a better way to do it
                {
                    if (friendsListUsernames.Contains(usernameTell))
                    {
                        messengerFriendUsername.color = new Color32(86, 255, 0, 255);
                    }
                    else if (!friendsListUsernames.Contains(usernameTell))
                    {
                        messengerFriendUsername.color = new Color32(250, 60, 214, 255);
                    }

                    messengerFriendUsername.text = "[" + usernameTell + "]";
                    messengerFriendUserID.text = child.Key;

                    ResizeChatInputField(); //todo resize test

                    StartCoroutine(FriendsMessengerSendEnumerator(usernameTell, child.Key, messageTell));
                }
            }
        }
        else
        {
            messengerErorr.text = "User does not exist"; //if nothing was found, the user does not exist
        }
    }
    IEnumerator FriendsMessengerSendEnumerator(string usernameToSend, string userIDtoSend, string messageToSend)
    {
        //todo disable input field
        if (PlayerPrefs.HasKey("DisableWhisper") == false || PlayerPrefs.GetString("DisableWhisper") == "F")
        {
            FriendsMessage sentMessage = new FriendsMessage(username, usernameToSend.Replace("[", "").Replace("]", ""), userID, userIDtoSend, messageToSend);

            string json1 = JsonUtility.ToJson(sentMessage);
            var sendMessage = dbReference.Child("whisperChat").Child(userID).Push().SetRawJsonValueAsync(json1);
            yield return new WaitUntil(predicate: () => sendMessage.IsCompleted);
            if (sendMessage.IsFaulted)
            {
                Debug.Log("error1");
                messengerErorr.text = "Error A sending message";
            }
            else if (sendMessage.IsCompleted)
            {
                FriendsMessage receievedMessage = sentMessage;

                string json2 = JsonUtility.ToJson(receievedMessage);
                var receieveMessage = dbReference.Child("whisperChat").Child(userIDtoSend).Push().SetRawJsonValueAsync(json2);
                yield return new WaitUntil(predicate: () => receieveMessage.IsCompleted);
                if (receieveMessage.IsFaulted)
                {
                    Debug.Log("error2");
                    messengerErorr.text = "Error B sending message";
                }
                else if (receieveMessage.IsCompleted)
                {
                    StartCoroutine(SelectMessengerInputFieldSendMessage());
                }

            }
        }
        else if (PlayerPrefs.GetString("DisableWhisper") == "T")
        {
            messengerErorr.text = "Whisper chat is disabled";
        }

    }
    IEnumerator GlobalMessengerSendEnumerator(string messageToSend)
    {

        if (PlayerPrefs.HasKey("DisableGlobal") == false || PlayerPrefs.GetString("DisableGlobal") == "F")
        {
            GlobalMessage globalMessage = new GlobalMessage(username, userID, messageToSend);

            string json3 = JsonUtility.ToJson(globalMessage);
            var globMessage = dbReference.Child("globalChat").Push().SetRawJsonValueAsync(json3);
            yield return new WaitUntil(predicate: () => globMessage.IsCompleted);
            if (globMessage.IsFaulted)
            {
                Debug.Log("error3");
                messengerErorr.text = "Error sending message";
            }
            else if (globMessage.IsCompleted)
            {
                GlobalChat();
                StartCoroutine(SelectMessengerInputFieldSendMessage());
            }
        }
        else if (PlayerPrefs.GetString("DisableGlobal") == "T")
        {
            messengerErorr.text = "Global chat is disabled";
        }
        //todo disable input field

    }

    public void FriendsListenForNewMessages()
    {
        void CurrentListener(object o, ChildChangedEventArgs args) //o does not do anything
        {

            if (args.DatabaseError != null)
            {
                Debug.Log("there were errors");
            }
            else
            {
                myFriendsJson = JsonUtility.FromJson<FriendsDeserialiseJson>(args.Snapshot.GetRawJsonValue());
                InstantiateMessageFriends(myFriendsJson.senderUsername.ToString(), myFriendsJson.receiverUsername.ToString(), myFriendsJson.senderID.ToString(), myFriendsJson.receiverID.ToString(), myFriendsJson.messageText.ToString());
            }

        }

        newMessageListener = CurrentListener;
        dbReference.Child("whisperChat").Child(userID).LimitToLast(10).ChildAdded += newMessageListener; //limiting how much data is pulled from firebase initially
    }

    public void GlobalListenForNewMessages()//trying to match the same code as friends
    {
        void CurrentListener2(object q, ChildChangedEventArgs args2) //o does not do anything
        {

            if (args2.DatabaseError != null)
            {
                Debug.Log("there were errors");
            }
            else
            {
                globalJson = JsonUtility.FromJson<GlobalDeserialiseJson>(args2.Snapshot.GetRawJsonValue());
                InstantiateMessageGlobal(globalJson.globalSenderUsername.ToString(), globalJson.globalSenderID.ToString(), globalJson.globalMessageText.ToString());
            }

        }

        newMessageListener2 = CurrentListener2;
        dbReference.Child("globalChat").LimitToLast(10).ChildAdded += newMessageListener2;
    }

    IEnumerator CountGlobalMessageTotal()
    {
        var globalCount = dbReference.Child("globalChat").GetValueAsync();
        yield return new WaitUntil(predicate: () => globalCount.IsCompleted);

        DataSnapshot snapshotGlobalCount = globalCount.Result;

        if (globalCount.Exception != null) //basic error checking, unsure if it actually does anything here of use
        {
            messengerErorr.text = "Issue counting total global messages";
        }
        else if (snapshotGlobalCount.Exists) //check if username is a real username that exists in the firebase users node
        {
            if (snapshotGlobalCount.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
            {
                messengerErorr.text = "Issue counting total global messages";
            }
            else if (snapshotGlobalCount.HasChildren)
            {
                globalCountTotal = (int)snapshotGlobalCount.ChildrenCount;
                Debug.Log("Total message count: " + globalCountTotal);
            }
        }
    }

    public void GlobalChatSetting() //clicking button to enable global chat
    {
        if (globalImage.sprite == chatImages[0].sprite) // if global chat is enabled
        {
            globalImage.sprite = chatImages[1].sprite;
            PlayerPrefs.SetString("DisableGlobal", "T");
            PlayerPrefs.Save();
            Debug.Log("off");
        }
        else if (globalImage.sprite == chatImages[1].sprite) // if global chat is disabled
        {
            globalImage.sprite = chatImages[0].sprite;
            PlayerPrefs.SetString("DisableGlobal", "F");
            PlayerPrefs.Save();
            Debug.Log("on");
        }
    }
    public void WhisperChatSetting() //clicking button to disable global chat
    {
        if (whisperImage.sprite == chatImages[2].sprite) // if whisper chat is enabled
        {
            whisperImage.sprite = chatImages[3].sprite;
            PlayerPrefs.SetString("DisableWhisper", "T");
            PlayerPrefs.Save();
            Debug.Log("off");
        }
        else if (whisperImage.sprite == chatImages[3].sprite) // if whisper chat is disabled
        {
            whisperImage.sprite = chatImages[2].sprite;
            PlayerPrefs.SetString("DisableWhisper", "F");
            PlayerPrefs.Save();
            Debug.Log("on");
        }

    }

    public void InstantiateMessageFriends(string _usernameSender, string _receiverUsername, string _senderID, string _receiverID, string _messageText)
    {
        if (messageClonesFriends.Count >= 10)//removing the old clones after some point, todo add "if not blocked"
        {
            var destroyMessage = messageClonesFriends[messageClonesFriends.Count - 10];
            messageClonesFriends.Remove(messageClonesFriends.Count - 10);
            Destroy((UnityEngine.Object)destroyMessage);
        }

        if (blockedListUsernames.Contains(_usernameSender))
        {
            Debug.Log("user is blocked, ignored the message");
        }
        else if (PlayerPrefs.GetString("DisableWhisper") == "T") //hiding the first historic global message
        {
            Debug.Log("whisper chat is disabled, ignored the message");
        }
        else
        {
            if (friendsListUserIDs.Contains(_receiverID) && _senderID == userID) // send someone a message on my friends list
            {
                var newMessage = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesFriends.Add(newMessage);
                newMessage.transform.SetParent(messageContainter, false);

                newMessage.GetComponent<TextMeshProUGUI>().color = new Color32(86, 255, 0, 255); //friends green
                newMessage.GetComponent<TextMeshProUGUI>().text = "You tell " + "[" + _receiverUsername + "]: " + _messageText;

                newMessage.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _senderID;
                newMessage.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _usernameSender;

            }
            else if (friendsListUserIDs.Contains(_senderID) && _receiverID == userID) //someone sends me a message that's on my friends list
            {
                var newMessage = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesFriends.Add(newMessage);
                newMessage.transform.SetParent(messageContainter, false);

                newMessage.GetComponent<TextMeshProUGUI>().color = new Color32(86, 255, 0, 255); //friends green
                newMessage.GetComponent<TextMeshProUGUI>().text = "[" + _usernameSender + "]" + " tells you: " + _messageText;

                newMessage.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _senderID;
                newMessage.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _usernameSender;

            }
            else if (!friendsListUserIDs.Contains(_receiverID) && _senderID == userID)// you send someone a message NOT on my friends list
            {
                var newMessage = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesFriends.Add(newMessage);
                newMessage.transform.SetParent(messageContainter, false);

                newMessage.GetComponent<TextMeshProUGUI>().color = new Color32(250, 60, 214, 255); //whisper purple
                newMessage.GetComponent<TextMeshProUGUI>().text = "You tell " + "[" + _receiverUsername + "]: " + _messageText;

                newMessage.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _senderID;
                newMessage.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _usernameSender;

            }
            else if (!friendsListUserIDs.Contains(_senderID) && _receiverID == userID)//someone sends me a message that's NOT on my friends list
            {
                var newMessage = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesFriends.Add(newMessage);
                newMessage.transform.SetParent(messageContainter, false);

                newMessage.GetComponent<TextMeshProUGUI>().color = new Color32(250, 60, 214, 255); //whisper purple
                newMessage.GetComponent<TextMeshProUGUI>().text = "[" + _usernameSender + "]" + " tells you: " + _messageText;

                newMessage.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _senderID;
                newMessage.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _usernameSender;

            }
        }

    }

    public void InstantiateMessageGlobal(string _gloalUsernameSender, string _globalSenderID, string _globalMessageText)
    {
        if (globalCountCheck <= 10) //added this to try and enable faster global chat
        {
            globalCountCheck = globalCountCheck + 1;
        }

        if (messageClonesGlobal.Count >= 10)//removing the old clones after 10 messages
        {
            var destroyMessageGlobal = messageClonesGlobal[messageClonesGlobal.Count - 10];
            messageClonesGlobal.Remove(messageClonesGlobal.Count - 10);
            Destroy((UnityEngine.Object)destroyMessageGlobal);
        }

        if (blockedListUsernames.Contains(_gloalUsernameSender))
        {
            Debug.Log("user is blocked, ignored the message");
        }
        else if (PlayerPrefs.GetString("DisableGlobal") == "T") //hiding the first historic global message
        {
            Debug.Log("global chat is disabled, ignored the message");
        }
        else if (globalCountTotal <= 10)
        {
            if (_globalSenderID == userID && globalCountCheck > globalCountTotal)
            {
                var newMessageGlobal = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesGlobal.Add(newMessageGlobal);
                newMessageGlobal.transform.SetParent(messageContainter, false);

                newMessageGlobal.GetComponent<TextMeshProUGUI>().text = "You say: " + _globalMessageText;

                newMessageGlobal.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _globalSenderID;
                newMessageGlobal.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _gloalUsernameSender;
            }
            else if (_globalSenderID != userID && globalCountCheck > globalCountTotal)
            {
                var newMessageGlobal = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesGlobal.Add(newMessageGlobal);
                newMessageGlobal.transform.SetParent(messageContainter, false);

                newMessageGlobal.GetComponent<TextMeshProUGUI>().text = _gloalUsernameSender + " says: " + _globalMessageText;

                newMessageGlobal.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _globalSenderID;
                newMessageGlobal.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _gloalUsernameSender;
            }
        }
        else if (globalCountTotal > 10)
        {
            if (_globalSenderID == userID && globalCountCheck > 10)
            {
                var newMessageGlobal = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesGlobal.Add(newMessageGlobal);
                newMessageGlobal.transform.SetParent(messageContainter, false);

                newMessageGlobal.GetComponent<TextMeshProUGUI>().text = "You say: " + _globalMessageText;

                newMessageGlobal.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _globalSenderID;
                newMessageGlobal.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _gloalUsernameSender;
            }
            else if (_globalSenderID != userID && globalCountCheck > 10)
            {
                var newMessageGlobal = Instantiate(instance.messagePrefab, instance.transform.position, Quaternion.identity);
                messageClonesGlobal.Add(newMessageGlobal);
                newMessageGlobal.transform.SetParent(messageContainter, false);

                newMessageGlobal.GetComponent<TextMeshProUGUI>().text = _gloalUsernameSender + " says: " + _globalMessageText;

                newMessageGlobal.transform.Find("WhisperSenderUserID").GetComponent<TextMeshProUGUI>().text = _globalSenderID;
                newMessageGlobal.transform.Find("WhisperUsername").GetComponent<TextMeshProUGUI>().text = _gloalUsernameSender;
            }
        }
    }

    [Serializable]
    public class FriendsMessage
    {
        public string senderUsername;
        public string receiverUsername;
        public string senderID;
        public string receiverID;
        public string messageText;

        public FriendsMessage(string senderUsername, string receiverUsername, string senderID, string receiverID, string messageText)
        {
            this.senderUsername = senderUsername;
            this.receiverUsername = receiverUsername;
            this.senderID = senderID;
            this.receiverID = receiverID;
            this.messageText = messageText;
        }
    }

    [Serializable]
    public class FriendsDeserialiseJson
    {
        public string senderUsername;
        public string receiverUsername;
        public string senderID;
        public string receiverID;
        public string messageText;
    }

    [Serializable]
    public class GlobalMessage
    {
        public string globalSenderUsername;
        public string globalSenderID;
        public string globalMessageText;

        public GlobalMessage(string globalSenderUsername, string globalSenderID, string globalMessageText)
        {
            this.globalSenderUsername = globalSenderUsername;
            this.globalSenderID = globalSenderID;
            this.globalMessageText = globalMessageText;
        }
    }

    [Serializable]
    public class GlobalDeserialiseJson
    {
        public string globalSenderUsername;
        public string globalSenderID;
        public string globalMessageText;
    }


    IEnumerator SelectFriendsAddInputField() //ensures pressing tab/enter doesn't cause any weird bugs
    {
        yield return new WaitForEndOfFrame();
        friendsAddInputField.ActivateInputField();
    }
    IEnumerator SelectFriendsRemoveInputField() //ensures pressing tab/enter doesn't cause any weird bugs
    {
        yield return new WaitForEndOfFrame();
        friendsRemoveInputField.ActivateInputField();
    }

    IEnumerator SelectFriendsBlockAddInputField() //ensures pressing tab/enter doesn't cause any weird bugs
    {
        yield return new WaitForEndOfFrame();
        friendsBlockAddInputField.ActivateInputField();
    }
    IEnumerator SelectFriendsBlockRemoveInputField() //ensures pressing tab/enter doesn't cause any weird bugs
    {
        yield return new WaitForEndOfFrame();
        friendsBlockRemoveInputField.ActivateInputField();
    }

    IEnumerator SelectMessengerInputFieldSendMessage() //ensures pressing tab/enter doesn't cause any weird bugs
    {
        yield return new WaitForEndOfFrame();
        ClearUI();
        messengerInputField.text = "";
        messengerInputField.ActivateInputField();
    }
    IEnumerator SelectMessengerInputField() //ensures pressing tab/enter doesn't cause any weird bugs
    {
        yield return new WaitForEndOfFrame();
        messengerInputField.ActivateInputField();
    }

    public IEnumerator RetrieveFriends() //find the current user and look in their friends node in firebase to retrieve their currently added friends
    {

        friendsImageLoading.SetActive(true);

        friendsListUserIDs = new List<string>(); //resetting our lists/views so that the scrollview does not continue to add values
        friendsListUsernames = new List<string>();

        var friendsLoad = FirebaseDatabase.DefaultInstance.GetReference("friends").Child(userID).GetValueAsync();
        yield return new WaitUntil(predicate: () => friendsLoad.IsCompleted);

        if (friendsLoad.IsFaulted)
        {
            Debug.Log("Potential error loading friends list - check if user has friends1");
            outputTextFriendsErorr.text = "Error when retrieving friends, try again";
            friendsImageLoading.SetActive(false);
        }
        else if (friendsLoad.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
        {
            DataSnapshot snapshot = friendsLoad.Result;
            string childString = snapshot.ChildrenCount.ToString();
            childCount = int.Parse(childString);
            int i = 0;

            foreach (DataSnapshot child in snapshot.Children)
            {
                //firebase structure is friends->userID of current user-> friends username: friends userID
                friendsListUserIDs.Add(child.Value.ToString()); ///add the key of the found childs to the list. These are the usernames of their friends
                friendsListUsernames.Add(child.Key.ToString());
                i++;
            }

            if (friendsObjectHolder.activeSelf && friendsUI.activeSelf)
            {
                if (childCount == 0)
                {
                    StartCoroutine(RetrievePFPs());
                }
                else if (i >= childCount)
                {
                    StartCoroutine(RetrievePFPs());
                }
                else
                {
                    outputTextFriendsErorr.text = "Error loading friends, please try again1";
                }
            }

            else if (!friendsObjectHolder.activeSelf || !friendsUI.activeSelf) //this is for when this first loads and we want to start listening for messages from friends (i.e. we need to know who our friends are)
            {
                if (childCount == 0)
                {
                    yield return new WaitForEndOfFrame();
                }
                else if (i >= childCount)
                {
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    outputTextFriendsErorr.text = "Error loading friends, please try again2";
                }
            }
        }
    }
    public IEnumerator RetrievePFPs()
    {
        friendsListUserPFPs = new List<string>();
        friendsListOnlineStatus = new List<string>();

        int i = 0;

        for (i = 0; i < friendsListUsernames.Count; i++)
        {
            string userID = friendsListUserIDs[i];

            var friendsRetrieved = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("pfp").GetValueAsync();
            yield return new WaitUntil(predicate: () => friendsRetrieved.IsCompleted);

            if (friendsRetrieved.IsFaulted)
            {
                outputTextFriendsErorr.text = "Potential error at Fetching Friend PFPs";
            }
            else if (friendsRetrieved.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
            {
                DataSnapshot snapshot = friendsRetrieved.Result;
                friendsListUserPFPs.Add(snapshot.Value.ToString()); //add the key of the found childs to the list. These are the usernames of their friends
            }

        }

        if (friendsListUsernames.Count == 0)
        {
            StartCoroutine(RetrieveOnlineStatus(friendsListOnlineStatus));
        }
        else if (i >= friendsListUsernames.Count)
        {
            StartCoroutine(RetrieveOnlineStatus(friendsListOnlineStatus));
        }
        else
        {
            outputTextFriendsErorr.text = "Error loading pfps, please try again";
        }

    }

    public IEnumerator RetrieveOnlineStatus(List<string> friendsListOnlineStatus)
    {

        yield return new WaitForEndOfFrame(); //todo need better way to give enough time for online status list to propregate
        int i = 0;

        for (i = 0; i < friendsListUsernames.Count; i++)
        {
            string userID = friendsListUserIDs[i];

            var onlineStatus = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("online").GetValueAsync();
            yield return new WaitUntil(predicate: () => onlineStatus.IsCompleted);

            if (onlineStatus.IsFaulted)
            {
                outputTextFriendsErorr.text = "Potential error at fetching online status";
            }
            else if (onlineStatus.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
            {
                DataSnapshot snapshot2 = onlineStatus.Result;
                friendsListOnlineStatus.Add(snapshot2.Value.ToString()); //add the key of the found childs to the list. These are the usernames of their friends
            }
        }

        if (friendsListUsernames.Count == 0)
        {
            friendsScrollViewAdapter = GameObject.FindObjectOfType<FriendsScrollViewAdapter>();
            friendsScrollViewAdapter.UpdateItems2(friendsListUsernames.Count); //send friends list size to this script to begin calculating friends to display
        }
        else if (i >= friendsListUsernames.Count)
        {
            friendsScrollViewAdapter = GameObject.FindObjectOfType<FriendsScrollViewAdapter>();
            friendsScrollViewAdapter.UpdateItems2(friendsListUsernames.Count); //send friends list size to this script to begin calculating friends to display
        }
        else
        {
            outputTextFriendsErorr.text = "Error loading statuses, please try again";
        }

    }

    public IEnumerator RetrieveBlockedUsers()
    {
        friendsImageLoading.SetActive(true);

        blockedListUserIDs = new List<string>(); //resetting our lists/views so that the scrollview does not continue to add values
        blockedListUsernames = new List<string>();

        var blockedLoad = FirebaseDatabase.DefaultInstance.GetReference("blocked").Child(userID).GetValueAsync();
        yield return new WaitUntil(predicate: () => blockedLoad.IsCompleted);

        if (blockedLoad.IsFaulted)
        {
            Debug.Log("Potential error loading block list - check if user has blocked users1");
            outputTextFriendsErorr.text = "Error when retrieving blocked users, try again";
            friendsImageLoading.SetActive(false);
        }
        else if (blockedLoad.IsCompleted) //if we were able to query successfully, save query results into a datasnapshot and send it to a list
        {
            DataSnapshot snapshot = blockedLoad.Result;
            string childString = snapshot.ChildrenCount.ToString();
            childCount = int.Parse(childString);
            int i = 0;

            foreach (DataSnapshot child in snapshot.Children)
            {
                //firebase structure is friends->userID of current user-> friends username: friends userID
                blockedListUserIDs.Add(child.Value.ToString()); ///add the key of the found childs to the list. These are the usernames of their friends
                blockedListUsernames.Add(child.Key.ToString());
                i++;
            }

            if (friendsBlockedUI.activeSelf && friendsUI.activeSelf)
            {
                if (childCount == 0)
                {
                    blockedScrollViewAdapter = GameObject.FindObjectOfType<BlockedScrollViewAdapter>(); //find the scrollview script for the friends list scroll UI
                    blockedScrollViewAdapter.UpdateItems2(blockedListUsernames.Count);
                }
                else if (i >= childCount)
                {
                    blockedScrollViewAdapter = GameObject.FindObjectOfType<BlockedScrollViewAdapter>(); //find the scrollview script for the friends list scroll UI
                    blockedScrollViewAdapter.UpdateItems2(blockedListUsernames.Count);
                }
                else
                {
                    outputTextFriendsErorr.text = "Error loading blocked users, please try again1";
                }
            }

            else if (!friendsBlockedUI.activeSelf || !friendsUI.activeSelf)//this is for when this first loads and we want to start listening for messages from friends (i.e. we need to know who our friends are)
            {
                if (childCount == 0)
                {
                    yield return new WaitForEndOfFrame();
                }
                else if (i >= childCount)
                {
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    outputTextFriendsErorr.text = "Error loading blocked users, please try again2";
                }
            }
        }
    }

    public void AddFriendsButton() //press enter or click the magnify glass to search for friends
    {
        outputTextFriendsErorr.text = ""; //reset any old output text
        outputTextFriendsSuccess.text = "";


        if (friendsAddInputField.text == "" && friendsBlockAddInputField.text == "") //check if entered name is blank, if blank then throw output message
        {
            outputTextFriendsErorr.text = "Enter a valid username"; //works fine
        }
        else if (friendsAddInputField.text.Length > 12 || friendsBlockAddInputField.text.Length > 12)
        {
            outputTextFriendsErorr.text = "Name too long";
        }
        else if (friendsAddInputField.text == username || friendsBlockAddInputField.text == username)
        {
            outputTextFriendsErorr.text = "You cannot add yourself";
        }
        else if (friendsListUsernames.Count >= 50 && friendsObjectHolder.activeSelf)
        {
            outputTextFriendsErorr.text = "You cannot add anymore friends";
        }
        else if (blockedListUsernames.Count >= 200 && !friendsObjectHolder.activeSelf)
        {
            outputTextFriendsErorr.text = "You cannot block anymore users";
        }
        else if (blockedListUsernames.Contains(friendsAddInputField.text) && friendsObjectHolder.activeSelf)
        {
            outputTextFriendsErorr.text = friendsAddInputField.text + " is blocked";
        }
        else if (friendsListUsernames.Contains(friendsBlockAddInputField.text) && friendsBlockedUI.activeSelf)
        {
            outputTextFriendsErorr.text = friendsBlockAddInputField.text + " is added as a friend";
        }
        else
        {
            StartCoroutine(AddFriendsButtonEnumerator()); //if text has been entered, begin firebase search
        }
    }

    IEnumerator AddFriendsButtonEnumerator() //kick off the username check coroutine
    {
        yield return StartCoroutine(CheckIfUserAlreadyAdded((string returnedUsername) =>
        {
            //fire check if user is already added, wait until value is returned before proceeding

            if (returnedUsername == "Duplicate" && friendsObjectHolder.activeSelf) //check friends list and if the user is already added, return duplicate
            {
                outputTextFriendsErorr.text = friendsAddInputField.text + " is already added";
            }
            else if (returnedUsername == "Duplicate" && !friendsObjectHolder.activeSelf)
            {
                outputTextFriendsErorr.text = friendsBlockAddInputField.text + " is already added";
            }
            else if (returnedUsername == "Error") //check friends list and if the user is already added, return duplicate
            {
                outputTextFriendsErorr.text = "Error looking up user in friends data structure";
            }
            else if (returnedUsername == "Continue") //if returned True, then update friends list (explicity stating True for ease in remembering what was coded)
            {
                StartCoroutine(CheckIfUserExists());
            }
            else
            {
                outputTextFriendsErorr.text = "Error searching for if the user exists";
            }
        }));
    }

    IEnumerator CheckIfUserAlreadyAdded(Action<string> onCallback)
    {
        if (friendsObjectHolder.activeSelf)
        {
            var alreadyAdded = FirebaseDatabase.DefaultInstance.GetReference("friends").Child(userID).Child(friendsAddInputField.text).GetValueAsync();
            yield return new WaitUntil(predicate: () => alreadyAdded.IsCompleted);

            DataSnapshot userAlreadyAdded = alreadyAdded.Result;

            if (alreadyAdded.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at CheckIfUsernameExistsAdd";
            }
            else if (userAlreadyAdded.Exists) //check if username is already on a user's friends list
            {
                if (userAlreadyAdded.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    onCallback.Invoke("Duplicate"); ; //if the username is already added, i.e. if it was found, return as such
                }
            }
            else
            {
                onCallback.Invoke("Continue");
            }
        }
        else if (!friendsObjectHolder.activeSelf)
        {
            var alreadyAdded = FirebaseDatabase.DefaultInstance.GetReference("blocked").Child(userID).Child(friendsBlockAddInputField.text).GetValueAsync();
            yield return new WaitUntil(predicate: () => alreadyAdded.IsCompleted);

            DataSnapshot userAlreadyAdded = alreadyAdded.Result;

            if (alreadyAdded.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at CheckIfUsernameExistsAdd";
            }
            else if (userAlreadyAdded.Exists) //check if username is already on a user's friends list
            {
                if (userAlreadyAdded.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    onCallback.Invoke("Duplicate"); ; //if the username is already added, i.e. if it was found, return as such
                }
            }
            else
            {
                onCallback.Invoke("Continue");
            }
        }
    }

    IEnumerator CheckIfUserExists()
    {

        yield return StartCoroutine(CheckingIfUserExists((string returnedUserID) =>
        {
            //after checking that they are not already addded, check that they are a real user

            if (returnedUserID == "False" && friendsObjectHolder.activeSelf) //if username was unable to be found in firebase users node, return false
            {
                outputTextFriendsErorr.text = friendsAddInputField.text + " does not exist";
            }
            else if (returnedUserID == "False" && !friendsObjectHolder.activeSelf)
            {
                outputTextFriendsErorr.text = friendsBlockAddInputField.text + " does not exist";
            }
            else if (returnedUserID == "Error") //check friends list and if the user is already added, return duplicate
            {
                outputTextFriendsErorr.text = "Error looking up user in users data structure";
            }
            else
            {
                StartCoroutine(SetFriendAddValue(returnedUserID));
            }
        }));
    }

    IEnumerator SetFriendAddValue(string _returnedUserID) //also make a new retrieve method for when blocking to add in if statement
    {
        if (friendsObjectHolder.activeSelf)
        {
            var friendAdd = dbReference.Child("friends").Child(userID).Child(friendsAddInputField.text).SetValueAsync(_returnedUserID);
            yield return new WaitUntil(predicate: () => friendAdd.IsCompleted); //todo add check for failure
            StartCoroutine(RetrieveFriends()); //reload friends
            outputTextFriendsSuccess.text = friendsAddInputField.text + " has been added";

            if (messengerFriendUsername.text.Replace("[", "").Replace("]", "") == friendsAddInputField.text)
            {
                messengerFriendUsername.color = new Color32(86, 255, 0, 255);
            }
        }
        else if (!friendsObjectHolder.activeSelf)
        {
            var friendAdd = dbReference.Child("blocked").Child(userID).Child(friendsBlockAddInputField.text).SetValueAsync(_returnedUserID);
            yield return new WaitUntil(predicate: () => friendAdd.IsCompleted); //todo add check for failure
            StartCoroutine(RetrieveBlockedUsers()); //reload blocked users
            outputTextFriendsSuccess.text = friendsBlockAddInputField.text + " has been blocked";
        }
    }

    IEnumerator CheckingIfUserExists(Action<string> onCallback)
    {
        if (friendsObjectHolder.activeSelf)
        {
            var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("username").EqualTo(friendsAddInputField.text).GetValueAsync();
            yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

            DataSnapshot snapshotUsername = usernameCheck.Result;

            if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at CheckingIfUserExists";
            }
            else if (snapshotUsername.Exists) //check if username is a real username that exists in the firebase users node
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>(); //this is grabbing the key which is the userID of the friend being added
                    foreach (DataSnapshot child in snapshotUsername.Children) //this should only ever loop once in this firebase node, but there might be a better way to do it
                    {
                        dict.Add(child.Key, child.Value);
                        //Debug.Log(child.Key);
                        onCallback.Invoke(child.Key.ToString()); //send back the user's key because they exist and are not a duplicate
                    }
                }
            }
            else
            {
                onCallback.Invoke("False"); //if nothing was found, the user does not exist
            }
            //Debug.Log("the raw json " + snapshotUsername.GetRawJsonValue()); //debugging the returned value
        }
        else if (!friendsObjectHolder.activeSelf)
        {
            var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("username").EqualTo(friendsBlockAddInputField.text).GetValueAsync();
            yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

            DataSnapshot snapshotUsername = usernameCheck.Result;

            if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at CheckingIfUserExists";
            }
            else if (snapshotUsername.Exists) //check if username is a real username that exists in the firebase users node
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>(); //this is grabbing the key which is the userID of the friend being added
                    foreach (DataSnapshot child in snapshotUsername.Children) //this should only ever loop once in this firebase node, but there might be a better way to do it
                    {
                        dict.Add(child.Key, child.Value);
                        //Debug.Log(child.Key);
                        onCallback.Invoke(child.Key.ToString()); //send back the user's key because they exist and are not a duplicate
                    }
                }
            }
            else
            {
                onCallback.Invoke("False"); //if nothing was found, the user does not exist
            }
            //Debug.Log("the raw json " + snapshotUsername.GetRawJsonValue()); //debugging the returned value
        }
    }

    public void RemoveFriendsButton() //same setup as add friends function
    {
        outputTextFriendsErorr.text = "";
        outputTextFriendsSuccess.text = "";

        if (friendsRemoveInputField.text == "" && friendsBlockRemoveInputField.text == "")
        {
            outputTextFriendsErorr.text = "Enter a valid username";
        }
        else if (friendsRemoveInputField.text == username || friendsBlockRemoveInputField.text == username)
        {
            outputTextFriendsErorr.text = "You cannot remove yourself";
        }
        else if (friendsListUsernames.Count == 0 && friendsObjectHolder.activeSelf)
        {
            outputTextFriendsErorr.text = "You have no friends";
        }
        else if (blockedListUsernames.Count == 0 && friendsBlockedUI.activeSelf)
        {
            outputTextFriendsErorr.text = "You have not blocked anyone";
        }
        else
        {
            StartCoroutine(CheckIfUsernameExistsRemove());//todo: this is the first place weird issues might happen, i.e. removing users that don't exist.
        }

    }

    IEnumerator CheckIfUsernameExistsRemove() //looking for if the username actually exists, if it does then continue because we now have that user's key
    {
        yield return StartCoroutine(findFriendsUserIDRemove((string returnedUserID) =>
        {
            if (returnedUserID == "False" && friendsObjectHolder.activeSelf)
            {
                outputTextFriendsErorr.text = friendsRemoveInputField.text + " does not exist";
            }
            else if (returnedUserID == "False" && friendsBlockedUI.activeSelf)
            {
                outputTextFriendsErorr.text = friendsBlockRemoveInputField.text + " does not exist";
            }
            else if (returnedUserID == "Error") //check friends list and if the user is already added, return duplicate
            {
                outputTextFriendsErorr.text = "Error looking up user in users data structure";
            }
            else
            {
                StartCoroutine(checkIfFriendisAdded(returnedUserID)); //pass the returned username to check to see if the user is actually a friend

            }
        }));
    }

    IEnumerator findFriendsUserIDRemove(Action<string> onCallback) //this is making sure the user actually exists and if they do it passes the firebase key back (key = username here)
    {
        if (friendsObjectHolder.activeSelf)
        {
            var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("username").EqualTo(friendsRemoveInputField.text).GetValueAsync();
            yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

            DataSnapshot snapshotUsername = usernameCheck.Result;

            if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at findFriendsUserIDRemove1";
            }
            else if (snapshotUsername.Exists) //check if username is a real username that exists in the firebase users node
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>(); //this is grabbing the key which is the userID of the friend being added
                    foreach (DataSnapshot child in snapshotUsername.Children)
                    {
                        dict.Add(child.Key, child.Value);
                        onCallback.Invoke(child.Key.ToString()); //sending back the key which is the username
                    }
                }
            }
            else
            {
                onCallback.Invoke("False"); //send back false if we could not find the user
            }
        }
        else if (friendsBlockedUI.activeSelf)
        {
            var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("username").EqualTo(friendsBlockRemoveInputField.text).GetValueAsync();
            yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

            DataSnapshot snapshotUsername = usernameCheck.Result;

            if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at findFriendsUserIDRemove2";
            }
            else if (snapshotUsername.Exists) //check if username is a real username that exists in the firebase users node
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>(); //this is grabbing the key which is the userID of the friend being added
                    foreach (DataSnapshot child in snapshotUsername.Children)
                    {
                        dict.Add(child.Key, child.Value);
                        onCallback.Invoke(child.Key.ToString()); //sending back the key which is the username
                    }
                }
            }
            else
            {
                onCallback.Invoke("False"); //send back false if we could not find the user
            }
        }
        //Debug.Log("the raw json " + snapshotUsername.GetRawJsonValue());
    }

    IEnumerator checkIfFriendisAdded(string userKey) //now that we know the user exists and we have their userID, we can query against the users friends list to
                                                     //make sure they are actually a friend that has been added
    {
        //Debug.Log("the userID being checked is " + userKey);
        if (friendsObjectHolder.activeSelf)
        {
            var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("friends").OrderByChild(friendsRemoveInputField.text).EqualTo(userKey).GetValueAsync();
            yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

            DataSnapshot snapshotUsername = usernameCheck.Result;

            if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at findFriendsUserIDRemove";
            }
            else if (snapshotUsername.Exists) //if they are on their friends list, remove them
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    outputTextFriendsErorr.text = "Error looking up user in friends data structure";
                }
                else
                {
                    var valueSet = dbReference.Child("friends").Child(userID).Child(friendsRemoveInputField.text).SetValueAsync(null);
                    yield return new WaitUntil(predicate: () => valueSet.IsCompleted); //todo maybe add some exception checking here eventually

                    outputTextFriendsErorr.text = "";
                    outputTextFriendsSuccess.text = friendsRemoveInputField.text + " has been removed";

                    if (messengerFriendUsername.text.Replace("[", "").Replace("]", "") == friendsRemoveInputField.text)
                    {
                        messengerFriendUsername.color = new Color32(250, 60, 214, 255);
                    }

                    StartCoroutine(RetrieveFriends());
                }
            }
            else //if not, then throw an output
            {
                outputTextFriendsErorr.text = friendsRemoveInputField.text + " is not on your friends list";
            }
        }
        else if (friendsBlockedUI.activeSelf)
        {
            var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("blocked").OrderByChild(friendsBlockRemoveInputField.text).EqualTo(userKey).GetValueAsync();
            yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

            DataSnapshot snapshotUsername = usernameCheck.Result;

            if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
            {
                outputTextFriendsErorr.text = "Error at findFriendsUserIDRemove";
            }
            else if (snapshotUsername.Exists) //if they are on their friends list, remove them
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    outputTextFriendsErorr.text = "Error looking up user in friends data structure";
                }
                else
                {
                    outputTextFriendsErorr.text = "";
                    outputTextFriendsSuccess.text = friendsBlockRemoveInputField.text + " has been unblocked";

                    var valueSet = dbReference.Child("blocked").Child(userID).Child(friendsBlockRemoveInputField.text).SetValueAsync(null);
                    yield return new WaitUntil(predicate: () => valueSet.IsCompleted); //todo maybe add some exception checking here eventually

                    StartCoroutine(RetrieveBlockedUsers());
                }
            }
            else //if not, then throw an output
            {
                outputTextFriendsErorr.text = friendsBlockRemoveInputField.text + " is not on your blocked list";
            }
        }

        //Debug.Log("the raw json for removal is " + snapshotUsername.GetRawJsonValue());
    }

    //
    //all functions below here are related to the settings menu, scene changes, and player prefs
    //

    public void ResolutionSettings()
    {
        if (settingsResolutionClosed.activeSelf)
        {
            settingsResolutionOpen.SetActive(true);
            settingsResolutionGrouper.SetActive(true);
            settingsResolutionClosed.SetActive(false);
            settingsQualityOpen.SetActive(false);
            settingsQualityGrouper.SetActive(false);
            settingsQualityClosed.SetActive(true);
        }
        else if (settingsResolutionOpen.activeSelf)
        {
            settingsResolutionClosed.SetActive(true);
            settingsResolutionOpen.SetActive(false);
            settingsResolutionGrouper.SetActive(false);
        }
    }

    public void ResolutionSettings1024()
    {
        PlayerPrefs.SetString("Resolution", "1024");
        PlayerPrefs.Save();

        if (settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(1024, 576, FullScreenMode.FullScreenWindow);
            settingsResolution.text = "1024x576";
        }
        else if (!settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(1024, 576, false);
            settingsResolution.text = "1024x576";
        }

        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }

    public void ResolutionSettings1280()
    {
        PlayerPrefs.SetString("Resolution", "1280");
        PlayerPrefs.Save();

        if (settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
            settingsResolution.text = "1280x720";
        }
        else if (!settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(1280, 720, false);
            settingsResolution.text = "1280x720";
        }

        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }

    public void ResolutionSettings1920()
    {
        PlayerPrefs.SetString("Resolution", "1920");
        PlayerPrefs.Save();

        if (settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            settingsResolution.text = "1920x1080";
        }
        else if (!settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(1920, 1080, false);
            settingsResolution.text = "1920x1080";
        }

        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }

    public void ResolutionSettings2560()
    {
        PlayerPrefs.SetString("Resolution", "2560");
        PlayerPrefs.Save();

        if (settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow);
            settingsResolution.text = "2560x1440";
        }
        else if (!settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(2560, 1440, false);
            settingsResolution.text = "2560x1440";
        }

        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }
    public void ResolutionSettings3840()
    {
        PlayerPrefs.SetString("Resolution", "3840");
        PlayerPrefs.Save();

        if (settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow);
            settingsResolution.text = "3840x2160";
        }
        else if (!settingsFullscreenEnabled.activeSelf)
        {
            Screen.SetResolution(3840, 2160, false);
            settingsResolution.text = "3840x2160";
        }

        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }

    public void ResolutionSettingsDynamic()
    {
        string desktopResolution = Screen.currentResolution.ToString();
        int desktopIndex = desktopResolution.IndexOf(" @");
        if (desktopIndex >= 0)
        {
            desktopResolution = desktopResolution.Substring(0, desktopIndex);
        }
        string spaceRemove = " ";
        desktopResolution = desktopResolution.Replace(spaceRemove, "");

        string[] desktopResolutionArray = desktopResolution.Split(char.Parse("x"));
        int resWidth = int.Parse(desktopResolutionArray[0]);
        int resHeight = int.Parse(desktopResolutionArray[1]);

        if (resWidth > 0 && resHeight > 0)
        {
            if (resWidth != PlayerPrefs.GetInt("resWidth") && resHeight != PlayerPrefs.GetInt("resHeight"))
            {
                Screen.SetResolution(PlayerPrefs.GetInt("resWidth"), PlayerPrefs.GetInt("resHeight"), FullScreenMode.FullScreenWindow);
                desktopResolution = PlayerPrefs.GetInt("resWidth").ToString() + "x" + PlayerPrefs.GetInt("resHeight").ToString();
                settingsResolution.text = desktopResolution;
            }
            else
            {
                Screen.SetResolution(resWidth, resHeight, FullScreenMode.FullScreenWindow);
                settingsResolution.text = desktopResolution;
            }
        }
        else
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            settingsResolution.text = "Error - Dynamic";
        }

        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }

    public void ResetGraphics()
    {
        PlayerPrefs.DeleteKey("Resolution");
        PlayerPrefs.DeleteKey("fullscreenDisabled");
        PlayerPrefs.DeleteKey("tooltipDisabled");
        PlayerPrefs.DeleteKey("Quality");
        PlayerPrefs.SetString("DisableDeleteDeckWarning", "F");

        settingsFullscreenEnabled.SetActive(true);
        settingsHelpTextEnabled.SetActive(false);
        settingsDeleteDeckEnabled.SetActive(false);

        ResolutionSettingsDynamic();

        QualitySettingMed();
    }

    public void QualitySetting()
    {
        if (settingsQualityClosed.activeSelf)
        {
            settingsQualityOpen.SetActive(true);
            settingsQualityGrouper.SetActive(true);
            settingsQualityClosed.SetActive(false);
            settingsResolutionOpen.SetActive(false);
            settingsResolutionGrouper.SetActive(false);
            settingsResolutionClosed.SetActive(true);
        }
        else if (settingsQualityOpen.activeSelf)
        {
            settingsQualityClosed.SetActive(true);
            settingsQualityOpen.SetActive(false);
            settingsQualityGrouper.SetActive(false);
        }
    }

    public void QualitySettingLow()
    {
        PlayerPrefs.SetString("Quality", "Low");
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(0);

        settingsQuality.text = "Low (MSSA Off)";
        settingsQualityClosed.SetActive(true);
        settingsQualityOpen.SetActive(false);
        settingsQualityGrouper.SetActive(false);
    }
    public void QualitySettingMed()
    {
        PlayerPrefs.SetString("Quality", "Med");
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(1);

        settingsQuality.text = "Med (MSSA 2x)";
        settingsQualityClosed.SetActive(true);
        settingsQualityOpen.SetActive(false);
        settingsQualityGrouper.SetActive(false);
    }
    public void QualitySettingHigh()
    {
        PlayerPrefs.SetString("Quality", "High");
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(2);

        settingsQuality.text = "High (MSSA 8x)";
        settingsQualityClosed.SetActive(true);
        settingsQualityOpen.SetActive(false);
        settingsQualityGrouper.SetActive(false);
    }
    public void FullscreenSetting()
    {
        if (settingsFullscreenEnabled.activeSelf)
        {
            settingsFullscreenEnabled.SetActive(false);
            Debug.Log("fullscreen disabled");
            PlayerPrefs.SetString("fullscreenDisabled", "1");
            Screen.fullScreen = false;
        }
        else if (!settingsFullscreenEnabled.activeSelf)
        {
            settingsFullscreenEnabled.SetActive(true);
            Debug.Log("fullscreen enabled");
            PlayerPrefs.DeleteKey("fullscreenDisabled");
            Screen.fullScreen = true;
        }
    }

    public void HelpTextSetting()
    {
        if (settingsHelpTextEnabled.activeSelf)
        {
            PlayerPrefs.DeleteKey("tooltipDisabled");
            Debug.Log("helptext enabled");
            PlayerPrefs.Save();
            settingsHelpTextEnabled.SetActive(false);
        }
        else if (!settingsHelpTextEnabled.activeSelf)
        {
            PlayerPrefs.SetString("tooltipDisabled", "1");
            Debug.Log("helptext disabled");
            settingsHelpTextEnabled.SetActive(true);
        }
    }
    public void DisableDecktSetting()
    {
        if (settingsDeleteDeckEnabled.activeSelf)
        {
            PlayerPrefs.SetString("DisableDeleteDeckWarning", "F");
            Debug.Log("deck warnings will show");
            PlayerPrefs.Save();
            settingsDeleteDeckEnabled.SetActive(false);
        }
        else if (!settingsDeleteDeckEnabled.activeSelf)
        {
            PlayerPrefs.SetString("DisableDeleteDeckWarning", "T");
            Debug.Log("deck warnings will not show");
            PlayerPrefs.Save();
            settingsDeleteDeckEnabled.SetActive(true);
        }
    }

    public void ResAndQualityDropdownClose()
    {
        settingsQualityClosed.SetActive(true);
        settingsQualityOpen.SetActive(false);
        settingsQualityGrouper.SetActive(false);
        settingsResolutionClosed.SetActive(true);
        settingsResolutionOpen.SetActive(false);
        settingsResolutionGrouper.SetActive(false);
    }
    public void Skirmish()
    {
        settingsHelpTextEnabled.SetActive(false);
        cursorManager.CursorNormal();
        RemoveListener();
	StopCoroutines();
        GameManager.instance.ChangeScene(3);
    }
    public void DeckBuilder()
    {
        settingsHelpTextEnabled.SetActive(false);
        cursorManager.CursorNormal();
        RemoveListener();
	StopCoroutines();
        GameManager.instance.ChangeScene(2);
    }

    private void RemoveListener()
    {
        if (newMessageListener != null)
        {
            dbReference.Child("whisperChat").Child(userID).LimitToLast(10).ChildAdded -= newMessageListener;
        }

        if (newMessageListener2 != null)
        {
            dbReference.Child("globalChat").LimitToLast(1).ChildAdded -= newMessageListener2;
        }
    }

    private void StopCoroutines()
    {
        if (countOnlineUsers != null)
        {
            StopCoroutine(countOnlineUsers);
        }
    }

    public class PlayerMetrics
    {
        public int xpOpen;
        public int mmrOpen;
        public int totalTimePlayedOpen;
        public int totalTurnsTakenOpen;
        public int margoDeckWinOpen;
        public int margoDeckLossOpen;
        public int miosDeckWinOpen;
        public int miosDeckLossOpen;
        public int nasseDeckWinOpen;
        public int nasseDeckLossOpen;
        public int voidDeckWinOpen;
        public int voidDeckLossOpen;
        public int tootDeckWinOpen;
        public int tootDeckLossOpen;

        public PlayerMetrics(int xpOpen, int mmrOpen, int totalTimePlayedOpen, int totalTurnsTakenOpen, int margoDeckWinOpen, int margoDeckLossOpen,
            int miosDeckWinOpen, int miosDeckLossOpen, int nasseDeckWinOpen, int nasseDeckLossOpen, int voidDeckWinOpen, int voidDeckLossOpen,
            int tootDeckWinOpen, int tootDeckLossOpen)
        {
            this.xpOpen = xpOpen;
            this.mmrOpen = mmrOpen;
            this.totalTimePlayedOpen = totalTimePlayedOpen;
            this.totalTurnsTakenOpen = totalTurnsTakenOpen;
            this.margoDeckWinOpen = margoDeckWinOpen;
            this.margoDeckLossOpen = margoDeckLossOpen;
            this.miosDeckWinOpen = miosDeckWinOpen;
            this.miosDeckLossOpen = miosDeckLossOpen;
            this.nasseDeckWinOpen = nasseDeckWinOpen;
            this.nasseDeckLossOpen = nasseDeckLossOpen;
            this.voidDeckWinOpen = voidDeckWinOpen;
            this.voidDeckLossOpen = voidDeckLossOpen;
            this.tootDeckWinOpen = tootDeckWinOpen;
            this.tootDeckLossOpen = tootDeckLossOpen;
        }

    }
}
