using ChemicalDatabaseLoader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class Card : MonoBehaviour
{
    // 化学物质属性
    // public CDL.Chemical Chemical;
    public List<CDL.Chemical> Chemicals;
    public List<int> CheCount;
    public int Count;
    public string State;
    public string Form;
    
    // 所有显示文本
    public TMP_Text NameText;
    public TMP_Text FormulaText;
    public TMP_Text CountText;
    public TMP_Text CategoryText;
    public TMP_Text StateText;
    public TMP_Text FormText;

    private void Awake()
    {
        // 加载化学物质列表
        // CDL.LoadChemicals();
    }

    void Start()
    {
        ShowChemicalInformation();
    }

    public void ShowChemicalInformation()
    {
        // 显示化学物质信息
        if (Chemicals.Count > 1) NameText.text = "未知";
        else NameText.text = Chemicals[0].Name;
        FormulaText.text = "[";
        for (int i = 0; i < Chemicals.Count; i++)
        {
            FormulaText.text +=
                Chemicals[i].Formula + "*" + CheCount[i].ToString() + ";";
        }
        FormulaText.text += "]";
        CountText.text = "*" + Count.ToString();
        if (Chemicals.Count > 1) CategoryText.text = "混合物";
        else
            CategoryText.text = Chemicals[0].Category;
        StateText.text = State;
        FormText.text = Form;
    }
}
