using UnityEngine;
using UnityEngine.Audio;

public class MainMenuUIVolume : MonoBehaviour
{

    //this is awful and lazy code, but i'm just too lazy to fix it :)

    public static MainMenuUIVolume instance;

    [Header("Master Volume UI")]
    [SerializeField]
    private GameObject MasterVolumeEnabled;
    [SerializeField]
    private GameObject MasterVolume1;
    [SerializeField]
    private GameObject MasterVolume2;
    [SerializeField]
    private GameObject MasterVolume3;
    [SerializeField]
    private GameObject MasterVolume4;
    [SerializeField]
    private GameObject MasterVolume5;
    [SerializeField]
    private GameObject MasterVolume6;
    [SerializeField]
    private GameObject MasterVolume7;
    [SerializeField]
    private GameObject MasterVolume8;
    [SerializeField]
    private GameObject MasterVolume9;
    [SerializeField]
    private GameObject MasterVolume10;
    [SerializeField]
    private GameObject MasterVolume11;
    [SerializeField]
    private GameObject MasterVolume12;
    [SerializeField]
    private GameObject MasterVolume13;
    [Space(5f)]

    [Header("Music Volume UI")]
    [SerializeField]
    private GameObject MusicVolumeEnabled;
    [SerializeField]
    private GameObject MusicVolume1;
    [SerializeField]
    private GameObject MusicVolume2;
    [SerializeField]
    private GameObject MusicVolume3;
    [SerializeField]
    private GameObject MusicVolume4;
    [SerializeField]
    private GameObject MusicVolume5;
    [SerializeField]
    private GameObject MusicVolume6;
    [SerializeField]
    private GameObject MusicVolume7;
    [SerializeField]
    private GameObject MusicVolume8;
    [SerializeField]
    private GameObject MusicVolume9;
    [SerializeField]
    private GameObject MusicVolume10;
    [SerializeField]
    private GameObject MusicVolume11;
    [SerializeField]
    private GameObject MusicVolume12;
    [SerializeField]
    private GameObject MusicVolume13;
    [Space(5f)]

    [Header("Effects Volume UI")]
    [SerializeField]
    private GameObject EffectsVolumeEnabled;
    [SerializeField]
    private GameObject EffectsVolume1;
    [SerializeField]
    private GameObject EffectsVolume2;
    [SerializeField]
    private GameObject EffectsVolume3;
    [SerializeField]
    private GameObject EffectsVolume4;
    [SerializeField]
    private GameObject EffectsVolume5;
    [SerializeField]
    private GameObject EffectsVolume6;
    [SerializeField]
    private GameObject EffectsVolume7;
    [SerializeField]
    private GameObject EffectsVolume8;
    [SerializeField]
    private GameObject EffectsVolume9;
    [SerializeField]
    private GameObject EffectsVolume10;
    [SerializeField]
    private GameObject EffectsVolume11;
    [SerializeField]
    private GameObject EffectsVolume12;
    [SerializeField]
    private GameObject EffectsVolume13;
    [Space(5f)]

    //Mixer
    [Header("Mixer")]
    [SerializeField]
    public AudioMixer mixer;
    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("MasterVolume") == true)
        {
            if (PlayerPrefs.GetFloat("MasterVolumeTier") == 0)
            {
                MasterVolumeEnabled.SetActive(false);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 1)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(true);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 2)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(true);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 3)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(true);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 4)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(true);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 5)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(true);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 6)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(true);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 7)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(true);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 8)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(true);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 9)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(true);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 10)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(true);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 11)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(true);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 12)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(true);
                MasterVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MasterVolumeTier") == 13)
            {
                MasterVolumeEnabled.SetActive(true);
                MasterVolume1.SetActive(false);
                MasterVolume2.SetActive(false);
                MasterVolume3.SetActive(false);
                MasterVolume4.SetActive(false);
                MasterVolume5.SetActive(false);
                MasterVolume6.SetActive(false);
                MasterVolume7.SetActive(false);
                MasterVolume8.SetActive(false);
                MasterVolume9.SetActive(false);
                MasterVolume10.SetActive(false);
                MasterVolume11.SetActive(false);
                MasterVolume12.SetActive(false);
                MasterVolume13.SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey("MusicVolume") == true)
        {
            if (PlayerPrefs.GetFloat("MusicVolumeTier") == 0)
            {
                MusicVolumeEnabled.SetActive(false);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 1)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(true);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 2)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(true);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 3)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(true);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 4)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(true);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 5)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(true);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 6)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(true);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 7)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(true);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 8)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(true);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 9)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(true);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 10)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(true);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 11)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(true);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 12)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(true);
                MusicVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("MusicVolumeTier") == 13)
            {
                MusicVolumeEnabled.SetActive(true);
                MusicVolume1.SetActive(false);
                MusicVolume2.SetActive(false);
                MusicVolume3.SetActive(false);
                MusicVolume4.SetActive(false);
                MusicVolume5.SetActive(false);
                MusicVolume6.SetActive(false);
                MusicVolume7.SetActive(false);
                MusicVolume8.SetActive(false);
                MusicVolume9.SetActive(false);
                MusicVolume10.SetActive(false);
                MusicVolume11.SetActive(false);
                MusicVolume12.SetActive(false);
                MusicVolume13.SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey("EffectsVolume") == true)
        {
            if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 0)
            {
                EffectsVolumeEnabled.SetActive(false);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 1)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(true);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 2)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(true);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 3)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(true);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 4)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(true);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 5)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(true);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 6)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(true);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 7)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(true);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 8)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(true);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 9)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(true);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 10)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(true);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 11)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(true);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 12)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(true);
                EffectsVolume13.SetActive(false);
            }
            else if (PlayerPrefs.GetFloat("EffectsVolumeTier") == 13)
            {
                EffectsVolumeEnabled.SetActive(true);
                EffectsVolume1.SetActive(false);
                EffectsVolume2.SetActive(false);
                EffectsVolume3.SetActive(false);
                EffectsVolume4.SetActive(false);
                EffectsVolume5.SetActive(false);
                EffectsVolume6.SetActive(false);
                EffectsVolume7.SetActive(false);
                EffectsVolume8.SetActive(false);
                EffectsVolume9.SetActive(false);
                EffectsVolume10.SetActive(false);
                EffectsVolume11.SetActive(false);
                EffectsVolume12.SetActive(false);
                EffectsVolume13.SetActive(true);
            }
        }
    }
    public void EnableMasterVolume()
    {
        if (!MasterVolumeEnabled.activeSelf)
        {
            MasterVolumeEnabled.SetActive(true);
            SetMasterVolume1();
        }
        else if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolumeEnabled.SetActive(false);
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 0 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 0);
            PlayerPrefs.Save();
        }
    }

    public void EnableMusicVolume()
    {
        if (!MusicVolumeEnabled.activeSelf)
        {
            MusicVolumeEnabled.SetActive(true);
            SetMusicVolume1();
        }
        else if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolumeEnabled.SetActive(false);
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 0 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 0);
            PlayerPrefs.Save();
        }
    }

    public void EnableEffectsVolume()
    {
        if (!EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolumeEnabled.SetActive(true);
            SetEffectsVolume1();
        }
        else if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolumeEnabled.SetActive(false);
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 0 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 0);
            PlayerPrefs.Save();
        }
    }
    public void SetMasterVolume1()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(true);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 1 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 1);
            PlayerPrefs.Save();
        }
    }
    public void SetMasterVolume2()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(true);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 2 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 2);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume3()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(true);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 3 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 3);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume4()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(true);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 4 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 4);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume5()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(true);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 5 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 5);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume6()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(true);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 6 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 6);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume7()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(true);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 7 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 7);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume8()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(true);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 8 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 8);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume9()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(true);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 9 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 9);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume10()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(true);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 10 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 10);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume11()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(true);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 11 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 11);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume12()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(true);
            MasterVolume13.SetActive(false);

            masterVolume = -80 + 12 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 12);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume13()
    {
        if (MasterVolumeEnabled.activeSelf)
        {
            MasterVolume1.SetActive(false);
            MasterVolume2.SetActive(false);
            MasterVolume3.SetActive(false);
            MasterVolume4.SetActive(false);
            MasterVolume5.SetActive(false);
            MasterVolume6.SetActive(false);
            MasterVolume7.SetActive(false);
            MasterVolume8.SetActive(false);
            MasterVolume9.SetActive(false);
            MasterVolume10.SetActive(false);
            MasterVolume11.SetActive(false);
            MasterVolume12.SetActive(false);
            MasterVolume13.SetActive(true);

            masterVolume = -80 + 13 * (100 / 13);
            mixer.SetFloat("MasterVol", masterVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MasterVolumeTier", 13);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume1()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(true);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 1 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 1);
            PlayerPrefs.Save();
        }
    }
    public void SetMusicVolume2()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(true);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 2 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 2);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume3()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(true);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 3 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 3);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume4()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(true);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 4 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 4);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume5()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(true);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 5 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 5);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume6()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(true);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 6 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 6);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume7()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(true);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 7 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 7);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume8()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(true);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 8 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 8);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume9()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(true);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 9 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 9);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume10()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(true);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 10 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 10);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume11()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(true);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 11 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 11);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume12()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(true);
            MusicVolume13.SetActive(false);

            musicVolume = -80 + 12 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 12);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume13()
    {
        if (MusicVolumeEnabled.activeSelf)
        {
            MusicVolume1.SetActive(false);
            MusicVolume2.SetActive(false);
            MusicVolume3.SetActive(false);
            MusicVolume4.SetActive(false);
            MusicVolume5.SetActive(false);
            MusicVolume6.SetActive(false);
            MusicVolume7.SetActive(false);
            MusicVolume8.SetActive(false);
            MusicVolume9.SetActive(false);
            MusicVolume10.SetActive(false);
            MusicVolume11.SetActive(false);
            MusicVolume12.SetActive(false);
            MusicVolume13.SetActive(true);

            musicVolume = -80 + 13 * (100 / 13);
            mixer.SetFloat("MusicVol", musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("MusicVolumeTier", 13);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume1()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(true);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 1 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 1);
            PlayerPrefs.Save();
        }
    }
    public void SetEffectsVolume2()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(true);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 2 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 2);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume3()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(true);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 3 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 3);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume4()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(true);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 4 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 4);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume5()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(true);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 5 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 5);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume6()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(true);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 6 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 6);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume7()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(true);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 7 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 7);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume8()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(true);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 8 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 8);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume9()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(true);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 9 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 9);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume10()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(true);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 10 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 10);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume11()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(true);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 11 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 11);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume12()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(true);
            EffectsVolume13.SetActive(false);

            effectsVolume = -80 + 12 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 12);
            PlayerPrefs.Save();
        }
    }

    public void SetEffectsVolume13()
    {
        if (EffectsVolumeEnabled.activeSelf)
        {
            EffectsVolume1.SetActive(false);
            EffectsVolume2.SetActive(false);
            EffectsVolume3.SetActive(false);
            EffectsVolume4.SetActive(false);
            EffectsVolume5.SetActive(false);
            EffectsVolume6.SetActive(false);
            EffectsVolume7.SetActive(false);
            EffectsVolume8.SetActive(false);
            EffectsVolume9.SetActive(false);
            EffectsVolume10.SetActive(false);
            EffectsVolume11.SetActive(false);
            EffectsVolume12.SetActive(false);
            EffectsVolume13.SetActive(true);

            effectsVolume = -80 + 13 * (100 / 13);
            mixer.SetFloat("EffectsVol", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
            PlayerPrefs.SetFloat("EffectsVolumeTier", 13);
            PlayerPrefs.Save();
        }
    }

}
