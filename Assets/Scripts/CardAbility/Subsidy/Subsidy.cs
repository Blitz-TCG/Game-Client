using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subsidy : Card
{
    public float spendLessGold = 30f;

    public int UseSubsidyAbility(float gold, float spendLessGold)
    {
        float discountedGold = gold * (1 - spendLessGold / 100f); 
        Debug.Log(discountedGold + " *** Discounted gold");
        return Mathf.RoundToInt(discountedGold);
    }
}
