using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelButtons : MonoBehaviour
{
    public List<GameObject> AllChapters;
    public List<GameObject> AllChaptersButtons;

    public LevelLoader Loader;
    public GameObject Attention;

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void Start()
    {
        Attention.SetActive(false);

        // 确保是关卡按钮，调整关卡按钮颜色
        if (gameObject.tag == "levelbtn")
        {
            // 获取当前按钮对应的关卡
            string levelIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
            int chapter = (int)levelIndex[0] - '0';
            int topic = (int)levelIndex[2] - '0';

            UnityEngine.UI.Image img = gameObject.GetComponent<UnityEngine.UI.Image>();

            // 获取玩家进度信息
            int completedChapter = PlayerPrefs.GetInt("chapter");
            int completedTopic = PlayerPrefs.GetInt("topic");

            // 已完成的关卡
            if (completedChapter > chapter ||
                (completedChapter == chapter && completedTopic > topic))
            {
                // 调整为绿色
                img.color = new Color(0F, 255F, 0F);
            }

            // 当前关卡
            else if (completedChapter == chapter && completedTopic == topic)
            {
                // 调整为黄色
                img.color = new Color(255F, 180F, 0F);
            }

            // 未解锁关卡
            else if (completedChapter < chapter ||
                (completedChapter == chapter && completedTopic < topic))
            {
                // 调整为红色
                img.color = new Color(255F, 0F, 0F);
            }
        }
    }

    // 返回主菜单
    public void BackEvent()
    {
        SceneManager.LoadScene(0);
    }

    // 进入某个关卡
    public void EnterLevel()
    {
        // 检查按钮
        string levelIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
        Debug.Log(levelIndex);
        int chapter = (int)levelIndex[0] - '0';
        Debug.Log(chapter);
        int topic = (int)levelIndex[2] - '0';
        if (levelIndex[2] == 'X') topic = 10;
        Debug.Log(topic);

        // 确保是已完成关卡或者进行中关卡
        if (chapter <= PlayerPrefs.GetInt("chapter") && topic <= PlayerPrefs.GetInt("topic"))
        {
            Loader.LoadLevel(chapter, topic);
        }
        else
        {
            // 显示警告信息
            Attention.SetActive(true);
        }
    }

    public void EnterChapter()
    {
        // 获取当前按钮文本的第二个（中文）字符
        string chapterIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
        Debug.Log(chapterIndex[1]);

        for (int i = 0; i < AllChapters.Count; i++)
        {
            string index = AllChaptersButtons[i].GetComponentInChildren<TMP_Text>().text;
            if (index[1] != chapterIndex[1])
            {
                // 隐藏不是当前章节的章节
                AllChapters[i].SetActive(false);
            }
            else
            {
                AllChapters[i].SetActive(true);
            }
        }
    }

    public void Alright()
    {
        Attention.SetActive(false);
    }
}
