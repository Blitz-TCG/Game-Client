using Photon.Pun;
using TMPro;
using UnityEngine;

public class GeneralBoon : Card
{
    public int healAmount = 3;
    private GameObject gameBoardParent;

    public void OnSetHealGeneral(GameObject general, PhotonView pv)
    {
        Debug.Log("OnSetHealGeneral called " + general + " general " + pv + " view ");
        int generalHealth = int.Parse(general.transform.Find("Player Deck Health").Find("Remaining Health").gameObject.GetComponent<TMP_Text>().text);
        int generalTotalHealth = int.Parse(general.transform.Find("Player Deck Health").Find("Total Health").gameObject.GetComponent<TMP_Text>().text);

        Debug.Log(generalHealth + " generalHealth " + generalTotalHealth + " generalTotalHealth");

        generalHealth += healAmount;
        if (generalHealth > generalTotalHealth) { generalHealth = generalTotalHealth; }
        general.transform.Find("Player Deck Health").GetChild(0).gameObject.GetComponent<TMP_Text>().SetText(generalHealth.ToString());
        pv.RPC("SetGeneralHealthToOthers", RpcTarget.Others, generalHealth);

        //HP += healAmount;
        //Debug.Log(healAmount + " heal amount " + HP + " hp value");
        //if (HP > maxCardHP) { HP = maxCardHP; }
        //HPText.SetText(HP.ToString());
        //return HP;
    }

    //[PunRPC]
    //private void SetGeneralHealthToOthers(int health)
    //{
    //    Debug.Log("SetGeneralHealthToOthers called " + health);
    //    gameBoardParent = GameObject.Find("Game Board Parent");
    //    Debug.Log(gameBoardParent + " gameboard parent ");
    //    GameObject enemyGeneral = gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Profile").gameObject;
    //    enemyGeneral.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(health.ToString());
        
    //}
}
