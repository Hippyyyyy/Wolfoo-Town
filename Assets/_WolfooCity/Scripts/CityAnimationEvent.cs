using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity
{
    public class CityAnimationEvent : MonoBehaviour
    {
        [SerializeField] DoorAnimation cloudAnim;

        public static System.Action OnAnimComplete;

        public void PlayCloudAnim()
        {
            cloudAnim.PlayExcute();
        }
        public void OnComplete()
        {
            OnAnimComplete?.Invoke();
            
        }
    }
}
