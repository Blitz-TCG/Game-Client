using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static CardDataBase instance;
    public string cardPath = "Cards/";
    public List<CardDetails> cardDetails;
    [HideInInspector] public Dictionary<CardAbility, AllField> requirements = new();
    [HideInInspector] public readonly Dictionary<CardAbility, Type> abilityTypeMap = new();

    private List<int> aboveCards = new List<int> { 1, 2, 3, 4, 5, 6 };
    private List<int> belowCards = new List<int> { 7, 8, 9, 10, 11, 12 };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {
        AddLimit();
        GetAbilityType();
    }

    public void AddLimit()
    {
        requirements.Add(CardAbility.GoodFavor, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.GoodFavor));
        requirements.Add(CardAbility.Crit, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Crit));
        requirements.Add(CardAbility.Goad, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Goad));
        requirements.Add(CardAbility.Meteor, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Meteor));
        requirements.Add(CardAbility.Kamikaze, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Kamikaze));
        requirements.Add(CardAbility.Renewal, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Renewal));
        requirements.Add(CardAbility.Hunger, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Hunger));
        requirements.Add(CardAbility.Clone, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Clone));
        requirements.Add(CardAbility.Evolve, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Evolve));
        requirements.Add(CardAbility.Malignant, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Malignant));
        requirements.Add(CardAbility.Mutate, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Mutate));
        requirements.Add(CardAbility.Farmer, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Farmer));
        requirements.Add(CardAbility.Buster, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Buster));
        requirements.Add(CardAbility.Summon, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Summon));
        requirements.Add(CardAbility.Repair, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Repair));
        requirements.Add(CardAbility.GeneralBoon, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.GeneralBoon));
        requirements.Add(CardAbility.Serenity, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Serenity));
        requirements.Add(CardAbility.Scattershot, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Scattershot));
        requirements.Add(CardAbility.Berserker, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Berserker));
        requirements.Add(CardAbility.Mason, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Mason));
        requirements.Add(CardAbility.Paralyze, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Paralyze));
        requirements.Add(CardAbility.Curse, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Curse));
        requirements.Add(CardAbility.Smite, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Smite));
        requirements.Add(CardAbility.Doom, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Doom));
        requirements.Add(CardAbility.Gambit, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Gambit));
        requirements.Add(CardAbility.GeneralBane, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.GeneralBane));
        requirements.Add(CardAbility.Blackhole, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Blackhole));
        requirements.Add(CardAbility.Nuclear, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Nuclear));
        requirements.Add(CardAbility.None, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.None));
        requirements.Add(CardAbility.DeActivate, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.DeActivate));
        requirements.Add(CardAbility.Sacrifice, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Sacrifice));
        requirements.Add(CardAbility.Mimic, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Mimic));
        requirements.Add(CardAbility.Endgame, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Endgame));
        requirements.Add(CardAbility.Silence, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Silence));
        requirements.Add(CardAbility.Stifle, FieldManager.instance.fieldsLimit.Find(field => field.ability == CardAbility.Stifle));
    }

    public void GetAbilityType()
    {
        abilityTypeMap.Add(CardAbility.GoodFavor, typeof(GoodFavor));
        abilityTypeMap.Add(CardAbility.Crit, typeof(Crit));
        abilityTypeMap.Add(CardAbility.Goad, typeof(Goad));
        abilityTypeMap.Add(CardAbility.Meteor, typeof(Meteor));
        abilityTypeMap.Add(CardAbility.Kamikaze, typeof(Kamikaze));
        abilityTypeMap.Add(CardAbility.Renewal, typeof(Renewal));
        abilityTypeMap.Add(CardAbility.Hunger, typeof(Hunger));
        abilityTypeMap.Add(CardAbility.Clone, typeof(Clone));
        abilityTypeMap.Add(CardAbility.Evolve, typeof(Evolve));
        abilityTypeMap.Add(CardAbility.Malignant, typeof(Malignant));
        abilityTypeMap.Add(CardAbility.Mutate, typeof(Mutate));
        abilityTypeMap.Add(CardAbility.Farmer, typeof(Farmer));
        abilityTypeMap.Add(CardAbility.Buster, typeof(Buster));
        abilityTypeMap.Add(CardAbility.Summon, typeof(Summon));
        abilityTypeMap.Add(CardAbility.Repair, typeof(Repair));
        abilityTypeMap.Add(CardAbility.GeneralBoon, typeof(GeneralBoon));
        abilityTypeMap.Add(CardAbility.Serenity, typeof(Serenity));
        abilityTypeMap.Add(CardAbility.Scattershot, typeof(Scattershot));
        abilityTypeMap.Add(CardAbility.Berserker, typeof(Berserker));
        abilityTypeMap.Add(CardAbility.Mason, typeof(Mason));
        abilityTypeMap.Add(CardAbility.Paralyze, typeof(Paralyze));
        abilityTypeMap.Add(CardAbility.Curse, typeof(Curse));
        abilityTypeMap.Add(CardAbility.Smite, typeof(Smite));
        abilityTypeMap.Add(CardAbility.Doom, typeof(Doom));
        abilityTypeMap.Add(CardAbility.Gambit, typeof(Gambit));
        abilityTypeMap.Add(CardAbility.GeneralBane, typeof(GeneralBane));
        abilityTypeMap.Add(CardAbility.Blackhole, typeof(Blackhole));
        abilityTypeMap.Add(CardAbility.Nuclear, typeof(Nuclear));
        abilityTypeMap.Add(CardAbility.None, typeof(None));
        //abilityTypeMap.Add(CardAbility.DeActivate, typeof(dec));
        abilityTypeMap.Add(CardAbility.Sacrifice, typeof(Sacrifice));
        abilityTypeMap.Add(CardAbility.Mimic, typeof(Mimic));
        abilityTypeMap.Add(CardAbility.Endgame, typeof(EndGame));
        abilityTypeMap.Add(CardAbility.Silence, typeof(Silence));
        abilityTypeMap.Add(CardAbility.Stifle, typeof(Stifle));
    }

    public List<int> GetSurroundingPositions(int selectedPosition)
    {
        List<int> surroundingPositions = new List<int>();

        bool isSelectedCardIsAbove = IsInsideList(selectedPosition, aboveCards);
        bool isSelectedCardIsBelow = IsInsideList(selectedPosition, belowCards);

        if (isSelectedCardIsAbove)
        {
            int index = aboveCards.IndexOf(selectedPosition);
            if (index == 0)
            {
                surroundingPositions.Add(aboveCards[index] + 1);
                surroundingPositions.Add(belowCards[index]);
                surroundingPositions.Add(belowCards[index] + 1);
            }
            else if (index == aboveCards.Count - 1)
            {
                surroundingPositions.Add(aboveCards[index] - 1);
                surroundingPositions.Add(belowCards[index]);
                surroundingPositions.Add(belowCards[index] - 1);
            }
            else
            {
                surroundingPositions.Add(aboveCards[index] - 1);
                surroundingPositions.Add(aboveCards[index] + 1);
                surroundingPositions.Add(belowCards[index] - 1);
                surroundingPositions.Add(belowCards[index]);
                surroundingPositions.Add(belowCards[index] + 1);
            }
        }
        else if (isSelectedCardIsBelow)
        {
            int index = belowCards.IndexOf(selectedPosition);
            if (index == 0)
            {
                surroundingPositions.Add(belowCards[index] + 1);
                surroundingPositions.Add(aboveCards[index]);
                surroundingPositions.Add(aboveCards[index] + 1);
            }
            else if (index == belowCards.Count - 1)
            {
                surroundingPositions.Add(belowCards[index] - 1);
                surroundingPositions.Add(aboveCards[index]);
                surroundingPositions.Add(aboveCards[index] - 1);
            }
            else
            {
                surroundingPositions.Add(belowCards[index] - 1);
                surroundingPositions.Add(belowCards[index] + 1);
                surroundingPositions.Add(aboveCards[index] - 1);
                surroundingPositions.Add(aboveCards[index]);
                surroundingPositions.Add(aboveCards[index] + 1);
            }
        }

        return surroundingPositions;
    }

    bool IsInsideList(int position, List<int> selectedList)
    {
        return selectedList.Contains(position);
    }
}

public class CardRequirements
{
    public string fieldLimit;
    public string fieldPosition;

    public CardRequirements(string limit, string position)
    {
        fieldLimit = limit;
        fieldPosition = position;
    }
}

public enum FieldLimit
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Unlimited = 8
}

public enum FieldPosition
{
    FrontLine,
    None
}

[Serializable]
public class AllField
{
    public CardAbility ability;
    public FieldLimit limit;
    public FieldPosition position;
}