using Photon.Pun;
using UnityEngine;

public class Blackhole : Card
{
    public void UseBlackHoleAbility(GameObject field, PhotonView view, int count)
    {
        Debug.Log("UseBlackHoleAbility called " + field + " field " + view);
        for (int i = 0; i < count; i++)
        {
            if (field.transform.GetChild(i).childCount == 1)
            {
                Debug.Log(i + " i value");
                Destroy(field.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        view.RPC("BlackHoleInOthers", RpcTarget.Others);

    }
}
