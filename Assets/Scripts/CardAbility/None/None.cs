using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class None : Card
{
    public NoneConstraint constraint;
    public int constraintValue;

    public int SetConstraintValue(int value)
    {
        Debug.Log(value + " value");
       return constraintValue = value;
    }

    public int UpdateValue(int value)
    {
        Debug.Log(constraintValue + " CV " + value);
        if (constraintValue == 0) { return 0; }
        constraintValue += value;
        Debug.Log(constraintValue + " Updated CV " + value);
        return constraintValue;
    }
}

public enum NoneConstraint
{
    Permenent, 
    TurnBased
}