using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ArmState
{
    Drop,
    Dirty,
    Hold
}
public enum ArmEnum
{
    Right = 0,
    Left = 1
}


public class ArmController : MonoBehaviour
{
    public Action<ArmState> OnArmStateChanged;

    ArmState currentState;

    public void ChangeArmState(ArmState newState)
    {
        currentState = newState;

        OnArmStateChanged.Invoke(newState);
    }
}
