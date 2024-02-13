using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBane : Card
{
    public int damageValue = 5;

    public int GetAttackValue(int currentHealth , int damage)
    {
        if(currentHealth < 0) return 0;
        
        currentHealth -= damage;
        if(currentHealth <= 0) currentHealth = 0;
        return currentHealth;
    }
}
