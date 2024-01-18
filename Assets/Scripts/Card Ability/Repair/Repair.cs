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
        int playerHealth = int.Parse(wall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().text);
        int playerTotalHealth = int.Parse(wall.transform.Find("Total Health").gameObject.GetComponent<TMP_Text>().text);

        playerHealth += healAmount;
        if(playerHealth > playerTotalHealth ) { playerHealth = playerTotalHealth; }
        wall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(playerHealth.ToString());
        pv.RPC("SetWallHealthToOthers", RpcTarget.Others, playerHealth);

        //HP += healAmount;
        //Debug.Log(healAmount + " heal amount " + HP + " hp value");
        //if (HP > maxCardHP) { HP = maxCardHP; }
        //HPText.SetText(HP.ToString());
        //return HP;
    }

    [PunRPC]
    private void SetWallHealthToOthers(int health)
    {
        gameBoardParent = GameObject.Find("Game Board Parent");
        Debug.Log(gameBoardParent + " gameboard parent ");
        GameObject enemyWall =gameBoardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").gameObject;
        enemyWall.transform.Find("Remaining Health").gameObject.GetComponent<TMP_Text>().SetText(health.ToString());
    }
}
