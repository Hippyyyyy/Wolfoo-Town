using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] AnimationClip dancingClip;
        [SerializeField] Tile slideTile;
        [SerializeField] Tile teleTile;
        [SerializeField] Transform[] paths;

        Animator animator;
        public int Id { get; set; }
        public Transform[] Paths { get => paths; set => paths = value; }
        public Tile SlideTile { get => slideTile; }
        public Tile TeleTile { get => teleTile; }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnDestroy()
        {
        }

        public void OnDancing()
        {
            animator.Play(dancingClip.name, 0, 0);
        }
    }
}