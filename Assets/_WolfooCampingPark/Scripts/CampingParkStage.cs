using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CampingParkStage : BackItemWorld
    {
        [SerializeField] Animator animator;
        [SerializeField] string playName;
        [SerializeField] AudioClip myMusicClip;

        List<CharacterWorld> myCharacters = new List<CharacterWorld>();
        private AudioSource myAus;

        public override void Setup()
        {
            base.Setup();
            myAus = SoundCampingParkManager.Instance.CreateNewAus(new SoundBase<SoundCampingParkManager>.Item(Id, "Stage " + Id, myMusicClip, true));
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var character = collision.gameObject.GetComponent<CharacterWorld>();
            if (character != null)
            {
                if (myCharacters.Contains(character))
                {
                    myCharacters.Remove(character);
                    if (myCharacters.Count == 0)
                    {
                        animator.enabled = false;
                        myAus.Stop();
                    }
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var character = collision.gameObject.GetComponent<CharacterWorld>();
            if (character != null)
            {
                animator.enabled = true;
                // animator.Play(playName, 0, 0);
                if (!myCharacters.Contains(character))
                    myCharacters.Add(character);

                myAus.Play();
            }
        }
    }
}
