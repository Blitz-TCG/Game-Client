using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviourPunCallbacks
{
    [HideInInspector] public int enemyGold;
    [HideInInspector] public int enemyXP;
    [HideInInspector] public int totalXP;
    [HideInInspector] public int totalGold;
    [HideInInspector] public int enemyGainedGold = 0;
    [HideInInspector] public int enemyGainedXP = 0;
    private GameObject gameboardParent;
    private GameObject playerXPProgressBar;
    private GameObject enemyXPProgressBar;
    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();

    private void Start()
    {
        gameboardParent = GameObject.Find("Game Board Parent");
    }

    public void DestributeGoldAndXPForEnemy(PhotonView view, int gold, int xp, string name)
    {
        if(name == "master")
        {
            totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["clientGold"]);
        }
        else if(name == "client")
        {
            totalGold = (int)(PhotonNetwork.CurrentRoom.CustomProperties["masterGold"]);
        }
        enemyGainedGold += gold;
        enemyGainedXP += xp;
        totalGold += gold;
        totalXP += xp;
        enemyXPProgressBar = gameboardParent.transform.GetChild(1).GetChild(0).Find("Top Progress bar").gameObject;
        enemyXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXP);
        view = GetComponent<PhotonView>();
        if(name  == "master")
        {
            properties["clientGold"] = totalGold;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        else if(name == "client")
        {
            properties["masterGold"] = totalGold;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        view.RPC("DistributeGoldAndXP", RpcTarget.Others, enemyGainedGold, enemyGainedXP, totalGold, totalXP);
    }

    [PunRPC]
    private void DistributeGoldAndXP(int gold, int xp, int totalGold, int totalXP)
    {
        PlayerController playerController = gameboardParent.transform.GetChild(1).GetChild(0).Find("Player Field").GetComponent<PlayerController>();
        playerController.playerGainedGold = gold;
        playerController.playerGainedXP = xp;
        playerController.totalGold = totalGold;
        playerController.totalXP = totalXP;
        playerXPProgressBar = gameboardParent.transform.GetChild(1).GetChild(0).Find("Bottom Progress bar").gameObject;
        playerXPProgressBar.GetComponent<ProgressBar>().SetFillValue(totalXP);
        Gold.instance.SetGold(totalGold);
        //Debug.LogError("Gold settted " + totalGold + " parent of gold " + Gold.instance.transform.parent.parent.name);
    }

}
