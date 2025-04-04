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
    // ��ѧ��������
    // public CDL.Chemical Chemical;
    public List<CDL.Chemical> Chemicals;
    public List<int> CheCount;
    public int Count;
    public string State;
    public string Form;
    
    // ������ʾ�ı�
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
        // ���ػ�ѧ�����б�
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
        // ��ʾ��ѧ������Ϣ
        if (Chemicals.Count > 1) NameText.text = "���";
        else NameText.text = Chemicals[0].Name;
        FormulaText.text = "[";
        for (int i = 0; i < Chemicals.Count; i++)
        {
            FormulaText.text +=
                Chemicals[i].Formula + "*" + CheCount[i].ToString() + ";";
        }
        FormulaText.text += "]";
        CountText.text = "*" + Count.ToString();
        if (Chemicals.Count > 1) CategoryText.text = "�����";
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
        // �����ڷ�Ӧ�����Ƿ����
        foreach (GameObject che in reactionPool.Chemicals)
        {
            // ���ڣ���������
            if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, Chemicals))
            {
                InstantiateChemical();
            }
        }

        // �����ڷ�Ӧ���в����ڣ����ҷ�Ӧ���е����������Ѿ��ﵽ����
        if (reactionPool.Chemicals.Count >= 2)
        {
            return;
        }
        // �����ڷ�Ӧ���в����ڣ��ҷ�Ӧ���е���������û�дﵽ����
        // ��������
        InstantiateChemical();
    }
}
