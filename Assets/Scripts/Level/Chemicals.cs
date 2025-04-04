using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public struct CardData
{
    public List<CDL.Chemical> Chemicals;// 每一种纯净物
    public List<int> CheCount;          // 每一种纯净物的分子数
    public int Count;                   // 卡牌数量
    public string State;                // 物质状态
    public string Form;                 // 物质存在形式
}

public class Chemicals : MonoBehaviour
{
    public List<CDL.Chemical> ChemicalsInclude;

    public TMP_Text FormulaText;
    public TMP_Text CountText;
    public GameObject ReactionLayer;
    public GameObject CommitLayer;
    public GameObject Notice;

    public bool following = true;
    public bool entering = false;
    public bool commiting = false;
    public int Count = 1; // 物质数量
    public GameObject ParentCard;
    public CardData ParentCardData;
    public GameObject CardPrefab; // 物质对应的卡牌预制体
    public GameObject Content;      // 这里指卡牌
    public GameObject CommitContent;// 这里指提交

    public CanvasScaler canvasScaler;
    public ReactionPool reactionPool;
    public CommitController commitPool;
    public LevelBuilder Builder;

    void Start()
    {
        canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
        reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();
        commitPool = GameObject.Find("CommitArea").GetComponent<CommitController>();
        Content = GameObject.Find("CardContent");
        CommitContent = GameObject.Find("CommitContent");
        Builder = Camera.main.GetComponent<LevelBuilder>();

        FormulaText.text = "[";
        for (int i = 0; i < ChemicalsInclude.Count; i++)
        {
            FormulaText.text +=
                ChemicalsInclude[i].Formula + "*1;";
        }
        FormulaText.text += "]";

        ReactionLayer = GameObject.Find("Reaction");
        CommitLayer = GameObject.Find("CommitLayer");
    }

    void Update()
    {
        entering = CheckEntering(ReactionLayer);
        commiting = CheckEntering(CommitLayer);
        CountText.text = Count.ToString();
        // 让对象跟随鼠标
        if (following)
        {
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = screenPos;
        }
        if ((entering || commiting) && following)
        {
            Notice.SetActive(true);
        }
        else
        {
            Notice.SetActive(false);
        }
    }

    // 检测是否进入某层
    public bool CheckEntering(GameObject touch)
    {
        RectTransform rectReaction = touch.GetComponent<RectTransform>();
        RectTransform rectObj = GetComponent<RectTransform>();

        Vector2 posO = rectObj.position;
        Vector2 posR = rectReaction.position;
        posO = Camera.main.WorldToScreenPoint(posO);
        posR = Camera.main.WorldToScreenPoint(posR);

        float referenceResolutionWidth = canvasScaler.referenceResolution.x; // 参考分辨率的宽度
        float referenceResolutionHeight = canvasScaler.referenceResolution.y; // 参考分辨率的高度
        float currentCanvasScale = Screen.width / referenceResolutionWidth; // 当前Canvas相对于参考分辨率的缩放比例
        float widthInScreenPixels = rectReaction.rect.width * currentCanvasScale; // 计算宽度对应的屏幕像素值
        float heightInScreenPixels = rectReaction.rect.height * currentCanvasScale; // 计算高度对应的屏幕像素值

        // Debug.Log("Success");
        // Debug.Log(posO.x.ToString() + ", " + posO.y.ToString() + "; " + posR.x.ToString() + "," + posR.y.ToString());

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

    public void Clicked()
    {
        // 仍在跟随鼠标
        if (entering && following)
        {
            // 物质在反应池中是否存在
            foreach (GameObject che in reactionPool.Chemicals)
            {
                if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, ChemicalsInclude))
                {
                    // 只增加数量
                    che.GetComponent<Chemicals>().Count++;
                    Destroy(gameObject);
                    return;
                }
            }

            if (following)
            {
                // 不存在
                ReactionLayer.GetComponent<ReactionPool>().Chemicals.Add(gameObject);
                gameObject.transform.SetParent(ReactionLayer.transform);
                Notice.SetActive(false);
                following = false;
            }
        }
        // 在提交区中
        else if (commiting && following)
        {
            // 物质在提交池中是否存在
            foreach (GameObject che in commitPool.CommitChemicals)
            {
                if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, ChemicalsInclude))
                {
                    // 只增加数量
                    che.GetComponent<Chemicals>().Count++;
                    Destroy(gameObject);
                    return;
                }
            }

            if (following)
            {
                // 不存在
                commitPool.CommitChemicals.Add(gameObject);
                gameObject.transform.SetParent(CommitContent.transform);
                Notice.SetActive(false);
                following = false;
            }
        }

        // 点击将其退回卡牌区
        else if (!following)
        {
            // 其对应的卡牌仍在手牌区中存在
            if (ParentCard != null)
            {
                ParentCard.GetComponent<Card>().Count += Count;
                ParentCard.GetComponent<Card>().ShowChemicalInformation();
                if (entering)
                    reactionPool.Chemicals.Remove(gameObject);
                else if (commiting)
                    commitPool.CommitChemicals.Remove(gameObject);
                Destroy(gameObject);
            }

            // 其对应的卡牌已经被销毁
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
