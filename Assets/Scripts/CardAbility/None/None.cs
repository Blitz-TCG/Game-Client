using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class None : Card
{
    public NoneConstraint constraint;
    public int constraintValue;
}

public enum NoneConstraint
{
    Permenent, 
    TurnBased
}