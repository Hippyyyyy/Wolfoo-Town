using _Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [System.Serializable]
    public class DataStorage : ILocalSaveLoad<DataStorage>
    {
        private string KEY = "DATA_STORAGE_KEY";
        private bool IsCreated { get => PlayerPrefs.HasKey(KEY); }

        public List<UnlockEpisode> unlockEpisodes;
        public List<bool> unlockCharacters;
        public List<bool> unlockNewCharacters;

        public bool isTryFree;
        public bool isRemoveAds;
        public bool FirstOpen = true;

        public void Init()
        {
            unlockEpisodes = new List<UnlockEpisode>();
            unlockCharacters = new List<bool>();
            unlockNewCharacters = new List<bool>();
            FirstOpen = true;
            isRemoveAds = false;
            isTryFree = false;
        }

        public void Display()
        {
            var log = string.Format("===== DATA STORAGE ===== \n" +
                "First Open: {0}\n" +
                "isRemoveAds: {1}\n" +
                "Is Try Free: {2}\n"
                , FirstOpen, isRemoveAds, isTryFree);
            var log1 = "Episode:\n";
            foreach (var episode in unlockEpisodes)
            {
                log1 += episode + "\t";
            }
            var log2 = "Character:\n";
            foreach (var character in unlockCharacters)
            {
                log2 += character + "\t";
            }
            var log3 = "New Character:\n";
            foreach (var character in unlockNewCharacters)
            {
                log3 += character + "\t";
            }
            Debug.Log(log + log1 + log2 + log3);
        }

        public void Setup(bool isUnlockAll = false)
        {
            var data = DataSceneManager.Instance.BackItemDataSO;
            for (int i = 0; i < data.filmData.clipsData.Length; i++)
            {
                if (unlockEpisodes.Count < data.filmData.clipsData[i].episodeClips.Length)
                {
                    unlockEpisodes.Add(new UnlockEpisode());
                    for (int j = 0; j < data.filmData.clipsData[i].episodeClips.Length; j++)
                    {
                        unlockEpisodes[i].unlockVideos.Add(isUnlockAll ? true : j < 1);
                    }
                }
                else
                {
                    for (int j = 0; j < data.filmData.clipsData[i].episodeClips.Length; j++)
                    {
                        unlockEpisodes[i].unlockVideos[j] = isUnlockAll ? true : j < 1;
                    }
                }
            }
            Display();

            var data1 = DataSceneManager.Instance.ItemDataSO;
            var characterPbs = data1.CharacterData.characterPbs;
            for (int i = 0; i < characterPbs.Length; i++)
            {
                if (unlockCharacters.Count < characterPbs.Length)
                {
                    unlockCharacters.Add(isUnlockAll ? true : i < 3);
                }
                else
                {
                    unlockCharacters[i] = isUnlockAll ? true : i < 3;
                }
            }
            Display();

            var data2 = DataSceneManager.Instance.MainCharacterData;
            var characterPbs2 = data2.CharacterData.characterPbs;
            for (int i = 0; i < characterPbs.Length; i++)
            {
                if (unlockNewCharacters.Count < characterPbs2.Length)
                {
                    unlockNewCharacters.Add(isUnlockAll ? true : i < 3);
                }
                else
                {
                    unlockNewCharacters[i] = isUnlockAll ? true : i < 3;
                }
            }
            Display();

            Save();
        }

        public DataStorage Load()
        {
            if (IsCreated)
            {
                var jsonData = PlayerPrefs.GetString(KEY);
                Debug.Log($"DataGame Local Load: {jsonData} \n =====> Loading Completed <=====");
                var data = JsonUtility.FromJson<DataStorage>(jsonData);

                return data;
            }
            else
            {
                return null;
            }
        }

        public void Reset()
        {
            Init();
        }

        public void Save()
        {
            var jsonData = JsonUtility.ToJson(this);
            Debug.Log($"DataGame Local Save: {jsonData} \n =====> Saving Completed <=====");
            PlayerPrefs.SetString(KEY, jsonData);
        }

        public void Read()
        {
        }
    }
    [System.Serializable]
    public class UnlockEpisode
    {
        public List<bool> unlockVideos = new List<bool>();
    }
}
