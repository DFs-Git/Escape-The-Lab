using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

// ʹ��Nuget�����CSV������
using CsvHelper;

public class ChemicalDatabaseLoader : MonoBehaviour
{
    /// <summary>
    /// ��ѧ�������ݽṹ�����ڴ洢������ѧ���ʵ�����
    /// </summary>
    public struct Chemical
    {
        // Ψһ��ʶ��
        public int ID { get; }
        // ��ѧ�������ƣ����ģ�
        public string Name { get; }
        // ��ѧ����ʽ����H2O��
        public string Formula { get; }
        // ����������ᡢ��л���ȣ�
        public string Category { get; }

        /// <summary>
        /// ���캯�������ڴ����µĻ�ѧ����ʵ��
        /// </summary>
        /// <param name="id">ΨһID</param>
        /// <param name="name">��������</param>
        /// <param name="formula">��ѧ����ʽ</param>
        /// <param name="category">�������</param>
        public Chemical(int id, string name, string formula, string category)
        {
            ID = id;
            Name = name;
            Formula = formula;
            Category = category;
        }
    }

    // �洢���л�ѧ�������ݵľ�̬�б�
    private static List<Chemical> allChemicals = new List<Chemical>();

    /// <summary>
    /// Unity����ʱ����ڷ���
    /// </summary>
    void Start()
    {
        // ����CSV�ļ��еĻ�ѧ��������
        LoadChemicals();
        // ��ӡ���ؽ��������̨
        PrintChemicals();
    }

    /// <summary>
    /// ��CSV�ļ����ػ�ѧ��������
    /// </summary>
    private void LoadChemicals()
    {
        // ��Resources�ļ��м���CSV�ļ�
        TextAsset csvFile = Resources.Load<TextAsset>("chemicals.csv");

        // ����ļ��Ƿ����
        if (csvFile == null)
        {
            Debug.LogError("δ�ҵ�chemicals.csv�ļ�����ȷ���ļ�λ��Resources�ļ���");
            return;
        }

        // ʹ��StringReader��CsvReader����CSV�ļ�
        using var reader = new StringReader(csvFile.text);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // ���������У������һ��Ϊ���⣩
        csv.Read();
        csv.ReadHeader();

        // ���ж�ȡCSV����
        while (csv.Read())
        {
            // �����µĻ�ѧ����ʵ������ӵ��б�
            allChemicals.Add(new Chemical(
                csv.GetField<int>("ID"),            // ��ȡID�ֶΣ��Զ�ת��Ϊint��
                csv.GetField("����"),                // ��ȡ���������ֶ�
                csv.GetField("��ѧʽ"),              // ��ȡ��ѧ����ʽ�ֶ�
                csv.GetField("���")                 // ��ȡ��������ֶ�
            ));
        }
    }

    /// <summary>
    /// ��ӡ���м��صĻ�ѧ������Ϣ��Unity����̨
    /// </summary>
    private void PrintChemicals()
    {
        // �����������
        Debug.Log($"������ {allChemicals.Count} ����ѧ����");

        // �������л�ѧ���ʲ���ӡ��ϸ��Ϣ
        foreach (var chemical in allChemicals)
        {
            Debug.Log($"ID: {chemical.ID} | ����: {chemical.Name} | ��ѧʽ: {chemical.Formula} | ���: {chemical.Category}");
        }
    }
}