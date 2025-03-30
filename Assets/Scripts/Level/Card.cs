using ChemicalDatabaseLoader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class Card : MonoBehaviour
{
    // 化学物质ID，用于查找化学物质
    public int ID;

    // 化学物质属性
    public CDL.Chemical Chemical;
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
        CDL.LoadChemicals();
    }

    void Start()
    {
        // 根据ID查找化学物质
        List<CDL.Chemical> Results;
        Results = CDL.FindChemicals(ID);

        if (Results.Count > 0)
        {
            Chemical = Results[0];
        }
        // 显示化学物质信息
        NameText.text = Chemical.Name;
        FormulaText.text = Chemical.Formula;
        CountText.text = "*" + Count.ToString();
        CategoryText.text = Chemical.Category;
        StateText.text = State;
        FormText.text = Form;
    }
}
