using System;
using UnityEngine;

public class Mutate : Card
{
    public int healthAmount = 3;
    public int attackAmount = 2;

    public Tuple<int, int> MutatingCard(Card card)
    {
        Debug.Log(" use renewal called " + card.name);
        int replicateAttack = card.SetCardAttack(attackAmount);
        int replicateHealth = card.HealCard(healthAmount);
        return new Tuple<int, int>(replicateAttack, replicateHealth);
    }
}
