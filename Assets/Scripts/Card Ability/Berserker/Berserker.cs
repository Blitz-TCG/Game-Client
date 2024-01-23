using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker: Card
{
    public int multiplier = 3;
    public float failureChance = 0.25f;

    public int UseBerserkerAbility(int damage)
    {
        int actualDamage = 0;
        if (Random.value < failureChance)
        {
            Debug.Log("Attack failed!");
            return 0;
        }

        actualDamage = damage * multiplier;
        Debug.Log("Attack passed! " + actualDamage);
        
        return actualDamage;
    }
}
