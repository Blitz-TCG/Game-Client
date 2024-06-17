using System;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Card
{
    public void SetMimicData(Card fromCard, Card toCard)
    {
        Debug.Log("from card " + fromCard + " to card " + toCard);
        Debug.Log(fromCard.cardImage + " from card image " + toCard.cardImage + " to card image");
        Debug.Log(fromCard.id + " fromCard.id " + toCard.ergoTokenId + " toCard.ergoTokenId " + toCard.ergoTokenAmount + " toCard.ergoTokenAmount " + toCard.cardName + " toCard.cardName "+ toCard.cardDescription + " toCard.cardDescription " + fromCard.attack + " fromCard.attack "+ fromCard.HP + " fromCard.HP " + fromCard.gold + " fromCard.gold " + fromCard.XP + " fromCard.XP " + fromCard.fieldLimit + " fromCard.fieldLimit " + fromCard.fieldPosition + " fromCard.fieldPosition " + fromCard.clan + " fromCard.clan " + fromCard.levelRequired + "  fromCard.levelRequired "+ toCard.cardImage + " toCard.cardImage " + toCard.cardFrame + " toCard.cardFrame " + fromCard.cardClass + " fromCard.cardClass " + fromCard.ability + " fromCard.ability");
        //toCard.SetProperties(fromCard.id, toCard.ergoTokenId == null ? "" :  toCard.ergoTokenId, toCard.ergoTokenAmount == 0 ? 0 :  toCard.ergoTokenAmount, toCard.cardName == null ? "" : toCard.cardName, toCard.cardDescription == null ? "" : toCard.cardDescription, fromCard.attack, fromCard.HP, fromCard.gold, fromCard.XP, fromCard.fieldLimit, fromCard.fieldPosition, fromCard.clan, fromCard.levelRequired, toCard.cardImage, toCard.cardFrame, fromCard.cardClass, fromCard.ability);
        toCard.SetMiniCard(fromCard.id, toCard.ergoTokenId == null ? "" :  toCard.ergoTokenId, toCard.ergoTokenAmount == 0 ? 0 :  toCard.ergoTokenAmount, toCard.cardName == null ? "" : toCard.cardName, fromCard.attack, fromCard.HP, fromCard.gold, fromCard.XP, toCard.image.sprite, fromCard.ability);
        Destroy(toCard.GetComponent<Mimic>());
        Type cardType = FieldManager.instance.GetAbility(fromCard.ability);
        toCard.gameObject.AddComponent(cardType);
    }
}
