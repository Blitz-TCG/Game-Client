using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Card
{
    public int cardToBeSpawwned = 2;

    public List<int> GetPosition(GameObject field)
    {
        List<int> positions = new();
        for(int i = 0; i < field.transform.childCount; i++)
        {
            if(field.transform.GetChild(i).childCount == 0)
            {
                Debug.Log("field.transform.GetChild(i).childCount " + field.transform.GetChild(i).name);
                int positionIndex = int.Parse(field.transform.GetChild(i).name.Split(" ")[2]);
                positions.Add(positionIndex);
                Debug.Log("i value " + i + " position count " + positions.Count);
            }
        }
        if(positions.Count > cardToBeSpawwned)
        {
            Debug.Log(positions.Count + " position count " + cardToBeSpawwned + " cardToBeSpawwned ");
            positions = TrimList(positions, cardToBeSpawwned);
            Debug.Log(positions.Count + " count of position");
        }
        Debug.Log(positions.Count + " count of position");
        return positions;
    }

    List<int> TrimList(List<int> list, int newSize)
    {
        if (list.Count > newSize)
        {
            list.RemoveRange(newSize, list.Count - newSize);
        }
        return list;
    }
}
