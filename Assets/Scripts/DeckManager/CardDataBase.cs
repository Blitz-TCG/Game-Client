using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static CardDataBase instance;
    public List<CardDetails> cardDetails;
    [HideInInspector] public Dictionary<CardAbility, CardRequirements>  requirements = new();

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

        AddLimit();
    }

   

    public void AddLimit()
    {
        requirements.Add(CardAbility.GoodFavor, new CardRequirements("2", "Frontline"));
        requirements.Add(CardAbility.Crit, new CardRequirements("Unlimited", "None"));
        requirements.Add(CardAbility.Goad, new CardRequirements("2", "Frontline"));
        requirements.Add(CardAbility.Meteor, new CardRequirements("1", "Frontline"));
        requirements.Add(CardAbility.Kamikaze, new CardRequirements("Unlimited", "Frontline"));
        requirements.Add(CardAbility.Renewal, new CardRequirements("Unlimited", "Frontline"));
        requirements.Add(CardAbility.Hunger, new CardRequirements("Unlimited", "None"));
        requirements.Add(CardAbility.Clone, new CardRequirements("1", "Frontline"));
        requirements.Add(CardAbility.Evolve, new CardRequirements("Unlimited", "Frontline"));
        requirements.Add(CardAbility.Malignant, new CardRequirements("1", "Frontline"));
        requirements.Add(CardAbility.Mutate, new CardRequirements("Unlimited", "Frontline"));
        requirements.Add(CardAbility.Farmer, new CardRequirements("Unlimited", "Frontline"));
        requirements.Add(CardAbility.Buster, new CardRequirements("Unlimited", "Frontline"));
        requirements.Add(CardAbility.Summon, new CardRequirements("1", "None"));
        requirements.Add(CardAbility.Repair, new CardRequirements("Unlimited", "None"));
        requirements.Add(CardAbility.GeneralBoon, new CardRequirements("Unlimited", "None"));
        requirements.Add(CardAbility.Serenity, new CardRequirements("1", "Frontline"));
        requirements.Add(CardAbility.Scattershot, new CardRequirements("Unlimited", "None"));
        requirements.Add(CardAbility.Berserker, new CardRequirements("Unlimited", "None"));
        requirements.Add(CardAbility.Mason, new CardRequirements("1", "Frontline"));
        requirements.Add(CardAbility.Paralyze, new CardRequirements("2", "Frontline"));
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