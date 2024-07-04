using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _WolfooShoppingMall;
using SCN.BinaryData;

public class CharacterController : MonoBehaviour
{
    ArmController armController;
    LegController legController;
    [SerializeField] CharacterFeatureSet characterFeatureSet;
    [SerializeField] CharacterPartHelper characterPartHelper;
    [SerializeField] int id;

    public CharacterFeatureSet CharacterFeatureSet { get => characterFeatureSet; set => characterFeatureSet = value; }
    public CharacterPartHelper CharacterPartHelper { get => characterPartHelper; set => characterPartHelper = value; }
    public int Id { get => id; set => id = value; }

    void Start()
    {
        //
    }

}
