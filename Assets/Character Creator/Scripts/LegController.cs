using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LegState
{
    Drop,
    Dirty,
    Hold
}
public enum LegEnum
{
    Right = 0,
    Left = 1
}


public class LegController : MonoBehaviour
{
    public Action<LegState> OnLegStateChanged;

    private LegState currentState;

    public void ChangeLegState(LegState newState)
    {
        currentState = newState;

        OnLegStateChanged.Invoke(newState);
    }
}
