using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChemicalDatabaseLoader;
using static ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using Unity.VisualScripting;
using Button = UnityEngine.UI.Button;
using TMPro;

/// <summary>
/// 化学物品集合加载器：动态生成化学物品按钮并显示详细信息
/// </summary>
public class CollectionsLoader : MonoBehaviour
{
    [Header("UI配置")]
    public Button Item; // 按钮预制体（用于动态生成）

    void Start()
    {
        // 初始化UI组件引用 
        // 获取显示化学物品详情的三个文本组件
        TextMeshProUGUI Name = GameObject.Find("Shower/Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Formula = GameObject.Find("Shower/Formula").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Content = GameObject.Find("Shower/Content").GetComponent<TextMeshProUGUI>();

        // 加载化学数据
        CDL.allChemicals.Clear();    // 清空现有数据缓存
        CDL.LoadChemicals();         // 从数据库加载数据
        List<Chemical> allChemicals = CDL.allChemicals; // 获取全部化学品列表

        // 按钮点击事件处理（嵌套方法）
        void OnButtonClick(int ID)
        {
            // 根据ID查找对应化学品（假设总能找到）
            Chemical Che = CDL.FindChemicals(ID)[0];

            // 更新UI显示内容
            Name.text = $"{Che.Name} {Che.Category}";       // 名称 + 类别
            Formula.text = Che.Formula;                     // 化学式
            Content.text = $"物理性质：{Che.PhysicaProperty}\n化学性质：{Che.ChemicalProperty}"; // 物化性质
        }

        // 动态生成按钮逻辑
        Transform[] transforms = GetComponentsInChildren<Transform>(); // 检测当前子物体

        // 仅当没有子物体时生成按钮（避免重复创建）!!!!!!!!!我也不知道他为什么会运行两次，只能这样了
        if (transforms.Length <= 1) // 注意：transform自身会被计入数量
        {
            foreach (Chemical Che in allChemicals)
            {
                // 实例化按钮并设置到当前物体下
                Button btn = (Button)Instantiate(Item, this.transform);
                btn.transform.SetParent(this.transform, false); // 设置父物体并重置位置

                // 设置按钮显示文本
                btn.GetComponentInChildren<TextMeshProUGUI>().text = Che.Name;

                // 闭包问题处理：将ID复制到局部变量（关键！勿删）!!!!!!!!!!!!!
                int ID = Che.ID;
                // 绑定点击事件：点击时调用OnButtonClick并传入ID
                btn.onClick.AddListener(() => OnButtonClick(ID));
            }
        }

        // 默认显示第一个化学品的信息
        OnButtonClick(1);
    }
}