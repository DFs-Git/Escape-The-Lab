using Fungus;               // 用于对话系统的命名空间
using Newtonsoft.Json;      // JSON序列化/反序列化库
using Newtonsoft.Json.Linq; // 用于处理动态JSON对象
using System;               // 基础系统命名空间
using System.Collections.Generic; // 集合类命名空间
using UnityEngine;          // Unity引擎核心命名空间
using CL = ChemicalLoader;  // 化学加载器的别名
using System.Linq;          // LINQ查询扩展
using System.IO;            // 文件IO操作

/// <summary>
/// 表示化学反应方程式的结构体
/// </summary>
[Serializable] // 可序列化标记，允许Unity序列化此结构体
public struct Equation
{
    /// <summary>反应物列表，包含化学物质和对应摩尔数</summary>
    public List<MolChemical> Reactants;

    /// <summary>反应条件描述字符串</summary>
    public string Condition;

    /// <summary>生成物列表，包含化学物质和对应摩尔数</summary>
    public List<MolChemical> Products;

    /// <summary>
    /// 构造函数，初始化方程式
    /// </summary>
    /// <param name="_Reactants">反应物列表</param>
    /// <param name="_Condition">反应条件</param>
    /// <param name="_Products">生成物列表</param>
    public Equation(List<MolChemical> _Reactants, string _Condition, List<MolChemical> _Products)
    {
        Reactants = _Reactants;
        Condition = _Condition;
        Products = _Products;
    }
}

/// <summary>
/// 用于JSON反序列化的化学反应类
/// </summary>
[Serializable] // 可序列化标记
public class ChemicalReaction
{
    /// <summary>反应物数据，每个元素是[摩尔数, 化学式]的列表</summary>
    [JsonProperty("反应物")] // 指定JSON属性映射
    public List<List<object>> Reactants { get; set; }

    /// <summary>反应条件字符串</summary>
    [JsonProperty("反应条件")] // JSON属性映射
    public string Condition { get; set; }

    /// <summary>生成物数据，每个元素是[摩尔数, 化学式]的列表</summary>
    [JsonProperty("生成物")] // JSON属性映射
    public List<List<object>> Products { get; set; }
}

/// <summary>
/// 静态类，负责加载和处理化学反应方程式
/// 提供多种查询方法用于检索化学反应
/// </summary>
public static class EquationLoader
{
    /// <summary>存储所有加载的方程式</summary>
    public static List<Equation> allEquations = new List<Equation>();

    /// <summary>方程式字典缓存，用于快速查找，键为反应物组合的字符串表示</summary>
    private static Dictionary<string, List<Equation>> reactionDict;

    /// <summary>
    /// 从JSON文件加载所有方程式
    /// 文件路径: Resources/Equation/equation.json
    /// </summary>
    public static void LoadEquations()
    {
        
        //allEquations.Clear();
        // 从Resources/Equation文件夹加载名为"equation"的JSON文件
        TextAsset jsonFile = Resources.Load<TextAsset>("Equations/equations");

        // 文件不存在检查
        if (jsonFile == null)
        {
            Debug.LogError("无法加载JSON文件");
            return;
        }

        try
        {
            string jsonString = jsonFile.text; // 获取JSON文本内容

            // 使用Newtonsoft.Json反序列化JSON数据
            var reactions = JsonConvert.DeserializeObject<List<ChemicalReaction>>(jsonString);

            // 使用LINQ转换和验证数据
            allEquations = reactions?.Select(reaction =>
            {
                // 数据完整性验证
                if (reaction.Reactants == null || reaction.Products == null)
                {
                    throw new InvalidDataException("反应物或生成物数据缺失");
                }

                // 转换反应物数据
                var reactants = reaction.Reactants.Select(r =>
                {
                    if (r.Count < 2) throw new InvalidDataException("反应物数据格式错误");
                    // 创建MolChemicals对象：化学式字符串转化学物质，并包含摩尔数
                    return new MolChemical(r[1].ToString(), Convert.ToInt32(r[0]));
                }).ToList();

                // 转换生成物数据
                var products = reaction.Products.Select(p =>
                {
                    if (p.Count < 2) throw new InvalidDataException("生成物数据格式错误");
                    // 创建MolChemicals对象
                    return new MolChemical(p[1].ToString(), Convert.ToInt32(p[0]));
                }).ToList();

                // 调试输出反应信息
                Debug.Log(
                    "加载反应方程式:" +
                    $"{string.Join(" + ", reactants.Select(r => $"{r.MolNum} {r.Chemical.Formula}"))}" +
                    $" ->({reaction.Condition}) " +
                    $"{string.Join(" + ", products.Select(p => $"{p.MolNum} {p.Chemical.Formula}"))}"
                );

                // 创建并返回新的Equation对象
                return new Equation(reactants, reaction.Condition, products);
            }).ToList() ?? new List<Equation>(); // 如果reactions为null则返回空列表

            // 重置字典缓存，下次查询时会重新构建
            reactionDict = null;
        }
        catch (Exception ex)
        {
            // 异常处理
            Debug.LogError($"加载方程式失败: {ex.Message}");
            allEquations = new List<Equation>(); // 出错时清空列表
        }
    }

    /// <summary>
    /// 构建反应字典缓存，用于加速反应物组合查询
    /// 字典键为标准化后的反应物组合字符串
    /// </summary>
    private static void BuildReactionDictionary()
    {
        reactionDict = new Dictionary<string, List<Equation>>();

        // 遍历所有方程式，构建字典索引
        foreach (var eq in allEquations)
        {
            // 生成反应物组合的标准化键
            var reactantsKey = GenerateReactantsKey(eq.Reactants);

            // 如果字典中不存在该键，则添加新条目
            if (!reactionDict.ContainsKey(reactantsKey))
            {
                reactionDict[reactantsKey] = new List<Equation>();
            }
            // 将方程式添加到对应键的列表中
            reactionDict[reactantsKey].Add(eq);
        }
    }

    /// <summary>
    /// 生成反应物组合的标准键
    /// 格式: "化学式1_摩尔数+化学式2_摩尔数..."
    /// 反应物按化学式字母顺序排序确保一致性
    /// </summary>
    /// <param name="reactants">反应物列表</param>
    /// <returns>标准化后的反应物组合字符串</returns>
    private static string GenerateReactantsKey(List<MolChemical> reactants)
    {
        // 按化学式排序后拼接生成唯一键
        return string.Join("+", reactants
            .OrderBy(r => r.Chemical.Formula)
            .Select(r => $"{r.Chemical.Formula}_{r.MolNum}"));
    }

    #region 查询方法
    /// <summary>
    /// 打印方程式列表，提供格式化输出
    /// </summary>
    /// <param name="equations">要打印的方程式列表</param>
    public static void PrintEquations(List<Equation> equations)
    {
        if (equations == null || equations.Count == 0)
        {
            Debug.Log($"没有找到匹配的方程式{equations == null}{equations.Count == 0}");
            return;
        }

        Debug.Log($"找到 {equations.Count} 个匹配的方程式:");

        for (int i = 0; i < equations.Count; i++)
        {
            Equation eq = equations[i];

            // 格式化反应物部分
            string reactantsStr = string.Join(" + ",
                eq.Reactants.Select(r => $"{r.MolNum} {r.Chemical.Formula}"));

            // 格式化生成物部分
            string productsStr = string.Join(" + ",
                eq.Products.Select(p => $"{p.MolNum} {p.Chemical.Formula}"));

            // 构建完整方程式字符串
            string equationStr = $"{i + 1}. {reactantsStr} →({eq.Condition}) {productsStr}";

            Debug.Log(equationStr);
        }
    }

    /// <summary>
    /// 打印单个化学反应方程式，提供格式化输出
    /// </summary>
    /// <param name="equation">要打印的方程式</param>
    public static void PrintEquations(Equation equation)
    {
        // 检查是否为默认空结构体
        if (equation.Equals(default(Equation)))
        {
            Debug.Log("方程式为空，无有效数据");
            return;
        }

        // 格式化反应物部分
        string reactantsStr = string.Join(" + ",
            equation.Reactants.Select(r => $"{r.MolNum}{r.Chemical.Formula}"));

        // 格式化生成物部分
        string productsStr = string.Join(" + ",
            equation.Products.Select(p => $"{p.MolNum}{p.Chemical.Formula}"));

        // 构建完整方程式字符串
        string equationStr = $"{reactantsStr} ->({equation.Condition}) {productsStr}";

        Debug.Log(equationStr);
    }
    /// <summary>
    /// 根据反应物查询可能的反应方程式
    /// </summary>
    /// <param name="reactants">反应物列表</param>
    /// <returns>匹配的方程式列表，若无匹配则返回空列表</returns>
    public static List<Equation> GetReactionsByReactants(List<MolChemical> reactants)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();
        // 确保字典已构建
        if (reactionDict == null) BuildReactionDictionary();

        // 生成查询键
        var key = GenerateReactantsKey(reactants);
        // 尝试从字典获取匹配的方程式
        return reactionDict.TryGetValue(key, out var equations)
            ? equations
            : new List<Equation>();
    }

    /// <summary>
    /// 根据生成物反查可能的反应方程式
    /// 查找所有生成物包含指定化学物质的反应
    /// </summary>
    /// <param name="products">生成物列表</param>
    /// <returns>匹配的方程式列表</returns>
    public static List<Equation> GetReactionsByProducts(List<MolChemical> products)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();

        // 使用LINQ查询所有生成物包含指定化学物质的反应
        return allEquations.Where(eq =>
            products.All(p =>
                eq.Products.Any(prod =>
                    prod.Chemical.Formula == p.Chemical.Formula &&
                    prod.MolNum >= p.MolNum)))
            .ToList();
    }

    /// <summary>
    /// 根据反应条件查询方程式
    /// 查找条件描述中包含指定字符串的反应(不区分大小写)
    /// </summary>
    /// <param name="condition">条件查询字符串</param>
    /// <returns>匹配的方程式列表</returns>
    public static List<Equation> GetReactionsByCondition(string condition)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();

        // 使用LINQ查询条件匹配的反应
        return allEquations
            .Where(eq => eq.Condition.Contains(condition, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// 模糊查询 - 根据部分反应物/生成物查询
    /// 查找反应物或生成物中包含指定字符串的化学式
    /// </summary>
    /// <param name="partialFormula">部分化学式字符串</param>
    /// <returns>匹配的方程式列表</returns>
    public static List<Equation> SearchReactions(string partialFormula)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();

        // 使用LINQ查询反应物或生成物中包含指定字符串的反应
        return allEquations.Where(eq =>
            eq.Reactants.Any(r => r.Chemical.Formula.Contains(partialFormula)) ||
            eq.Products.Any(p => p.Chemical.Formula.Contains(partialFormula)))
            .ToList();
    }

    /// <summary>
    /// 多条件组合查询
    /// 可以同时指定反应物、生成物和条件进行组合查询
    /// 所有参数都是可选的，未指定的条件将被忽略
    /// </summary>
    /// <param name="reactants">反应物列表(可选)</param>
    /// <param name="products">生成物列表(可选)</param>
    /// <param name="condition">条件字符串(可选)</param>
    /// <returns>匹配所有条件的方程式列表</returns>
    public static List<Equation> AdvancedSearch(
        List<MolChemical> reactants = null,
        List<MolChemical> products = null,
        string condition = null)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();

        // 初始查询为所有方程式
        var query = allEquations.AsEnumerable();

        // 如果指定了反应物条件
        if (reactants != null && reactants.Count > 0)
        {
            // 提取反应物化学式集合
            var reactantFormulas = reactants.Select(r => r.Chemical.Formula).ToHashSet();
            // 添加反应物数量匹配和化学式匹配条件
            query = query.Where(eq =>
                eq.Reactants.Count == reactants.Count &&
                eq.Reactants.All(r => reactantFormulas.Contains(r.Chemical.Formula)));
        }

        // 如果指定了生成物条件
        if (products != null && products.Count > 0)
        {
            // 提取生成物化学式集合
            var productFormulas = products.Select(p => p.Chemical.Formula).ToHashSet();
            // 添加生成物化学式匹配条件
            query = query.Where(eq =>
                eq.Products.Any(p => productFormulas.Contains(p.Chemical.Formula)));
        }

        // 如果指定了条件字符串
        if (!string.IsNullOrEmpty(condition))
        {
            // 添加条件描述匹配条件
            query = query.Where(eq => eq.Condition.Contains(condition));
        }

        // 执行查询并返回结果
        return query.ToList();
    }

    /// <summary>
    /// 严格搜索 - 精确匹配反应物和条件
    /// 查找反应物完全匹配且条件完全匹配的单一反应
    /// </summary>
    /// <param name="reactants">反应物列表</param>
    /// <param name="condition">反应条件</param>
    /// <returns>匹配的方程式，若无匹配则返回空结构体</returns>
    public static Equation StrictSearch(List<MolChemical> reactants, string condition)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();

        // 如果反应物为空或条件为空，返回默认值
        if (reactants == null || reactants.Count == 0 || string.IsNullOrEmpty(condition))
        {
            return default(Equation);
        }

        // 遍历所有方程式进行匹配检查
        foreach (var eq in allEquations)
        {
            // 首先检查条件是否匹配（不区分大小写）
            if (!string.Equals(eq.Condition, condition, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // 检查反应物数量是否匹配
            if (eq.Reactants.Count != reactants.Count)
            {
                continue;
            }

            // 检查所有反应物是否匹配（化学式和摩尔数）
            bool allReactantsMatch = true;
            foreach (var reqReactant in reactants)
            {
                bool found = false;
                foreach (var eqReactant in eq.Reactants)
                {
                    if (eqReactant.Chemical.Formula == reqReactant.Chemical.Formula &&
                        eqReactant.MolNum == reqReactant.MolNum)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    allReactantsMatch = false;
                    break;
                }
            }

            if (allReactantsMatch)
            {
                return eq;
            }
        }

        return default(Equation);
    }

    /// <summary>
    /// 获取所有涉及特定化学物质的反应
    /// 包括作为反应物或生成物的反应
    /// </summary>
    /// <param name="formula">化学式</param>
    /// <returns>匹配的方程式列表</returns>
    public static List<Equation> GetReactionsInvolvingChemical(string formula)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();

        // 查询反应物或生成物中包含指定化学式的反应
        return allEquations.Where(eq =>
            eq.Reactants.Any(r => r.Chemical.Formula == formula) ||
            eq.Products.Any(p => p.Chemical.Formula == formula))
            .ToList();
    }

    
    /// <summary>
    /// 检查两个化学物质是否能直接反应
    /// 查找是否存在以这两种物质为反应物的反应
    /// </summary>
    /// <param name="formula1">第一种化学式</param>
    /// <param name="formula2">第二种化学式</param>
    /// <returns>是否能反应</returns>
    public static bool CanReact(string formula1, string formula2)
    {
        // 确保数据已加载
        if (allEquations.Count == 0) LoadEquations();
        // 确保字典已构建
        if (reactionDict == null) BuildReactionDictionary();

        // 生成两种可能的组合键(顺序不同)
        var testCombinations = new[]
        {
            $"{formula1}_1+{formula2}_1",
            $"{formula2}_1+{formula1}_1"
        };

        // 检查字典中是否存在任一组合
        return testCombinations.Any(combo => reactionDict.ContainsKey(combo));
    }

    #endregion
}