using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideButtons : MonoBehaviour
{
    public LevelLoader Loader;

    public TMP_Text TipsText;
    public int TipsGotten;

    public Flowchart flowchart;

    void Awake()
    {
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
        SceneManager.LoadScene(3);
    }

    public void ShowDialogs()
    {
        // 执行对话
        flowchart.ExecuteBlock(Loader.level.chapter.ToString() + "-" + Loader.level.topic.ToString());
    }
}
