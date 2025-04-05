using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        // 2个正整数，以英文半角逗号分隔，分别表示化学物质的序号、卡牌张数
        // 以英文半角逗号结束
        string value = Input.text;

        List<int> data = new List<int>();
        List<int> cheCount = new List<int>();

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
        parentCard.GetComponent<Card>().molChemical.Chemical = CL.FindChemicals(data[0])[0];
        parentCard.GetComponent<Card>().molChemical.MolNum = data[1];

        parentCard.GetComponent<Card>().ShowChemicalInformation();

        Panel.SetActive(false);
    }
}
