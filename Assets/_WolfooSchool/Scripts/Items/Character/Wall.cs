using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class Wall : BackItem
    {
        protected override void Start()
        {
            base.Start();
            skinType = SkinBackItemType.Wall;
        }


    }
}