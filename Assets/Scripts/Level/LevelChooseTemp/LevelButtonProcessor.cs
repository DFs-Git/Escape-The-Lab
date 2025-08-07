using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelStructure
{
    public int chapterCount;
    public List<string> chapterName;
    public List<string> chapterIntro;
    public List<int> topicCount;
    public List<List<string>> topicName;
}

public class LevelButtonProcessor : MonoBehaviour
{
    public List<GameObject> LevelButtons;

    public GameObject LevelButtonPrefab;

    public RectTransform rectTransform;

    public LevelStructure Structure;

    public float t;
    public float eps;

    public float singleYOffset;
    public float maxY, minY;
    public Vector2 InitialPosition;
    public Vector2 TargetPosition;
    public Vector2 ButtonInitialPosition;
    public Vector2 ButtonOffset;

    public float ScrollSpeed;

    private Vector2 ToVec2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    void Awake()
    {
        // ButtonInitialPosition = new Vector2(400.0F, 150.0F);

        rectTransform = GetComponent<RectTransform>();

        // 读取关卡结构 json 文件
        TextAsset jsonText = Resources.Load<TextAsset>("Levels/level_structures");
        Structure = JsonConvert.DeserializeObject<LevelStructure>(jsonText.text);

        LevelButtons = new List<GameObject>();
    }

    public void LoadButtons(int chapter)
    {
        rectTransform.position = InitialPosition;
        TargetPosition = new Vector2(200.0F, 0F);

        // 进度未达到，不予加载
        if (PlayerPrefs.GetInt("chapter") < chapter)
        {
            return;
        }

        // 清除之前的按钮
        LevelButtons.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        int topicCount = Structure.topicCount[chapter];

        for (int i = 0; i < topicCount; i++)
        {
            if (chapter != 0 && PlayerPrefs.GetInt("chapter") == chapter && PlayerPrefs.GetInt("topic") < i + 1)
            {
                // 如果章节不是0，且当前章节的进度未达到，则不加载该按钮
                break;
            }

            // 创建新的按钮
            GameObject newButton = Instantiate(LevelButtonPrefab, rectTransform);
            newButton.name = $"Chapter_{chapter}_Topic_{i}";
            newButton.GetComponent<ChooseButtonEvent>().buttonChapter = chapter;
            if (chapter == 0)
                newButton.GetComponent<ChooseButtonEvent>().buttonTopic = 0;
            else if (i != topicCount - 1)
                newButton.GetComponent<ChooseButtonEvent>().buttonTopic = i + 1;
            else
                newButton.GetComponent<ChooseButtonEvent>().buttonTopic = 10;
            // 设置按钮位置
            // 存在上一个按钮
            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            if (LevelButtons.Count > 0)
            {
                buttonTransform.localPosition = 
                    ToVec2(LevelButtons[i - 1].GetComponent<RectTransform>().localPosition) + ButtonOffset;
                // Debug.Log($"Value {ToVec2(LevelButtons[i - 1].GetComponent<RectTransform>().localPosition)}, Position {buttonTransform.localPosition}");
            }
            // 不存在上一个按钮
            else
            {
                buttonTransform.localPosition = ButtonInitialPosition;
                // Debug.Log($"Value {ButtonInitialPosition}, Position {buttonTransform.localPosition}");
            }
            newButton.GetComponent<ChooseButtonEvent>().originPosition =
                buttonTransform.localPosition;

            // 设置按钮文本
            string buttonText = Structure.topicName[chapter][i];
            newButton.GetComponent<ChooseButtonEvent>().buttonName = buttonText;

            // 加入数组
            LevelButtons.Add(newButton);
        }

        maxY = minY + singleYOffset * (LevelButtons.Count - 1);
        rectTransform.localPosition = InitialPosition;
    }

    void Update()
    {
        // Debug.Log(rectTransform.localPosition);

        // 误差分析
        if (Vector2.Distance(rectTransform.localPosition, TargetPosition) > eps)
            rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, TargetPosition, t);
        else
            rectTransform.localPosition = TargetPosition;

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        scrollDelta = 0 - scrollDelta;

        if (scrollDelta > 0)
        {
            // 向上滚动
            if (TargetPosition.y < maxY)
            {
                TargetPosition += new Vector2(0, scrollDelta * ScrollSpeed * singleYOffset);
            }
            else
            {
                TargetPosition = new Vector2(TargetPosition.x, maxY);
            }
        }
        else if (scrollDelta < 0)
        {
            // 向下滚动
            if (TargetPosition.y > minY)
            {
                TargetPosition += new Vector2(0, scrollDelta * ScrollSpeed * singleYOffset);
            }
            else
            {
                TargetPosition = new Vector2(TargetPosition.x, minY);
            }
        }
    }
}
