using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : Card
{
    public int turnToDeactivate = 2;
    public int currectTurnCount = 0;

    public bool ActivateEnemysCard()
    {
        if(currectTurnCount == turnToDeactivate)
        {
            return true;
        }
        return false;
    }


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
        currectTurnCount = 0;
        Debug.Log(currectTurnCount + " current round");
    }

    public void SetRoundNumber(int round)
    {
        currectTurnCount += round;
        Debug.Log(currectTurnCount + " current round");
    }

    public bool IsUseAbility()
    {
        Debug.Log(currectTurnCount + " current round " + requireRoundToUseAbility + " requireRoundToUseAbility ");
        if (currectTurnCount == requireRoundToUseAbility)
        {
            Debug.Log("currentRound == requireRoundToUseAbility return true");
            return true;
        }
        return false;
    }
}
