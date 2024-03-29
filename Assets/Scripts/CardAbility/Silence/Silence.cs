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
}
