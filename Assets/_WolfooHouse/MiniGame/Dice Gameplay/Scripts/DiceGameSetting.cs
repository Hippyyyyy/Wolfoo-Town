using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceGameSetting", menuName = "MiniGame/DiceGameplay Setting", order = 1)]
public class DiceGameSetting : ScriptableObject
{
    public int StartMapIndex;

    [Header("Test Environment")]

    public bool isTest1Chess;
    public int testStepIndex;
}
