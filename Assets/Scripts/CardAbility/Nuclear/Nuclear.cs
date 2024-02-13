using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuclear : Card
{
    public void UseNuclearAbility(GameObject field, PhotonView view)
    {
        Debug.Log("UseNuclearAbility called " + field + " field " + view);
        for (int i = 0; i < field.transform.childCount; i++)
        {
            if (field.transform.GetChild(i).childCount == 1)
            {
                Debug.Log(i + " i value");
                Destroy(field.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        view.RPC("NuclearAbilityInOthers", RpcTarget.Others);
    }
}
