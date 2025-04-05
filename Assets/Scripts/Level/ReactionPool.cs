using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CL = ChemicalLoader; // 化学加载器的别名

// 反应池类，用于管理化学反应中的化学物质
public class ReactionPool : MonoBehaviour
{
    // 化学物质的GameObject列表（用于UI可视化）
    public List<GameObject> Chemicals;

    // 当前反应池中的化学物质及其摩尔数列表
    public static List<MolChemical> MolChemicalsInReactionPool = new();

    // 序列化字段，带有属性设置器，用于在Inspector中显示反应池内容
    [SerializeField, SetProperty("内容")]
    public string content; // 反应池内容的字符串表示

    // 每帧更新的Unity生命周期方法
    private void Update()
    {
        // 如果content与当前ToString()结果不一致，则更新content
        if (content != ToString()) content = ToString();
    }

    // 向反应池添加化学物质数据
    public void AddData(MolChemical data)
    {
        var c = data;
        
        // 检查反应池中是否已存在相同ID的化学物质
        for (int i = 0; i < MolChemicalsInReactionPool.Count; i++)
        {
            if (MolChemicalsInReactionPool[i].Chemical.ID == c.Chemical.ID)
            {
                // 如果存在，则合并摩尔数（原有摩尔数 + 新增摩尔数）
                MolChemicalsInReactionPool[i] = new MolChemical(
                    c.Chemical,
                    c.MolNum + MolChemicalsInReactionPool[i].MolNum
                );
                return; // 合并后立即返回
            }
        }

        // 如果不存在相同ID的化学物质，则直接添加到列表
        MolChemicalsInReactionPool.Add(c);
    }

    // 从反应池减少化学物质数据（完全移除指定ID的所有物质）
    public void ReduceData(MolChemical data)
    {
        var c = data;

        // 使用RemoveAll移除所有匹配ID的化学物质
        MolChemicalsInReactionPool.RemoveAll(n => (n.Chemical.ID == c.Chemical.ID));
    }

    // 重写ToString方法，返回反应池内容的字符串表示
    override public string ToString()
    {
        string s = "";
        // 拼接所有化学物质的字符串表示
        foreach (MolChemical t in MolChemicalsInReactionPool)
        {
            s += $"{t.ToString()},"; // 每个化学物质后加逗号
        }
        return s;
    }
    public static void Print()
    {
        string s = "";
        // 拼接所有化学物质的字符串表示
        foreach (MolChemical t in MolChemicalsInReactionPool)
        {
            s += $"{t.ToString()},"; // 每个化学物质后加逗号
        }
        Debug.Log(s);
    }
}