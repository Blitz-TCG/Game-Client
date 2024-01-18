using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renewal : Card
{
    public int healthAmount = 3;

    public int UseAbility(Card card, int maxHP)
    {
        Debug.Log(" use renewal called " + card.name);
        return card.HealCard(healthAmount, maxHP);
    }
}

