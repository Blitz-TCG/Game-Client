using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit : Card
{
    public int multiplier = 2;

    public void SetAbility(int multiplayerValue = 2) 
    {
        multiplier = multiplayerValue;
    }

    public int UseAbilityAndCalculateDamage(int baseDamage)
    {
        int randomValue = GenerateRandomNumber();
        Debug.Log("Random Integer (0 or 1): " + randomValue + " base damage " + baseDamage);

        int calculatedDamage = (randomValue == 1) ? baseDamage * multiplier : baseDamage;

        return calculatedDamage;
    }

    private int GenerateRandomNumber()
    {
        Debug.Log(" generate random number called ");
        return Random.Range(0, 2);
    }
}
