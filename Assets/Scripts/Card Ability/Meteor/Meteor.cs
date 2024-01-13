using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Card
{
    public int requireRoundToDamege = 3;
    public int damageAmount = 2;
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
        Debug.Log(currentRound + " current round " + requireRoundToDamege + " req damege");
        if(currentRound == requireRoundToDamege)
        {
            return true;
        }
        return false;
    }
}

