
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using CL = ChemicalLoader;

public class Card : MonoBehaviour
{
    // 化学物质属性
    public MolChemical molChemical;
    
    // 所有显示文本
    public TMP_Text NameText;
    public TMP_Text FormulaText;
    public TMP_Text CountText;
    public TMP_Text CategoryText;

    public GameObject ChemicalPrefab;
    public GameObject Canva;

    public ReactionPool reactionPool;
    public LevelBuilder Builder;

    private void Awake()
    {
        // 加载化学物质列表
        // CDL.LoadChemicals();
    }

    void Start()
    {
        ShowChemicalInformation();
        Canva = GameObject.Find("Canvas");
        reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();
        Builder = Camera.main.GetComponent<LevelBuilder>();
    }

    public void ShowChemicalInformation()
    {
        // 显示化学物质信息
        NameText.text = molChemical.Chemical.Name;
        FormulaText.text = molChemical.Chemical.Formula;
        CountText.text =molChemical.MolNum.ToString();
        CategoryText.text = molChemical.Chemical.Category;
    }

    public void InstantiateChemical()
    {
        GameObject newChemical = Instantiate(ChemicalPrefab, Canva.transform);
        newChemical.GetComponent<Chemicals>().ChemicalInclude = molChemical.Chemical;
        newChemical.GetComponent<Chemicals>().ParentCard = gameObject;
        // 设置关于这张卡牌的属性，便于重新生成
        newChemical.GetComponent<Chemicals>().ParentMolChemical.Chemical = molChemical.Chemical;
        newChemical.GetComponent<Chemicals>().ParentMolChemical.MolNum = molChemical.MolNum;

        molChemical.MolNum--;
        ShowChemicalInformation();
        if (molChemical.MolNum == 0)
        {
            Builder.Cards.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void GetChemical()
    {
        // 物质在反应池中是否存在
        foreach (GameObject che in reactionPool.Chemicals)
        {
            // 存在，正常生成
            if (Equals(che.GetComponent<Chemicals>().ChemicalInclude, molChemical.Chemical))
            {
                InstantiateChemical();
                return;
            }
        }


        // 物质在反应池中不存在，而且反应池中的物质种类已经达到上限
        // 不再生成
        //if (reactionPool.Chemicals.Count >= 2)
        //{
        //    return;
        //}


        // 物质在反应池中不存在，且反应池中的物质种类没有达到上限
        // 正常生成
        InstantiateChemical();
    }
}
