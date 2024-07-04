using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ExpandableScrollView : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public GameObject itemPrefab;
        public Transform contentTransform;

        void Start()
        {
            // Thêm các item vào ScrollView
            for (int i = 0; i < 10; i++)
            {
                CreateItem(i);
            }
        }

        void CreateItem(int index)
        {
            // Tạo một button cho mỗi item
            GameObject item = Instantiate(itemPrefab, contentTransform);
            Button itemButton = item.GetComponent<Button>();
            Text buttonText = item.GetComponentInChildren<Text>();

            // Thiết lập văn bản của button và gắn sự kiện khi nhấn vào
            buttonText.text = "Item " + index.ToString();
            itemButton.onClick.AddListener(() => ToggleItemExpansion(item));

            // Khởi tạo item ở trạng thái đóng (nội dung bên dưới không hiển thị)
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, 50f); // Chiều cao item khi đóng
        }

        void ToggleItemExpansion(GameObject item)
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            RectTransform contentRect = contentTransform.GetComponent<RectTransform>();

            // Toggle chiều cao của item khi nhấn vào
            itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x == 50f ? 200f : 50f, itemRect.sizeDelta.y);

            // Cập nhật chiều cao của nội dung tổng cộng
            float totalHeight = 0f;
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                totalHeight += contentTransform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            }
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
        }
    }
}
