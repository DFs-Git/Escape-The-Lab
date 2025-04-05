using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;
using CL = ChemicalLoader;

public class LevelDevelop : MonoBehaviour
{
    public TMP_InputField Input;
    public LevelBuilder Builder;
    public GameObject Panel;

    public GameObject CardPrefab;
    public GameObject Content;

    public void Submit()
    {
        // 解析输入内容
        // 这里获取纯净物
        // 4个正整数，以英文半角逗号分隔，分别表示化学物质的序号、卡牌张数、物质状态、物质存在形式
        // 以英文半角逗号结束
        string value = Input.text;

        List<int> data = new List<int>();
        List<int> cheCount = new List<int>();
        cheCount.Add(1);

        int number = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != ',')
            {
                number *= 10;
                number += value[i] - '0';
            }
            else
            {
                data.Add(number);
                number = 0;
            }
        }

        CL.LoadChemicals();

        // 创建卡牌
        GameObject parentCard = Instantiate(CardPrefab, Content.transform);
        Builder.Cards.Add(parentCard);
        parentCard.GetComponent<Card>().Chemicals = CL.FindChemicals(data[0]);
        parentCard.GetComponent<Card>().CheCount = cheCount;
        parentCard.GetComponent<Card>().State = Builder.ChemicalStatesMap[data[2]];
        parentCard.GetComponent<Card>().Form = Builder.ExistFormMap[data[3]];
        parentCard.GetComponent<Card>().Count = data[1];

        parentCard.GetComponent<Card>().ShowChemicalInformation();

        Panel.SetActive(false);
    }
}
