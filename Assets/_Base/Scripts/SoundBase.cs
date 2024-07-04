using _Base;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SoundBase<T> : SingletonBind<T> where T : Component
    {
        [SerializeField] protected AudioSource sfx;
        [SerializeField] private AudioSource ausPb;
        protected Dictionary<int, AudioSource> myItems;

        protected float startMusicVolume;
        protected float startSoundVolume;

        protected bool IsMusicMuted { get; private set; }
        protected bool IsSoundMuted { get; private set; }

        [System.Serializable]
        public class Item
        {
            public int id;
            public string name;
            public AudioClip clip;
            public bool isLoop;

            public Item(int id, string name, AudioClip clip, bool isLoop)
            {
                this.id = id;
                this.name = name;
                this.clip = clip;
                this.isLoop = isLoop;
            }
        }
        protected override void OnAwake()
        {
            base.OnAwake();
            startSoundVolume = sfx.volume;
        }
        private void Start()
        {
            OnInit();
        }
        protected virtual void OnInit()
        {
            if (BaseDataManager.Instance.IsMuteSound)
            {
                MuteSound(SoundType.SFx);
            }
            else
            {
                EnableSound(SoundType.SFx);
            }
        }

        protected virtual void MuteSound(SoundType type)
        {
            if (type != SoundType.SFx) return;
            if (sfx == null) return;

            IsSoundMuted = IsMusicMuted = true;
            sfx.volume = 0;
            if (myItems != null)
            {
                for (int i = 0; i < myItems.Count; i++) { myItems[i].volume = 0; }
            }
        }
        protected virtual void EnableSound(SoundType type)
        {
            if (type != SoundType.SFx) return;
            if (sfx == null) return;

            IsSoundMuted = IsMusicMuted = false;
            sfx.volume = startSoundVolume;

            if(myItems != null)
            {
                foreach (var item in myItems)
                {
                    item.Value.volume = startSoundVolume;
                }
            }
        }
        public AudioSource CreateNewAus(Item item)
        {
            if (item == null) return null;
            if (myItems != null && myItems.ContainsKey(item.id)) return null;

            var newItem = Instantiate(ausPb, transform);
            newItem.name = item.name;
            newItem.clip = item.clip;
            newItem.loop = item.isLoop;
            newItem.volume = IsSoundMuted ? 0 : startSoundVolume;


            Debug.Log("Create new Aus");
            if (myItems == null) myItems = new Dictionary<int, AudioSource>();
            myItems.Add(item.id, newItem);

            return newItem;
        }
    }
}