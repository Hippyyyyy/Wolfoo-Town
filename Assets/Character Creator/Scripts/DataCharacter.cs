using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceState
{
    Default = 0,
    Hair = 1,
    Eyes = 2,
    EyeBrow = 3,
    Freckle = 4,
    Wrinkle = 5,
    Nose = 6,
    Blush = 7,
    Mouth = 8,
    Beard = 9,
}

public enum BodyState
{
    Default = 0,
    Skin = 1,
    Hand = 2,
}

public enum SkinTypeState
{
    Glasses, Hat, Clothes
}
[CreateAssetMenu(fileName = "DataCharacter", menuName = "MySO/Data/DataCharacter")]
public class DataCharacter : ScriptableObject
{
    public List<Character> listCharacter;

    [System.Serializable]
    public struct Character
    {
        public AgeSetting ageSetting;
        public Hand hand;
        public Color colorBody;
        public List<Face> listFace;
        public List<SkinType> listSkinType;
    }
    [System.Serializable]
    public struct Face
    {
        public FaceState faceState;
        public FacePart facePart;
    }
    [System.Serializable]
    public struct FacePart
    {
        public int id;
        public Sprite sprFace;
        public Sprite sprColor;
        public Color color;
    }
    [System.Serializable]
    public struct SkinType
    {
        public SkinTypeState skinTypeState;
        public Skin skin;
    }
    [System.Serializable]
    public struct Skin
    {
        public Sprite sprSkin;
        public List<Sprite> listChangeSkin;

    }
    [System.Serializable]
    public struct Hand
    {
        public int id;
        public Sprite sprHand;
        BodyState bodyState;
    }
}
