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

public struct MolChemical
{
    public Chemical Chemical;//纯净物
    public int MolNum;  //摩尔数
    public MolChemical(Chemical _Chemicals, int _MolNum)
    {
        Chemical = _Chemicals;
        MolNum = _MolNum;
    }
    public MolChemical(string _Chemicals, int _MolNum)
    {
        Chemical = ChemicalLoader.FindChemicals(formula : _Chemicals)[0];
        MolNum = _MolNum;
    }
    override public string ToString()
    {
        return $"{MolNum}*{Chemical.Formula}";
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

    //将allChemicals按照与str的匹配程度排序并返回
    //public static List<Chemical> FindChemicals(string str)
    //{

    //}
    // 将allChemicals按照与str的匹配程度排序并返回（增强模糊匹配版）
    public static List<Chemical> FindChemicals(string str)
    {
        if (allChemicals.Count == 0)
        {
            LoadChemicals();
        }

        // 如果输入为空，返回所有化学物质
        if (string.IsNullOrWhiteSpace(str)||str=="")
        {
            return allChemicals.ToList();
        }

        // 预处理搜索字符串：去除空格并转换为小写
        string searchStr = str.Trim().ToLower();

        // 计算每个化学物质与搜索字符串的匹配分数
        var scoredChemicals = allChemicals.Select(chemical =>
        {
            //float score = 0;
            float maxFieldScore = 0; // 记录各字段的最高分，用于最终得分计算

            // 1. 名称匹配（最高权重）
            if (!string.IsNullOrEmpty(chemical.Name))
            {
                string name = chemical.Name.ToLower();
                float nameScore = CalculateMatchScore(name, searchStr, isPrimaryField: true);
                maxFieldScore = Mathf.Max(maxFieldScore, nameScore);
            }

            // 2. 化学式匹配（中等权重）
            if (!string.IsNullOrEmpty(chemical.Formula))
            {
                string formula = chemical.Formula.ToLower();
                float formulaScore = CalculateMatchScore(formula, searchStr, isPrimaryField: false);
                maxFieldScore = Mathf.Max(maxFieldScore, formulaScore * 0.8f); // 化学式权重为名称的80%
            }

            // 3. 类别匹配（较低权重）
            if (!string.IsNullOrEmpty(chemical.Category))
            {
                string category = chemical.Category.ToLower();
                float categoryScore = CalculateMatchScore(category, searchStr, isPrimaryField: false);
                maxFieldScore = Mathf.Max(maxFieldScore, categoryScore * 0.6f); // 类别权重为名称的60%
            }

            // 4. 化学性质匹配（最低权重）
            if (!string.IsNullOrEmpty(chemical.ChemicalProperty))
            {
                string property = chemical.ChemicalProperty.ToLower();
                float propertyScore = CalculateMatchScore(property, searchStr, isPrimaryField: false);
                maxFieldScore = Mathf.Max(maxFieldScore, propertyScore * 0.4f); // 性质权重为名称的40%
            }

            return new { Chemical = chemical, Score = maxFieldScore };
        })
        // 过滤掉分数为0的结果
        .Where(x => x.Score >= 0)
        // 按分数降序排序
        .OrderByDescending(x => x.Score)
        .Select(x => x.Chemical)
        .ToList();

        return scoredChemicals;
    }

    // 计算字符串匹配分数的通用方法
    private static float CalculateMatchScore(string fieldValue, string searchStr, bool isPrimaryField)
    {
        float score = 0;

        // 1. 完全匹配（最高分）
        if (fieldValue == searchStr)
        {
            score = isPrimaryField ? 100f : 80f;
        }
        // 2. 开头匹配（较高分）
        else if (fieldValue.StartsWith(searchStr))
        {
            float lengthRatio = searchStr.Length / (float)fieldValue.Length;
            score = (isPrimaryField ? 90f : 70f) * lengthRatio;
        }
        // 3. 包含匹配（中等分）
        else if (fieldValue.Contains(searchStr))
        {
            float lengthRatio = searchStr.Length / (float)fieldValue.Length;
            score = (isPrimaryField ? 70f : 50f) * lengthRatio;
        }
        // 4. 模糊匹配（使用改进的相似度算法）
        else
        {
            float similarity = ImprovedStringSimilarity(fieldValue, searchStr);
            score = (isPrimaryField ? 60f : 40f) * similarity;
        }

        return score;
    }

    // 改进的字符串相似度算法（结合Levenshtein距离和公共子序列）
    private static float ImprovedStringSimilarity(string s1, string s2)
    {
        // 如果任一字符串为空，相似度为0
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            return 0f;

        // 计算Levenshtein距离
        int levenshteinDistance = LevenshteinDistance(s1, s2);
        float maxLen = Mathf.Max(s1.Length, s2.Length);
        float levenshteinSimilarity = 1 - (levenshteinDistance / maxLen);

        // 计算最长公共子序列相似度
        float lcsSimilarity = LongestCommonSubsequence(s1, s2) / maxLen;

        // 结合两种相似度算法（加权平均）
        return (levenshteinSimilarity * 0.6f) + (lcsSimilarity * 0.4f);
    }

    // Levenshtein距离算法
    private static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return m;
        if (m == 0) return n;

        for (int i = 0; i <= n; d[i, 0] = i++) ;
        for (int j = 0; j <= m; d[0, j] = j++) ;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Mathf.Min(
                    Mathf.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }

    // 计算最长公共子序列长度
    private static float LongestCommonSubsequence(string s1, string s2)
    {
        int[,] dp = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; i++)
        {
            for (int j = 0; j <= s2.Length; j++)
            {
                if (i == 0 || j == 0)
                    dp[i, j] = 0;
                else if (s1[i - 1] == s2[j - 1])
                    dp[i, j] = dp[i - 1, j - 1] + 1;
                else
                    dp[i, j] = Mathf.Max(dp[i - 1, j], dp[i, j - 1]);
            }
        }

        return dp[s1.Length, s2.Length];
    }
}
