using System;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Card
{
    public void SetMimicData(Card fromCard, Card toCard)
    {
        toCard.SetProperties(fromCard.id, toCard.ergoTokenId, toCard.ergoTokenAmount, toCard.cardName, toCard.cardDescription, fromCard.attack, fromCard.HP, fromCard.gold, fromCard.XP, fromCard.fieldLimit, fromCard.fieldPosition, fromCard.clan, fromCard.levelRequired, toCard.cardImage, toCard.cardFrame, fromCard.cardClass, fromCard.ability);
        Destroy(toCard.GetComponent<Mimic>());
        Type cardType = FieldManager.instance.GetAbility(fromCard.ability);
        toCard.gameObject.AddComponent(cardType);
    }
}
