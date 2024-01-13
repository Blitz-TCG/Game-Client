using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Card
{
    public int beforeAttackGiveHealth = 3;
    public int UseAbility(Card card)
    {
        base.UseAbility();
        Debug.Log(" use renewal called " + card.name);
        return card.HealCard(beforeAttackGiveHealth);
    }
}
