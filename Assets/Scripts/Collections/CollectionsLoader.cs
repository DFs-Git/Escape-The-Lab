using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader; // 使用别名简化类名引用
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 化学物质集合界面加载器，负责动态生成化学物质按钮并显示详细信息
/// </summary>
public class CollectionsLoader : MonoBehaviour
{
    [Header("UI配置")]
    [SerializeField] private Button itemPrefab;        // 按钮预制体，用于生成每个化学物质项
    [SerializeField] private TextMeshProUGUI nameDisplay;     // 化学物质名称显示区域
    [SerializeField] private TextMeshProUGUI formulaDisplay;  // 化学式显示区域
    [SerializeField] private TextMeshProUGUI contentDisplay;  // 详细信息显示区域

    private void Awake()
    {
        // 确保在场景加载时初始化化学数据库
        // 避免在数据未加载时进行后续操作
        if (CDL.allChemicals.Count == 0)
        {
            CDL.LoadChemicals();
        }
    }

    void Start()
    {
        if (nameDisplay == null || formulaDisplay == null || contentDisplay == null) {
            Debug.Log("组件为空");
        }
        // 清空现有子对象，防止重复生成
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(GenerateButtons());

        // 默认展示第一个化学物质信息，提供初始界面状态
        if (CDL.allChemicals.Count > 0)
        {
            OnButtonClick(CDL.allChemicals[0].ID);
        }
    }

    /// <summary>
    /// 协程生成化学物质按钮，每生成5个等待一帧优化性能
    /// </summary>
    IEnumerator GenerateButtons()
    {
        foreach (Chemical che in CDL.allChemicals)
        {
            // 实例化按钮并设置显示文本
            Button btn = Instantiate(itemPrefab, transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = che.Name;

            // 使用局部变量避免闭包问题
            int currentID = che.ID;
            btn.onClick.AddListener(() => OnButtonClick(currentID));

            // 分帧处理优化性能：每生成5个按钮等待一帧
            if (CDL.allChemicals.IndexOf(che) % 5 == 0)
                yield return null;
        }
    }

    /// <summary>
    /// 按钮点击事件处理，更新右侧详细信息显示
    /// </summary>
    /// <param name="id">化学物质唯一标识符</param>
    void OnButtonClick(int id)
    {
        List<Chemical> result = CDL.FindChemicals(id);

        // 安全检查搜索结果有效性
        if (result != null && result.Count > 0)
        {
            Chemical che = result[0];
            // 格式化显示化学物质信息
            nameDisplay.text = $"{che.Name} {che.Category}";
            formulaDisplay.text = che.Formula;
            contentDisplay.text = $"物理性质：{che.PhysicalProperty}\n化学性质：{che.ChemicalProperty}";
        }
        else
        {
            Debug.LogWarning($"未找到ID为{id}的化学物质");
            // 可以在此处添加UI错误提示，例如清空显示区域或显示错误信息
        }
    }
}