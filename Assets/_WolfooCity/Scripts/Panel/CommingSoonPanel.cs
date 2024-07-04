using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooCity
{
    public class CommingSoonPanel : UIPanel
    {
        public void OnBack()
        {
            Hide(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}