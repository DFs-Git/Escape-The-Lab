using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader; // ʹ�ñ�������������
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��ѧ���ʼ��Ͻ��������������̬���ɻ�ѧ���ʰ�ť����ʾ��ϸ��Ϣ
/// </summary>
public class CollectionsLoader : MonoBehaviour
{
    [Header("UI����")]
    [SerializeField] private Button itemPrefab;        // ��ťԤ���壬��������ÿ����ѧ������
    [SerializeField] private TextMeshProUGUI nameDisplay;     // ��ѧ����������ʾ����
    [SerializeField] private TextMeshProUGUI formulaDisplay;  // ��ѧʽ��ʾ����
    [SerializeField] private TextMeshProUGUI contentDisplay;  // ��ϸ��Ϣ��ʾ����

    private void Awake()
    {
        // ȷ���ڳ�������ʱ��ʼ����ѧ���ݿ�
        // ����������δ����ʱ���к�������
        if (CDL.allChemicals.Count == 0)
        {
            CDL.LoadChemicals();
        }
    }

    void Start()
    {
        if (nameDisplay == null || formulaDisplay == null || contentDisplay == null) {
            Debug.Log("���Ϊ��");
        }
        // ��������Ӷ��󣬷�ֹ�ظ�����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(GenerateButtons());

        // Ĭ��չʾ��һ����ѧ������Ϣ���ṩ��ʼ����״̬
        if (CDL.allChemicals.Count > 0)
        {
            OnButtonClick(CDL.allChemicals[0].ID);
        }
    }

    /// <summary>
    /// Э�����ɻ�ѧ���ʰ�ť��ÿ����5���ȴ�һ֡�Ż�����
    /// </summary>
    IEnumerator GenerateButtons()
    {
        foreach (Chemical che in CDL.allChemicals)
        {
            // ʵ������ť��������ʾ�ı�
            Button btn = Instantiate(itemPrefab, transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = che.Name;

            // ʹ�þֲ���������հ�����
            int currentID = che.ID;
            btn.onClick.AddListener(() => OnButtonClick(currentID));

            // ��֡�����Ż����ܣ�ÿ����5����ť�ȴ�һ֡
            if (CDL.allChemicals.IndexOf(che) % 5 == 0)
                yield return null;
        }
    }

    /// <summary>
    /// ��ť����¼����������Ҳ���ϸ��Ϣ��ʾ
    /// </summary>
    /// <param name="id">��ѧ����Ψһ��ʶ��</param>
    void OnButtonClick(int id)
    {
        List<Chemical> result = CDL.FindChemicals(id);

        // ��ȫ������������Ч��
        if (result != null && result.Count > 0)
        {
            Chemical che = result[0];
            // ��ʽ����ʾ��ѧ������Ϣ
            nameDisplay.text = $"{che.Name} {che.Category}";
            formulaDisplay.text = che.Formula;
            contentDisplay.text = $"�������ʣ�{che.PhysicalProperty}\n��ѧ���ʣ�{che.ChemicalProperty}";
        }
        else
        {
            Debug.LogWarning($"δ�ҵ�IDΪ{id}�Ļ�ѧ����");
            // �����ڴ˴����UI������ʾ�����������ʾ�������ʾ������Ϣ
        }
    }
}