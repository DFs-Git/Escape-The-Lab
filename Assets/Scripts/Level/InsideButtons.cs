using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InsideButtons : MonoBehaviour
{
    public LevelLoader Loader;
    public ChatController chatController;
    public ChatBuilder chatBuilder;
    public Mask mask;

    public TMP_Text TipsText;
    public int TipsGotten;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    // 获取提示
    public void GetTips()
    {
        if (TipsGotten < Loader.level.tips.Count)
        {
            if (TipsGotten == 0) TipsText.text = "提示：";
            TipsText.text += '\n';
            TipsText.text += (TipsGotten + 1).ToString() + ". ";
            TipsText.text += Loader.level.tips[TipsGotten];
            TipsGotten++;
            if (TipsGotten == Loader.level.tips.Count)
            {
                TipsText.text += '\n' + "<color=grey>>>>由[System]倾情提供。</color>";
                // 如果提示全部获取完毕，禁用按钮
                GetComponent<Button>().interactable = false;
            }
        }
    }

    public void Back()
    {
        StartCoroutine(mask.MaskFadeIn("ChooseLevel"));
    }

    public void ShowDialogs()
    {
        GetComponent<Button>().interactable = false;
        // 执行对话
        chatBuilder = GameObject.Find("ChatBuilder").GetComponent<ChatBuilder>();
        chatController = GameObject.Find("ChatController").GetComponent<ChatController>();
        // 先加载当前关卡的对话
        chatController.LoadChat("dialog" + Loader.level.chapter.ToString() + "-" + Loader.level.topic.ToString());
        chatBuilder.BuilderShowDialog(() => { });
        GetComponent<Button>().interactable = true;
    }

    public void ShowTutorial()
    {
        GetComponent<Button>().interactable = false;
        // 执行对话
        chatBuilder = GameObject.Find("ChatBuilder").GetComponent<ChatBuilder>();
        chatController = GameObject.Find("ChatController").GetComponent<ChatController>();
        // 先加载教程的对话
        chatController.LoadChat("tutorial");
        chatBuilder.BuilderShowDialog(() => { });
        GetComponent<Button>().interactable = true;
    }
}
