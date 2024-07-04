using _Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace _WolfooCity
{
    public class CityTown : MonoBehaviour
    {
        [SerializeField] Button btn;
        [SerializeField] HouseAnimation myAnim;
        [SerializeField] CityType cityType;
        public AssetLabelReference labelRef;
        public string cityAddressableName;

        private void Start()
        {
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if(cityType == CityType.School)
            {
                LoadSceneManager.Instance.OnLoadScene(cityAddressableName, null);
            }
            else
            {
                LoadSceneManager.Instance.OnLoadScene(cityAddressableName, labelRef);
            }
            FirebaseManager.instance.LogClick(cityType.ToString(), "null");
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);
        }

        public void Play()
        {
            myAnim.PlayExcute(() =>
            {
                myAnim.PlayIdleExcuted();
            });
        }
    }
}
