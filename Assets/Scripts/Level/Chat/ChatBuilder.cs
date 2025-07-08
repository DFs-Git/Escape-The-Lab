using Fungus;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ChatBuilder : MonoBehaviour
{
    public static ChatBuilder Instance;

    public ChatController Controller;
    public GameObject Canvas;

    public GameObject NormalDialog;
    public GameObject SpecialDialog;
    public GameObject ChoiceButton;

    public GameObject SkipButton;
    public GameObject ButtonInstance;

    public Transform Choice;

    public Mask mask;
    public LevelLoader Loader;
    public ChoiceCollector Collector;

    public string CG_Path;

    public bool dialogStopped = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保该对象在切换场景时不会被销毁
        }
        else
        {
            Destroy(gameObject); // 销毁重复的实例
        }
    }
    
    void Start()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        mask = GameObject.Find("Mask").GetComponent<Mask>();
        Controller = GameObject.Find("ChatController").GetComponent<ChatController>();

        StartCoroutine(StartDialog(() => { }));
    }

    void Update()
    {
        // 确保该实例只在 Level.unity 或 CG.unity 出现
        if (SceneManager.GetActiveScene().name != "CG" &&
            SceneManager.GetActiveScene().name != "Level")
            Destroy(gameObject);
    }

    // 让 ChatBuilder 开启协程，防止开启协程的对象不确定被销毁而导致协程意外终止
    public void BuilderShowDialog(Action action_after)
    {
        StartCoroutine(StartDialog(action_after));
    }

    // 参数 action_after 是对话结束后执行的回调函数
    // 如果没有想要执行的回调函数，可以传入一个空的 Action
    // 你甚至可以用这个把几个对话串联起来……
    public IEnumerator StartDialog(Action action_after)
    {
        // 索引指针，记录当前进行中的对话
        // 如果 index 为 -1，说明本次对话结束
        int index = 0;
        List<string> single = new List<string>();

        if (Controller.dialog.dialogs.Count == 0)
        {
            yield return null;
        }

        if (mask == null)
            mask = GameObject.Find("Mask").GetComponent<Mask>();
        if (Canvas == null)
            Canvas = GameObject.Find("Canvas");
        ButtonInstance = Instantiate(SkipButton, Canvas.transform); // 生成跳过按钮

        do
        {
            if (Collector == null)
                Collector = GameObject.Find("Choice").GetComponent<ChoiceCollector>();
            if (Choice == null)
                Choice = GameObject.Find("Choice").transform;
            if (Canvas == null)
                Canvas = GameObject.Find("Canvas");
            if (mask == null)
                mask = GameObject.Find("Mask").GetComponent<Mask>();

            if (ButtonInstance == null)
                ButtonInstance = Instantiate(SkipButton, Canvas.transform); // 重新生成跳过按钮

            // 确保对话时不能操作
            mask.image.raycastTarget = true;
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

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || dialogStopped);

                // 销毁对话框
                Destroy(dia);
            }

            // 冥想(背景变暗)
            if (single[0] == "1")
            {
                if (index == 0)
                    yield return new WaitUntil(() => { return mask.image.color.a <= 0.0F; });
                StartCoroutine(mask.MaskFadeIn(0.8F));

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
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || dialogStopped);
                if (dialogStopped) break;

                // 销毁对话框
                Destroy(dia);

                if (index == -1)
                    StartCoroutine(mask.MaskFadeOut());
                else if (Controller.dialog.dialogs[index][0] == "0")
                    StartCoroutine(mask.MaskFadeOut());
            }

            // 选择
            if (single[0] == "2")
            {
                if (index == 0)
                    yield return new WaitUntil(() => { return mask.image.color.a <= 0.0F; });
                StartCoroutine(mask.MaskFadeIn(0.8F));

                // 获取选项个数
                int count = StringToInt(single[1]);

                // 选项按钮的列表，方便后续处理
                List<GameObject> allBtn = new List<GameObject>();

                // 遍历 json 中对每一个选项的描述
                for (int i = 2; i < single.Count; i += 2)
                {
                    string content = single[i];                             // 选项内容
                    int jump = StringToInt(single[i + 1]);                  // 点击选项后跳转到的对话
                    if (jump == StringToInt("!")) jump = -2;                // 这里改成 -2 防止和没有点击选项的情况冲突

                    GameObject btn = Instantiate(ChoiceButton, Choice);     // 生成选项按钮
                    btn.GetComponent<ChatButton>().jump = jump;             // 设置选项跳转
                    btn.GetComponentInChildren<TMP_Text>().text = content;  // 设置选项文本
                    allBtn.Add(btn);                                        // 加入按钮至列表
                }
                
                // 没有点击选项就等待
                yield return new WaitUntil(() => Collector.ChoiceJump != -1 || dialogStopped);

                if (dialogStopped) break;

                // 将指针(index)跳转到应该跳转的索引
                index = Collector.ChoiceJump;
                Collector.ChoiceJump = -1;

                // 销毁所有按钮
                foreach (GameObject btn in allBtn)
                {
                    Destroy(btn);
                }

                if (index == -2)
                {
                    // 终止对话
                    StartCoroutine(mask.MaskFadeOut());
                    break;
                }
                else if (Controller.dialog.dialogs[index][0] == "0")
                    StartCoroutine(mask.MaskFadeOut());
            }

            // 播放 CG
            if (single[0] == "3")
            {
                if (index == 0)
                    yield return new WaitUntil(() => { return mask.image.color.a <= 0.0F; });

                CG_Path = single[1];
                if (single[2] == "!") index = -1;
                else index = StringToInt(single[2]);
                // 切换场景到 CG.unity
                StartCoroutine(mask.MaskFadeIn("CG"));

                // 等待 CG 播放完成
                // 等待切换到场景 CG.unity，再等待播放完成
                yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "CG");
                
                // 调低背景音乐音量
                GameObject audioManager = GameObject.Find("AudioManager");
                audioManager.GetComponent<AudioSource>().volume = 0f;
                CGPlay player = GameObject.Find("Video Player").GetComponent<CGPlay>();
                yield return new WaitUntil(() => player.VideoCompleted);                    // 等待播放完成

                // 音量恢复正常
                audioManager.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                mask = GameObject.Find("Mask").GetComponent<Mask>();    // 获取 CG.unity 的 Mask
                StartCoroutine(mask.MaskFadeIn("Level"));               // 返回 Level.unity
                yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level");
            }

            if (dialogStopped)
            {
                break;
            }
        } while (index != -1);

        if (Collector == null)
            Collector = GameObject.Find("Choice").GetComponent<ChoiceCollector>();
        if (Choice == null)
            Choice = GameObject.Find("Choice").transform;
        if (Canvas == null)
            Canvas = GameObject.Find("Canvas");
        if (mask == null)
            mask = GameObject.Find("Mask").GetComponent<Mask>();

        // 删除 Mask 的所有子物体
        foreach (Transform child in mask.transform)
        {
            Destroy(child.gameObject);
        }
        // 删除 Choice 的所有子物体
        foreach (Transform child in Choice)
        {
            Destroy(child.gameObject);
        }
        // 删除 NormalDialogue
        foreach (Transform child in Canvas.transform)
        {
            if (child.name == "NormalDialogue(Clone)" || child.name == "SpecialDialogue(Clone)")
            {
                Destroy(child.gameObject);
            }
        }

        // 解除操作权限
        mask.image.raycastTarget = false;
        // mask.fadingIn = false;
        StartCoroutine(mask.MaskFadeOut());

        if (!dialogStopped)
            // 执行对话结束后的回调函数
            action_after();

        dialogStopped = false;
    }

    public void EndDialog()
    {
        dialogStopped = true;                       // 标记对话已停止

        // 删除跳过按钮
        if (ButtonInstance != null)
            Destroy(ButtonInstance);
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
