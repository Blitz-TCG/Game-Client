using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Card
{
    public int increasedAttack = 2;

    public int SetCardAttack(int attackAmount, Card card)
    {
            attack = card.attack + attackAmount;
            Debug.Log(attackAmount + " attack Amount " + attack + " attack value");
            if (attack <= 0) { attack = 0; }
            card.SetAttack(attack);
            return attack;
    }
}
