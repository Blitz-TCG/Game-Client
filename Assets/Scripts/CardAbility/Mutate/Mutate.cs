using System;
using UnityEngine;

public class Mutate : Card
{
    public int healthAmount = 3;
    public int attackAmount = 2;

    public Tuple<int, int> MutatingCard(Card card, CardDetails originalCard)
    {
        Debug.Log(" use renewal called " + card.name + "  original card attack " + originalCard.attack
             + " original card hp " + originalCard.HP);
        int replicateAttack = card.SetCardAttack(attackAmount, originalCard.attack);
        int replicateHealth = card.HealCard(healthAmount, originalCard.HP);
        return new Tuple<int, int>(replicateAttack, replicateHealth);
    }
}
