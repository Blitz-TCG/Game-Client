using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : Card
{
    public int receivedGoldAndXPMultiplier = 3;

    public int GainGoldAndXP(int goldAmount, int xpAmount, bool master)
    {
        Debug.Log("gain gold called " + master + " amt " + goldAmount + " xp " + xpAmount);
        if (master)
        {
            int totalMasterGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            int totalMasterXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterXP"];
            Debug.Log(totalMasterGold + " total master gold " + totalMasterXP + " total master xp before");
            totalMasterGold += (receivedGoldAndXPMultiplier * goldAmount);
            totalMasterXP += (receivedGoldAndXPMultiplier * xpAmount);
            PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalMasterGold;
            PhotonNetwork.CurrentRoom.CustomProperties["masterXP"] = totalMasterXP;
            Debug.Log(totalMasterGold + " total master gold " + totalMasterXP + " total master xp ");
            Gold.instance.SetGold(totalMasterGold);
            return totalMasterXP;
        }
        else
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            int totalClientXP = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientXP"];
            Debug.Log(totalClientGold + " total client gold " + totalClientXP + " total client xp before");
            totalClientGold += (receivedGoldAndXPMultiplier * goldAmount);
            totalClientXP += (receivedGoldAndXPMultiplier * xpAmount);
            
            PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
            PhotonNetwork.CurrentRoom.CustomProperties["clientXP"] = totalClientXP;
            Debug.Log(totalClientGold + " total client gold " + totalClientXP + " total client xp ");
            Gold.instance.SetGold(totalClientGold);
            return totalClientXP;
        }
    }
}
