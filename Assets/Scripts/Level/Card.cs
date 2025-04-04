using ChemicalDatabaseLoader;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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

    public GameObject ChemicalPrefab;
    public GameObject Canva;

    public CardPool Pool;
    public ReactionPool reactionPool;

    private void Awake()
    {
        // 加载化学物质列表
        // CDL.LoadChemicals();
    }

    void Start()
    {
        ShowChemicalInformation();
        Canva = GameObject.Find("Canvas");
        Pool = GameObject.Find("CardArea").GetComponent<CardPool>();
        reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();
    }

    public void ShowChemicalInformation()
    {
        // 显示化学物质信息
        if (Chemicals.Count > 1) NameText.text = "混合";
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

    public void InstantiateChemical()
    {
        GameObject newChemical = Instantiate(ChemicalPrefab, Canva.transform);
        newChemical.GetComponent<Chemicals>().ChemicalsInclude = Chemicals;
        newChemical.GetComponent<Chemicals>().ParentCard = gameObject;

        Count--;
        ShowChemicalInformation();
        if (Count == 0)
        {
            newChemical.GetComponent<Chemicals>().parentExist = false;
            Destroy(gameObject);
        }
    }

    public void GetChemical()
    {
        // 物质在反应池中是否存在
        foreach (GameObject che in reactionPool.Chemicals)
        {
            // 存在，正常生成
            if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, Chemicals))
            {
                InstantiateChemical();
            }
        }

        // 物质在反应池中不存在，而且反应池中的物质数量已经达到上限
        if (reactionPool.Chemicals.Count >= 2)
        {
            return;
        }
        // 物质在反应池中不存在，且反应池中的物质数量没有达到上限
        // 正常生成
        InstantiateChemical();
    }
}
