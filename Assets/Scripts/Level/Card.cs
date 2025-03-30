using ChemicalDatabaseLoader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class Card : MonoBehaviour
{
    // ��ѧ����ID�����ڲ��һ�ѧ����
    public int ID;

    // ��ѧ��������
    public CDL.Chemical Chemical;
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
        CDL.LoadChemicals();
    }

    void Start()
    {
        // ����ID���һ�ѧ����
        List<CDL.Chemical> Results;
        Results = CDL.FindChemicals(ID);

        if (Results.Count > 0)
        {
            Chemical = Results[0];
        }
        // ��ʾ��ѧ������Ϣ
        NameText.text = Chemical.Name;
        FormulaText.text = Chemical.Formula;
        CountText.text = "*" + Count.ToString();
        CategoryText.text = Chemical.Category;
        StateText.text = State;
        FormText.text = Form;
    }
}
