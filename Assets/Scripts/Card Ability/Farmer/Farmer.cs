using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Card
{
    public int multiplier = 2;

    public int UseFarmerAbility(int baseDamage)
    {
        Debug.Log(" base damage " + baseDamage);

        int calculatedDamage = baseDamage * multiplier;

        return calculatedDamage;
    }

}
