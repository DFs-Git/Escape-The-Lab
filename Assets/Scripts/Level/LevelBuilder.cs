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

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        CL.LoadChemicals();
    }

    void Start()
    {
        // 先获取关卡信息
        Level level = Loader.level;

        Title.text = level.chapter.ToString() + "-" + level.topic.ToString() + " " + level.title;
        TaskDescription.text = level.task_description;
        Tips.text = "想不到了吗？点击下面的按钮获取提示。";

        // 生成卡片
        // offer 表示每一条化学物质
        foreach (List<int> offer in level.offered)
        {
            // 生成卡片
            GameObject newCard = Instantiate(CardPrefab, Content.transform);
            Cards.Add(newCard);

            // 设置卡片化学物质信息
            int count = offer[0];
            List<Chemical> cheInclude = new List<Chemical>();
            int i = 0;
            for (i = 1; i <= count * 2; i += 2)
            {
                cheInclude.Add(CL.allChemicals[offer[i] - 1]);
                Cards[Cards.Count - 1].GetComponent<Card>().CheCount.Add(offer[i + 1]);
            }
            Cards[Cards.Count - 1].GetComponent<Card>().Chemicals = cheInclude;

            // 设置卡片其他属性
            Cards[Cards.Count - 1].GetComponent<Card>().Count = offer[i];
            Cards[Cards.Count - 1].GetComponent<Card>().State = ChemicalStatesMap[offer[i + 1]];
            Cards[Cards.Count - 1].GetComponent<Card>().Form = ExistFormMap[offer[i + 2]];

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
}
