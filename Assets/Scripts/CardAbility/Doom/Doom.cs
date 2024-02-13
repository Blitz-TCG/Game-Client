using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Doom : Card
{
    public int cardsToBeDoomed = 2;

    public List<int> GenerateDoomedCardList(GameObject field, bool isDestroyed)
    {
        List<int> gettedCardsPos = new List<int>();
        for (int i = 0; i < (isDestroyed ? field.transform.childCount : field.transform.childCount / 2); i++)
        {
            if (field.transform.GetChild(i).childCount == 1)
            {
                gettedCardsPos.Add((i + 1));
            }
        }
        Debug.Log(gettedCardsPos.Count + " list of getted card ");
        List<int> shuffledList = Shuffle(gettedCardsPos);
        Debug.Log(gettedCardsPos.Count + " after shuffling " + string.Join(", ", shuffledList));
        List<int> trimmed = TrimList(shuffledList, cardsToBeDoomed);
        Debug.Log(trimmed.Count + " after triming " + string.Join(", ", trimmed));
        return trimmed;
    }

    public List<int> Shuffle(List<int> list)
    {
        Debug.Log("shuffle card called");
        return list.OrderBy(x => Guid.NewGuid()).ToList();
    }

    public List<int> TrimList(List<int> list, int trimValue)
    {
        Debug.Log("trim list called ");
        if (list.Count <= trimValue)
        {
            Debug.Log("list.Count <= trimValue");
            return list;
        }
        else
        {
            Debug.Log("else");
            return list.Take(trimValue).ToList();
        }
    }
}
