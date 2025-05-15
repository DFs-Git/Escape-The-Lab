using Fungus;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBuilder : MonoBehaviour
{
    public ChatController Controller;
    public GameObject Canvas;

    public GameObject NormalDialog;
    public GameObject SpecialDialog;
    public GameObject ChoiceButton;
    public GameObject SkipButton;

    public Transform Choice;

    public Mask mask;
    public ChoiceCollector Collector;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    void Start()
    {
        Instantiate(SkipButton, mask.transform);
        StartCoroutine(StartDialog());
    }


    public IEnumerator StartDialog()
    {
        // 索引指针，记录当前进行中的对话
        // 如果 index 为 -1，说明本次对话结束
        int index = 0;
        List<string> single = new List<string>();

        // 确保对话时不能进行游戏操作
        mask.image.raycastTarget = true;

        do
        {
            single = Controller.dialog.dialogs[index];

            // 普通对话
            if (single[0] == "0")
            {
                // 获取对话内容
                string speaker = single[1];
                string content = single[2];

                // 生成对话框
                GameObject dia = Instantiate(NormalDialog, Canvas.transform);
                GameObject spk = dia.transform.Find("Speaker").gameObject;
                TMP_Text cont = dia.transform.Find("Content").gameObject.GetComponent<TMP_Text>();

                spk.GetComponentInChildren<TMP_Text>().text = speaker;

                // 输出对话内容
                cont.text = "";
                for (int i = 0; i < content.Length; i++)
                {
                    cont.text += content[i];
                    yield return new WaitForSeconds(0.02F);
                }

                // 准备下一次对话
                if (single[3] == "!") index = -1;
                else index = StringToInt(single[3]);
                Debug.Log("Next " + StringToInt(single[3]).ToString());

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                // 销毁对话框
                Destroy(dia);
            }

            // 自己对话(背景变暗)
            if (single[0] == "1")
            {
                StartCoroutine(mask.MaskFadeIn(0.7F));

                // 获取对话内容
                string content = single[1];

                // 生成对话框
                GameObject dia = Instantiate(SpecialDialog, mask.transform);
                TMP_Text cont = dia.GetComponent<TMP_Text>();

                // 输出对话内容
                cont.text = "";
                for (int i = 0; i < content.Length; i++)
                {
                    cont.text += content[i];
                    yield return new WaitForSeconds(0.02F);
                }

                // 准备下一次对话
                if (single[2] == "!") index = -1;
                else index = StringToInt(single[2]);
                Debug.Log("Next " + StringToInt(single[2]).ToString());

                // 没有按下回车键就一直等待
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                // 销毁对话框
                Destroy(dia);
                StartCoroutine(mask.MaskFadeOut());
            }

            // 选择
            if (single[0] == "2")
            {
                // 启用遮罩使背景变暗
                StartCoroutine(mask.MaskFadeIn(0.7F));

                // 获取选项个数
                int count = StringToInt(single[1]);

                // 选项按钮的列表，方便后续处理
                List<GameObject> allBtn = new List<GameObject>();

                // 遍历 json 中对每一个选项的描述
                for (int i = 2; i < single.Count; i += 2)
                {
                    string content = single[i];                             // 选项内容
                    int jump = StringToInt(single[i + 1]);                  // 点击选项后跳转到的对话

                    GameObject btn = Instantiate(ChoiceButton, Choice);     // 生成选项按钮
                    btn.GetComponent<ChatButton>().jump = jump;             // 设置选项跳转
                    btn.GetComponentInChildren<TMP_Text>().text = content;  // 设置选项文本
                    allBtn.Add(btn);                                        // 加入按钮至列表
                }

                // 没有点击选项就等待
                yield return new WaitUntil(() => Collector.ChoiceJump != -1);

                // 将指针(index)跳转到应该跳转的索引
                index = Collector.ChoiceJump;
                Collector.ChoiceJump = -1;

                // 销毁所有按钮
                foreach (GameObject btn in allBtn)
                {
                    Destroy(btn);
                }
                StartCoroutine(mask.MaskFadeOut());
            }
        } while (index != -1);

        // 解除操作权限
        mask.image.raycastTarget = false;
    }

    // 实现字符串转数字(未判特殊情况)
    public int StringToInt(string s)
    {
        int res = 0;
        for (int i = 0; i < s.Length; i++)
        {
            res *= 10;
            res += s[i] - '0';
        }

        return res;
    }
}
