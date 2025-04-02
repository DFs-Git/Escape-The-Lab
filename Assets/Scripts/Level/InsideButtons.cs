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

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    // ��ȡ��ʾ
    public void GetTips()
    {
        if (TipsGotten < Loader.level.tips.Count)
        {
            if (TipsGotten == 0) TipsText.text = "��ʾ��";
            TipsText.text += '\n';
            TipsText.text += (TipsGotten + 1).ToString() + ". ";
            TipsText.text += Loader.level.tips[TipsGotten];
            TipsGotten++;
            if (TipsGotten == Loader.level.tips.Count) TipsText.text += '\n' + "û�и�����ʾ�ˡ�";
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(3);
    }
}
