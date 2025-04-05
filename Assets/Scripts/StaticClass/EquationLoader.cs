// 引入必要的命名空间
using Fungus; // 用于对话系统
using Newtonsoft.Json; // 用于JSON序列化和反序列化
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CL = ChemicalLoader; // 化学加载器的别名
using System.Linq; // LINQ扩展方法

// 定义方程式结构体，表示一个化学反应方程式
public struct Equation
{
    public List<MolChemicals> Reactants; // 反应物列表，包含化学物质和摩尔数
    public string Condition; // 反应条件描述
    public List<MolChemicals> Products; // 生成物列表，包含化学物质和摩尔数

    // 结构体构造函数
    public Equation(List<MolChemicals> _Reactants, string _Condition, List<MolChemicals> _Products)
    {
        Reactants = _Reactants;
        Condition = _Condition;
        Products = _Products;
    }
}

// 可序列化的化学反应类，用于JSON反序列化
[Serializable]
public class ChemicalReaction
{
    [JsonProperty("反应物")] // JSON属性映射
    public List<List<object>> Reactants { get; set; } // 反应物数据，每个元素是[摩尔数, 化学式]的列表

    [JsonProperty("反应条件")]
    public string Condition { get; set; } // 反应条件字符串

    [JsonProperty("生成物")]
    public List<List<object>> Products { get; set; } // 生成物数据，每个元素是[摩尔数, 化学式]的列表
}

// 静态类，负责加载和处理化学反应方程式
public static class EquationLoader
{
    public static List<Equation> allEquations = new List<Equation>(); // 存储所有加载的方程式

    // 从JSON文件加载所有方程式
    public static void LoadEquations()
    {
        // 从Resources文件夹加载名为"equation"的JSON文件
        TextAsset jsonFile = Resources.Load<TextAsset>("equation");

        // 检查文件是否加载成功
        if (jsonFile == null)
        {
            Debug.LogError("无法加载JSON文件");
            return;
        }

        allEquations.Clear(); // 清空现有方程式
        string jsonString = jsonFile.text; // 获取JSON文本内容

        // 使用Newtonsoft.Json反序列化JSON数据到ChemicalReaction列表
        List<ChemicalReaction> reactions = JsonConvert.DeserializeObject<List<ChemicalReaction>>(jsonString);

        // 使用LINQ将ChemicalReaction列表转换为Equation列表
        allEquations = reactions.Select(reaction =>
        {
            // 使用LINQ转换反应物列表：将每个反应物转换为MolChemicals对象
            var reactants = reaction.Reactants.Select(reactant =>
                new MolChemicals(reactant[1].ToString(), Convert.ToInt32(reactant[0])))
                .ToList();

            // 使用LINQ转换生成物列表：将每个生成物转换为MolChemicals对象
            var products = reaction.Products.Select(product =>
                new MolChemicals(product[1].ToString(), Convert.ToInt32(product[0])))
                .ToList();

            // 打印反应信息用于调试
            Debug.Log($"反应物: {string.Join(", ", reactants.Select(r => $"{r.MolNum} {r.Chemicals.Formula}"))}");
            Debug.Log($"反应条件: {reaction.Condition}");
            Debug.Log($"生成物: {string.Join(", ", products.Select(p => $"{p.MolNum} {p.Chemicals.Formula}"))}");
            Debug.Log("-------------------");

            // 创建并返回新的Equation对象
            return new Equation(reactants, reaction.Condition, products);
        }).ToList(); // 将结果转换为列表
    }
}