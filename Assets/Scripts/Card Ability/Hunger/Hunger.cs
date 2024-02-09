using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Card
{
    public int beforeAttackGiveHealth = 1;
    public int UseAbility(Card card, int cardHP)
    {
        base.UseAbility();
        Debug.Log(" use renewal called " + card.name);
        return card.HealCard(beforeAttackGiveHealth, cardHP);
    }
}
