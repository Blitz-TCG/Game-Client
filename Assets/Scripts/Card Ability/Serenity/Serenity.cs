using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serenity : Card
{
    public int healAmount = 4;

    public void UseSerenityAbility(List<int> positions, GameObject playerField, PhotonView pv)
    {
        Debug.Log("UseSerenityAbility called ");
        for(int i = 0; i < positions.Count; i++)
        {
            Debug.Log("position i " + i + " positions of i " + positions[i] + " player field " + playerField + " pv " + pv);
            if(playerField.transform.GetChild(positions[i] - 1) != null && playerField.transform.GetChild(positions[i] - 1).childCount == 1)
            {
                Debug.Log("playerField.transform.GetChild(positions[i] - 1) != null && playerField.transform.GetChild(positions[i] - 1).GetChild(0) != null");
                if (playerField.transform.GetChild(positions[i] - 1).tag.Contains("Front Line"))
                {
                    Card currentCard = playerField.transform.GetChild(positions[i] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
                    Debug.Log(currentCard + " card ");
                    CardDetails originalCard = CardDataBase.instance.cardDetails.Find(cardId => cardId.id == currentCard.id);
                    int cardHealth = currentCard.HealCard(healAmount, originalCard.HP);
                    pv.RPC("SerenityAbilityForOthers", RpcTarget.Others, (positions[i] - 1), cardHealth);
                }
            }
        }
    }
}
