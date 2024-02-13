using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Repair : Card
{
    public int healAmount = 3;
    private GameObject gameBoardParent;

    public void OnSetHealWall(GameObject wall, PhotonView pv)
    {
        Debug.Log("OnSetHealWall called " + wall + " wall " + pv + " photon view");
        int playerHealth = int.Parse(wall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().text);
        int playerTotalHealth = int.Parse(wall.transform.Find("Total Health").gameObject.GetComponent<TMP_Text>().text);

        Debug.Log(playerHealth + " " + playerTotalHealth);

        playerHealth += healAmount;
        if(playerHealth > playerTotalHealth ) { playerHealth = playerTotalHealth; }
        wall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(playerHealth.ToString());
        pv.RPC("SetWallHealthToOthers", RpcTarget.Others, playerHealth);
    }
}
