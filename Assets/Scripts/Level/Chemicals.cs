using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class Chemicals : MonoBehaviour
{
    public List<CDL.Chemical> ChemicalsInclude;

    public TMP_Text FormulaText;
    public TMP_Text CountText;
    public GameObject ReactionLayer;
    public GameObject Notice;

    public bool following = true;
    public bool entering = false;
    public bool parentExist = true;
    public int Count = 1; // ��������
    public GameObject ParentCard;

    public CanvasScaler canvasScaler;
    public CardPool cardPool;
    public ReactionPool reactionPool;

    void Start()
    {
        canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
        cardPool = GameObject.Find("CardArea").GetComponent<CardPool>();
        reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();

        FormulaText.text = "[";
        for (int i = 0; i < ChemicalsInclude.Count; i++)
        {
            FormulaText.text +=
                ChemicalsInclude[i].Formula + "*1;";
        }
        FormulaText.text += "]";

        ReactionLayer = GameObject.Find("Reaction");
    }

    void Update()
    {
        CheckEntering();
        CountText.text = Count.ToString();
        // �ö���������
        if (following)
        {
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = screenPos;
        }
        if (entering && following)
        {
            Notice.SetActive(true);
        }
        else
        {
            Notice.SetActive(false);
        }
    }

    // ����Ƿ���뷴Ӧ��
    public void CheckEntering()
    {
        RectTransform rectReaction = ReactionLayer.GetComponent<RectTransform>();
        RectTransform rectObj = GetComponent<RectTransform>();

        Vector2 posO = rectObj.position;
        Vector2 posR = rectReaction.position;
        posO = Camera.main.WorldToScreenPoint(posO);
        posR = Camera.main.WorldToScreenPoint(posR);

        float referenceResolutionWidth = canvasScaler.referenceResolution.x; // �ο��ֱ��ʵĿ��
        float referenceResolutionHeight = canvasScaler.referenceResolution.y; // �ο��ֱ��ʵĸ߶�
        float currentCanvasScale = Screen.width / referenceResolutionWidth; // ��ǰCanvas����ڲο��ֱ��ʵ����ű���
        float widthInScreenPixels = rectReaction.rect.width * currentCanvasScale; // �����ȶ�Ӧ����Ļ����ֵ
        float heightInScreenPixels = rectReaction.rect.height * currentCanvasScale; // ����߶ȶ�Ӧ����Ļ����ֵ

        // Debug.Log("Success");
        // Debug.Log(posO.x.ToString() + ", " + posO.y.ToString() + "; " + posR.x.ToString() + "," + posR.y.ToString());

        if (posO.x >= (posR.x - (widthInScreenPixels / 2)) &&
            posO.x <= (posR.x + (widthInScreenPixels / 2)))
        {
            if (posO.y >= (posR.y - (heightInScreenPixels / 2)) &&
                posO.y <= (posR.y + (heightInScreenPixels / 2)))
            {
                entering = true;
            }
            else
            {
                entering = false;
            }
        }
        else
        {
            entering = false;
        }
    }

    public void Clicked()
    {
        if (entering && following)
        {
            // �����ڷ�Ӧ�����Ƿ����
            foreach (GameObject che in reactionPool.Chemicals)
            {
                if (Equals(che.GetComponent<Chemicals>().ChemicalsInclude, ChemicalsInclude))
                {
                    Debug.Log("Found");
                    che.GetComponent<Chemicals>().Count++;
                    following = false;
                    Destroy(gameObject);

                    return;
                }
            }

            // ������
            ReactionLayer.GetComponent<ReactionPool>().Chemicals.Add(gameObject);
            gameObject.transform.SetParent(ReactionLayer.transform);
            Notice.SetActive(false);
            following = false;
        }

        if (!following)
        {

        }
    }
}
