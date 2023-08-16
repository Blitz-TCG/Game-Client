using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector] public int playerGold;
    [HideInInspector] public int playerXP;
    [HideInInspector] public int totalXP;
    [HideInInspector] public int totalGold;
    [HideInInspector] public int playerGainedGold = 0;
    [HideInInspector] public int playerGainedXP = 0;
    private GameObject gameboardParent;
    private GameObject playerXPProgressBar;
    private GameObject enemyXPProgressBar;
    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();

    private void Start()
    {
        gameboardParent = GameObject.Find("Game Board Parent");
    }

    public void DestributeGoldAndXPForPlayer(PhotonView view, int gold, int xp, string name)
    {
        if(name == "master")
        {
            totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
        }
        else if(name == "client")
        {
            totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
        }
        playerGainedGold += gold;
        playerGainedXP += xp;
        totalGold += gold;
        totalXP += xp;
        playerXPProgressBar = gameboardParent.transform.GetChild(1).GetChild(0).Find("Bottom Progress bar").gameObject;
        playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXP);
        Gold.instance.SetGold(totalGold);
        //Debug.LogError(" total gold setted " + Gold.instance.transform.parent.parent.name + " " + totalGold);
        if (name == "master")
        {
            properties["masterGold"] = totalGold;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        else if (name == "client")
        {
            properties["clientGold"] = totalGold;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        view = GetComponent<PhotonView>();
        view.RPC("DistributeGoldAndXP", RpcTarget.Others, playerGainedGold, playerGainedXP, totalGold, totalXP);
    }

    [PunRPC]
    private void DistributeGoldAndXP(int gold, int xp, int totalGold, int totalXP)
    {
        EnemyController enemyController = gameboardParent.transform.GetChild(1).GetChild(0).Find("Enemy Field").GetComponent<EnemyController>();
        enemyController.enemyGainedGold = gold;
        enemyController.enemyGainedXP = xp;
        enemyController.totalGold = totalGold;
        enemyController.totalXP = totalXP;
        enemyXPProgressBar = gameboardParent.transform.GetChild(1).GetChild(0).Find("Top Progress bar").gameObject;
        enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXP);
    }
}
