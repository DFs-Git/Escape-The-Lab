using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BackGroud: MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 screenSize;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateImageSize();
    }

    void Update()
    {
        // 检查屏幕尺寸是否发生变化
        if (screenSize != new Vector2(Screen.width, Screen.height))
        {
            UpdateImageSize();
        }
    }

    void UpdateImageSize()
    {
        // 获取当前屏幕尺寸
        screenSize = new Vector2(Screen.width + 100, Screen.height + 100);
        rectTransform.sizeDelta = screenSize;
    }
}