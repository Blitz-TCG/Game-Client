using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class CardDetails
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
    //public int race;
    public int fieldLimit;
    public CardLevel levelRequired;
    public string clan;
    public CardClass cardClass;
    public Sprite cardImage;
    public Sprite cardFrame;
    public CardAbility ability;

    public static explicit operator CardDetails(UnityEngine.Object v)
    {
        throw new NotImplementedException();
    }
    // public T abilityscript;
    //public AbilityRequirements requirements;
    //public int abilityLevel = 0;

#if UNITY_EDITOR
    [HideInInspector] public bool showMultiplierAndFailureChance;
    [HideInInspector] public bool showHealth;
    [HideInInspector] public bool showDamage;

    private void OnValidate()
    {
        showMultiplierAndFailureChance = ability == CardAbility.Clone;
        showHealth = ability == CardAbility.Meteor;
        showDamage = ability == CardAbility.Evolve;
    }
#endif
}

//[Serializable]
//public class AbilityPair
//{
//    public CardAbility ability;
//    public Good_Favor good_Favor;
//}

