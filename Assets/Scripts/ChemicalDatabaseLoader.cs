using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

// 使用Nuget导入的CSV解析包
using CsvHelper;

public class ChemicalDatabaseLoader : MonoBehaviour
{
    /// <summary>
    /// 化学物质数据结构，用于存储单个化学物质的属性
    /// </summary>
    public struct Chemical
    {
        // 唯一标识符
        public int ID { get; }
        // 化学物质名称（中文）
        public string Name { get; }
        // 化学分子式（如H2O）
        public string Formula { get; }
        // 分类类别（如酸、碱、有机物等）
        public string Category { get; }

        /// <summary>
        /// 构造函数，用于创建新的化学物质实例
        /// </summary>
        /// <param name="id">唯一ID</param>
        /// <param name="name">中文名称</param>
        /// <param name="formula">化学分子式</param>
        /// <param name="category">分类类别</param>
        public Chemical(int id, string name, string formula, string category)
        {
            ID = id;
            Name = name;
            Formula = formula;
            Category = category;
        }
    }

    // 存储所有化学物质数据的静态列表
    private static List<Chemical> allChemicals = new List<Chemical>();

    /// <summary>
    /// Unity启动时的入口方法
    /// </summary>
    void Start()
    {
        // 加载CSV文件中的化学物质数据
        LoadChemicals();
        // 打印加载结果到控制台
        PrintChemicals();
    }

    /// <summary>
    /// 从CSV文件加载化学物质数据
    /// </summary>
    private void LoadChemicals()
    {
        // 从Resources文件夹加载CSV文件
        TextAsset csvFile = Resources.Load<TextAsset>("chemicals.csv");

        // 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("未找到chemicals.csv文件！请确认文件位于Resources文件夹");
            return;
        }

        // 使用StringReader和CsvReader解析CSV文件
        using var reader = new StringReader(csvFile.text);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // 跳过标题行（假设第一行为标题）
        csv.Read();
        csv.ReadHeader();

        // 逐行读取CSV数据
        while (csv.Read())
        {
            // 创建新的化学物质实例并添加到列表
            allChemicals.Add(new Chemical(
                csv.GetField<int>("ID"),            // 获取ID字段（自动转换为int）
                csv.GetField("名称"),                // 获取中文名称字段
                csv.GetField("化学式"),              // 获取化学分子式字段
                csv.GetField("类别")                 // 获取分类类别字段
            ));
        }
    }

    /// <summary>
    /// 打印所有加载的化学物质信息到Unity控制台
    /// </summary>
    private void PrintChemicals()
    {
        // 输出加载总数
        Debug.Log($"共加载 {allChemicals.Count} 条化学数据");

        // 遍历所有化学物质并打印详细信息
        foreach (var chemical in allChemicals)
        {
            Debug.Log($"ID: {chemical.ID} | 名称: {chemical.Name} | 化学式: {chemical.Formula} | 类别: {chemical.Category}");
        }
    }
}