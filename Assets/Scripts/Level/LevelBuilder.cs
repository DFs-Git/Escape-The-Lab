using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.U2D.Animation;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class LevelBuilder : MonoBehaviour
{
    public LevelLoader Loader;

    public TMP_Text Title;
    public TMP_Text TaskDescription;
    public TMP_Text Tips;
    public TMP_Dropdown Condition;

    public GameObject CardPrefab;
    public GameObject Content;
    public List<GameObject> Cards;

    public List<string> ChemicalStatesMap;
    public List<string> ExistFormMap;
    public List<string> ConditionMap;

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        CDL.LoadChemicals();
    }

    void Start()
    {
        // �Ȼ�ȡ�ؿ���Ϣ
        Level level = Loader.level;

        Title.text = level.chapter.ToString() + "-" + level.topic.ToString() + " " + level.title;
        TaskDescription.text = level.task_description;
        Tips.text = "�벻�����𣿵������İ�ť��ȡ��ʾ��";

        // ���ɿ�Ƭ
        // offer ��ʾÿһ����ѧ����
        foreach (List<int> offer in level.offered)
        {
            GameObject newCard = Instantiate(CardPrefab, Content.transform);
            Cards.Add(newCard);

            int count = offer[0];
            List<CDL.Chemical> cheInclude = new List<CDL.Chemical>();
            int i = 0;
            for (i = 1; i <= count * 2; i += 2)
            {
                cheInclude.Add(CDL.allChemicals[offer[i] - 1]);
                Cards[Cards.Count - 1].GetComponent<Card>().CheCount.Add(offer[i + 1]);
            }
            Cards[Cards.Count - 1].GetComponent<Card>().Chemicals = cheInclude;

            Cards[Cards.Count - 1].GetComponent<Card>().Count = offer[i];
            Cards[Cards.Count - 1].GetComponent<Card>().State = ChemicalStatesMap[offer[i + 1]];
            Cards[Cards.Count - 1].GetComponent<Card>().Form = ExistFormMap[offer[i + 2]];

            Cards[Cards.Count - 1].GetComponent<Card>().ShowChemicalInformation();
        }
        // ���÷�Ӧ����
        Condition.ClearOptions();
        foreach (int condition in level.reaction_condition)
        {
            Condition.options.Add(new TMP_Dropdown.OptionData() { text = ConditionMap[condition] });
        }
    }
}
