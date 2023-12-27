using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static CardDataBase instance;
    public List<CardDetails> cardDetails;
    [HideInInspector] public Dictionary<CardAbility, CardRequirements>  requirements = new();

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