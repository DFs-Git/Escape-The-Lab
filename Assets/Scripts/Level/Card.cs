
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

    public TMP_Text GetAllText;
    public TMP_Text GetAllTips;
    public TMP_Text GetHalfTips;

    public GameObject ChemicalPrefab;
    public GameObject Canva;

    public ReactionPool reactionPool;
    public LevelBuilder Builder;

    public bool holdingLCtrl = false;
    public bool holdingLAlt = false;

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

        GetAllText = GameObject.Find("GetAllText").GetComponent<TMP_Text>();
        GetAllTips = GameObject.Find("GetAllTips").GetComponent<TMP_Text>();
        GetHalfTips = GameObject.Find("GetHalfTips").GetComponent<TMP_Text>();
    }

    void Update()
    {
        CheckIfHolding();

        if (holdingLCtrl)
        {
            GetAllText.text = "取出全部";
            GetAllTips.text = "松开L-Ctrl单个取出";
        }
        else if (holdingLAlt)
        {
            GetAllText.text = "对半取出";
            GetHalfTips.text = "松开L-Alt单个取出";
        }
        else
        {
            GetAllText.text = "单个取出";
            GetAllTips.text = "按住L-Ctrl取出全部";
            GetHalfTips.text = "按住L-Alt对半取出";
        }
    }

    public void CheckIfHolding()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !holdingLAlt)
        {
            holdingLCtrl = true;
        }
        else
        {
            holdingLCtrl = false;
        }

        if (Input.GetKey(KeyCode.LeftAlt) && !holdingLCtrl)
        {
            holdingLAlt = true;
        }
        else
        {
            holdingLAlt = false;
        }
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
        GameObject newChemical = Instantiate(ChemicalPrefab, transform.position, Quaternion.identity, Canva.transform);
        newChemical.GetComponent<Chemicals>().ChemicalInclude = molChemical.Chemical;
        newChemical.GetComponent<Chemicals>().ParentCard = gameObject;
        // 按下L-Ctrl取出全部
        if (holdingLCtrl)
        {
            Debug.Log("Hold");
            newChemical.GetComponent<Chemicals>().Count = molChemical.MolNum;
        }
        // 按下L-Alt取出一半
        else if (holdingLAlt)
        {
            newChemical.GetComponent<Chemicals>().Count = molChemical.MolNum / 2;
            if (molChemical.MolNum / 2 == 0)
            {
                Destroy(newChemical);
                return;
            }
        }

        // 设置关于这张卡牌的属性，便于重新生成
        newChemical.GetComponent<Chemicals>().ParentMolChemical.Chemical = molChemical.Chemical;
        newChemical.GetComponent<Chemicals>().ParentMolChemical.MolNum = molChemical.MolNum;

        molChemical.MolNum -= newChemical.GetComponent<Chemicals>().Count;
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
