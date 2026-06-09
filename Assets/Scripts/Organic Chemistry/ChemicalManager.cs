using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 表示一个原子
public class Atom
{
    public enum BondType
    {
        SINGLE = 0,         // 单键
        DOUBLE,             // 双键
        TRIPLE,             // 三键
        DELOCALIZED_PI      // 离域化π键（如苯环中的键）
    };

    public string Element;                      // 元素符号，如 "C"、"H"、"O" 等
    public List<Tuple<int, BondType>> Bonds = new List<Tuple<int, BondType>>();    // 与该原子相连的其他原子编号
}

// 表示一个有机物
// 因为 C# 9.0 不能在 struct 中使用构造函数，改成了 class
[Serializable]
public class OrganicMatter
{
    /// <summary>
    /// 一个图，表示它的结构，这应该是一个无向图
    /// </summary>
    public List<Atom> Structure;

    /// <summary>
    /// 原子数量统计，按 C H Br Cl F N O P S I 顺序
    /// </summary>
    public int[] AtomCount;
    public Dictionary<string, int> Atom2Num;
    public Dictionary<int, string> Num2Atom;

    public string MolecularFormula;

    /// <summary>
    /// 无参构造函数，初始化结构和原子数量统计
    /// </summary>
    public OrganicMatter()
    {
        Structure = new List<Atom>();
        AtomCount = new int[10];
        MolecularFormula = "";
        // 初始化原子对应字典
        Atom2Num = new Dictionary<string, int>();
        Atom2Num.TryAdd("C", 0);
        Atom2Num.TryAdd("H", 1);
        Atom2Num.TryAdd("Br", 2);
        Atom2Num.TryAdd("Cl", 3);
        Atom2Num.TryAdd("F", 4);
        Atom2Num.TryAdd("N", 5);
        Atom2Num.TryAdd("O", 6);
        Atom2Num.TryAdd("P", 7);
        Atom2Num.TryAdd("S", 8);
        Atom2Num.TryAdd("I", 9);
        Num2Atom = Atom2Num.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }

    /// <summary>
    /// 只添加一个原子，不指定它与哪个原子相连
    /// </summary>
    /// <param name="element">元素符号</param>
    public void AddAtom(string element)
    {
        Atom atom = new Atom();
        atom.Element = element;
        Structure.Add(atom);
    }

    /// <summary>
    /// 添加一个原子，并指定它与哪个原子相连，以及键的类型
    /// </summary>
    /// <param name="element">元素符号</param>
    /// <param name="bondType">连键类型</param>
    /// <param name="connect">连接原子</param>
    public void AddAtom(string element, Atom.BondType bondType, int connect)
    {
        Atom atom = new Atom();
        atom.Element = element;

        // 无向图
        atom.Bonds.Add(new Tuple<int, Atom.BondType>(connect, bondType));
        Structure.Add(atom);
        Structure[connect].Bonds.Add(new Tuple<int, Atom.BondType>(Structure.Count - 1, bondType));
    }

    /// <summary>
    /// 计算分子式，统计每种元素的数量，并生成分子式字符串
    /// </summary>
    public void CalculateMolecularFormula()
    {
        // 遍历结构中的原子
        foreach (var atom in Structure)
        {
            // 统计每种元素的数量
            AtomCount[Atom2Num[atom.Element]]++;
        }

        // 生成分子式字符串
        MolecularFormula = "";
        for (int i = 0; i < AtomCount.Length; i++)
        {
            if (AtomCount[i] > 0)
            {
                MolecularFormula += Num2Atom[i];
                if (AtomCount[i] > 1)
                {
                    MolecularFormula += $"<sub>{AtomCount[i]}</sub>";
                }
            }
        }
    }
}

public class ChemicalManager : MonoBehaviour
{
    /// <summary>
    /// 管理场景中所有的有机物
    /// </summary>
    public List<OrganicMatter> Matters = new List<OrganicMatter>();

    // Start is called before the first frame update
    void Start()
    {
        Matters.Add(new OrganicMatter());
        // 添加一个甲烷分子
        Matters[0].AddAtom("C");
        Matters[0].AddAtom("H", Atom.BondType.SINGLE, 0);
        Matters[0].AddAtom("H", Atom.BondType.SINGLE, 0);
        Matters[0].AddAtom("H", Atom.BondType.SINGLE, 0);
        Matters[0].AddAtom("Cl", Atom.BondType.SINGLE, 0);
        Matters[0].CalculateMolecularFormula();

        Debug.Log("成功添加，分子式为：" + Matters[0].MolecularFormula);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 在 Matters[] 里添加一个有机物
    /// </summary>
    /// <param name="matter"></param>
    void AddMatter(OrganicMatter matter)
    {
        // 先检查有机物是否合法

        Matters.Add(matter);
    }
}
