using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Smite : Card
{
    public int ChooseEnemyUnit(GameObject field, bool isDestroyed)
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
        return shuffledList[0];
    }

    public List<int> Shuffle(List<int> list)
    {
        Debug.Log("shuffle card called");
        return list.OrderBy(x => Guid.NewGuid()).ToList();
    }
}
