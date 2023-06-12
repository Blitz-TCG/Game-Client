using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum CardClass
{
    FreeToPlay = 0,
    GeneralMargo = 1,
    GeneralMios = 2,
    GeneralNassetari = 3,
    GeneralVoid = 4,
    GeneralToot = 5
}

[System.Serializable]
public class Card : MonoBehaviour
{
    
    public int id;
    public string ergoTokenId;
    public long ergoTokenAmount;
    public string cardName;
    public string cardDescription;  
    public int attack;
    public int HP;
    public int gold;
    public int XP;
    public int race;
    public string levelRequired;
    public string clan;
    public CardClass cardClass;
    public Sprite cardImage;
    public Sprite cardFrame;
    public bool isAlreadyAttacked;
    public int dropPosition = 0;


    public int cardId;
    public TMP_Text cId;
    public TMP_Text cardErgoTokenId;
    public TMP_Text cardErgoTokenAmount;
    public TMP_Text cardNameText;
    public TMP_Text cardDescriptionText;
    public TMP_Text attackText;
    public TMP_Text HPText;
    public TMP_Text goldText;
    public TMP_Text XPText;
    public TMP_Text cardClan;
    public TMP_Text levelRequiredText;
    public Image image;
    public Image frame;
    public void SetProperties(int identity, string ergoId, long ergoAmount, string cName,string cDescription, int cAttack, int cHP,int cGold, int cXP, string cClan, string cLevelRequired, Sprite cImage, Sprite cFrame, CardClass cardclass)
    {
        cardId = identity;
        cId.text = identity.ToString();
        cardErgoTokenId.text = ergoId;
        cardErgoTokenAmount.text = "Amount: " + ergoAmount;
        cardNameText.text = cName;
        cardDescriptionText.text = cDescription.ToString();
        attackText.text = cAttack.ToString();
        HPText.text = cHP.ToString();
        goldText.text = cGold.ToString();
        XPText.text = cXP.ToString();
        cardClan.text = cClan.ToString();
        levelRequiredText.text = cLevelRequired.ToString();
        image.sprite = cImage;
        frame.sprite = cFrame;

        id = identity;
        cId.text = identity.ToString();
        ergoTokenId = ergoId;
        ergoTokenAmount = ergoAmount;
        cardName = cName;
        cardDescription = cDescription;
        attack = cAttack;
        HP = cHP;
        gold = cGold;
        XP = cXP;
        clan = cClan;
        levelRequired = cLevelRequired;
        image.sprite = cImage;
        frame.sprite = cFrame;
        cardClass = cardclass;
    }

    public void SetMiniCard(int identity, string ergoId, long ergoAmount, string cName, int cAttack, int cHP, int cGold, int cXP, Sprite cImage)
    {
        cardId = identity;
        cId.text = identity.ToString();
        cardErgoTokenId.text = ergoId;
        cardErgoTokenAmount.text = "Amount: " + ergoAmount;
        cardNameText.text = cName;
        attackText.text = cAttack.ToString();
        HPText.text = cHP.ToString();
        goldText.text = cGold.ToString();
        XPText.text = cXP.ToString();
        image.sprite = cImage;

        id = identity;
        cId.text = identity.ToString();
        ergoTokenId = ergoId;
        ergoTokenAmount = ergoAmount;
        cardName = cName;
        attack = cAttack;
        HP = cHP;
        gold = cGold;
        XP = cXP;
        image.sprite = cImage;
    }

    public void DealDamage(int damage, GameObject card)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Destroy(card.gameObject);
        }
        HPText.SetText(HP.ToString());
    }

    public bool IsAttack()
    {
        return this.isAlreadyAttacked;
    }

    public void SetAttackValue(bool attackValue)
    {
        this.isAlreadyAttacked = attackValue;
    }

    public int GetDropCardPosition()
    {
        return this.dropPosition;
    }

    public void SetDropPosition(int dropvalue)
    {
        this.dropPosition = dropvalue;
    }
}
