using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class PeanutZone : MonoBehaviour
    {
        [SerializeField] Transform itemZone;
        [SerializeField] Transform[] limitZones;
        [SerializeField] int totalItem;
        [SerializeField] Peanut peanutPb;

        private FlowerData data;

        private void Start()
        {
            data = DataSceneManager.Instance.BackItemDataSO.flowerData;

            for (int i = 0; i < totalItem; i++)
            {
                int idx = Random.Range(0, data.peanutSprites.Length);

                var peanut = Instantiate(peanutPb, itemZone);
                peanut.transform.position = new Vector3(
                    Random.Range(limitZones[0].position.x, limitZones[1].position.x),
                    limitZones[0].position.y,
                    0);
                peanut.AssignItem(idx,
                    data.peanutSprites[Random.Range(0, data.peanutSprites.Length)],
                    data.idleSprites[Random.Range(0, data.idleSprites.Length)]);
            }
        }
    }
}