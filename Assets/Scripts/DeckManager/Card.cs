using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardClass
{
    FreeToPlay = 0,
    GeneralMargo = 1,
    GeneralMios = 2,
    GeneralNassetari = 3,
    GeneralVoid = 4,
    GeneralToot = 5
}

public enum CardLevel
{
    Starter = 0,
    Lower = 1,
    Middle = 2,
    Upper = 3,
}

public enum CardAbility
{
    //Clone,
    //Meteor,
    //Evolve,
    //Malignant,
    GoodFavor,
    Summon,
    Serenity,
    Mutate,
    Renewal,
    Goad,
    Kamikaze,
    Berserker,
    Crit,
    Hunger,
    Scattershot,
    Farmer,
    Buster,
    Mason,
    Paralyze,
    Curse,
    Doom,
    Gambit,
    Silence,
    Stealth,
    Smite,
    Sacrifice,
    Mimic,
    GeneralBane,
    Blackhole,
    Nuclear,
    Endgame,
    Fodder,
    Repair,
    GeneralBoon,
    WhiteFlag,
    Eclipse,
    GeneralAegis,
    Fear,
    Toxic,
    Shhh,
    Swap,
    Hex,
    Ban,
    Consume,
    Merge,
    Drain,
    Warcry,
    StackedOdds,
    Coward,
    Illusion,
    Heckler,
    Overtime,
    Assassin,
    Blitz,
    Executioner,
    Limitless,
    Shield,
    Rally,
    Feast,
    Truesight,
    Heathen,
    Evasive,
    FleetFooted,
    Stifle,
    Timecrunch,
    Taxes,
    Rage,
    Duel,
    GeneralWard,
    Subsidy,
    Stalemate,
    Morph,
    Tank,
    Guardian,
    Respite,
    Savings,
    Flourish,
}

public enum AbilityRequirements
{
    AtTheStartOfTurn,
    AtTheEndOfTurn,
    OnAttack,
    Goded
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
    public int fieldLimit;
    public CardLevel levelRequired;
    public string clan;
    public CardClass cardClass;
    public Sprite cardImage;
    public Sprite cardFrame;
    public bool isAlreadyAttacked;
    public int dropPosition = 0;
    public CardAbility ability;
    public AbilityRequirements requirements;


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
    public TMP_Text fieldLimitText;
    public TMP_Text cardClan;
    public TMP_Text levelRequiredText;
    public Image image;
    public Image frame;
    public void SetProperties(int identity, string ergoId, long ergoAmount, string cName, string cDescription, int cAttack, int cHP, int cGold, int cXP, int cFieldLimit, string cClan, CardLevel cLevelRequired, Sprite cImage, Sprite cFrame, CardClass cardclass, CardAbility cAbility, AbilityRequirements req)
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
        fieldLimitText.text = cFieldLimit.ToString();
        cardClan.text = cClan.ToString();
        levelRequiredText.text = cLevelRequired.ToString();
        image.sprite = cImage;
        frame.sprite = cFrame;
        ability = cAbility;
        
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
        fieldLimit = cFieldLimit;

        if (fieldLimit == 8)
        {
            RectTransform fieldLimitTransform = fieldLimitText.GetComponent<RectTransform>();
            fieldLimitTransform.rotation = Quaternion.Euler(0, 0, 90);
        }

        clan = cClan;
        levelRequired = cLevelRequired;
        image.sprite = cImage;
        frame.sprite = cFrame;
        cardClass = cardclass;
        requirements = req;
    }

    public void SetMiniCard(int identity, string ergoId, long ergoAmount, string cName, int cAttack, int cHP, int cGold, int cXP, Sprite cImage, CardAbility cAbility, AbilityRequirements req)
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
        ability = cAbility;
        requirements = req;
    }

    public bool DealDamage(int damage, GameObject card)
    {
        bool destroyed = false;
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(card.gameObject);
            destroyed = true;
        }
        HPText.SetText(HP.ToString());
        return destroyed;
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

    public virtual void UseAbility()
    {
        Debug.Log("Base ability used.");
    }
    public virtual void UseAbility(bool value)
    {
        Debug.Log("Base ability used.");
    }

}
