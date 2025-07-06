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
            if (TipsGotten == Loader.level.tips.Count) TipsText.text += '\n' + "没有更多提示了。";
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
}
