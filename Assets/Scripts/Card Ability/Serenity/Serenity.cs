using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serenity : Card
{
    public int healAmount = 4;

    //public void

    private List<int> aboveCards = new List<int> { 1, 2, 3, 4, 5, 6 };
    private List<int> belowCards = new List<int> { 7, 8, 9, 10, 11, 12 };

    public List<int> GetSurroundingPositions(int selectedPosition)
    {
        List<int> surroundingPositions = new List<int>();

        bool isSelectedCardIsAbove = IsInsideList(selectedPosition, aboveCards);
        bool isSelectedCardIsBelow = IsInsideList(selectedPosition, belowCards);

        if (isSelectedCardIsAbove)
        {
            int index = aboveCards.IndexOf(selectedPosition);
            if (index == 0)
            {
                surroundingPositions.Add(aboveCards[index] + 1);
                surroundingPositions.Add(belowCards[index]);
                surroundingPositions.Add(belowCards[index] + 1);
            }
            else if (index == aboveCards.Count - 1)
            {
                surroundingPositions.Add(aboveCards[index] - 1);
                surroundingPositions.Add(belowCards[index]);
                surroundingPositions.Add(belowCards[index] - 1);
            }
            else
            {
                surroundingPositions.Add(aboveCards[index] - 1);
                surroundingPositions.Add(aboveCards[index] + 1);
                surroundingPositions.Add(belowCards[index] - 1);
                surroundingPositions.Add(belowCards[index]);
                surroundingPositions.Add(belowCards[index] + 1);
            }
        }
        else if (isSelectedCardIsBelow)
        {
            int index = belowCards.IndexOf(selectedPosition);
            if (index == 0)
            {
                surroundingPositions.Add(belowCards[index] + 1);
                surroundingPositions.Add(aboveCards[index]);
                surroundingPositions.Add(aboveCards[index] + 1);
            }
            else if (index == belowCards.Count - 1)
            {
                surroundingPositions.Add(belowCards[index] - 1);
                surroundingPositions.Add(aboveCards[index]);
                surroundingPositions.Add(aboveCards[index] - 1);
            }
            else
            {
                surroundingPositions.Add(belowCards[index] - 1);
                surroundingPositions.Add(belowCards[index] + 1);
                surroundingPositions.Add(aboveCards[index] - 1);
                surroundingPositions.Add(aboveCards[index]);
                surroundingPositions.Add(aboveCards[index] + 1);
            }
        }

        return surroundingPositions;
    }

    bool IsInsideList(int position, List<int> selectedList)
    {
        return selectedList.Contains(position);
    }

    public void UseSerenityAbility(List<int> positions, GameObject playerField, PhotonView pv)
    {
        Debug.Log("UseSerenityAbility called ");
        for(int i = 0; i < positions.Count; i++)
        {
            Debug.Log("position i " + i + " positions of i " + positions[i] + " player field " + playerField + " pv " + pv);
            pv.RPC("SerenityAbilityForOthers", RpcTarget.Others, (positions[i] - 1));
            //if (playerField.transform.GetChild(positions[i] - 1).GetChild(0) != null)
            //{
            //    Debug.Log(" playerField.transform.GetChild(positions[i] - 1) " + playerField.transform.GetChild(positions[i] - 1).name + " playerField.transform.GetChild(positions[i] - 1).tag " + playerField.transform.GetChild(positions[i] - 1).tag);
            //    if(playerField.transform.GetChild(positions[i] - 1).tag.Contains("Front Line"))
            //    {
            //        Debug.Log("front line called " + (positions[i] - 1));
            //        Card card = playerField.transform.GetChild(positions[i] - 1).GetChild(0).GetChild(0).GetComponent<Card>();
            //        Debug.Log(card + " card ");
            //        CardDetails originalCard = CardDataBase.instance.cardDetails.Find(cardId => cardId.id == card.id);
            //        Debug.Log(" original card " + originalCard );
            //        int cardHealth = card.HealCard(healAmount, originalCard.HP);
            //        pv.RPC("SerenityAbilityForOthers", RpcTarget.Others, (positions[i] - 1), cardHealth);
            //    }
            //}
        }
    }
}
