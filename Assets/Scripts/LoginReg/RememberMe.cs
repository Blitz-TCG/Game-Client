using UnityEngine;
using TMPro;

public class RememberMe : MonoBehaviour
{
    //Other Variables
    public static RememberMe instance;
    [Space(5f)]

    [Header("References")]
    [SerializeField]
    public GameObject rememberMeButtonOn;
    [Space(5f)]

    //Login Variables
    [Header("Login Variables")]
    [SerializeField]
    private TMP_InputField loginEmailField;
    [SerializeField]
    private TMP_Text loginEmailText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }

        if (PlayerPrefs.GetString("toggleOn") == "1")
        {
            rememberMeButtonOn.SetActive(true);
        }

    }

    void Start()
    {
        loginEmailField.text = PlayerPrefs.GetString("loginEmail");
        loginEmailText.text = loginEmailText.text;
    }

    public void RememberMeSelect()
    {
        if (!rememberMeButtonOn.activeSelf)
        {
            rememberMeButtonOn.SetActive(true);
        }
        else
        {
            rememberMeButtonOn.SetActive(false);
        }
    }
}
