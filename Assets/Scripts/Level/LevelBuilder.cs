using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using CL = ChemicalLoader;
using Fungus;

public class LevelBuilder : MonoBehaviour
{
    public LevelLoader Loader;

    public TMP_Text Title;
    public TMP_Text TaskDescription;
    public TMP_Text Tips;
    public TMP_Dropdown Condition;
    public Flowchart flowchart;

    public GameObject CardPrefab;
    public GameObject Content;
    // 其他脚本可以引用该卡牌列表，均为引用类型
    public List<GameObject> Cards;

    // 对应了物质状态、存在形式、反应条件的数字->可视字符串映射表，具体见代表含义见Resources/Levels/Readme.txt
    public List<string> ChemicalStatesMap;
    public List<string> ExistFormMap;
    public List<string> ConditionMap;

    public GameObject DevelopText;
    public GameObject Panel;

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        CL.LoadChemicals();
        EquationLoader.LoadEquations();
    }

    void Start()
    {
        // 先获取关卡信息
        Level level = Loader.level;

        string top = level.topic == 10 ? "X" : level.topic.ToString();

        Title.text = level.chapter.ToString() + "-" + top + " " + level.title;
        TaskDescription.text = level.task_description;
        Tips.text = "想不到了吗？点击下面的按钮获取提示。";

        List<int> offer = Loader.level.offered;
        // 生成卡片
        // offer 表示每一条化学物质
        for (int i = 0; i < offer.Count; i += 2)
        {
            // 生成卡片
            GameObject newCard = Instantiate(CardPrefab, Content.transform);
            Cards.Add(newCard);

            // 设置卡片化学物质信息
            Cards[Cards.Count - 1].GetComponent<Card>().ChemicalInfo = CL.FindChemicals(offer[i])[0];

            // 设置卡片数量
            Cards[Cards.Count - 1].GetComponent<Card>().Count = offer[i + 1];

            // 显示化学物质信息
            Cards[Cards.Count - 1].GetComponent<Card>().ShowChemicalInformation();
        }
        // 设置反应条件
        Condition.ClearOptions();
        foreach (int condition in level.reaction_condition)
        {
            Condition.options.Add(new TMP_Dropdown.OptionData() { text = ConditionMap[condition] });
        }

        // 执行对话
        flowchart.ExecuteBlock(level.chapter.ToString() + "-" + level.topic.ToString());
    }

    // 主要用于处理开发者模式
    void Update()
    {
        if (PlayerPrefs.GetInt("DevelopmentMode") == 1)
        {
            DevelopText.SetActive(true);
        }
        else
        {
            DevelopText.SetActive(false);
        }

        if (PlayerPrefs.GetInt("DevelopmentMode") == 1)
        {
            if (Input.GetKey(KeyCode.Slash))
            {
                Panel.SetActive(true);
            }
        }
    }
}
