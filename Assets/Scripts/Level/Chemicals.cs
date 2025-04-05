using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using CL = ChemicalLoader; // 化学加载器的别名

// 卡牌数据结构体，用于存储化学物质相关信息
public struct CardData
{
    public List<Chemical> Chemicals; // 卡牌包含的化学纯净物列表
    public List<int> CheCount;      // 每种化学纯净物对应的分子数
    public int Count;               // 卡牌总数量
    public string State;            // 物质状态（固态/液态/气态等）
    public string Form;             // 物质存在形式

    // 将CardData转换为MolChemicals列表的方法
    public List<MolChemicals> To_MolChemicals()
    {
        List<MolChemicals> re = new();
        for (int i = 0; i < Chemicals.Count; i++)
        {
            MolChemicals temp = new MolChemicals(Chemicals[i], CheCount[i]);
            re.Add(temp);
        }
        return re;
    }
}

// 化学物质类，继承自MonoBehaviour，用于处理卡牌的交互逻辑
public class Chemicals : MonoBehaviour
{
    // 公共字段
    public List<Chemical> ChemicalsInclude; // 当前卡牌包含的化学物质列表
    public TMP_Text FormulaText;           // 显示化学式的文本组件
    public TMP_Text CountText;             // 显示数量的文本组件
    public GameObject ReactionLayer;       // 反应池游戏对象
    public GameObject CommitLayer;         // 提交区游戏对象
    public GameObject Notice;              // 提示UI对象

    // 状态标志
    public bool following = true;          // 是否跟随鼠标移动
    public bool entering = false;          // 是否进入反应池区域
    public bool commiting = false;         // 是否进入提交区区域
    public int Count = 1;                 // 当前物质数量

    // 关联对象引用
    public GameObject ParentCard;          // 父卡牌对象
    public CardData ParentCardData;        // 父卡牌数据
    public GameObject CardPrefab;          // 卡牌预制体
    public GameObject Content;             // 卡牌容器（手牌区）
    public GameObject CommitContent;       // 提交内容容器（提交区）

    // 系统组件引用
    public CanvasScaler canvasScaler;     // 画布缩放控制器
    public ReactionPool reactionPool;     // 反应池控制器
    public CommitController commitPool;   // 提交区控制器
    public LevelBuilder Builder;          // 关卡构建器

    // 初始化方法
    void Start()
    {
        // 获取必要的组件和对象引用
        canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
        reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();
        commitPool = GameObject.Find("CommitArea").GetComponent<CommitController>();
        Content = GameObject.Find("CardContent");
        CommitContent = GameObject.Find("CommitContent");
        Builder = Camera.main.GetComponent<LevelBuilder>();

        // 构建并显示化学式文本
        FormulaText.text = "[";
        for (int i = 0; i < ChemicalsInclude.Count; i++)
        {
            FormulaText.text += ChemicalsInclude[i].Formula + "*1;";
        }
        FormulaText.text += "]";

        // 获取反应层和提交层对象
        ReactionLayer = GameObject.Find("Reaction");
        CommitLayer = GameObject.Find("CommitLayer");
    }

    // 每帧更新方法
    void Update()
    {
        // 检查是否进入反应池或提交区
        entering = CheckEntering(ReactionLayer);
        commiting = CheckEntering(CommitLayer);

        // 更新数量显示
        CountText.text = Count.ToString();

        // 跟随鼠标移动逻辑
        if (following)
        {
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = screenPos;
        }

        // 显示/隐藏提示UI
        if ((entering || commiting) && following)
        {
            Notice.SetActive(true);
        }
        else
        {
            Notice.SetActive(false);
        }
    }

    // 检测是否进入指定区域的方法
    public bool CheckEntering(GameObject touch)
    {
        // 获取目标区域和自身的RectTransform组件
        RectTransform rectReaction = touch.GetComponent<RectTransform>();
        RectTransform rectObj = GetComponent<RectTransform>();

        // 获取位置信息
        Vector2 posO = rectObj.position;
        Vector2 posR = rectReaction.position;
        posO = Camera.main.WorldToScreenPoint(posO);
        posR = Camera.main.WorldToScreenPoint(posR);

        // 计算屏幕适配比例
        float referenceResolutionWidth = canvasScaler.referenceResolution.x;
        float referenceResolutionHeight = canvasScaler.referenceResolution.y;
        float currentCanvasScale = Screen.width / referenceResolutionWidth;
        float widthInScreenPixels = rectReaction.rect.width * currentCanvasScale;
        float heightInScreenPixels = rectReaction.rect.height * currentCanvasScale;

        // 检测是否在目标区域内
        if (posO.x >= (posR.x - (widthInScreenPixels / 2)) &&
            posO.x <= (posR.x + (widthInScreenPixels / 2)))
        {
            if (posO.y >= (posR.y - (heightInScreenPixels / 2)) &&
                posO.y <= (posR.y + (heightInScreenPixels / 2)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // 点击事件处理方法
    public void Clicked()
    {
        // 确保反应池引用不为空
        if (reactionPool == null) reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();

        // 跟随鼠标状态下进入反应池的逻辑
        if (entering && following)
        {
            // 将物质数据添加到反应池
            reactionPool.AddData(ParentCardData.To_MolChemicals());

            // 检查反应池中是否已存在相同物质
            foreach (GameObject che in reactionPool.Chemicals)
            {
                if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, ChemicalsInclude))
                {
                    // 存在则增加数量并销毁当前对象
                    che.GetComponent<Chemicals>().Count++;
                    Destroy(gameObject);
                    return;
                }
            }

            // 不存在则添加到反应池
            if (following)
            {
                ReactionLayer.GetComponent<ReactionPool>().Chemicals.Add(gameObject);
                gameObject.transform.SetParent(ReactionLayer.transform);
                Notice.SetActive(false);
                following = false;
            }
        }
        // 跟随鼠标状态下进入提交区的逻辑
        else if (commiting && following)
        {
            // 检查提交区中是否已存在相同物质
            foreach (GameObject che in commitPool.CommitChemicals)
            {
                if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, ChemicalsInclude))
                {
                    // 存在则增加数量并销毁当前对象
                    che.GetComponent<Chemicals>().Count++;
                    Destroy(gameObject);
                    return;
                }
            }

            // 不存在则添加到提交区
            if (following)
            {
                commitPool.CommitChemicals.Add(gameObject);
                gameObject.transform.SetParent(CommitContent.transform);
                Notice.SetActive(false);
                following = false;
            }
        }
        // 非跟随状态下的点击逻辑（退回卡牌区）
        else if (!following)
        {
            // 从反应池移除数据
            reactionPool.ReduceData(ParentCardData.To_MolChemicals());

            bool found = false;
            // 遍历卡牌区，检查是否存在父卡牌
            foreach (GameObject che in Builder.Cards)
            {
                if (che.GetComponent<Card>().Chemicals == ChemicalsInclude)
                {
                    // 设其为父卡牌
                    ParentCard = che;
                    found = true;
                    break;
                }
            }

            // 找到父卡牌
            if (found)
            {
                // 存在则恢复卡牌数量
                ParentCard.GetComponent<Card>().Count += Count;
                ParentCard.GetComponent<Card>().ShowChemicalInformation();
                if (entering)
                    reactionPool.Chemicals.Remove(gameObject);
                else if (commiting)
                    commitPool.CommitChemicals.Remove(gameObject);
                Destroy(gameObject);
            }
            // 父卡牌不存在则创建新卡牌
            else
            {
                GameObject parentCard = Instantiate(CardPrefab, Content.transform);
                Builder.Cards.Add(parentCard);
                parentCard.GetComponent<Card>().Chemicals = ParentCardData.Chemicals;
                parentCard.GetComponent<Card>().CheCount = ParentCardData.CheCount;
                parentCard.GetComponent<Card>().State = ParentCardData.State;
                parentCard.GetComponent<Card>().Form = ParentCardData.Form;
                parentCard.GetComponent<Card>().Count = Count;

                parentCard.GetComponent<Card>().ShowChemicalInformation();

                if (entering)
                    reactionPool.Chemicals.Remove(gameObject);
                else if (commiting)
                    commitPool.CommitChemicals.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

}