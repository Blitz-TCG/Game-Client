using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoodFavor : Card
{
    public int goldValue = 25;

    public void StartTurn(bool isMaster)
    {
        GainGold(goldValue, isMaster);

    }

    public override void UseAbility(bool isMaster)
    {
        base.UseAbility();
        Debug.Log("Ability 1 used with value: " + goldValue);
        StartTurn(isMaster);
    }


    private void GainGold(int amount, bool master)
    {
        if(master)
        {
            int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            totalPlayerGold += amount;
            PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalPlayerGold;
            Debug.Log(totalPlayerGold + " total PlayerGold gold ");
            Gold.instance.SetGold(totalPlayerGold);
        }
        else
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            totalClientGold += amount;
            PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
            Debug.Log(totalClientGold + " total client gold ");
            Gold.instance.SetGold(totalClientGold);
        }

        Debug.Log($"Gained {amount} gold. Total gold: {gold}");
    }
}
