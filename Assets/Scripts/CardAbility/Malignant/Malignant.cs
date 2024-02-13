using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malignant : Card
{
    public int numberOfRandomCard = 2;
    public int damageGivenToAllEnemy = 3;
    public List<int> GetRandomPositionListOfEnemyCards(List<int> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogError("List is null or empty!");
            return new List<int>(); 
        }

        if (numberOfRandomCard <= 0)
        {
            Debug.LogError("Number of elements should be greater than zero!");
            return new List<int>();
        }

        if (numberOfRandomCard >= list.Count)
        {
            return new List<int>(list);
        }

        List<int> tempList = new List<int>(list);

        for (int i = tempList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = tempList[i];
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;
        }

        List<int> randomElements = tempList.GetRange(0, numberOfRandomCard);
        Debug.Log(" list of randomElements " + randomElements.Count);
        return randomElements;
    }

    public bool GiveDamageToEnemyField(GameObject card, Card currentCard)
    {
        Debug.Log(card + " card gameobject " +  currentCard.name +  " current card ");
        return currentCard.DealDamage(damageGivenToAllEnemy, card);
    }
}
