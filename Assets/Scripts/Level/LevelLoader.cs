using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public int Chap, Top;
    public string TitleText;
    public string TaskDescription;

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

    // 加载某个关卡
    public void LoadLevel(int chapter, int topic)
    {
        // 读取相关关卡的json文件
        // (测试用例)
        Chap = 0;
        Top = 0;
        TitleText = "Turtorial";
        TaskDescription = "Make CO<sub>2</sub> by using the offered chemicals.";

        // 跳转到Level场景
        SceneManager.LoadScene(4);
    }
}
