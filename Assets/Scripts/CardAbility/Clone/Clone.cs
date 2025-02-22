using System;
using UnityEngine;

public class Clone : Card
{
    public int requireRoundToEndTurn = 2;
    public int damageAmount = 2;
    public int currentRound = 0;
    public bool isCloned = false;

    private void OnDestroy()
    {
        Debug.Log(" reset round called on destroy");
        ResetRound();
    }

    public void InitializedCard()
    {
        Debug.Log(" reset round called on initialized");
        ResetRound();
    }

    public void ResetRound()
    {
        currentRound = 0;
        Debug.Log(currentRound + " current round");
    }

    public void SetRoundNumber(int round)
    {
        currentRound += round;
        Debug.Log(currentRound + " current round");
    }

    public bool IsUseAbility()
    {
        Debug.Log(currentRound + " current round " + requireRoundToEndTurn + " req damege");

        if (!isCloned)
        {
            if (currentRound == requireRoundToEndTurn)
            {
                return true;
            }
        }
        return false;
    }

    public Tuple<int, int, int, int> ReplicateCard(Card card, CardDetails originalCard)
    {
        Debug.Log("ReplicateCard called ");
        int replicateAttack = card.SetCardAttack(card.attack, originalCard.attack);
        int replicateHealth = card.HealCard(card.HP, originalCard.HP);
        int replicateGold = card.SetCardGold(card.gold, originalCard.gold);
        int replicateXP = card.SetCardXP(card.XP, originalCard.XP);
        Debug.Log(" new values are " + replicateAttack + " attack " + replicateGold + " gold " + replicateHealth + " health " + replicateXP + " xp");
        return new Tuple<int, int, int, int>(replicateAttack, replicateHealth, replicateGold, replicateXP);
    }

    public int FindThePosition(GameObject field)
    {
        for (int i = 0; i < field.transform.childCount; i++)
        {
            if (field.transform.GetChild(i).childCount == 0)
            {
                return i;
            }
        }
        return -1;
    }
}
