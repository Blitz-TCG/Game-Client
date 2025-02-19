using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taxes : Card
{
    public float spendMoreGold = 10f;

    public int UseTaxesAbility(float gold, float spendMoreGold)
    {
        float increasedGold = gold * (1 + spendMoreGold / 100f); // for example if gold = 100 and spend = 20 => 100 * 1.2 = 120
        Debug.Log(increasedGold + " *** Increase gold");
        return Mathf.RoundToInt(increasedGold); // nearest int
    }
}
