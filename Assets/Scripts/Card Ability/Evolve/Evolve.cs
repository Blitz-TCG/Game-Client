using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolve : Card
{
    public int requireRoundToUseAbility = 3;
    public int requiredEvolveHealth = 2;
    public int requiredEvolveAttack = 2;
    public int currentRound = 0;

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
        Debug.Log(currentRound + " current round " + requireRoundToUseAbility + " requireRoundToUseAbility ");
        if (currentRound == requireRoundToUseAbility)
        {
            Debug.Log("currentRound == requireRoundToUseAbility return true" );
            return true;
        }
        return false;
    }
}
