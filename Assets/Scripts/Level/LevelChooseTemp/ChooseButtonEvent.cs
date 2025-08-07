using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseButtonEvent : MonoBehaviour
{
    public int buttonChapter;
    public int buttonTopic;
    public string buttonName;

    public bool mouseTouching = false;

    public LevelLoader Loader;

    public RectTransform rectTransform;

    public TMP_Text IDText;
    public TMP_Text NameText;

    public float eps;
    public Vector2 originPosition;
    public Vector2 targetPosition;
    public Vector2 touchedOffset;

    void Start()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        rectTransform = GetComponent<RectTransform>();

        if (buttonTopic == 10) IDText.text = buttonChapter.ToString() + "-X";
        else IDText.text = buttonChapter.ToString() + "-" + buttonTopic.ToString();
        NameText.text = buttonName;
    }

    public void EnterLevel()
    {
        Loader.LoadLevel(buttonChapter, buttonTopic);
    }

    void Update()
    {
        // 误差分析
        if (Vector2.Distance(rectTransform.localPosition, targetPosition) > eps)
            rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, targetPosition, 0.1F);
        else
            rectTransform.localPosition = targetPosition;

        if (CheckWhetherTouched())
        {
            targetPosition = originPosition + touchedOffset;
        }
        else
        {
            targetPosition = originPosition;
        }
    }

    private bool CheckWhetherTouched()
    {
        // 创建一个指针事件数据
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // 创建一个列表来存储射线结果
        List<RaycastResult> results = new List<RaycastResult>();

        // 进行射线检测
        EventSystem.current.RaycastAll(eventData, results);

        // 返回 Layer 为 "LevelButton" 的 UI 元素
        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject == gameObject)
                {
                    return true;
                }
            }

            return false;
        }
        else return false;
    }
}
