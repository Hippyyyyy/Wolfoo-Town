using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using SCN.BinaryData;
using System;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = fileNameSO, menuName = "Data/DataCharacter")]
    public class DataCharacterManager : ScriptableObject
    {

        const string fileNameSO = "Character Create Data";

        static DataCharacterManager instance;
        public static DataCharacterManager Instance
        {
            get
            {
                if (instance == null) Setup();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        [SerializeField] string dataFileName = "datacharacter";

        static void Setup()
        {
            // Load SO Data
            instance = LoadSource.LoadObject<DataCharacterManager>(fileNameSO);

            instance.localData = BinaryDataManager.LoadData<LocalDataCharacter>(instance.dataFileName);
            instance.localData.InitData();

            // Auto save khi quit game
            DDOL.Instance.OnApplicationPauseE += pause => { Debug.Log("local data " + pause); if (pause) instance.SaveLocalData(); };
            DDOL.Instance.OnApplicationQuitE += () => { instance.SaveLocalData(); };

#if UNITY_EDITOR
            DDOL.Instance.OnUpdateE += () => { if (Input.GetKeyDown(KeyCode.S)) instance.SaveLocalData(); };
#endif
        }

        [SerializeField] LocalDataCharacter localData;
        public LocalDataCharacter LocalData => localData;

        public void SaveLocalData()
        {
            Debug.Log("Save data");
            BinaryDataManager.SaveData<LocalDataCharacter>(localData, dataFileName);
        }
        [System.Serializable]
        public class LocalDataCharacter : BinaryData
        {
            [SerializeField] List<CharacterFeatureSet> listcharacters;

            [SerializeField] List<CharacterFeatureSet> listCharacterCreated;

            [SerializeField] List<int> listCharacterAds;

            [SerializeField] CharacterFeatureLibrary characterFeatureLibrary;

            [SerializeField] List<CategoryItemFeature> categoryItemFeatures;

            [SerializeField] List<CategoryColorItemFeature> categoryColorItemFeatures;

            public int UnlockCharacter;

            public bool IsWatchAdsAddSlot = false;

            public List<CharacterFeatureSet> ListCharacters { get => listcharacters; set => listcharacters = value; }
            public List<CharacterFeatureSet> ListCharacterCreated { get => listCharacterCreated; set => listCharacterCreated = value; }
            public CharacterFeatureLibrary CharacterFeatureLibrary { get => characterFeatureLibrary; set => characterFeatureLibrary = value; }

            public override void SetupDefault()
            {
                UnlockCharacter = 2;
                CharacterFeatureLibrary = Resources.Load<CharacterFeatureLibrary>("CharacterFeatureLibrary");
            }

            public void InitData()
            {

            }

            public void CheckListCharacter()
            {
                if (ListCharacters == null)
                {
                    ListCharacters = new List<CharacterFeatureSet>();
                }
            }
            public void CheckListCharacterAds()
            {
                if (listCharacterAds == null)
                {
                    listCharacterAds = new List<int>();
                }
            }

            public bool HasExistCharacter(int id)
            {
                var character = listcharacters.Find(x => x.CharacterID == id);
                return character != null;
            }
            public void AddCharacter(CharacterFeatureSet newCharacter, int id)
            {
                newCharacter.CharacterID = id;
                newCharacter.Age = (AgeSetting)UnityEngine.Random.Range(1, 2);
                newCharacter.EyebrowsColor = Color.blue;
                newCharacter.EyesColor = Color.blue;
                newCharacter.SkinColor = Color.blue;
                newCharacter.HairColor = Color.blue;
                newCharacter.IsCreated = false;
                listcharacters.Add(newCharacter);
            }
            public void RemoveCharacter(int id)
            {
                listcharacters.RemoveAll(character => character.CharacterID == id);
            }
            public void SortListCharacters()
            {
                if (listcharacters.Count > 0)
                {
                    listcharacters.Sort((x, y) => x.CharacterID.CompareTo(y.CharacterID));
                }
                else
                {
                    //
                }
            }
            public int GetIdCharacter(int id)
            {
                return listcharacters.Find(x => x.CharacterID == id).CharacterID;
            }

            public int FindColorID(int idCharacter, CharacterColorCategory characterColorCategory)
            {
                var charater = listcharacters.Find(x => x.CharacterID == idCharacter);
                if (characterColorCategory == CharacterColorCategory.Skin)
                {
                    return charater.SkinColorId;
                }
                else if (characterColorCategory == CharacterColorCategory.Eyebrows)
                {
                    return charater.EyebrowsColorId;
                }
                else if (characterColorCategory == CharacterColorCategory.Hair)
                {
                    return charater.HairColorId;
                }
                else
                {
                    return 0;
                }
            }
            public bool IsCharacterIdExist(int id)
            {
                if (listCharacterAds == null)
                {
                    return false;
                }
                if (id < 0)
                {
                    return false;
                }
                bool isExist = listCharacterAds.Contains(id);
                return isExist;
            }
            public void RemoveListCharacterAds(int id)
            {
                listCharacterAds.Remove(id);
            }
            public void AddCharacterAds(int id)
            {
                if (!listCharacterAds.Contains(id))
                {
                    listCharacterAds.Add(id);
                }
            }

            public void ActiveBool(CharacterFeatureCategoryEnum characterFeatureCategory, int id)
            {
                var item = characterFeatureLibrary.GetCharacterFeature(characterFeatureCategory, id);
                item.Metadata.IsFree = true;
            }

            public void AddCategoryItemFeature()
            {
                if (categoryItemFeatures == null)
                {
                    categoryItemFeatures = new List<CategoryItemFeature>();
                }

                if (categoryItemFeatures.Count > 10)
                {

                }
                else
                {
                    for (int i = 1; i < 12; i++)
                    {
                        var cate = new CategoryItemFeature();
                        cate.characterFeatureCategoryEnum = (CharacterFeatureCategoryEnum)i;
                        categoryItemFeatures.Add(cate);
                    }
                }
            }//
            public void AddCategoryColorItemFeature()
            {
                if (categoryColorItemFeatures == null)
                {
                    categoryColorItemFeatures = new List<CategoryColorItemFeature>();
                }

                if (categoryColorItemFeatures.Count > 4)
                {

                }
                else
                {
                    for (int i = 1; i < 6; i++)
                    {//
                        var cate = new CategoryColorItemFeature();
                        cate.characterColorCategory = (CharacterColorCategory)i;
                        categoryColorItemFeatures.Add(cate);
                    }
                }
            }

            public CategoryItemFeature HasCateItem(CharacterFeatureCategoryEnum characterFeatureCategoryEnum)
            {
                var find = categoryItemFeatures.Find(x => x.characterFeatureCategoryEnum == characterFeatureCategoryEnum);
                return find;
            }
            public CategoryColorItemFeature HasCateColorItem(CharacterColorCategory colorCategory)
            {
                var find = categoryColorItemFeatures.Find(x => x.characterColorCategory == colorCategory);
                return find;
            }

            [Serializable]
            public class CategoryItemFeature
            {
                public CharacterFeatureCategoryEnum characterFeatureCategoryEnum;
                public List<int> listFeature = new List<int>();

                public void AddFeature(int id)
                {
                    if (!listFeature.Contains(id))
                    {
                        listFeature.Add(id);
                    }
                }
                public void RemoveFeature(int id)
                {
                    listFeature.Remove(id);
                }
                public bool HasFeature(int id)
                {
                    return listFeature.Contains(id);
                }
            }
            [Serializable]
            public class CategoryColorItemFeature
            {
                public CharacterColorCategory characterColorCategory;
                public List<int> listFeature = new List<int>();
                //
                public void AddFeature(int id)
                {
                    if (!listFeature.Contains(id))
                    {
                        listFeature.Add(id);
                    }
                }
                public void RemoveFeature(int id)
                {
                    listFeature.Remove(id);
                }
                public bool HasFeature(int id)
                {
                    return listFeature.Contains(id);
                }
            }
        }


    }
}
