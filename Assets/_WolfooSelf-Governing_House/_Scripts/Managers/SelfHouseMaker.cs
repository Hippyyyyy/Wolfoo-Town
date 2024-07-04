using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SelfHouseMaker : MonoBehaviour
    {
        [SerializeField] string colorSpritesPath;
        [SerializeField] string whiteSpritesPath;
        [SerializeField] CustomRoomItem itemPb;
        [SerializeField] Transform itemHolder;
        [SerializeField] string formatName;
#if UNITY_EDITOR

        [NaughtyAttributes.Button]
        private void CreateItemFromPath()
        {
            string[] files = Directory.GetFiles(colorSpritesPath, "*.png", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                var fileName = file.Split("\\")[1];
                Debug.Log(fileName);

                var colorTexture = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(colorSpritesPath + "\\" + fileName, typeof(Sprite));
                var borderTexture = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(whiteSpritesPath + "\\" + fileName, typeof(Sprite));

                if(colorTexture != null && borderTexture != null)
                {
                    var item = Instantiate(itemPb, itemHolder);
                    item.name = formatName + " - " + fileName.Split(".png")[0];
                    item.Assign(colorTexture, borderTexture);
                    Debug.Log($"Color Item Path is SUCCESS... :{fileName}");
                }
                if (colorTexture == null)
                {
                    Debug.LogError($"Color Item Path is NOT FOUND... :{fileName}");
                }
                if(borderTexture == null)
                {
                    Debug.LogError($"Border Item Path is NOT FOUND... :{fileName}");
                }

            }
        }
#endif
    }
}
