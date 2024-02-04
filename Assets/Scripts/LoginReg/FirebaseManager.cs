using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    private CursorMode cursorModeReset = CursorMode.Auto;
    public CursorManager cursorManager;

    //Version Check
    public string versionCheck = "0.1";
    public TMP_Text currentVersionText;

    //Other Variables
    private GameObject AuthUI;
    private GameObject RememberMe;

    //Update Game
    [Header("Update Game")]
    public GameObject downloadButton;
    [Space(5f)]

    //Firebase Variables
    [Header("Firebase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
    [Space(5f)]

    //Login Variables
    [Header("Login References")]
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;
    [SerializeField]
    public GameObject resetPasswordUI;
    [SerializeField]
    private TMP_InputField loginEmail;
    [SerializeField]
    private TMP_InputField loginPassword;
    [SerializeField]
    public TMP_Text loginOutputTextError;
    [SerializeField]
    public TMP_Text loginOutputTextSuccess;
    [SerializeField]
    public GameObject loginAnimation;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button registerLoginButton;
    [SerializeField]
    private GameObject resendEmail;
    [SerializeField]
    private Button resendEmailButton;
    [Space(5f)]

    //Register Variables
    [Header("Register References")]
    [SerializeField]
    public TMP_InputField registerUsername;
    [SerializeField]
    private TMP_InputField registerEmail;
    [SerializeField]
    private TMP_InputField registerPassword;
    [SerializeField]
    private TMP_InputField registerConfirmPassword;
    [SerializeField]
    private TMP_Text registerOutputText;
    [SerializeField]
    public GameObject registerAnimation;
    [SerializeField]
    private GameObject registerOutput;
    [SerializeField]
    private Button registerButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private BlockedWords blockedWords;
    [Space(5f)]

    //Register Variables
    [Header("Reset Password References")]
    [SerializeField]
    public TMP_InputField resetPasswordEmail;
    [SerializeField]
    private TMP_Text resetEmailOutputText;
    [SerializeField]
    private Button resetSubmitButton;
    [SerializeField]
    private Button resetBackButton;
    [SerializeField]
    public GameObject resetAnimation;
    [Space(5f)]

    //Friends List and Validation
    private string userID;
    private DatabaseReference dbReference;
    static readonly Regex usernameValidator = new Regex(@"^[-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz]+$");
    static readonly Regex emailValidator = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");

    //Login & Reg Timer
    private IEnumerator LoginReg;
    public bool coroutineIsRunning = false;
    public float timeRemaining;
    public bool timerIsRunning = false;

    private void Awake()
    {
        currentVersionText.text = "Build Version: " + versionCheck;

        AuthUI = GameObject.FindGameObjectWithTag("AuthUIManager"); //accesing a gameobject via tags, could pick something better
        RememberMe = GameObject.FindGameObjectWithTag("RememberMe");

        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        timeRemaining = 10; //protection from faulty API calls/strange internet issues
    }

    private void Start() //initializing firebase
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(checkDependecyTask =>
        {
            var dependencyStatus = checkDependecyTask.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies:  {dependencyStatus}");
            }
        });
    }

    private void Update() //navigating the main menu, this uses a counter system in the AuthUIManager script
    {
        if (timerIsRunning && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Login_Reg")) //forcing the loading screen and coroutine to stop when there is an unexpected issue
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timerIsRunning = false;
                timeRemaining =10;
                StopCoroutine(LoginReg);
                if (loginAnimation.activeSelf && coroutineIsRunning == true)
                {
                    LoadingAnimationLoginOff();
                    coroutineIsRunning = false;
                    loginOutputTextError.text = "Error establishing a connection";
                }
                else if (registerAnimation.activeSelf && coroutineIsRunning == true)
                {
                    LoadingAnimationRegOff();
                    coroutineIsRunning = false;
                    registerOutputText.text = "Error establishing a connection";
                }
                else if (resetAnimation.activeSelf && coroutineIsRunning == true)
                {
                    ResetAnimationOff();
                    coroutineIsRunning = false;
                    resetEmailOutputText.text = "Error establishing a connection";
                }
            }
        }


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Login_Reg"))
        {
            if (resetPasswordUI.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    ResetPasswordButton();
                    cursorManager.audioClickButtonStandard.Play();
                }
            }
            if (AuthUI.GetComponent<AuthUIManager>().loginInput > 1 && registerButton.enabled == true)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    RegisterButton();
                    cursorManager.audioClickButtonStandard.Play();
                }
            }
            if (AuthUI.GetComponent<AuthUIManager>().loginInput < 2 && loginButton.enabled == true)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    LoginButton();
                    cursorManager.audioClickButtonStandard.Play();
                }
            }
            if (AuthUI.GetComponent<AuthUIManager>().loginInput > 1 && Input.GetKeyDown(KeyCode.Escape) & backButton.enabled == true)
            {
                LoginScreen();
                cursorManager.audioClickButtonStandard.Play();
            }
        }
    }

    private void InitializeFirebase() //setting the various firebase variables
    {
       // FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false); //trying to disable caching to fix the firebase bug of finding usernames that don't exist

        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        dbReference = FirebaseDatabase.DefaultInstance.RootReference; //could do a FirebaseApp.DefaultIntance.SetEditorDatabaseURL("link") right before this to make sure you point to the right database
    }

    private void AuthStateChanged(object sender, System.EventArgs eventargs)
    {
        if(auth.CurrentUser != user)
        { 
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null) //keeping track of user who is signed in and signed out in the console
            {
                Debug.Log("Signed Out");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"Signed In: {user.DisplayName}");
/*                auth.SignOut(); //to force a signout
                Debug.Log(user.UserId);*/
            }
        }
    }
    public void RegisterScreen()
    {
        ResetCursor();
        loginUI.SetActive(false);
        resendEmail.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        registerUI.SetActive(true);
        registerUsername.Select();
        downloadButton.SetActive(false);
        ClearOutputs();
    }
    public void LoginScreen()
    {
        ResetCursor();
        registerUI.SetActive(false);
        resetPasswordUI.SetActive(false);
        registerUsername.text = "";
        registerEmail.text = "";
        registerPassword.text = "";
        registerConfirmPassword.text = "";
        resetPasswordEmail.text = "";
        resetEmailOutputText.text = "";
        loginUI.SetActive(true);
        loginEmail.Select();
    }
    public void ResetPasswordScreen()
    {
        ResetCursor();
        loginUI.SetActive(false);
        resendEmail.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        resetPasswordUI.SetActive(true);
        resetPasswordEmail.Select();
        downloadButton.SetActive(false);
        ClearOutputs();
    }
    public void ClearOutputs() //output clearning
    {
        loginOutputTextError.text = "";
        loginOutputTextSuccess.text = "";
        registerOutputText.text = "";
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorModeReset);
    }

    public void LoadingAnimationRegOn()
    {
        registerAnimation.SetActive(true); //loading screen
        ResetCursor();
        registerUI.SetActive(false);

        registerButton.enabled = false;
        registerButton.interactable = false;

        backButton.enabled = false;
        backButton.interactable = false;
    }

    public void LoadingAnimationRegOff()
    {
        registerAnimation.SetActive(false);
        registerUI.SetActive(true);

        registerButton.enabled = true;
        registerButton.interactable = true;

        backButton.enabled = true;
        backButton.interactable = true;

        timerIsRunning = false;
        timeRemaining = 10;
        coroutineIsRunning = false;
    }

    public void LoadingAnimationLoginOn()
    {
        loginAnimation.SetActive(true); //loading screen
        ResetCursor();
        loginUI.SetActive(false);

        loginButton.enabled = false;
        loginButton.interactable = false;

        registerLoginButton.enabled = false;
        registerLoginButton.interactable = false;

        resendEmailButton.enabled = false;
        resendEmailButton.interactable = false;
    }

    public void LoadingAnimationLoginOff()
    {
        loginAnimation.SetActive(false);
        loginUI.SetActive(true);

        loginButton.enabled = true;
        loginButton.interactable = true;

        registerLoginButton.enabled = true;
        registerLoginButton.interactable = true;

        resendEmailButton.enabled = true;
        resendEmailButton.interactable = true;

        timerIsRunning = false;
        timeRemaining = 10;
        coroutineIsRunning = false;
    }

    public void ResetAnimationOn()
    {
        resetAnimation.SetActive(true); //loading screen
        ResetCursor();
        resetPasswordUI.SetActive(false);

        resetBackButton.enabled = false;
        resetBackButton.interactable = false;

        resetSubmitButton.enabled = false;
        resetSubmitButton.interactable = false;
    }

    public void ResetAnimationOff()
    {
        resetAnimation.SetActive(false);
        resetPasswordUI.SetActive(true);

        resetBackButton.enabled = true;
        resetBackButton.interactable = true;

        resetSubmitButton.enabled = true;
        resetSubmitButton.interactable = true;

        timerIsRunning = false;
        timeRemaining = 10;
        coroutineIsRunning = false;
    }

    public void LoginButton() //starts login coroutine
    {
        if (loginEmail.text == "" || loginPassword.text == "")
        {
            loginOutputTextError.text = "Fields must not be empty";
        }
        else
        {
            LoginReg = LoginLogic(loginEmail.text, loginPassword.text);
            StartCoroutine(VersionCheck());
        }
    }

    public void RegisterButton() //starts first register coroutine
    {
        if (registerUsername.text == "" || registerEmail.text == "" || registerPassword.text == "" || registerConfirmPassword.text == "")
        {
            registerOutputText.text = "Fields must not be empty";
        }
        else
        {
            LoginReg = RegisterButtonEnumerator();
            StartCoroutine(LoginReg);
            timerIsRunning = true;
        }
    }

    public void ResetPasswordButton() //starts first register coroutine
    {
        if (resetPasswordEmail.text == "")
        {
            resetEmailOutputText.text = "Email cannot be blank";
        }
        else
        {
            LoginReg = ResetButtonEnumerator();
            StartCoroutine(LoginReg);
            timerIsRunning = true;
        }
    }

    IEnumerator ResetButtonEnumerator()
    {
        ResetAnimationOn();
        coroutineIsRunning = true;
        yield return new WaitForSeconds(0.5f); //mimicking a loading time

        var resetTask = auth.SendPasswordResetEmailAsync(resetPasswordEmail.text);
        yield return new WaitUntil(predicate: () => resetTask.IsCompleted);

        if (resetTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)resetTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            resetEmailOutputText.text = "Unknown Error, try again later";

            switch (error)
            {
                case AuthError.MissingEmail:
                    resetEmailOutputText.text = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    resetEmailOutputText.text = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    resetEmailOutputText.text = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    resetEmailOutputText.text = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    resetEmailOutputText.text = "User does not exist";
                    break;
                case AuthError.TooManyRequests:
                    resetEmailOutputText.text = "Timed out, too many requests";
                    break;
            }

            ResetAnimationOff();
        }
        else
        {
            Debug.Log($"Verification email sent to " + resetPasswordEmail.text);
            StartCoroutine(ChangeUIAfterReset());
        }
    }

    IEnumerator RegisterButtonEnumerator() //starts the coroutine to see if a username already exists, this is likely where the issue is.
    {
        LoadingAnimationRegOn(); //shows a loading animation and disables buttons
        coroutineIsRunning = true;
        yield return new WaitForSeconds(0.5f); //mimicking a loading time

        yield return StartCoroutine(CheckIfUsernameExists((string checkedUsername) =>
         {
           if (checkedUsername == "False") //if username is not in use, then start register coroutine
           {
                 RegistrationFieldCheck();
           }
           else if (checkedUsername == "Error") //for when firebase returns a json that contains just "{}" - unsure why this happens
           {
                 registerOutputText.text = "Unable to query firebase successfully, try again later";
                 LoadingAnimationRegOff();
             }
           else //if True, username is in use
           {
                 LoadingAnimationRegOff();
                 registerOutputText.text = "Username in use, choose a different username";
             }
         }));
    }
    IEnumerator CheckIfUsernameExists(Action<string> onCallback) //ensuring usernames are unique
    {
        var usernameCheck = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("username").EqualTo(registerUsername.text).GetValueAsync(); //this query is behaving strangely
        yield return new WaitUntil(predicate: () => usernameCheck.IsCompleted);

        DataSnapshot snapshotUsername = usernameCheck.Result;

        if (usernameCheck.Exception != null) //basic error checking, unsure if it actually does anything here of use
        {
            registerOutputText.text = "Error at CheckIfUsernameExists";
        }
        else
        {
            if (snapshotUsername.Exists) //if the json is not empty, then the username is in use
            {
                if (snapshotUsername.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
                {
                    onCallback.Invoke("Error");
                }
                else
                {
                    onCallback.Invoke("True"); //if the username is in use, i.e. it was found, return true
                }

                Debug.Log("the raw json to string is " + snapshotUsername.GetRawJsonValue().ToString()); //debugging the returned value
                Debug.Log("the registerusername text is " + registerUsername.text);
            }
            else //if snapshotUsername does not exist, then no Json is returned, not even empty brackets, which means the username is not in use
            {
                onCallback.Invoke("False");
                Debug.Log("the registerusername text is " + registerUsername.text);
                //Debug.Log("the raw json to string is " + snapshotUsername.GetRawJsonValue().ToString()); //should throw a console error when working correctly (i.e. not exist)
            }
        }
    }

    private IEnumerator VersionCheck() //check for if user has latest game build
    {
        var version = FirebaseDatabase.DefaultInstance.GetReference("versionCheck").GetValueAsync();
        yield return new WaitUntil(predicate: () => version.IsCompleted);
        if (version.IsFaulted)
        {
            LoadingAnimationLoginOff();
            loginOutputTextSuccess.text = "";
            loginOutputTextError.text = "Unable to validate current game version";
        }
        else if (version.IsCompleted)
        {
            DataSnapshot versionData = version.Result;

            Debug.Log(version.Result);
            Debug.Log("Current Version: " + versionData.Value);

            string correctedVersion = versionData.Value.ToString().Replace(',', '.');

            Debug.Log("Coversion: " + correctedVersion);

            if (correctedVersion == "0")
            {
                LoadingAnimationLoginOff();
                loginOutputTextSuccess.text = "";
                loginOutputTextError.text = "Blitz is down for maintenance";
            }
            else if (correctedVersion != versionCheck)
            {
                LoadingAnimationLoginOff();
                loginOutputTextSuccess.text = "";
                loginOutputTextError.text = "Please download latest version: " + correctedVersion;
                downloadButton.SetActive(true);
            }
            else
            {
                StartCoroutine(LoginReg);
                timerIsRunning = true;
            }
        }
    }

    public void DownloadLatestVersion()
    {
        Application.OpenURL("https://blitztcg.com/home");
        Application.Quit();
    }

    private IEnumerator LoginLogic(string _email, string _password) //standard firebase login logic
    {
        LoadingAnimationLoginOn();
        coroutineIsRunning = true;
        yield return new WaitForSeconds(0.25f); //mimicking a loading time

        Credential credential = EmailAuthProvider.GetCredential(_email, _password);
        var loginTask = auth.SignInWithCredentialAsync(credential);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string output = "Unknown Error";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    output = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    output = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    output = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    output = "User does not exist";
                    break;
                case AuthError.TooManyRequests:
                    output = "Timed out, too many requests";
                    break;
            }
            loginOutputTextSuccess.text = "";
            loginOutputTextError.text = output;
            LoadingAnimationLoginOff();
        }
        else
        {
            if (user.IsEmailVerified)
            {
                if (PlayerPrefs.HasKey("toggleOn") == false) //logic for playerprefs
                {
                    if (RememberMe.GetComponent<RememberMe>().rememberMeButtonOn.activeSelf)
                    {
                        PlayerPrefs.SetString("loginEmail", loginEmail.text);
                        PlayerPrefs.SetString("toggleOn", "1");
                        PlayerPrefs.Save();
                    }
                }

                if (PlayerPrefs.HasKey("toggleOn") == true)
                {
                    if (loginEmail.text != PlayerPrefs.GetString("loginEmail"))
                    {
                        PlayerPrefs.SetString("loginEmail", loginEmail.text);
                        PlayerPrefs.Save();
                    }

                    else if (!RememberMe.GetComponent<RememberMe>().rememberMeButtonOn.activeSelf)

                    {
                        PlayerPrefs.DeleteKey("loginEmail");
                        PlayerPrefs.DeleteKey("toggleOn");
                    }
                }


                StartCoroutine(ConcurrentUserCheck()); //makes sure user is not already logged in

            }
            else
            {
                LoadingAnimationLoginOff();
                loginOutputTextSuccess.text = "";
                loginOutputTextError.text = "Please verify your email, check spam";
                resendEmail.SetActive(true);
            }
        }
    }

    public IEnumerator ConcurrentUserCheck()
    {
        var concurrent = FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("online").GetValueAsync();
        yield return new WaitUntil(predicate: () => concurrent.IsCompleted);
        if (concurrent.IsFaulted)
        {
            LoadingAnimationLoginOff();
            loginOutputTextSuccess.text = "";
            loginOutputTextError.text = "Unable to confirm if user is already logged in";
        }
        else if (concurrent.IsCompleted)
        {
            DataSnapshot concurrentData = concurrent.Result;

            if (concurrentData.Value.ToString() == "T")
            {
                LoadingAnimationLoginOff();
                loginOutputTextSuccess.text = "";
                loginOutputTextError.text = "You are already logged in";
            }
            else
            {
                //StartCoroutine(ErgoQuery.instance.LoadErgoTokens()); //change scenes from here no matter what
                StartCoroutine(LoadOrCreateDeckData());
            }
        }
    }

    public IEnumerator LoadOrCreateDeckData()
    {
        var deckData = FirebaseDatabase.DefaultInstance.GetReference("decks").Child(user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => deckData.IsCompleted);
        if (deckData.IsFaulted)
        {
            DataSnapshot snapshot = deckData.Result;
            LoadingAnimationLoginOff();
            loginOutputTextSuccess.text = "";
            loginOutputTextError.text = "Unable to load decks";
            Debug.Log(deckData.Result);
        }
        else if (deckData.IsCompleted)
        {
            DataSnapshot snapshot = deckData.Result;

            if (snapshot.Value == null)
            {
                Debug.Log("created new deck values");
                StartCoroutine(CreateDeckData());
            }
            else //values already exist so just load up
            {
                Debug.Log("deck values already created");
                StartCoroutine(ErgoQuery.instance.LoadErgoTokens());
            }
        }
    }

    public IEnumerator CreateDeckData()
    {
        Debug.Log(user.UserId);
        
        for (int i = 1; i <= 5; i++)
        {
            DeckCurrent deckCurrent = new DeckCurrent("", "", "Purple Moon", "generalPfpCurrentPlaceholder", "", "", i, 0);
            string jsonStringCurrent = Newtonsoft.Json.JsonConvert.SerializeObject(deckCurrent);

            DeckAvailable deckAvailable = new DeckAvailable("", 0, "", 0, "", 0, "");
            string jsonStringAvailable = Newtonsoft.Json.JsonConvert.SerializeObject(deckAvailable);

            var createCurrentDeck = dbReference.Child("decks").Child(user.UserId).Child("CurrentCards" + i).SetRawJsonValueAsync(jsonStringCurrent);
            yield return new WaitUntil(predicate: () => createCurrentDeck.IsCompleted);
            if (createCurrentDeck.IsFaulted)
            {
                Debug.LogError("Current deck push failure"); ;
            }
            else if (createCurrentDeck.IsCompleted)
            {
                Debug.Log("Current deck push success");
            }

            var createAvailableDeck = dbReference.Child("decks").Child(user.UserId).Child("AvailableCards" + i).SetRawJsonValueAsync(jsonStringAvailable);
            yield return new WaitUntil(predicate: () => createAvailableDeck.IsCompleted);
            if (createAvailableDeck.IsFaulted)
            {
                Debug.LogError("Available deck push failure"); ;
            }
            else if (createAvailableDeck.IsCompleted)
            {
                Debug.Log("Available deck push success");
            }
        }

        StartCoroutine(ErgoQuery.instance.LoadErgoTokens());
    }

        private void RegistrationFieldCheck()
    {
        if (registerUsername.text.Length > 12)
        {
            //If username is greater than 12 digits, then show warning
            LoadingAnimationRegOff();
            registerOutputText.text = "Username must be less than 12 characters";
        }
        else if (blockedWords.blockedWordsList.Any(s => registerUsername.text.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) == true) //checking for bad words
        {
            LoadingAnimationRegOff();
            registerOutputText.text = "Inappropriate Username";
        }

        else if (!usernameValidator.IsMatch(registerUsername.text))
        {
            //validate against regex
            LoadingAnimationRegOff();
            registerOutputText.text = "Usernames are alphanumeric and can contain '-' and '_'";
        }
        else if (registerEmail.text.Length > 64)
        {
            //Check to see if email length is reasonable
            LoadingAnimationRegOff();
            registerOutputText.text = "Email address too long";
        }
        else if (!emailValidator.IsMatch(registerEmail.text.ToLower()))
        {
            //validates against the regex
            LoadingAnimationRegOff();
            registerOutputText.text = "Invalid Email";
        }
        else if (registerPassword.text.Length < 8)
        {
            //If the password is less than 8 characters, then show warning
            LoadingAnimationRegOff();
            registerOutputText.text = "Password must contain 8 characters";
        }
        else if (registerPassword.text.All(char.IsDigit))
        {
            //checks if the password is all int, if so throw error 
            LoadingAnimationRegOff();
            registerOutputText.text = "Password must contain at least one letter";
        }
        else if (registerPassword.text.All(char.IsLetter))
        {
            //checks if the password is all letters, if so throw error
            LoadingAnimationRegOff();
            registerOutputText.text = "Password must contain at least one number";
        }
        else if (registerPassword.text.All(char.IsLetterOrDigit))
        {
            //checks if the password is all letters and numbers, if so throw error
            LoadingAnimationRegOff();
            registerOutputText.text = "Password must contain at least one symbol";
        }
        else if (registerPassword.text != registerConfirmPassword.text)
        {
            LoadingAnimationRegOff();
            //If the password does not match show a warning
            registerOutputText.text = "Passwords do not match";
        }
        else
        {
            StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text));
        }
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password) //standard register logic with some initial checks
    {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password); //start the create user function if all checks were passed
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                //If there are errors, then handle them
                Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "Unknown Error";

                switch (error) //some error checks are duplicative but do not hurt to have in incase someone gets around it
                {
                    case AuthError.MissingEmail:
                        output = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        output = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        output = "Weak Password";
                        break;
                    case AuthError.InvalidEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        output = "Email already in use";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Timed out, too many requests";
                        break;
                }
                registerOutputText.text = output;
                LoadingAnimationRegOff();
        }

            else //if there are no errors, then create the user - set their displayname and photo
            {

                UserProfile profile = new UserProfile
                {
                    DisplayName = _username,
                    //PhotoUrl = new System.Uri("https://i.imgur.com/wd0vE6l.png"),
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);

                if(defaultUserTask.Exception != null) //check for errors
                {
                    user.DeleteAsync();

                    Debug.LogWarning($"Failed to register task with {defaultUserTask.Exception}");
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "Unknown Error";

                    switch (error)
                    {
                       case AuthError.Cancelled:
                                output = "Update user was cancelled";
                       break;
                       case AuthError.SessionExpired:
                       output = "Session expired";
                       break;
                    }
                    registerOutputText.text = output;
                    LoadingAnimationRegOff();
            }
                else
                {
                    StartCoroutine(CreateUserinRealtimeDatabase()); //send data to firebase realtime database

                    Debug.Log($"Firebase User Created Successfully: {user.DisplayName} ({user.UserId})"); //show info in console

                    StartCoroutine(SendVerifcationEmail()); //sendverification email if user was able to be created
                }
            }
    }
    private IEnumerator SendVerifcationEmail() //standard verification email logic
    {
        if (user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

            if (emailTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with {emailTask.Exception}");

                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)(firebaseException.ErrorCode);

                string output = "Unknown Error";
                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "Verification Task was cancelled";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Too many requests, please wait";
                        break;
                }

                registerOutputText.text = output;
                LoadingAnimationRegOff();

            }
            else
            {
                Debug.Log($"Verification email sent to {user.Email}");
                StartCoroutine(ChangeUIAfterVerification());
            }
        }
    }

    public void ResendVerificationEmail()
    {
        loginOutputTextError.text = "";
        StartCoroutine(ResendVerificationEmailEnumerator());
        ResetCursor();
        resendEmail.SetActive(false);
    }
    private IEnumerator ResendVerificationEmailEnumerator() //standard verification email logic
    {
        if (user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

            if (emailTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with {emailTask.Exception}");

                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)(firebaseException.ErrorCode);

                string output = "Unknown Error";
                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "Verification Task was cancelled";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Too many requests, please wait";
                        break;
                }

                loginOutputTextError.text = output;

            }
            else
            {
                Debug.Log($"Verification email sent to {user.Email}");
                loginOutputTextSuccess.text = "Verification email resent, check spam";
            }
        }
    }

    IEnumerator ChangeUIAfterVerification()
    {
        LoadingAnimationRegOff();
        LoginScreen();
        yield return new WaitForEndOfFrame();
        loginOutputTextSuccess.text = "Verification email sent";
    }
    IEnumerator ChangeUIAfterReset()
    {
        ResetAnimationOff();
        LoginScreen();
        yield return new WaitForEndOfFrame();
        loginOutputTextSuccess.text = "Password reset email sent";
    }

    private IEnumerator CreateUserinRealtimeDatabase() //sending data to realtime database
    {
        userID = user.UserId; //this is the auth userID
        Debug.Log(userID);
        ulong creationDate = auth.CurrentUser.Metadata.CreationTimestamp;

        //DeckInfo deckInfo = new DeckInfo("5", "hello");
        UserLoginData UserLoginData = new UserLoginData(registerEmail.text, registerUsername.text, userID, "none", "default", "F", creationDate.ToString()); //, deckInfo);
        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(UserLoginData);

/*        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(jsonString).ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to send data to Firebase");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Data sent to Firebase");
            }
        });*/


        var createUserInDatabase = dbReference.Child("users").Child(userID).SetRawJsonValueAsync(jsonString);
        yield return new WaitUntil(predicate: () => createUserInDatabase.IsCompleted);
        if (createUserInDatabase.IsFaulted)
        {
            Debug.LogError("Failed to send data to Firebase"); ;
        }
        else if (createUserInDatabase.IsCompleted)
        {
            Debug.Log("Data sent to Firebase");
        }
    }

    public class UserLoginData //setting up data to be sent to firebase realtime database
    {
        public string email;
        public string username;
        public string userID;
        public string wallet;
        public string pfp;
        public string online;
        public string creationDate;
        //public DeckInfo deckInfo;

        public UserLoginData(string email, string username, string userID, string wallet, string pfp, string online, string creationDate)//, DeckInfo deckInfo)
        {
            this.email = email;
            this.username = username;
            this.userID = userID;
            this.wallet = wallet;
            this.pfp = pfp;
            this.online = online;
            this.creationDate = creationDate;
            //this.deckInfo = deckInfo;
        }

    }

    /*    public class DeckInfo //this is just for an example of nested JSON logic
        {
            public string deckID;
            public string deckNotes;
            public DeckInfo(string deckID, string deckNotes)
            {
                this.deckID = deckID;
                this.deckNotes = deckNotes;
            }
        }*/

    public class DeckCurrent //setting up data to be sent to firebase realtime database
    {
        public string cardIdCurrent;
        public string filterCardsCurrent;
        public string gameboardCurrent;
        public string generalPfpCurrent;
        public string deckTitle;
        public string deckBody;
        public int deckId;
        public int deckGeneral;


        public DeckCurrent(string cardIdCurrent, string filterCardsCurrent, string gameboardCurrent, string generalPfpCurrent, string deckTitle, string deckBody, int deckId, int deckGeneral)
        {
            this.cardIdCurrent = cardIdCurrent;
            this.filterCardsCurrent = filterCardsCurrent;
            this.gameboardCurrent = gameboardCurrent;
            this.generalPfpCurrent = generalPfpCurrent;
            this.deckTitle = deckTitle;
            this.deckBody = deckBody;
            this.deckId = deckId;
            this.deckGeneral = deckGeneral;
        }
    }

    public class DeckAvailable//setting up data to be sent to firebase realtime database
    {
        public string cardIdAvailable;
        public int hideCardsAvailable;
        public string filterCardsAvailable;
        public int hideGameboards;
        public string filterGameboards;
        public int hideGeneralPfp;
        public string filterGeneralPfp;


        public DeckAvailable(string cardIdAvailable, int hideCardsAvailable, string filterCardsAvailable, int hideGameboards, string filterGameboards, int hideGeneralPfp, string filterGeneralPfp)
        {
            this.cardIdAvailable = cardIdAvailable;
            this.hideCardsAvailable = hideCardsAvailable;
            this.filterCardsAvailable = filterCardsAvailable;
            this.hideGameboards = hideGameboards;
            this.filterGameboards = filterGameboards;
            this.hideGeneralPfp = hideGeneralPfp;
            this.filterGeneralPfp = filterGeneralPfp;
        }
    }

}