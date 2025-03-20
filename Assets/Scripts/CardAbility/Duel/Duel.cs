using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duel : Card
{
    public int healthAmount = 3;
    public int attackAmount = 2;

    public int SetCardAttack(int attackValue, Card card)
    {
        attack = card.attack + attackValue;
        Debug.Log(attackValue + " attack Amount " + attack + " attack value");
        if (attack <= 0) { attack = 0; }
        card.SetAttack(attack);
        return attack;
    }

    public int SetCardHealth(int healthValue, Card card)
    {
        HP = card.HP + healthValue;
        Debug.Log(healthValue + " attack Amount " + HP + " attack value");
        if (HP <= 0) {  HP = 0; }
        card.SetHP(HP);
        return HP;
    }
}
