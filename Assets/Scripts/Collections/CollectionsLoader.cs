using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChemicalDatabaseLoader;
using static ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using Unity.VisualScripting;
using Button = UnityEngine.UI.Button;
using TMPro;

/// <summary>
/// ��ѧ��Ʒ���ϼ���������̬���ɻ�ѧ��Ʒ��ť����ʾ��ϸ��Ϣ
/// </summary>
public class CollectionsLoader : MonoBehaviour
{
    [Header("UI����")]
    public Button Item; // ��ťԤ���壨���ڶ�̬���ɣ�

    void Start()
    {
        // ��ʼ��UI������� 
        // ��ȡ��ʾ��ѧ��Ʒ����������ı����
        TextMeshProUGUI Name = GameObject.Find("Shower/Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Formula = GameObject.Find("Shower/Formula").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Content = GameObject.Find("Shower/Content").GetComponent<TextMeshProUGUI>();

        // ���ػ�ѧ����
        CDL.allChemicals.Clear();    // ����������ݻ���
        CDL.LoadChemicals();         // �����ݿ��������
        List<Chemical> allChemicals = CDL.allChemicals; // ��ȡȫ����ѧƷ�б�

        // ��ť����¼�����Ƕ�׷�����
        void OnButtonClick(int ID)
        {
            // ����ID���Ҷ�Ӧ��ѧƷ�����������ҵ���
            Chemical Che = CDL.FindChemicals(ID)[0];

            // ����UI��ʾ����
            Name.text = $"{Che.Name} {Che.Category}";       // ���� + ���
            Formula.text = Che.Formula;                     // ��ѧʽ
            Content.text = $"�������ʣ�{Che.PhysicaProperty}\n��ѧ���ʣ�{Che.ChemicalProperty}"; // �ﻯ����
        }

        // ��̬���ɰ�ť�߼�
        Transform[] transforms = GetComponentsInChildren<Transform>(); // ��⵱ǰ������

        // ����û��������ʱ���ɰ�ť�������ظ�������!!!!!!!!!��Ҳ��֪����Ϊʲô���������Σ�ֻ��������
        if (transforms.Length <= 1) // ע�⣺transform����ᱻ��������
        {
            foreach (Chemical Che in allChemicals)
            {
                // ʵ������ť�����õ���ǰ������
                Button btn = (Button)Instantiate(Item, this.transform);
                btn.transform.SetParent(this.transform, false); // ���ø����岢����λ��

                // ���ð�ť��ʾ�ı�
                btn.GetComponentInChildren<TextMeshProUGUI>().text = Che.Name;

                // �հ����⴦����ID���Ƶ��ֲ��������ؼ�����ɾ��!!!!!!!!!!!!!
                int ID = Che.ID;
                // �󶨵���¼������ʱ����OnButtonClick������ID
                btn.onClick.AddListener(() => OnButtonClick(ID));
            }
        }

        // Ĭ����ʾ��һ����ѧƷ����Ϣ
        OnButtonClick(1);
    }
}