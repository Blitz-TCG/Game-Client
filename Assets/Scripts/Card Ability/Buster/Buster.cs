using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buster : Card
{
    public int multiplier = 3;

    public int UseBusterAbility(int baseDamage)
    {
        Debug.Log(" base damage " + baseDamage);

        int calculatedDamage = baseDamage * multiplier;

        return calculatedDamage;
    }
}
