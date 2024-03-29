using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : Card
{
    public int receivedGoldAndXPMultiplier = 3;

    public void GainGoldAndXP(int goldAmount, int xpAmount, bool master)
    {
        Debug.Log("gain gold called " + master + " amt " + goldAmount + " xp " + xpAmount);
        if (master)
        {
            int totalMasterGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            totalMasterGold += (receivedGoldAndXPMultiplier * goldAmount);
            int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
            totalMasterXP += (receivedGoldAndXPMultiplier * xpAmount);
            PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalMasterGold;
            Debug.Log(totalMasterGold + " total master gold " + totalMasterXP + " total master xp ");
            Gold.instance.SetGold(totalMasterGold);
        }
        else
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            totalClientGold += (receivedGoldAndXPMultiplier * goldAmount);
            int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
            totalClientXP += (receivedGoldAndXPMultiplier * xpAmount);
            PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
            PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
            Debug.Log(totalClientGold + " total client gold " + totalClientXP + " total client xp ");
            Gold.instance.SetGold(totalClientGold);
        }
    }
}
