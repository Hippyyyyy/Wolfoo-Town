using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ShapePuzzleGame : BackItem
    {
        [SerializeField] Animator animator;
        [SerializeField] string playName;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            animator.Play(playName, 0, 0);
        }
    }
}
