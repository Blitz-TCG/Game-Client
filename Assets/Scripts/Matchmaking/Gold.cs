using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    public static Gold instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int GetGold()
    {
        return PlayerPrefs.GetInt("gold", 500);
    }

   public void SetGold(int gold)
    {
        PlayerPrefs.SetInt("gold", gold);
        goldText.SetText(gold.ToString());
        //Debug.LogError(" gold parent " + goldText.transform.parent.parent.name);
    }

    public int GetXP()
    {
        return PlayerPrefs.GetInt("xp", 0);
    }

    public void SetXP(int xp)
    {
        PlayerPrefs.SetInt("xp", xp);
        //goldText.SetText(gold.ToString());
    }
}