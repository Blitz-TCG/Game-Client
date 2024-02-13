using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gambit : Card
{
    public int GetRandomValue(GameObject field, bool isDestroyed)
    {
        List<int> gettedCardsPos = new List<int>();
        for (int i = 0; i < (isDestroyed ? field.transform.childCount : field.transform.childCount / 2); i++)
        {
            if (field.transform.GetChild(i).childCount == 1)
            {
                gettedCardsPos.Add((i + 1));
            }
        }

        if (gettedCardsPos.Count > 0)
        {
            int randomIndex = Random.Range(0, gettedCardsPos.Count);
            return gettedCardsPos[randomIndex];
        }
        else
        {
            return 0; 
        }
    }
}
