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
        // �����Ļ�ߴ��Ƿ����仯
        if (screenSize != new Vector2(Screen.width, Screen.height))
        {
            UpdateImageSize();
        }
    }

    void UpdateImageSize()
    {
        // ��ȡ��ǰ��Ļ�ߴ�
        screenSize = new Vector2(Screen.width + 100, Screen.height + 100);
        rectTransform.sizeDelta = screenSize;
    }
}