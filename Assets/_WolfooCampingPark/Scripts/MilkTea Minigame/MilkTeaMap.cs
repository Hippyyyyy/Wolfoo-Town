using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall.Minigame.MilkTea
{
    public class MilkTeaMap : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] string spawnName;
        [SerializeField] MilkTeaPearl[] pearls;

        private void Start()
        {
        }
        IEnumerator PlayAnim()
        {
            yield return new WaitForSeconds(0.5f);
            animator.enabled = true;
            animator.Play(spawnName, 0, 0);
        }

        public void Spawn()
        {
            foreach (var pearl in pearls)
            {
                pearl.Spawn();
            }
            StartCoroutine(PlayAnim());

        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
