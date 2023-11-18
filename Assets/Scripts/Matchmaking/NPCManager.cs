using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NPCManager : MonoBehaviour
{
    //public int npcId;
    public string npcName;
    public int attack;
    public int defence;
    public int gold;
    public int XP;
    public float health;
    public Image npcHealth;
    public Slider slider;
    //public Sprite npcImage;
    public string player;


    //public int id;
    //public TMP_Text npcNameText;
    public TMP_Text attackText;
    public TMP_Text defenceText;
    public TMP_Text goldText;
    public TMP_Text XPText;
    //public Image image;

    public void SetNPCProperties(int npcAttack, int npcDefence, int npcGold, int npcXP, int npcHealth)
    {
        ////npcId = identity;
        //attackText.text = npcAttack.ToString();
        //defenceText.text = npcDefence.ToString();
        //goldText.text = npcGold.ToString();
        //XPText.text = npcXP.ToString();
        ////image.sprite = npcImage;
        ///

        //id = identity;
        attack = npcAttack;
        defence = npcDefence;
        gold = npcGold;
        XP = npcXP;
        health = npcHealth;
    }

    public bool DealDamage(int damage, GameObject npc)
    {
        bool destroyed = false;
        health -= damage;
        slider.value = health / 30f;
        //npcHealth.fillAmount = health / 100;
        //StartCoroutine(DecreaseHealth());
        if (health <= 0)
        {
            Destroy(npc.gameObject);
            destroyed = true;
        }
        return destroyed;
    }

    //private IEnumerator DecreaseHealth()
    //{
    //    float timer = 0f;
    //    float startHealth = npcHealth.fillAmount;
    //    float decreaseTime = 1f; // Time it takes to decrease health

    //    while (timer < decreaseTime)
    //    {
    //        timer += Time.deltaTime;
    //        npcHealth.fillAmount = Mathf.Lerp(startHealth, health, timer / decreaseTime);
    //        yield return null;
    //    }

    //    npcHealth.fillAmount = health;
    //}
}
