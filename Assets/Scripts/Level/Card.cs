using ChemicalDatabaseLoader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void Awake()
    {
        // ���ػ�ѧ�����б�
        // CDL.LoadChemicals();
    }

    void Start()
    {
        ShowChemicalInformation();
    }

    public void ShowChemicalInformation()
    {
        // ��ʾ��ѧ������Ϣ
        if (Chemicals.Count > 1) NameText.text = "δ֪";
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
}
