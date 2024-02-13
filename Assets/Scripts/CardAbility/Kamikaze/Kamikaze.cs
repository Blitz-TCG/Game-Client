using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Card
{
    public int multiplier = 3;

    public int CalculateDamage(int baseDamage)
    {
        int calculatedDamage = baseDamage * multiplier ;

        return calculatedDamage;
    }

}
