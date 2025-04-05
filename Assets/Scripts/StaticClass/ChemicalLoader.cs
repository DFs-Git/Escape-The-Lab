// 使用必要的命名空间
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

// 使用Nuget导入的CSV解析包
using CsvHelper;
using System.Linq;

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
    // 化学性质描述
    public string ChemicalProperty { get; }
    // 物理性质描述
    public string PhysicalProperty { get; }

    /// <summary>
    /// 构造函数，用于创建新的化学物质实例
    /// </summary>
    /// <param name="id">唯一ID</param>
    /// <param name="name">中文名称</param>
    /// <param name="formula">化学分子式</param>
    /// <param name="category">分类类别</param>
    /// <param name="chemicalProperty">化学性质描述</param>
    /// <param name="physicaProperty">物理性质描述</param>
    public Chemical(int id, string name, string formula, string category, string chemicalProperty, string physicaProperty)
    {
        ID = id;
        Name = name;
        Formula = formula;
        Category = category;
        ChemicalProperty = chemicalProperty;
        PhysicalProperty = physicaProperty;
    }
}

public struct MolChemicals
{
    public Chemical Chemicals;//纯净物
    public int MolNum;  //摩尔数
    public MolChemicals(Chemical _Chemicals, int _MolNum)
    {
        Chemicals = _Chemicals;
        MolNum = _MolNum;
    }
    public MolChemicals(string _Chemicals, int _MolNum)
    {
        Chemicals = ChemicalLoader.FindChemicals(formula : _Chemicals)[0];
        MolNum = _MolNum;
    }
    override public string ToString()
    {
        return $"{MolNum}*{Chemicals.Formula}";
    }
}
// 数据库加载器类：负责加载和处理化学物质数据
public static class ChemicalLoader
{
    
    

    // 存储所有化学物质数据的静态列表（内存数据库）
    public static List<Chemical> allChemicals = new List<Chemical>();


    /// <summary>
    /// CSV数据加载核心方法：从Resources/chemicals.csv读取并解析数据
    /// </summary>
    public static void LoadChemicals()
    {
        allChemicals.Clear();
        // 从Unity资源系统加载CSV文本文件
        TextAsset csvFile = Resources.Load<TextAsset>("Chemicals/chemicals");

        // 空文件检查
        if (csvFile == null)
        {
            Debug.LogError("未找到chemicals.csv文件！请确认文件位于Resources文件夹");
            return;
        }

        // 创建CSV阅读器并配置解析规则
        using var reader = new StringReader(csvFile.text);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // 跳过标题行（第一行为字段名称）
        csv.Read();
        csv.ReadHeader();

        // 逐行解析CSV数据
        while (csv.Read())
        {
            // 构造Chemical对象并添加到列表
            allChemicals.Add(new Chemical(
                csv.GetField<int>("ID"),            // 解析整型ID字段
                csv.GetField("名称"),               // 解析中文名称
                csv.GetField("化学式"),             // 解析化学分子式
                csv.GetField("类别"),               // 解析分类类别
                csv.GetField("化学性质"),           // 解析化学性质描述
                csv.GetField("物理性质")            // 解析物理性质描述
            ));
        }
    }

    /// <summary>
    /// 调试输出方法：在控制台打印所有加载的化学物质信息
    /// </summary>
    public static void PrintChemicals()
    {
        if (allChemicals.Count == 0)
        {
            LoadChemicals();
        }
        Debug.Log($"共加载 {allChemicals.Count} 条化学数据");
        foreach (var chemical in allChemicals)
        {
            Debug.Log($"ID: {chemical.ID} | 名称: {chemical.Name} | 化学式: {chemical.Formula} | 类别: {chemical.Category}|化学性质: {chemical.ChemicalProperty} | 物理性质: {chemical.PhysicalProperty}");
        }
    }

    /// <summary>
    /// 重载打印方法：输出指定化学物质列表的详细信息
    /// </summary>
    /// <param name="chemicals">需要打印的化学物质集合</param>
    public static void PrintChemicals(List<Chemical> chemicals)
    {
        Debug.Log($"找到 {chemicals.Count} 条结果");
        foreach (var chemical in chemicals)
        {
            Debug.Log($"ID: {chemical.ID} | 名称: {chemical.Name} | 化学式: {chemical.Formula} | 类别: {chemical.Category}|化学性质: {chemical.ChemicalProperty} | 物理性质: {chemical.PhysicalProperty}");
        }
    }

    public static void PrintChemical(Chemical chemical)
    {

        Debug.Log($"ID: {chemical.ID} | 名称: {chemical.Name} | 化学式: {chemical.Formula} | 类别: {chemical.Category}|化学性质: {chemical.ChemicalProperty} | 物理性质: {chemical.PhysicalProperty}");

    }

    /// <summary>
    /// 高级查询方法：支持多条件组合检索化学物质
    /// </summary>
    /// <param name="id">精确ID查询</param>
    /// <param name="name">名称模糊查询（包含匹配）</param>
    /// <param name="formula">化学式精确匹配</param>
    /// <param name="category">类别精确匹配</param>
    /// <returns>满足条件的化学物质集合</returns>
    public static List<Chemical> FindChemicals(
        int? id = null,
        string name = null,
        string formula = null,
        string category = null)
    {
        if (allChemicals.Count == 0)
        {
            LoadChemicals();
        }

        // 初始化LINQ查询
        var query = allChemicals.AsQueryable();

        // 构建动态查询条件
        if (id.HasValue) query = query.Where(c => c.ID == id.Value);
        if (!string.IsNullOrEmpty(name)) query = query.Where(c => c.Name.Contains(name));
        if (!string.IsNullOrEmpty(formula)) query = query.Where(c => c.Formula == formula);
        if (!string.IsNullOrEmpty(category)) query = query.Where(c => c.Category == category);

        return query.ToList();
    }
}
